using Photon.Pun;
using System.Collections;

using UnityEngine;


public class Bullet : MonoBehaviour
{

    public float disableDelay = 1.0f;
    public GameObject LaserImpact;
    PhotonView view;
    private GameObject shooter;

    private void Start()
    {
            view = GetComponent<PhotonView>();
    }
    private void OnEnable()
    {
        StartCoroutine(DisableAfterDelay());
    }

    public void SetShooter(GameObject shooter)
    {
        this.shooter = shooter;
    }

    private void OnTriggerEnter(Collider other)
    {



        if (other.gameObject.CompareTag("Player"))
        {
            PhotonView otherPhotonView = other.GetComponent<PhotonView>();
            if (otherPhotonView != null && !otherPhotonView.IsMine)
            {
                string playerName = otherPhotonView.gameObject.name;
                Debug.Log("Other player's name: " + playerName);
            }

            if (view.IsMine)
            {
                PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
                
                float victimsHealth = playerHealth.getHealth();
                Debug.Log(victimsHealth);
            
                if (playerHealth != null)
                {
                  
                    playerHealth.view.RPC("TakeDamage", RpcTarget.AllBuffered, 5f);

                   
                    shooter.GetComponent<PlayerHealth>().OnPlayerKilled(other.gameObject);

                 
                    /*      if (shooter != null && shooter.GetComponent<PhotonView>().IsMine)
                          {

                              shooter.GetComponent<PlayerHealth>().OnPlayerKilled(other.gameObject);

                              //s shooter.GetComponent<PhotonView>().RPC("RpcIncreaseKillCount", RpcTarget.All);

                          }
      */

                }


                Debug.Log(other.gameObject.name);

                GameObject laser = PhotonNetwork.Instantiate(LaserImpact.name, transform.position, Quaternion.identity);
                Destroy(laser, 2);
                gameObject.SetActive(false);
         
            }

          


        }


    }

    private IEnumerator DisableAfterDelay()
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(disableDelay);
   
        gameObject.SetActive(false);
    }

   
}