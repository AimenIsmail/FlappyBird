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
    //Laser
    public Button LaserButton; 
    public GameObject laserPrefab;
    public Transform laserSpawnPoint;
    public static int NoOfLasers ;
    public TextMeshProUGUI inGameLaserText;
    //laser bar
    public Image LaserBar;
    public float CurrentLasers; 
    public float MaxLasers = 5;
    //shield
    public GameObject shieldPrefab;  

    void Start()
    {
        birdColors = new Color[]
        {
            Color.red,
            Color.green,
            Color.yellow,
        };

        // SpriteRenderer component
        birdSprite = GetComponent<SpriteRenderer>();
        if (birdSprite != null)
        {
            Color randomBirdColor = birdColors[Random.Range(0, birdColors.Length)];
            birdSprite.color = randomBirdColor;
        }

        scoreNumber = 0;
        NoOfLasers = 5;
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
        //Update socre and laser values in UI
        inGameScoreText.text = scoreNumber.ToString();
        inGameLaserText.text = NoOfLasers.ToString();
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
            if(NoOfLasers < 5)
            {
                NoOfLasers++;
                CurrentLasers = NoOfLasers;
                LaserBar.fillAmount = CurrentLasers/MaxLasers;
            }
        }
        else if (collision.CompareTag("GreenApple"))
        {
            // Hide the apple when collected
            collision.gameObject.SetActive(false);

            // Instantiate the shield at the calculated spawn position
            GameObject shld = Instantiate(shieldPrefab, transform.position, Quaternion.identity);
            // Set the shield's parent to the bird so it moves with it
            shld.transform.SetParent(transform);
            // Destroy the shield after some time
            Destroy(shld, 10f);

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
        // Save the original time scale
        float originalTimeScale = Time.timeScale; 
        // Set the new time scale
        Time.timeScale = newSpeed; 
        // Adjust fixed time step to match the new time scale
        Time.fixedDeltaTime = 0.02f * Time.timeScale; 
        // Wait in real time (ignoring the time scale)
        yield return new WaitForSecondsRealtime(duration); 
        // Reset to the original time scale
        Time.timeScale = originalTimeScale; 
        // Restore the fixed time step
        Time.fixedDeltaTime = 0.02f * Time.timeScale; 
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
        // Wait before allowing collisions again
        yield return new WaitForSeconds(0.5f); 
        ignoreCollision = false;
    }

    // Method to shoot the laser
    private void ShootLaser()
    {
        //decrease the laser 
        if (NoOfLasers > 0)
        {
            //Laser and laserBar decreases
            NoOfLasers--;
            CurrentLasers = NoOfLasers;
            LaserBar.fillAmount = CurrentLasers / MaxLasers;

            //bird's position to spawn the laser
            Vector3 spawnPosition = transform.position;

            // Optionally adjust the position slightly ,Offset to the right
            spawnPosition += new Vector3(0.8f, 0f, 0.5f); 

            // Instantiate the laser at the calculated spawn position
            GameObject lsr = Instantiate(laserPrefab, spawnPosition, Quaternion.identity);

            // Destroy the laser after some time
            Destroy(lsr, 0.2f);
        }
    }

    public void playAgain()
    {
        SceneManager.LoadScene(0);
    }
}
