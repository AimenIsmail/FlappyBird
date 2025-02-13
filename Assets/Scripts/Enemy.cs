using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float maxTime = 50f; // Time spawning
    private float timer = 0f;
    public GameObject enemyPrefab;
    public float enemySpeed = 2f; 
    private GameObject currentEnemy; // current enemy
    public Transform playerBird; 

    void Update()
    {
        // Check if no enemy exists and spawn a new one after maxTime
        if (currentEnemy == null && timer > maxTime)
        {
            SpawnEnemy();
            timer = 0f; // Reset timer
        }
        timer += Time.deltaTime;

        // Move enemy towards the bird
        if (currentEnemy != null)
        {
            MoveEnemy();
        }
    }

    void SpawnEnemy()
    {
        // Spawn an enemy 
        currentEnemy = Instantiate(enemyPrefab);
        currentEnemy.transform.position = new Vector3((transform.position.x)*3, playerBird.position.y, playerBird.position.z);
        
        // Ensure enemy has a Rigidbody2D (if not already added)
        if (currentEnemy.GetComponent<Rigidbody2D>() == null)
        {
            Rigidbody2D rb = currentEnemy.AddComponent<Rigidbody2D>();
            rb.isKinematic = true; // So it doesn't fall
        }

        // Ensure enemy has a Collider2D (if not already added)
        if (currentEnemy.GetComponent<Collider2D>() == null)
        {
            currentEnemy.AddComponent<BoxCollider2D>().isTrigger = true;
        }
    }

    void MoveEnemy()
    {
        if (playerBird == null) return; // player bird exists

        // Move the enemy 
        float step =   Time.deltaTime/enemySpeed; //  speed
        currentEnemy.transform.position = Vector3.MoveTowards(
            currentEnemy.transform.position, 
            new Vector3(playerBird.position.x,playerBird.position.y, currentEnemy.transform.position.z), 
            step);
    }
}
