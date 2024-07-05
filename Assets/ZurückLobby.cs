using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ZurückLobby : MonoBehaviour
{
    public Button zurückLobbyButton;

    void Start()
    {
        // Fügen Sie den Listener zum Button hinzu
        zurückLobbyButton.onClick.AddListener(ZurückZurLobby);
    }

    void ZurückZurLobby()
    {
        SceneManager.LoadScene("Lobby"); // Name der Lobby-Szene
    }
}