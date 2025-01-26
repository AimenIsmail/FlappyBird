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
    }

    void Update()
    {
        Button btn = TapButton.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
        inGameScoreText.text = scoreNumber.ToString();
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
        }
        else if (collision.CompareTag("GreenApple"))
        {
            // Hide the apple when collected
            collision.gameObject.SetActive(false);  
        }
        else if (collision.CompareTag("Pipe"))
        {
           //Add score passing through pipe
            scoreNumber++;  
        }
        
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

    public void playAgain()
    {
        SceneManager.LoadScene(0);
    }
}
