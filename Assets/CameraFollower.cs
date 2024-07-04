using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CameraFollow : MonoBehaviour
{
    public float FollowSpeed = 2f;
    public float yOffset = 1f;
    public Transform target;

    private PhotonView view;

    void Start()
    {
        // Hole das PhotonView, um die Eigentümerschaft zu überprüfen
        view = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        // Nur die Kamera des lokalen Spielers aktualisieren
        if (view.IsMine)
        {
            Vector3 newPos = new Vector3(target.position.x, target.position.y + yOffset, -10f);
            transform.position = Vector3.Slerp(transform.position, newPos, FollowSpeed * Time.deltaTime);
        }
    }
}
