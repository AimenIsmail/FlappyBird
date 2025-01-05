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
        pipeColors = new Color[]
        {
            Color.red,
            Color.green,
            Color.blue,
            Color.yellow,
        };
        // Call spawn on start
        SpawnPipe();
    }

    void Update()
    {
        // Update timer and spawn pipe when needed
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

        // Find all Renderers in the pipe object (assuming both pipes are children of the instantiated object)
        Renderer[] renderers = newpipe.GetComponentsInChildren<Renderer>();

         if (renderers.Length > 0 && pipeColors.Length > 0)
        {
            Color randomColor = pipeColors[Random.Range(0, pipeColors.Length)];
            foreach (Renderer rend in renderers)
            {
                rend.material.color = randomColor;
            }
        }
        // Destroy pipe after some time (15 seconds for example)
        Destroy(newpipe, 15f);
    }
}