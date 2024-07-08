using UnityEngine; 
using TMPro; 
using UnityEngine.UI; 

public class BookInfoMenu : MonoBehaviour 
{
    public static BookInfoMenu instance; // Statische Instanz der BookInfoMenu-Klasse zur Singleton-Verwendung.
    public GameObject menuPanel; 
    public TextMeshProUGUI titleText; 
    public TextMeshProUGUI authorText; 
    public TextMeshProUGUI descriptionText; 
    public TextMeshProUGUI escText; 
    public Button borrowButton; 
    public TextMeshProUGUI borrowStatusText; 
    public TextMeshProUGUI availabilityText; 
    public TMP_InputField borrowerInputField;  
    private string currentBookTitle; 
    private bool isCurrentlyBorrowed; 

    void Awake() 
    {
        if (instance == null) // Prüft, ob sie breit exestiert 
        {
            instance = this; // Setzt die Instanz auf dieses Objekt.
            DontDestroyOnLoad(gameObject); // Verhindert, dass das Objekt beim Laden einer neuen Szene zerstört wird.
        }
        else
        {
            Destroy(gameObject); // Zerstört das aktuelle Objekt, wenn bereits eine Instanz existiert.
        }
    }

    public void ShowMenu(string title, string author, string description, bool isBorrowed, string borrower = "") // Methode zur Anzeige des Menüs.
    {
        if (menuPanel == null || titleText == null || authorText == null || descriptionText == null ||
            escText == null || borrowButton == null || borrowStatusText == null || availabilityText == null || borrowerInputField == null)
        {
            Debug.LogError("Ein oder mehrere erforderliche Komponenten fehlen im BookInfoMenu."); 
            return; // Beendet die Methode, wenn Komponenten fehlen.
        }

        currentBookTitle = title; 
        isCurrentlyBorrowed = isBorrowed; 
        titleText.text = "Titel: " + title; 
        authorText.text = "Autor: " + author; 
        descriptionText.text = "Beschreibung: " + description; 
        escText.text = "Esc Taste zum Verlassen"; 
        borrowerInputField.text = borrower; 
        UpdateBorrowStatus(); 
        menuPanel.SetActive(true); 
    }

    private void UpdateBorrowStatus() 
    {
        if (isCurrentlyBorrowed) // Prüft, ob das Buch ausgeliehen ist.
        {
            availabilityText.text = "Status: Nicht verfügbar"; 
            borrowStatusText.text = "Zurückgeben"; 
            borrowButton.interactable = true; 
            borrowerInputField.interactable = false; 
        }
        else
        {
            availabilityText.text = "Status: Verfügbar"; 
            borrowStatusText.text = "Ausleihen"; 
            borrowButton.interactable = true; 
            borrowerInputField.interactable = true; 
        }
    }

    public void HideMenu() // Methode zur Ausblendung des Menüs.
    {
        if (menuPanel != null) // Prüft, ob das Menü-Panel vorhanden ist.
        {
            menuPanel.SetActive(false); 
        }
    }



    void Update() // Unity-Methode, die in jedem Frame aufgerufen wird.
    {
        if (menuPanel != null && menuPanel.activeSelf && Input.GetKeyDown(KeyCode.Escape)) // Prüft, ob das Menü aktiv ist und die Escape-Taste gedrückt wird.
        {
            HideMenu(); 
        }
    }

    public void ToggleBorrowStatus() // Methode zum Umschalten des Ausleihstatus.
    {
        if (isCurrentlyBorrowed) // Prüft, ob das Buch ausgeliehen ist.
        {
            DatabaseManager.Instance.ReturnBook(currentBookTitle); // Ruft die Methode zur Rückgabe des Buches auf.
            isCurrentlyBorrowed = false;
            borrowerInputField.text = ""; // Leert das Ausleiher-Eingabefeld.
        }
        else
        {
            string borrower = borrowerInputField.text;
            if (string.IsNullOrEmpty(borrower)) // Prüft, ob das Ausleiher-Feld leer ist.
            {
                Debug.LogWarning("Bitte geben Sie einen Namen für den Ausleiher ein."); 
                return; 
            }
            DatabaseManager.Instance.BorrowBook(currentBookTitle, borrower); // Ruft die Methode zum Ausleihen des Buches auf.
            isCurrentlyBorrowed = true; // Setzt den Ausleihstatus auf ausgeliehen.
        }
        UpdateBorrowStatus(); 
    }
}
