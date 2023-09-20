using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineExplosion : MonoBehaviour
{
    public GameObject ExplosionEffect;

    [SerializeField]
    private float tumble;

    void Start()
    {
        GetComponent<Rigidbody>().angularVelocity = Random.insideUnitSphere * tumble;
    }


    private void OnTriggerEnter(Collider other)
    {
        gameObject.SetActive(false);

        if (other.gameObject.tag == "Bullet")
        {
            GameObject Go = Instantiate(ExplosionEffect, transform.position, transform.rotation);
            Destroy(Go, 2f);

        }

        if (other.gameObject.tag == "ObjectdisableManger")
        {
            gameObject.SetActive(false);
            Debug.Log("disabled");
            GameObject Go = Instantiate(ExplosionEffect, transform.position, transform.rotation);
            Destroy(Go, 1f);


        }
        if (other.gameObject.tag == "Player")
        {
           
            GameObject Go = Instantiate(ExplosionEffect, transform.position, transform.rotation);
            Destroy(Go, 1f);


        }


    }
}
