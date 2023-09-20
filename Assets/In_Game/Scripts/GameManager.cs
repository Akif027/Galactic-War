
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager instance;
    public Text playerKillCountText;
 
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();

                if (instance == null)
                {
                    GameObject singleton = new GameObject("GameManager");
                    instance = singleton.AddComponent<GameManager>();
                }
            }
            return instance;
        }
    }


    private void Awake()
    {
        instance = this;

    }


  
}
