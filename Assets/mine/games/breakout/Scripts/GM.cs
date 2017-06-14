using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;

public class GM : MonoBehaviour
{
    public int lives = 3;
    public int bricks = 20;
    public float resetDelay = 1f;
    public Text livesText;
    public Slider ballSpeedSlider;
    public Text ballSpeedSlider_label;

    public GameObject gameOver;
    public GameObject youWon;
    public GameObject bricksPrefab;
    public GameObject paddle;
    public GameObject deathParticles;

    public static GM instance = null;

    private GameObject clonePaddle;

    // Use this for initialization
    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        Setup();
    }

    public void Setup()
    {
        clonePaddle = Instantiate(paddle, transform.position, Quaternion.identity) as GameObject;
        Instantiate(bricksPrefab, transform.position, Quaternion.identity);

        bricks = bricksPrefab.transform.childCount;

        livesText.text = string.Format("Lives: {0}", lives);
    }

    void CheckGameOver()
    {
        if (bricks < 1)
        {
            youWon.SetActive(true);
            Time.timeScale = .25f;
            Invoke("Reset", resetDelay);
        }

        if (lives < 1)
        {
            gameOver.SetActive(true);
            Time.timeScale = .25f;
            Invoke("Reset", resetDelay);
        }
    }

    void Reset()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        //Application.LoadLevel(Application.loadedLevel);
    }

    public void LoseLife()
    {
        lives--;
        livesText.text = string.Format("Lives: {0}", lives);
        Instantiate(deathParticles, clonePaddle.transform.position, Quaternion.identity);

        Destroy(clonePaddle);

        Invoke("SetupPaddle", resetDelay);

        CheckGameOver();
    }

    void SetupPaddle()
    {
        clonePaddle = Instantiate(paddle, transform.position, Quaternion.identity) as GameObject;
    }

    public void DestroyBrick()
    {
        bricks--;
        CheckGameOver();
    }

    public void Update()
    {
        if (clonePaddle != null && clonePaddle.GetComponentInChildren<Ball>() != null)
        {
            //GameObject.Find("Ball").GetComponent<Ball>().ballInitialVelocity = ballSpeedSlider.value;
            clonePaddle.GetComponentInChildren<Ball>().ballInitialVelocity = ballSpeedSlider.value;
            ballSpeedSlider_label.text = "Ball Speed: " + ballSpeedSlider.value;
        }
    }
}