using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MinimapCameracontroller : MonoBehaviour
{
    public Transform player;  // Reference to the player's Transform
  
    public Transform playerIcon;
    PhotonView view;
    private void Start()
    {
        view = player.GetComponentInParent<PhotonView>();
    }
    void LateUpdate()
    {
        transform.rotation = Quaternion.Euler(90f, player.eulerAngles.y, 0f);
        if (player != null & view.IsMine)
        {
            // Update the position of the minimap camera based on player's position
            transform.position = new Vector3(player.position.x, transform.position.y, player.position.z);
            // Update the position of the player's minimap icon
            // view.RPC("UpdatePlayerIconPosition", RpcTarget.All);
            // UpdatePlayerIconPosition();
        }
        else if (!view.IsMine)
        {
       
            gameObject.SetActive(false);
           
        }
    }

    //   [PunRPC]
    void UpdatePlayerIconPosition()
    {
        if (player != null)
        {
            // Get the player's minimap icon (assuming it's a child of the player)


            if (playerIcon != null)
            {
                // Calculate the player's position on the minimap
                Vector3 playerPosOnMap = new Vector3(player.position.x, player.position.y, player.position.z);

                // Update the position of the player's minimap icon
                playerIcon.position = playerPosOnMap;
            }
        }
    }
}
