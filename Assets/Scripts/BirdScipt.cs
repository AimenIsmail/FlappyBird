using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BirdScipt : MonoBehaviour
{
    public float velocity = 10;
    private Rigidbody2D rb;
    public static int scoreNumber = 0;
    public TextMeshProUGUI inGameScoreText;
    public GameObject gameOverCanvas;
    public Animator birdAnim;
    public Button TapButton;
    public Color[] birdColors;
    private SpriteRenderer birdSprite;
    private bool ignoreCollision = false; 
    public Button LaserButton; 
    public GameObject laserPrefab;
    public Transform laserSpawnPoint;
    // public static int NoOfLasers = 5;
    // public TextMeshProUGUI inGameLaserText;

    void Start()
    {
        birdColors = new Color[]
        {
            Color.red,
            Color.green,
            Color.yellow,
        };

        //  SpriteRenderer component
        birdSprite = GetComponent<SpriteRenderer>();
        if (birdSprite != null)
        {
            Color randomBirdColor = birdColors[Random.Range(0, birdColors.Length)];
            birdSprite.color = randomBirdColor;
        }

        scoreNumber = 0;
        rb = GetComponent<Rigidbody2D>();
        Time.timeScale = 1;
        
        // Add listeners to the buttons
        TapButton.onClick.AddListener(TaskOnClick);
        LaserButton.onClick.AddListener(ShootLaser);
    }

    void Update()
    {
        Button btn = TapButton.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
        inGameScoreText.text = scoreNumber.ToString();
        // inGameLaserText.text = NoOfLasers.ToString();
    }

    public void TaskOnClick()
    {
        birdAnim.Play("BirdFlap");
        rb.velocity = Vector2.up * velocity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        Renderer pipeRenderer = collision.gameObject.GetComponent<Renderer>();
        if (pipeRenderer != null)
        {
            Color pipeColor = pipeRenderer.material.color;

            if (birdSprite.color == pipeColor)
            {
                // Ignore collision and change bird color
                Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>());
                ignoreCollision = true; 
                ChangeBirdColor();
                StartCoroutine(ResetIgnoreCollision()); 
            }
            else
            {
                // End the game if colors don't match
                gameOverCanvas.SetActive(true);
                Time.timeScale = 0;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("RedApple"))
        {
            // Hide the apple when collected
            collision.gameObject.SetActive(false);
            // Temporarily speed up the entire game
            StartCoroutine(ChangeGameSpeed(1.5f, 15f)); // Increase game speed by 2x for 15 seconds
            //increase the laser 
            // NoOfLasers = NoOfLasers++;
        }
        else if (collision.CompareTag("GreenApple"))
        {
            // Hide the apple when collected
            collision.gameObject.SetActive(false);
            // Temporarily speed up the entire game
            StartCoroutine(ChangeGameSpeed(0.5f, 15f)); // Increase game speed by 0.5 for 15 seconds
        }
        else if (collision.CompareTag("Pipe"))
        {
           //Add score passing through pipe
            scoreNumber++;  
        }
        
    }

    // Coroutine to change game speed temporarily
    private IEnumerator ChangeGameSpeed(float newSpeed, float duration)
    {
        float originalTimeScale = Time.timeScale; // Save the original time scale
        Time.timeScale = newSpeed; // Set the new time scale
        Time.fixedDeltaTime = 0.02f * Time.timeScale; // Adjust fixed time step to match the new time scale

        yield return new WaitForSecondsRealtime(duration); // Wait in real time (ignoring the time scale)

        Time.timeScale = originalTimeScale; // Reset to the original time scale
        Time.fixedDeltaTime = 0.02f * Time.timeScale; // Restore the fixed time step
    }

    private void ChangeBirdColor()
    {
        if (birdSprite != null && birdColors.Length > 0)
        {
            Color newColor = birdColors[Random.Range(0, birdColors.Length)];
            birdSprite.color = newColor;
        }
    }

    private IEnumerator ResetIgnoreCollision()
    {
        yield return new WaitForSeconds(0.5f); // Wait before allowing collisions again
        ignoreCollision = false;
    }

    // Method to shoot the laser
    private void ShootLaser()
    {
        //decrease the laser 
        // NoOfLasers = NoOfLasers--;
        // Use the bird's position to spawn the laser
        Vector3 spawnPosition = transform.position;

        // Optionally adjust the position slightly (e.g., move it slightly forward relative to the bird)
        spawnPosition += new Vector3(2f, 0, 0); // Offset to the right

        // Instantiate the laser at the calculated spawn position
        GameObject lsr = Instantiate(laserPrefab, spawnPosition, Quaternion.identity);

        // Destroy the laser after some time
        Destroy(lsr, 0.2f);
    }

    public void playAgain()
    {
        SceneManager.LoadScene(0);
    }
}
