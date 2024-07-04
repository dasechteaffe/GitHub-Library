using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;  // Importiere Text Mesh Pro

public class CreatJoin : MonoBehaviourPunCallbacks
{
    public TMP_InputField createInput;  // Ersetze InputField durch TMP_InputField
    public TMP_InputField joinInput;    // Ersetze InputField durch TMP_InputField

    public void CreatRoom()
    {
        PhotonNetwork.CreateRoom(createInput.text);
    }

    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(joinInput.text);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("Library_M");
    }
}
