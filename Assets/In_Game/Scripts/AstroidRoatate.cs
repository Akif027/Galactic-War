using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstroidRoatate : MonoBehaviour
{
    public GameObject ExplosionEffect;
    void Update()
    {
        transform.Rotate(new Vector3(10 * Time.deltaTime, 10 * Time.deltaTime, 10 * Time.deltaTime));
       
    }

    private void OnTriggerEnter(Collider other)
    {
        gameObject.SetActive(false);

        if (other.gameObject.tag =="Bullet")
        {
           // PlayerHealth.Score += 10;
            GameObject Go = Instantiate(ExplosionEffect, transform.position, transform.rotation);
            Destroy(Go, 1f);

        }

        if (other.gameObject.tag == "ObjectdisableManger")
        {
            gameObject.SetActive(false);
            Debug.Log("disabled");
     


        }
        if (other.gameObject.tag == "Player")
        {

            GameObject Go = Instantiate(ExplosionEffect, transform.position, transform.rotation);
            Destroy(Go, 1f);


        }

    }
}
