using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Data;
using Mono.Data.Sqlite;
using TMPro; // Für TextMeshPro

public class MainMenu : MonoBehaviour
{
    public Button startButton;
    public Button databaseButton;
    public TextMeshProUGUI databaseText; // Geändert zu TextMeshProUGUI

    private string dbName = "URI=file:BookDatabase.db"; // Geändert zu BookDatabase.db

    void Start()
    {
        startButton.onClick.AddListener(StartGame);
        databaseButton.onClick.AddListener(AccessDatabase);
    }

    void StartGame()
    {
        SceneManager.LoadScene("Library"); // Geändert zu "Library"
    }

    void AccessDatabase()
    {
        using (var connection = new SqliteConnection(dbName))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM Books"; // Angenommen, Ihre Tabelle heißt "Books"
                using (IDataReader reader = command.ExecuteReader())
                {
                    string data = "";
                    while (reader.Read())
                    {
                        // Angenommen, Ihre Tabelle hat Spalten "Title" und "Author"
                        data += reader["Title"] + " - " + reader["Author"] + " - " + reader["Borrower"] + "\n";

                    }
                    databaseText.text = data; // TextMeshPro verwendet einfach .text
                }
            }
        }
    }
}
