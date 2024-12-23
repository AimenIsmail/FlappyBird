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
    // Start is called before the first frame update
    void Start()
    {
        scoreNumber = 0;
        rb = GetComponent<Rigidbody2D>();
        Time.timeScale = 1;
	    
    }

    // Update is called once per frame
   public void Update()
    {
        Button btn = TapButton.GetComponent<Button>();
       btn.onClick.AddListener(TaskOnClick);
       // inGameScoreText.text = scoreNumber.ToString();
    }
    void TaskOnClick(){
	    birdAnim.Play("BirdFlap");
	    rb.velocity = Vector2.up * velocity;
    }

    private void OnCollisionEnter2D(Collider2D collision)
    {
        gameOverCanvas.SetActive(true);
        Time.timeScale = 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        scoreNumber++;
    }

    public void playAgain()
    {
        SceneManager.LoadScene(0);
    }
}
