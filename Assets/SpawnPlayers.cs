using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnPlayers : MonoBehaviour
{
    public GameObject playerPrefab;

    public float minX;
    public float maxX;
    private float fixedY = -4.25f; // Feste y-Koordinate
    private float fixedZ = -4.25f;

    private void Start()
    {
        float randomX = Random.Range(minX, maxX);
        Vector3 spawnPosition = new Vector3(randomX, fixedY, fixedZ);
        PhotonNetwork.Instantiate(playerPrefab.name, spawnPosition, Quaternion.identity);
    }
}