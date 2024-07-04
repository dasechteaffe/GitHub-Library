using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

public class ConnecttoServer : MonoBehaviourPunCallbacks
{
    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.ConnectToRegion("eu"); // oder eine andere gewünschte Region
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master");
        PhotonNetwork.JoinLobby();
    }

    public override async void OnJoinedLobby()
    {
        Debug.Log("Joined Room, waiting for 5 seconds before loading Lobby scene");

        // Warte 5 Sekunden asynchron
        await Task.Delay(5000);

        // Szene wechseln nach der Wartezeit
        Debug.Log("Loading Lobby scene");
        SceneManager.LoadScene("Lobby");    
    }
}
