using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Photon.Pun;

public class Objectpool : MonoBehaviour
{
  
    public List<GameObject> PooledObject_list;//define
    public static Objectpool poolSharedInstance;
    public GameObject ObjectToPool;
    public int AmountToPool;

    private void Awake()
    {
        poolSharedInstance = this;
    
        PooledObject_list = new List<GameObject>();

        for (int i = 0; i < AmountToPool; i++)
        {
            GameObject temp = PhotonNetwork.Instantiate(ObjectToPool.name, Vector3.zero, Quaternion.identity);
            temp.transform.SetParent(transform);
            temp.SetActive(false);
            PooledObject_list.Add(temp);
        }
    }


     public GameObject GetpoolObject()
    {
        for (int i = 0; i < AmountToPool; i++)
        {
            if (!PooledObject_list[i].activeInHierarchy)
            {

                PooledObject_list[i].SetActive(true);
                return PooledObject_list[i];
            }
        }
        return null;

    }


/*
    private GameObject GetPoolObjectRPC()
    {
        
    }
 */

}