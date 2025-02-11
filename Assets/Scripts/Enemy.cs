using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float maxTime = 20f; // Time interval for spawning
    private float timer = 0f;
    public GameObject enemyPrefab; // Prefab for enemy bird
    public float enemySpeed = 2f; // Speed of enemy movement
    private GameObject currentEnemy; // Reference to the current enemy
    public Transform playerBird; // Reference to Flappy Bird

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
        // Spawn an enemy at the same Y and Z as Flappy Bird
        currentEnemy = Instantiate(enemyPrefab);
        currentEnemy.transform.position = new Vector3((transform.position.x), playerBird.position.y, playerBird.position.z);
    }

    void MoveEnemy()
    {
        if (playerBird == null) return; // Ensure player bird exists

        // Move the enemy towards Flappy Bird on the X-axis
        float step =   Time.deltaTime*enemySpeed; //  speed
        currentEnemy.transform.position = Vector3.MoveTowards(
            currentEnemy.transform.position, 
            new Vector3(currentEnemy.transform.position.x, playerBird.position.y, currentEnemy.transform.position.z), 
            step);
    }

    public void DestroyEnemy()
    {
        if (currentEnemy != null)
        {
            Destroy(currentEnemy);
            currentEnemy = null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // If collides with Flappy Bird
        {
            Debug.Log("Enemy hit Flappy Bird!");
            // Handle collision logic (e.g., reduce health, game over, etc.)
        }
        else if (collision.CompareTag("Laser")) // If hit by laser
        {
            DestroyEnemy(); // Destroy enemy
        }
    }
}
