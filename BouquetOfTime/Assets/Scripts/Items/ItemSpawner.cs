using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public GameObject goldKeyPrefab;
    public GameObject silverKeyPrefab;
    private GameObject[] spawnPoints;

    private void OnEnable()
    {
        spawnPoints = GameObject.FindGameObjectsWithTag("ItemSpawn");
        int doorCount = GameObject.FindGameObjectsWithTag("Door").Length;

        //spawning gold key
        int spawnPoint = Random.Range(0, spawnPoints.Length);
        Instantiate(goldKeyPrefab, spawnPoints[spawnPoint].transform.position, spawnPoints[spawnPoint].transform.rotation, spawnPoints[spawnPoint].transform);

        //spawning a silver key for every door
        for (int i = 0; i < doorCount; i++)
        {
            spawnPoint = Random.Range(0, spawnPoints.Length);
            while (spawnPoints[spawnPoint].transform.childCount != 0)
            {
                spawnPoint = Random.Range(0, spawnPoints.Length);
            }
            Instantiate(silverKeyPrefab, spawnPoints[spawnPoint].transform.position, spawnPoints[spawnPoint].transform.rotation, spawnPoints[spawnPoint].transform);
        }
    }
}
