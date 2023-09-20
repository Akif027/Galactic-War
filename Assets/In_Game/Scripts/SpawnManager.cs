using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{

  
        public Transform spawnPoints;
        public float initialSpawnInterval = 3f;
        public float minSpawnInterval = 1f;
        public float spawnIntervalDecreaseRate = 0.1f;
        public float spawnDistance = 20f; // used to clamp X
        public float MaxSpawnDistanceY, minSpawnDistanceY = -25; // used to clamp y
        [SerializeField] float EnemySpeed = 10f;

              
        public GameObject[] SpawnObjectPrefab;  // 
        private float currentSpawnInterval;

       

        private void Start()
        {

           currentSpawnInterval = initialSpawnInterval;
           for (int i = 0; i < SpawnObjectPrefab.Length; i++)
           {
             EnemyObjectPool.Instance.CreatePool(SpawnObjectPrefab[i], 10);
           }

            StartCoroutine(DynamicSpawnRoutine());
        }

        private IEnumerator DynamicSpawnRoutine()
        {
            while (true)
            {
                float randomX = Random.Range(-spawnDistance, spawnDistance);
                float randomY = Random.Range(MaxSpawnDistanceY, minSpawnDistanceY);
                Vector3 spawnPosition = new Vector3(randomX, randomY, spawnPoints.position.z);

            // Randomly select a prefab to spawn
            GameObject prefabToSpawn = SpawnObjectPrefab[Random.Range(0, SpawnObjectPrefab.Length - 1)];


            GameObject spawnedObject = EnemyObjectPool.Instance.GetPooledObject(prefabToSpawn);
                if (spawnedObject != null)
                {
                    spawnedObject.transform.position = spawnPosition;
                    spawnedObject.SetActive(true);
                    Rigidbody objectRigidbody = spawnedObject.GetComponent<Rigidbody>();
                    if (objectRigidbody != null)
                    {
                        objectRigidbody.velocity = -transform.forward * EnemySpeed;
                    }
                    else
                    {
                        Debug.LogWarning("Spawned object doesn't have a Rigidbody component.");
                    }
                }
                else
                {
                    Debug.LogWarning("No available object in the pool.");
                }



   
            yield return new WaitForSeconds(currentSpawnInterval);
                // Decrease the spawn interval over time
                currentSpawnInterval = Mathf.Max(currentSpawnInterval - spawnIntervalDecreaseRate, minSpawnInterval);
            }
        }

        // ...
    }
