using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeySpawner : MonoBehaviour
{
    public GameObject keyPrefab;
    private GameObject[] spawnPoints;

    private void OnEnable()
    {
        spawnPoints = GameObject.FindGameObjectsWithTag("KeySpawn");

        int spawnPoint = Random.Range(0, spawnPoints.Length);

        Instantiate(keyPrefab, spawnPoints[spawnPoint].transform.position, spawnPoints[spawnPoint].transform.rotation);
    }
}
