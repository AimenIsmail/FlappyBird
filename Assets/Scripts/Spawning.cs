using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawning : MonoBehaviour
{
    // Time interval between spawns
    public float maxTime = 1f;
    private float timer = 0f;
    public GameObject pipe;
    public float height;

    // Array of possible colors for the pipes
    public Color[] pipeColors;

    void Start()
    {
        // Initialize the pipe colors
        pipeColors = new Color[]
        {
            Color.red,
            Color.green,
            Color.blue,
            Color.yellow,
        };

        // Spawn a pipe at the start
        SpawnPipe();
    }

    void Update()
    {
        // Update the timer and spawn pipes
        if (timer > maxTime)
        {
            SpawnPipe();
            timer = 0f;
        }
        timer += Time.deltaTime;
    }

    void SpawnPipe()
    {
        // Instantiate a new pipe
        GameObject newpipe = Instantiate(pipe);

        // Set the random position
        newpipe.transform.position = transform.position + new Vector3(0, Random.Range(-height, height), 0);

        // Assign a random color to the pipe
        Renderer[] renderers = newpipe.GetComponentsInChildren<Renderer>();
        if (renderers.Length > 0 && pipeColors.Length > 0)
        {
            Color randomColor = pipeColors[Random.Range(0, pipeColors.Length)];

            foreach (Renderer rend in renderers)
            {
                rend.material.color = randomColor; // Set color for each renderer
            }

            // Store the color in the pipe's tag for reference
            newpipe.tag = randomColor.ToString();
        }

        // Destroy the pipe after some time
        Destroy(newpipe, 15f);
    }
}
