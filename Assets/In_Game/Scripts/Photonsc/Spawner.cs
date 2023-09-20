using Photon.Pun;

using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Transform[] spawnPoint;
    public GameObject enemy;
    public float startTimeBtwSpawns;
    float timeBtwSpawns;



    private void Update()
    {

        if (PhotonNetwork.IsMasterClient== false  || PhotonNetwork.CurrentRoom.PlayerCount!=2)
        {
            return; 
        }


        if (timeBtwSpawns <= 0)
        {

            Vector3 spawnPos = spawnPoint[Random.Range(0, spawnPoint.Length)].position;
            PhotonNetwork.Instantiate(enemy.name, spawnPos, Quaternion.identity);
            timeBtwSpawns = startTimeBtwSpawns;

        }
        else
        {
            timeBtwSpawns -= Time.deltaTime;
        }
    }
}
