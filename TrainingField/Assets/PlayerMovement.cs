using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public Canvas menuCanvas;
    public GameObject menuPanel;

    public Slider speedSlider;

    private const float DefaultSpeed = 12f;

    public float speed = DefaultSpeed;
    public float gravity = -9.81f * 2f;
    public float jumpHeight = 3f;

    private Vector3 velocity;
    private bool isPaused = false;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !isPaused)
        {
            isPaused = true;
            print("Paused");
            //menuCanvas.enabled = true;
            menuPanel.SetActive(true);
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && isPaused)
        {
            isPaused = false;
            print("Resumed");
            //menuCanvas.enabled = false;
            menuPanel.SetActive(false);
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.Locked;
        }

        if (isPaused)
        {
            //SliderUpdater();
            return;
        }

        //Ground Check?

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
}
