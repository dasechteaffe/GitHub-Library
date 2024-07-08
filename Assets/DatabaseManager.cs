using UnityEngine; 
using System; 
using Mono.Data.Sqlite; 

public class DatabaseManager : MonoBehaviour
{
    // Singleton-Instanz
    private static DatabaseManager _instance; 
    public static DatabaseManager Instance 
    {
        get
        {
            // Erstellen oder Abrufen der Instanz
            if (_instance == null)
            {
                // Wenn keine Instanz existiert, neue erstellen oder vorhandene finden
                _instance = FindObjectOfType<DatabaseManager>() ?? new GameObject("DatabaseManager").AddComponent<DatabaseManager>();
            }
            return _instance; // Zurückgeben der Instanz
        }
    }

    private string dbName = "URI=file:BookDatabase.db"; 

    void Awake() 
    {
        // Singleton-Logik zur Sicherstellung, dass nur eine Instanz existiert
        if (_instance == null)
        {
            _instance = this; // Setzen der Instanz auf das aktuelle Objekt
            DontDestroyOnLoad(gameObject); 
        }
        else if (_instance != this)
        {
            Destroy(gameObject); 
        }

        InitializeDatabase(); 
    }

    private void InitializeDatabase()
    {
        using (var connection = new SqliteConnection(dbName)) // Erstellen einer SQLite-Verbindung
        {
            connection.Open(); 
            using (var command = connection.CreateCommand()) // Erstellen eines SQLite-Befehls
            {
                command.CommandText = @"
                    CREATE TABLE IF NOT EXISTS Books (
                        Id INTEGER PRIMARY KEY,
                        Title TEXT NOT NULL,
                        Author TEXT NOT NULL,
                        Description TEXT,
                        IsBorrowed INTEGER DEFAULT 0,
                        Borrower TEXT
                    );";
                command.ExecuteNonQuery(); 
            }
        }
    }

    // Funktion zum Hinzufügen eines Buches zur Datenbank
    public void AddBook(string title, string author, string description)
    {
        using (var connection = new SqliteConnection(dbName)) 
        {
            connection.Open(); 
            using (var command = connection.CreateCommand()) 
            {
                // SQL-Befehl zum Einfügen eines Buches in die Bücher-Tabelle
                command.CommandText = "INSERT INTO Books (Title, Author, Description) VALUES (@title, @autor, @beschreibung);";
                command.Parameters.AddWithValue("@title", title); // Hinzufügen des Titels als Parameter
                command.Parameters.AddWithValue("@autor", author); 
                command.Parameters.AddWithValue("@beschreibung", description); 
                command.ExecuteNonQuery(); 
            }
        }
    }

    // Funktion zur Überprüfung, ob ein Buch ausgeliehen ist
    public bool IsBookBorrowed(string title)
    {
        using (var connection = new SqliteConnection(dbName)) // Erstellen einer SQLite-Verbindung
        {
            connection.Open(); // Öffnen der Verbindung
            using (var command = connection.CreateCommand()) // Erstellen eines SQLite-Befehls
            {
                // SQL-Befehl zum Abrufen des Ausleihstatus eines Buches
                command.CommandText = "SELECT IsBorrowed FROM Books WHERE Title = @title;";
                command.Parameters.AddWithValue("@title", title); // Hinzufügen des Titels als Parameter
                var result = command.ExecuteScalar(); // Ausführen des Befehls und Abrufen des Ergebnisses
                return result != null && Convert.ToInt32(result) == 1; // Rückgabe, ob das Buch ausgeliehen ist
            }
        }
    }

    // Funktion zum Ausleihen eines Buches
    public void BorrowBook(string title, string borrower)
    {
        using (var connection = new SqliteConnection(dbName)) 
        {
            connection.Open(); // Öffnen der Verbindung
            using (var command = connection.CreateCommand()) 
            {
                // SQL-Befehl zum Aktualisieren des Ausleihstatus und des Entleihers
                command.CommandText = "UPDATE Books SET IsBorrowed = 1, Borrower = @borrower WHERE Title = @title;";
                command.Parameters.AddWithValue("@title", title);
                command.Parameters.AddWithValue("@borrower", borrower); 
                command.ExecuteNonQuery(); 
            }
        }
    }

    // Funktion zum Zurückgeben eines Buches
    public void ReturnBook(string title)
    {
        using (var connection = new SqliteConnection(dbName)) 
        {
            connection.Open(); 
            using (var command = connection.CreateCommand()) 
            {
                // SQL-Befehl zum Zurücksetzen des Ausleihstatus und Entfernen des Entleihers
                command.CommandText = "UPDATE Books SET IsBorrowed = 0, Borrower = NULL WHERE Title = @title;";
                command.Parameters.AddWithValue("@title", title); 
                command.ExecuteNonQuery(); 
            }
        }
    }

    // Funktion zum Abrufen des Entleihers eines Buches
    public string GetBookBorrower(string title)
    {
        using (var connection = new SqliteConnection(dbName)) 
        {
            connection.Open(); // Öffnen der Verbindung
            using (var command = connection.CreateCommand()) 
            {
                // SQL-Befehl zum Abrufen des Entleihers eines Buches
                command.CommandText = "SELECT Borrower FROM Books WHERE Title = @title;";
                command.Parameters.AddWithValue("@title", title);
                var result = command.ExecuteScalar();
                return result?.ToString() ?? string.Empty; // Rückgabe des Entleihers oder leer, wenn nicht ausgeliehen
            }
        }
    }
}
