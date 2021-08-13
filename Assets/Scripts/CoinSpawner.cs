using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    public GameObject coinPrefab;

    public float timer = 0f;
    public float spawnTime = 3f;

    public Vector3 spawnLocation;
    // Start is called before the first frame update
    void Start()
    {
        //set up timer
        timer = spawnTime;
    }

    // Update is called once per frame
    void Update()
    {
        //randomize spawn location
        spawnLocation = new Vector3(Random.Range(-9.5f, 9.5f), Random.Range(-4.5f, 0.5f), 1);

        //randomize timer?
        spawnTime = Random.Range(5, 8);

        //decrement timer
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            Instantiate(coinPrefab, spawnLocation, transform.rotation);
            timer = spawnTime; // reset for new spawn
        }

    }
}
