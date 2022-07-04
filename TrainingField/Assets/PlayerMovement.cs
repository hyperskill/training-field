using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public Canvas menuCanvas;
    public GameObject menuPanel;

    public Text scoreText;
    public Text maxScoreText;

    public Slider speedSlider;

    private const float totalTime = 2.8f;
    private const float DefaultSpeed = 12f;

    public float speed = DefaultSpeed;
    public float gravity = -9.81f * 2f;
    public float jumpHeight = 3f;

    private Vector3 velocity;
    private bool isPaused = false;
    private float numSecs = totalTime;
    public Image TimerImg;

    public int score;

    // Start is called before the first frame update
    void Start()
    {
        StopAllCoroutines();
        score = 0;
        StartCoroutine(TimerTick());
    }
    IEnumerator TimerTick()
    {
        while (numSecs > 0)
        {
            numSecs -= 0.01f;
            yield return new WaitForSeconds(.01f);
        }

        SceneManager.LoadScene("Application");
        scoreText.text = "0";
    }

    // Update is called once per frame
    void Update()
    {
        TimerImg.fillAmount = (numSecs / totalTime) > 0f ? numSecs / totalTime : 0f;

        if (Input.GetKeyDown(KeyCode.Escape) && !isPaused)
        {
            isPaused = true;
            menuPanel.SetActive(true);
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            if (PlayerPrefs.HasKey("maxScore"))
            {
                var oldmax = PlayerPrefs.GetInt("maxScore");
                maxScoreText.text = oldmax.ToString();
            }
            else
            {
                maxScoreText.text = "0";
            }
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && isPaused)
        {
            isPaused = false;
            menuPanel.SetActive(false);
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.Locked;
        }

        if (isPaused)
        {
            return;
        }
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        var transform1 = transform;
        Vector3 move = transform1.right * x + transform1.forward * z;

        controller.Move(move * (speed * Time.deltaTime));

        if (Input.GetButtonDown("Jump"))
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    private void SliderUpdater()
    {
        speed = DefaultSpeed * speedSlider.value;
    }

    public void SpeedSliderListener(System.Single speedSlideVal)
    {
        speed = DefaultSpeed * speedSlider.value;
        print("Slider updated.");
    }
    public void IncrementScore()
    {
        score += 10;
        scoreText.text = score.ToString();
        numSecs = totalTime;

        if (PlayerPrefs.HasKey("maxScore"))
        {
            var oldmax = PlayerPrefs.GetInt("maxScore");
            if (score > oldmax)
            {
                PlayerPrefs.SetInt("maxScore", score);
            }
        }
        else
        {
            PlayerPrefs.SetInt("maxScore", score);
        }
    }
    public void IncrementScore(int points)
    {
        score += points;
        scoreText.text = score.ToString();
        numSecs = totalTime;

        if (PlayerPrefs.HasKey("maxScore"))
        {
            var oldmax = PlayerPrefs.GetInt("maxScore");
            if (score > oldmax)
            {
                PlayerPrefs.SetInt("maxScore", score);
            }
        }
        else
        {
            PlayerPrefs.SetInt("maxScore", score);
        }
    }
}
