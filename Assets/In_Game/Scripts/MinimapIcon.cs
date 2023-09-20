using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MinimapIcon : MonoBehaviour
{
    public Transform player; // Reference to the player's Transform
    PhotonView view;


    private void Start()
    {
        view = GetComponentInParent<PhotonView>();
    }
    void LateUpdate()
    {
        transform.rotation = Quaternion.Euler(90f, player.eulerAngles.y, 0f);
        //  view.RPC("UpdatePos", RpcTarget.All);
        if (view.IsMine)
        {
            if (player != null)
            {
                // Update the position of the minimap icon to follow the player
                transform.position = new Vector3(player.position.x, transform.position.y, player.position.z);

                // Set the minimap icon's rotation to be the same as the initial rotation
                // This will keep the icon upright on the minimap

            }

            GetComponent<SpriteRenderer>().color = Color.cyan;
        }
        else if (!view.IsMine)
        {
            GetComponent<SpriteRenderer>().color = Color.red;
        }

    }


}
