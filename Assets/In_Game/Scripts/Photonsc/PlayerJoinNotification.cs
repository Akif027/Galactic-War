
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Realtime;
using DG.Tweening;

using ExitGames.Client.Photon;
using System.Collections;

public class PlayerJoinNotification : MonoBehaviourPunCallbacks
{
    [Header("Notificaion Setting :")]
    [Space]
    public GameObject notificationContainer; // Reference to the parent GameObject
    public float notificationDuration = 2f;
    public Vector3 initialPosition;
    public Vector3 targetPosition;

    [HideInInspector]public
    PhotonView view;
 
    private void Start()
    {
        view = GetComponent<PhotonView>();

        notificationContainer.SetActive(false);
  
        view.RPC("ShowKillNotification", RpcTarget.All, PhotonNetwork.NickName);
      
    }


    [PunRPC]
    public void ShowKillNotification(string PlayerName)
    {
        // Set the text and position
        Text notificationText = notificationContainer.GetComponentInChildren<Text>();
        notificationText.text = PlayerName + " Joined ";
        notificationContainer.transform.localPosition = initialPosition;

        // Show the container and animate it
        notificationContainer.SetActive(true);
        notificationContainer.transform.DOLocalMove(targetPosition, notificationDuration)
            .SetEase(Ease.OutQuad)
            .OnComplete(HideKillNotification);
    }

    private void HideKillNotification()
    {
        StartCoroutine(waitforsec());
        // Hide the container when the animation is complete
     
    }

    IEnumerator waitforsec()
    {
        yield return new WaitForSeconds(2);
        notificationContainer.SetActive(false);
    }
}
