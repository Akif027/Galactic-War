using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

class ArrowObjectPool : MonoBehaviour
{
    public static ArrowObjectPool current;

    [Tooltip("Assign the arrow prefab.")]
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
            Indicator arrow = pooledObject;
            GameObject arrowP = PhotonNetwork.Instantiate(arrow.name, transform.position, Quaternion.identity);
            arrowP.transform.SetParent(transform, false);
            Indicator indicator = arrowP.GetComponent<Indicator>();

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
            Indicator arrow = pooledObject;
            GameObject arrowP = PhotonNetwork.Instantiate(arrow.name,transform.position,Quaternion.identity);
            arrowP.transform.SetParent(transform, false);
          arrowP.GetComponent<Indicator>().Activate(false);
            pooledObjects.Add(arrow);
            return arrow;
        }
        return null;
    }

    /// <summary>
    /// Deactive all the objects in the pool.
    /// </summary>
    public void DeactivateAllPooledObjects()
    {
        foreach (Indicator arrow in pooledObjects)
        {
            arrow.Activate(false);
        }
    }
}
