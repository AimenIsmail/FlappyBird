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
    public Transform ShootTrans;
    //shield
    public GameObject shieldPrefab; 
    private GameObject activeShield; 
    // Loading Slider
    public Slider loadingSlider;
    public float loadingSpeed = 0.5f; // Adjust speed of filling
    private bool gameStarted = false;
    //Audio
    AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

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

        // Freeze the bird at start
        rb.gravityScale = 0;
        rb.velocity = Vector2.zero; 

        // Add listeners to buttons
        TapButton.onClick.AddListener(TaskOnClick);
        LaserButton.onClick.AddListener(ShootLaser);

        // Start the loading coroutine
        StartCoroutine(FillSliderAndStartGame());
}

    void Update()
    {
        if (!gameStarted) return; // Don't start the game until slider fills

        transform.rotation = Quaternion.Euler(0, 0, 0);
        Button btn = TapButton.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
        //Update socre and laser values in UI
        inGameScoreText.text = scoreNumber.ToString();
        inGameLaserText.text = NoOfLasers.ToString();
        ///shield rotation
        if (activeShield != null)
        {
            activeShield.transform.Rotate(100f * Time.deltaTime, 100f * Time.deltaTime, 100f * Time.deltaTime); 
        }
    }

    IEnumerator FillSliderAndStartGame()
    {
        loadingSlider.value = 0f;

        while (loadingSlider.value < 1)
        {
            loadingSlider.value += loadingSpeed * Time.deltaTime;
            yield return null;
        }

        // Hide slider after loading
        loadingSlider.gameObject.SetActive(false);

        // Enable bird physics after loading
        rb.gravityScale = 0.5f;

        gameStarted = true; // Now allow the game to start
    }
    
    public void TaskOnClick()
    {
        if (!gameStarted) return;

        birdAnim.Play("BirdFlap");
        // audioManager.PlayFlapSound();
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
                audioManager.PlaySFX(audioManager.GameOver);
                gameOverCanvas.SetActive(true);
                Time.timeScale = 0;
            }
        }
    }
     

     ////////////////////Colliders triggering//////////////////
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("RedApple"))
        {
            audioManager.PlaySFX(audioManager.Eat);
            // Hide the apple when collected
            collision.gameObject.SetActive(false);
            if (activeShield == null)
            {
                // Temporarily speed up the entire game
                StartCoroutine(ChangeGameSpeed(1.5f, 15f)); // Increase game speed by 2x for 15 seconds
            }
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
            //Audio voice
            audioManager.PlaySFX(audioManager.Eat);
            // Hide the apple when collected
            collision.gameObject.SetActive(false);

            // If a shield is already active, destroy it first
            if (activeShield != null)
            {
                Destroy(activeShield);
            }

            // Instantiate the shield at the bird's position
            activeShield = Instantiate(shieldPrefab, transform.position, Quaternion.identity);

            // Set the shield's parent to the bird so it moves with it
            activeShield.transform.SetParent(transform);

            // Disable collisions with pipes
            IgnorePipeCollisions(true);

            // Destroy the shield after 10 seconds
            Destroy(activeShield, 6f);

            // Temporarily slow down the game
            StartCoroutine(ChangeGameSpeed(0.5f, 15f));
        }
        else if(collision.CompareTag("Pipe"))
        {
           //Add score passing through pipe
            scoreNumber++;  
        }
        
    }

    ////////////////////////  to change game speed temporarily/////////////////////////////////////
    private IEnumerator ChangeGameSpeed(float newSpeed, float duration)
    {
        // original time scale
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
    //////////////////Bird color chang on passing through same color pipe & ignor collision/////////////////
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

    ///////////////////////////////to shoot the laser spawn laser and manage bar////////////
    private void ShootLaser()
    {
        if (!gameStarted) return;
        //decrease the laser 
        if (NoOfLasers > 0)
        {
            //Laser and laserBar decreases
            NoOfLasers--;
            CurrentLasers = NoOfLasers;
            LaserBar.fillAmount = CurrentLasers / MaxLasers;

            //bird's position to spawn the laser
            Vector3 spawnPosition = transform.position;

            // laser position Offset to the right
            spawnPosition += new Vector3(0.8f, 0f, 0.5f); 

            // Instantiate the laser at position
            GameObject lsr = Instantiate(laserPrefab, ShootTrans.position, ShootTrans.rotation);

            // Destroy the laser after some time
            Destroy(lsr, 0.2f);
        }
    }


    //////////////////For Shield //////////////////////////////
    private void IgnorePipeCollisions(bool ignore)
    {
        GameObject[] pipes = GameObject.FindGameObjectsWithTag("Pipe");

        foreach (GameObject pipe in pipes)
        {
            Collider2D pipeCollider = pipe.GetComponent<Collider2D>();
            Collider2D birdCollider = GetComponent<Collider2D>();

            if (pipeCollider != null && birdCollider != null)
            {
                Physics2D.IgnoreCollision(birdCollider, pipeCollider, ignore);
            }
        }
    }


    public void playAgain()
    {
        SceneManager.LoadScene(1);
    }
    public void Home()
    {
        SceneManager.LoadScene(0);
    }
}
