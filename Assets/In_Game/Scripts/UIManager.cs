using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
 
    public static UIManager instance;
   
    public Text TimeMonitorText;

    public Text SpeedMetorText;
    public Slider SpeedMetorSlider;

    public Text respawnTimerText;
    public Slider boostSlider; // Add this slider
    public Text boostText; // Add this slider

    float timer = 0f;

    public Text HealthText;

    public Text NameDisplay;

    public GameObject MainDisplayUiPanel;
    public RectTransform crosshair; // Reference to the crosshair UI element
    public GameObject respawnPanel;

    public Slider healthSlider;

    public static UIManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<UIManager>();

                if (instance == null)
                {
                    GameObject singleton = new GameObject("UiManager");
                    instance = singleton.AddComponent<UIManager>();
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
       



        TimeMonitorText.text = timer.ToString();


    }

    private void Update()
    {
        

        if (timer >= 1f)
        {
            
            TimeMonitorText.text= Mathf.FloorToInt(timer).ToString();
           
        }
        float elapsedTime = Time.time - timer;

        // Calculate minutes and seconds
        int minutes = Mathf.FloorToInt(elapsedTime / 60);
        int seconds = Mathf.FloorToInt(elapsedTime % 60);

        // Update the timer text
        TimeMonitorText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }


}
