using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BookInfoMenu : MonoBehaviour
{
    public static BookInfoMenu instance;
    public GameObject menuPanel;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI authorText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI escText;
    public Button borrowButton;
    public TextMeshProUGUI borrowStatusText;
    public TextMeshProUGUI availabilityText;
    public TMP_InputField borrowerInputField;  // Neues Eingabefeld für den Ausleiher
    private string currentBookTitle;
    private bool isCurrentlyBorrowed;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ShowMenu(string title, string author, string description, bool isBorrowed, string borrower = "")
    {
        if (menuPanel == null || titleText == null || authorText == null || descriptionText == null ||
            escText == null || borrowButton == null || borrowStatusText == null || availabilityText == null || borrowerInputField == null)
        {
            Debug.LogError("Ein oder mehrere erforderliche Komponenten fehlen im BookInfoMenu.");
            return;
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
        if (isCurrentlyBorrowed)
        {
            availabilityText.text = "Status: Nicht verfuegbar";
            borrowStatusText.text = "Zurueckgeben";
            borrowButton.interactable = true;
            borrowerInputField.interactable = false;
        }
        else
        {
            availabilityText.text = "Status: Verfuegbar";
            borrowStatusText.text = "Ausleihen";
            borrowButton.interactable = true;
            borrowerInputField.interactable = true;
        }
    }

    public void HideMenu()
    {
        if (menuPanel != null)
        {
            menuPanel.SetActive(false);
        }
    }

    void Update()
    {
        if (menuPanel != null && menuPanel.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            HideMenu();
        }
    }

    public void ToggleBorrowStatus()
    {
        if (isCurrentlyBorrowed)
        {
            DatabaseManager.Instance.ReturnBook(currentBookTitle);
            isCurrentlyBorrowed = false;
            borrowerInputField.text = "";
        }
        else
        {
            string borrower = borrowerInputField.text;
            if (string.IsNullOrEmpty(borrower))
            {
                Debug.LogWarning("Bitte geben Sie einen Namen fuer den Ausleiher ein.");
                return;
            }
            DatabaseManager.Instance.BorrowBook(currentBookTitle, borrower);
            isCurrentlyBorrowed = true;
        }
        UpdateBorrowStatus();
    }
}
