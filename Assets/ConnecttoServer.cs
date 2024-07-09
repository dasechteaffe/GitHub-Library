using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

public class ConnecttoServer : MonoBehaviourPunCallbacks
{
    public AudioClip Hochfahren;
    private AudioSource audioSource;

    private void Start()
    {
        // Initialize AudioSource
        audioSource = gameObject.AddComponent<AudioSource>();
        
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.ConnectToRegion("eu"); 
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
        
        PlaySound(Hochfahren);

        // Szene wechseln nach der Wartezeit
        Debug.Log("Loading Lobby scene");
        SceneManager.LoadScene("Lobby");
    }

    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.clip = clip;
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("keine audio hinterlegt");
        }
    }
}