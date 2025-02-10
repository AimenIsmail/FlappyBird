using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float maxTime = 10f;
    private float timer = 0f;
    public GameObject enemy;
    public float height;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Update the timer and spawn pipes
        if (timer > maxTime)
        {
            SpawnEnemy();
            timer = 0f;
        }
        timer += Time.deltaTime;
    }
    void SpawnEnemy()
    {
        // Instantiate a Enemy
        GameObject newEnemy = Instantiate(enemy);

        // Set the random position
        newEnemy.transform.position = transform.position + new Vector3(0, Random.Range(-height, height), 0);
    }
}
