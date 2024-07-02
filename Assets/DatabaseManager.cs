using UnityEngine;
using System;
using System.Data;
using Mono.Data.Sqlite;

public class DatabaseManager : MonoBehaviour
{
    private static DatabaseManager _instance;
    public static DatabaseManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<DatabaseManager>();
                if (_instance == null)
                {
                    GameObject go = new GameObject("DatabaseManager");
                    _instance = go.AddComponent<DatabaseManager>();
                }
            }
            return _instance;
        }
    }

    private string dbName = "URI=file:BookDatabase.db";

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
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
        using (var connection = new SqliteConnection(dbName))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
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

    public void AddBook(string title, string author, string description)
    {
        using (var connection = new SqliteConnection(dbName))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "INSERT INTO Books (Title, Author, Description) VALUES (@title, @author, @description);";
                command.Parameters.AddWithValue("@title", title);
                command.Parameters.AddWithValue("@author", author);
                command.Parameters.AddWithValue("@description", description);
                command.ExecuteNonQuery();
            }
        }
    }

    public bool IsBookBorrowed(string title)
    {
        using (var connection = new SqliteConnection(dbName))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT IsBorrowed FROM Books WHERE Title = @title;";
                command.Parameters.AddWithValue("@title", title);

                var result = command.ExecuteScalar();
                return result != null && Convert.ToInt32(result) == 1;
            }
        }
    }

    public void BorrowBook(string title, string borrower)
    {
        using (var connection = new SqliteConnection(dbName))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "UPDATE Books SET IsBorrowed = 1, Borrower = @borrower WHERE Title = @title;";
                command.Parameters.AddWithValue("@title", title);
                command.Parameters.AddWithValue("@borrower", borrower);
                command.ExecuteNonQuery();
            }
        }
    }

    public void ReturnBook(string title)
    {
        using (var connection = new SqliteConnection(dbName))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "UPDATE Books SET IsBorrowed = 0, Borrower = NULL WHERE Title = @title;";
                command.Parameters.AddWithValue("@title", title);
                command.ExecuteNonQuery();
            }
        }   
    }

    public string GetBookBorrower(string title)
    {
        using (var connection = new SqliteConnection(dbName))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT Borrower FROM Books WHERE Title = @title;";
                command.Parameters.AddWithValue("@title", title);
                var result = command.ExecuteScalar();
                return result != null ? result.ToString() : string.Empty;
            }
        }
    }
}
