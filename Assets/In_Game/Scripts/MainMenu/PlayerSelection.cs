using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSelection : MonoBehaviour
{

 
    public GameObject SelectShipPanel;
    int playerIndex = 0;
   

    public void OpenPlayerselectionPanel()
    {
        SelectShipPanel.SetActive(true);
    }
    public void shipSelection(int ShipIndex)
    {

        playerIndex = ShipIndex;
    
    }

    public void ShipSelected()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetInt("ShipIndex", playerIndex);
        SelectShipPanel.SetActive(false);
        Debug.Log(PlayerPrefs.GetInt("ShipIndex"));
    }
}
