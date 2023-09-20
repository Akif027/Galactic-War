
using Photon.Pun;

using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviourPunCallbacks
{

    [HideInInspector]
    public PhotonView view;
    public float maxHealth = 100f;
    public float currentHealth = 100f;
    public Text playerKillCountText;
    public int killCount = 0;


    [Header("UI Elements")]

    //public Text HelpText;

    public bool iSplayerdead = false;
    UIManager uimanager;

    //otherPlayer
    GameObject Victimobject;
    [HideInInspector]
    public bool CankillCount=  false;
    string OtherPlayerName;

    private PlayerJoinNotification playerNotify;
    private void Awake()
    {

        playerNotify = FindObjectOfType<PlayerJoinNotification>();
        uimanager = UIManager.instance;
    }
    private void Start()
    {
        view = GetComponent<PhotonView>();
        gameObject.name = view.Owner.NickName;
      
        if (view.IsMine)
        {
           
            playerKillCountText = GameManager.instance.playerKillCountText ;
            uimanager.NameDisplay.text = PhotonNetwork.NickName;
            Debug.Log("ischecking");
            //  playerName = PhotonNetwork.NickName;
            currentHealth = maxHealth;
        
        }
        else 
        {
           uimanager.NameDisplay.text = view.Owner.NickName;
         
        }
   
    }

   // 

    public float getHealth()
    {
        return currentHealth;
    }

    private void Update()
    {
        if (view.IsMine)
        {
            uimanager.healthSlider.value = currentHealth / maxHealth;
    
            uimanager.HealthText.text =  currentHealth.ToString() + "/100";

            playerKillCountText.text = killCount.ToString();
            Debug.Log("cankIll - " + CankillCount  );

            if (Victimobject !=null)
            {
                OtherPlayerName = Victimobject.name + " Killed ";
             
                if (!Victimobject.transform.GetChild(0).gameObject.activeSelf && !CankillCount)
                {
                    playerNotify.view.RPC("ShowKillNotification",RpcTarget.All,OtherPlayerName);
                    Debug.Log("isdead ");
                    killCount++;
                    CankillCount=true;
                  
                }
                if (Victimobject.transform.GetChild(0).gameObject.activeSelf)
                {
                    CankillCount = false;
                }
            }


        }

    }
 /*   private void KillPlayer(Player killedPlayer)
    {
        OtherPlayerName = killedPlayer.NickName;
        Debug.Log("Killed Player Nickname: " + OtherPlayerName);

        // Your game logic for handling the kill...
    }*/

    public void OnPlayerKilled(GameObject OtherPlayer)
    {
        if (view.IsMine)
        {
            Victimobject = OtherPlayer;
        }
        

    }



    [PunRPC]
    public void TakeDamage(float amount)
    {
        if (!view.IsMine)
            return;

        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            die();
            //  currentHealth =100;
          
            currentHealth = 0;
        }


        view.RPC("SetHealth", RpcTarget.OthersBuffered, currentHealth);
    }

    private void die()
    {
        if (!view.IsMine)
            return;

       iSplayerdead = true;
    }


    /*   [PunRPC]*/
    [PunRPC]
    public void ModifyHealth(float amount)
    {
        currentHealth = amount;
       iSplayerdead = false;
     
        if (currentHealth < 0)
        {
            currentHealth = 0;
        }
        view.RPC("SetHealth", RpcTarget.OthersBuffered, currentHealth);
    }



    [PunRPC]
    private void SetHealth(float newHealth)
    {
        currentHealth = newHealth;
    }


}
