using UnityEngine;
using Photon.Pun;

public class Camera_Follow_M : MonoBehaviourPunCallbacks
{
    public float FollowSpeed = 2f;
    public float yOffset = 1f;
    private Transform target;

    void Start()
    {
        // Finden Sie den lokalen Spieler und setzen Sie ihn als Ziel
        FindAndSetLocalPlayer();
    }

    void FindAndSetLocalPlayer()
    {
        var players = GameObject.FindGameObjectsWithTag("Player");
        foreach (var player in players)
        {
            PhotonView playerView = player.GetComponent<PhotonView>();
            if (playerView.IsMine)
            {
                target = player.transform;
                break;
            }
        }
    }

    void LateUpdate()
    {
        if (target == null)
        {
            FindAndSetLocalPlayer();
            return;
        }

        Vector3 newPos = new Vector3(target.position.x, target.position.y + yOffset, -10f);
        transform.position = Vector3.Slerp(transform.position, newPos, FollowSpeed * Time.deltaTime);
    }

    public override void OnJoinedRoom()
    {
        FindAndSetLocalPlayer();
    }
}