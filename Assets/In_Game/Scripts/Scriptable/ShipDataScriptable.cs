using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShipsHolder", menuName = "ShipsCollection/Ship")]
public class ShipDataScriptable : ScriptableObject
{
    [Tooltip("Ship Name")] public string ShipName; 
    [Tooltip("Main Ship")] public GameObject ShipGameObject;
    [Tooltip("Speed of Ship")] public float Speed = 100;

}
