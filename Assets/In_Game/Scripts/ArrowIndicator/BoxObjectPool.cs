using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

public class BoxObjectPool : MonoBehaviour
{
    public static BoxObjectPool current;

    [Tooltip("Assign the box prefab.")]
    public Indicator pooledObject;
    [Tooltip("Initial pooled amount.")]
    public int pooledAmount = 1;
    [Tooltip("Should the pooled amount increase.")]
    public bool willGrow = true;

    List<Indicator> pooledObjects;

    void Awake()
    {
        current = this;
    }

    void Start()
    {
        pooledObjects = new List<Indicator>();

        for (int i = 0; i < pooledAmount; i++)
        {
           /* Indicator box = pooledObject;
            GameObject BoxP = PhotonNetwork.Instantiate(box.name, Vector3.zero, Quaternion.identity);
            BoxP.transform.SetParent(transform, false);
            BoxP.GetComponent<Indicator>().Activate(false);
            pooledObjects.Add(box);*/

            Indicator box = pooledObject;
            GameObject BoxP = PhotonNetwork.Instantiate(box.name, Vector3.zero, Quaternion.identity);
            BoxP.transform.SetParent(transform, false);
            Indicator indicator = BoxP.GetComponent<Indicator>();

            // Activate or deactivate based on willGrow
            indicator.Activate(!willGrow);

            pooledObjects.Add(indicator);
        }
    }

    /// <summary>
    /// Gets pooled objects from the pool.
    /// </summary>
    /// <returns></returns>
    public Indicator GetPooledObject()
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].Active)
            {
                return pooledObjects[i];
            }
        }
        if (willGrow)
        {
            Indicator box = pooledObject;
            GameObject BoxP = PhotonNetwork.Instantiate(box.name, Vector3.zero, Quaternion.identity);
            BoxP.transform.SetParent(transform, false);
          BoxP.GetComponent<Indicator>().Activate(false);
            pooledObjects.Add(box);
            return box;
        }
        return null;
    }

    /// <summary>
    /// Deactive all the objects in the pool.
    /// </summary>
    public void DeactivateAllPooledObjects()
    {
        foreach (Indicator box in pooledObjects)
        {
          box.Activate(false);
        }
    }
}
