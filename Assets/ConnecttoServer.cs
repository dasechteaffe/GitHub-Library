using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.SceneManagement;


public class ConnecttoServer : MonoBehaviourPunCallbacks
{
    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.GameVersion = "1.0"; // Setzen Sie hier Ihre Spielversion
        PhotonNetwork.ConnectToRegion(""); // oder eine andere gewünschte Region
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined Room, loading Lobby scene");
        SceneManager.LoadScene("Lobby");
    }
}

