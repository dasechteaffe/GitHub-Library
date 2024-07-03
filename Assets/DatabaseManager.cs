using UnityEngine; 
using System; 
using System.Data; // Importiert die System-Datenbibliothek für Datenbankfunktionen.
using Mono.Data.Sqlite; // Importiert die Mono.Data.Sqlite-Bibliothek für die SQLite-Datenbankverwaltung. Die im Plugins Ordner ist

public class DatabaseManager : MonoBehaviour 
{
    private static DatabaseManager _instance; // Private statische Instanz zur Implementierung des Singleton-Patterns.
    public static DatabaseManager Instance // Öffentliche statische Eigenschaft zum Zugriff auf die Instanz.
    {
        get
        {
            if (_instance == null) // Prüft, ob die Instanz null ist.
            {
                _instance = FindObjectOfType<DatabaseManager>(); // Sucht nach einer Instanz in der Szene.
                if (_instance == null) // Wenn keine Instanz gefunden wurde, wird eine neue erstellt.
                {
                    GameObject go = new GameObject("DatabaseManager");
                    _instance = go.AddComponent<DatabaseManager>(); // Fügt dem GameObject das DatabaseManager-Skript hinzu.
                }
            }
            return _instance; // Gibt die Instanz zurück.
        }
    }

    private string dbName = "URI=file:BookDatabase.db"; 

    void Awake() 
    {
        if (_instance == null) // Prüft, ob _instance null ist.
        {
            _instance = this; 
            DontDestroyOnLoad(gameObject); 
        }
        else if (_instance != this) // Prüft, ob eine andere Instanz existiert.
        {
            Destroy(gameObject); 
        }

        InitializeDatabase(); 
    }

    private void InitializeDatabase() 
    {
        using (var connection = new SqliteConnection(dbName)) // Erstellt eine neue SQLite-Verbindung.
        {
            connection.Open(); 
            using (var command = connection.CreateCommand()) // Erstellt einen neuen SQL-Befehl.
            {
                command.CommandText = @"
                    CREATE TABLE IF NOT EXISTS Books ( // SQL-Befehl zur Erstellung der Tabelle, falls sie noch nicht existiert.
                        Id INTEGER PRIMARY KEY, // Primärschlüssel, der automatisch inkrementiert wird.
                        Title TEXT NOT NULL, // Titel des Buches, darf nicht null sein.
                        Author TEXT NOT NULL, // Autor des Buches, darf nicht null sein.
                        Description TEXT, // Beschreibung des Buches.
                        IsBorrowed INTEGER DEFAULT 0, // Ausleihstatus, Standardwert ist 0 (nicht ausgeliehen).
                        Borrower TEXT // Name des Ausleihers.
                    );";
                command.ExecuteNonQuery(); // Führt den SQL-Befehl aus, um die Tabelle zu erstellen.
            }
        }
    }

    public void AddBook(string title, string author, string description) // Methode zum Hinzufügen eines neuen Buches zur Datenbank.
    {
        using (var connection = new SqliteConnection(dbName)) 
        {
            connection.Open(); // Öffnet die Verbindung zur Datenbank.

            using (var command = connection.CreateCommand()) 
            {
                command.CommandText = "INSERT INTO Books (Title, Author, Description) VALUES (@title, @author, @description);"; // SQL-Befehl zum Einfügen eines neuen Buches.
                command.Parameters.AddWithValue("@title", title); // Bindet den Titel-Parameter.
                command.Parameters.AddWithValue("@author", author); 
                command.Parameters.AddWithValue("@description", description); 
                command.ExecuteNonQuery(); 
            }
        }
    }

    public bool IsBookBorrowed(string title) // Methode zur Überprüfung, ob ein Buch ausgeliehen ist.
    {
        using (var connection = new SqliteConnection(dbName)) 
        {
            connection.Open(); 

            using (var command = connection.CreateCommand()) 
            {
                command.CommandText = "SELECT IsBorrowed FROM Books WHERE Title = @title;"; // SQL-Befehl zur Abfrage des Ausleihstatus.
                command.Parameters.AddWithValue("@title", title);

                var result = command.ExecuteScalar(); // Führt den SQL-Befehl aus und gibt das Ergebnis zurück.
                return result != null && Convert.ToInt32(result) == 1; // Konvertiert das Ergebnis in eine Zahl und prüft, ob das Buch ausgeliehen ist.
            }
        }
    }

    public void BorrowBook(string title, string borrower) // Methode zum Ausleihen eines Buches.
    {
        using (var connection = new SqliteConnection(dbName)) 
        {
            connection.Open(); 
            using (var command = connection.CreateCommand()) 
            {
                command.CommandText = "UPDATE Books SET IsBorrowed = 1, Borrower = @borrower WHERE Title = @title;"; // SQL-Befehl zum Aktualisieren des Ausleihstatus und des Ausleihers.
                command.Parameters.AddWithValue("@title", title);
                command.Parameters.AddWithValue("@borrower", borrower); 
                command.ExecuteNonQuery(); // Führt den SQL-Befehl aus, um das Buch als ausgeliehen zu markieren.
            }
        }
    }

    public void ReturnBook(string title) // Methode zur Rückgabe eines Buches.
    {
        using (var connection = new SqliteConnection(dbName)) 
        {
            connection.Open(); 
            using (var command = connection.CreateCommand()) 
            {
                command.CommandText = "UPDATE Books SET IsBorrowed = 0, Borrower = NULL WHERE Title = @title;"; // SQL-Befehl zum Zurücksetzen des Ausleihstatus und des Ausleihers.
                command.Parameters.AddWithValue("@title", title); 
                command.ExecuteNonQuery(); // Führt den SQL-Befehl aus, um das Buch als zurückgegeben zu markieren.
            }
        }
    }

    public string GetBookBorrower(string title) // Methode zur Abfrage des aktuellen Ausleihers eines Buches.
    {
        using (var connection = new SqliteConnection(dbName)) 
        {
            connection.Open(); 
            using (var command = connection.CreateCommand()) 
            {
                command.CommandText = "SELECT Borrower FROM Books WHERE Title = @title;"; // SQL-Befehl zur Abfrage des Ausleihers.
                command.Parameters.AddWithValue("@title", title); 
                var result = command.ExecuteScalar(); 
                return result != null ? result.ToString() : string.Empty; // Gibt den Namen des Ausleihers zurück oder einen leeren String, wenn kein Ausleiher vorhanden ist.
            }
        }
    }
}
