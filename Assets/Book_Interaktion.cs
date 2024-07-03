using UnityEngine; 

public class BookInteraction : MonoBehaviour 
{
    public string bookTitle; 
    public string bookAuthor; 
    public string bookDescription; 
    public float interactionDistance = 2f; 
    private bool playerInRange = false; 
    private GameObject player; 

    void Start() 
    {
        player = GameObject.FindGameObjectWithTag("Player"); // Findet das Spieler-GameObject anhand seines Tags.
        DatabaseManager.Instance.AddBook(bookTitle, bookAuthor, bookDescription); 
    }

    void Update() 
    {
        if (player != null) // Prüft, ob das Spieler-GameObject gefunden wurde.
        {
            float distance = Vector2.Distance(transform.position, player.transform.position); // Berechnet die Distanz zwischen Buch und Spieler.
            playerInRange = distance <= interactionDistance; 
            if (playerInRange && Input.GetKeyDown(KeyCode.Return)) 
            {
                ShowBookInfo(); 
            }
        }
    }

    void ShowBookInfo() 
    {
        bool isBorrowed = DatabaseManager.Instance.IsBookBorrowed(bookTitle); 
        string borrower = DatabaseManager.Instance.GetBookBorrower(bookTitle); // Holt den Namen des Ausleihers des Buches.
        BookInfoMenu.instance.ShowMenu(bookTitle, bookAuthor, bookDescription, isBorrowed, borrower); 
    }

    void OnDrawGizmosSelected() //Interaktionsgebiet sehen und anzupassen.
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionDistance); 
    }
}
