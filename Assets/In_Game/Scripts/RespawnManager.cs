using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class RespawnManager : MonoBehaviourPunCallbacks
{
  
    public float respawnTime = 5f; // Adjust this value to set the respawn time.

    private float currentRespawnTime;
    private bool isRespawning = false;
    private Vector3 initialSpawnPosition;
    private PlayerHealth PH;
    private ShipController SC;
    PhotonView view;

    [SerializeField] GameObject PlayerModel;
 
    [SerializeField] GameObject MinnMapIcon;

    UIManager uimanager;
    private Target targetscript;
    private void Awake()
    {
        uimanager = UIManager.instance;
    }
    private void Start()
    {
        currentRespawnTime = respawnTime;
        SC = GetComponent<ShipController>();
        view = GetComponent<PhotonView>();
        uimanager.respawnPanel.SetActive(false); // Initially, hide the respawn panel.
        initialSpawnPosition = transform.position; // Store the initial spawn position.
        PH = GetComponentInParent<PlayerHealth>();
        targetscript = PH.gameObject.GetComponent<Target>();

    }

    private void Update()
    {
        if (isRespawning)
        {
            // Update the timer countdown text.
            uimanager.respawnTimerText.text = "Respawning in: " + Mathf.Ceil(currentRespawnTime).ToString();
  
            // Decrease the respawn timer.
            currentRespawnTime -= Time.deltaTime;

            // When the timer reaches 0, respawn is complete.
            if (currentRespawnTime <= 0)
            {

                uimanager.respawnPanel.SetActive(false);
                isRespawning = false;
          
                // Call the Respawn method for the local player.
                view.RPC("Respawn", RpcTarget.AllBuffered);
            }
        }
     
        if (view.IsMine)
        {
        
            if (PH.iSplayerdead) //checks for player is dead or not 
            {
                view.RPC("DisbalePlayerOverNetwork", RpcTarget.OthersBuffered);
                PlayerModel.SetActive(false);
                PlayerDied();
                SC.enabled = false;
       
            }
            else
            {
              
                SC.enabled = true;
          
            }
        }
    }
    [PunRPC]
    private void Respawn()

    {
        if (view.IsMine)
        {
            // Reset the player's position to the initial spawn position.
            transform.position = initialSpawnPosition;

            view.RPC("ModifyHealth", RpcTarget.AllBuffered, 100f);
            PlayerModel.SetActive(true);
            currentRespawnTime = respawnTime;
            view.RPC("EnablePlayerOverNetwork", RpcTarget.OthersBuffered);
           
            //  PH.iSplayerdead = false;
        }
    }

    [PunRPC]
    private void EnablePlayerOverNetwork()
    {
        PlayerModel.SetActive(true);
        targetscript.enabled = true;
        MinnMapIcon.SetActive(true);
  
    }
    [PunRPC]
    private void DisbalePlayerOverNetwork()
    {
        PlayerModel.SetActive(false);
        targetscript.enabled = false;
        MinnMapIcon.SetActive(false);

    }
    public void PlayerDied()
    {
      
        if (view.IsMine)
        {
            // Call this method when the player dies to start the respawn timer.
          
            uimanager.respawnPanel.SetActive(true);
            isRespawning = true;
        }
    }
}
