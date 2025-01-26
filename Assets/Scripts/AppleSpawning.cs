using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppleSpawning : MonoBehaviour
{
    public GameObject apple;
    public float maxTime = 1f;
    private float timer = 0f;
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
            SpawnApple();
            timer = 0f;
        }
        timer += Time.deltaTime;
    }
    void SpawnApple()
    {
        // Instantiate a new apple
        GameObject newApple = Instantiate(apple);

        // Set a random spawn position in world space
        Vector3 spawnPosition = new Vector3(0, Random.Range(-height, height), 0);
        newApple.transform.position = spawnPosition;
       
        // Destroy the Apple 
        Destroy(newApple, 15f);
    }       
}
