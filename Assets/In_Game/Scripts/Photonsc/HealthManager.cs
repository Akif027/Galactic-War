using Photon.Pun;

using TMPro;
using UnityEngine;

public class HealthManager : MonoBehaviour
{

    public TMP_Text HealthT;
    private float health;

    PhotonView view;

    private void Start()
    {
        health = 100;
        view = GetComponent<PhotonView>();
    }

    public void TakeDamage()
    {
        view.RPC("TakeDamageRPC", RpcTarget.All);

    }

    [PunRPC]
    private void TakeDamageRPC()
    {
        health--;
        HealthT.text = "Health : " + health.ToString();
    }
}
