using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class movement : MonoBehaviour
{
    public CharacterController controller;

    public float speed = 12f;

    private Vector3 velocity;

    public float gravity = -9.81f;

    public Transform groundCheck;

    public float groundDistance = 0.4f;

    public LayerMask groundMask;

    private bool isGrounded;

    public float jumpHeight = 3f;

    public Slider speedSlider;

    private List<GameObject> targets;
    private int targetIdx;
    
    public Camera fpsCam;
    float range = 100f;

    public Text scoreText;
    public Text maxScoreText;
    
    private int score;

    private int maxScore;

    private float timeRemaining = 10f;

    public GameObject timer;
    // Start is called before the first frame update
    void Start()
    {
        targets = new List<GameObject>();
        targetIdx = 0;
        score = 0;
        maxScore = 0;

        foreach (var obj in GameObject.FindGameObjectsWithTag("Target"))
        {
            targets.Add(obj);
        }

        for (int i = 1; i < targets.Count; i++)
        {
            targets[i].SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            timer.GetComponent<Image>().fillAmount = timeRemaining / 3f;
        }
        else if (timeRemaining <= 0)
        {
            SceneManager.LoadScene("Application");
        }
        
        speedSlider.onValueChanged.AddListener(delegate {valueChanged();});
        
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        if (Input.GetButtonDown("Fire1"))
        {
            OnClicked();
        }
        
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void valueChanged()
    {
        speed = 12f * speedSlider.value;
    }

    void OnClicked()
    {
        RaycastHit hit;

        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            GameObject gameObjectHit = hit.transform.gameObject;

            if (gameObjectHit.CompareTag("Target") && gameObjectHit != null)
            {
                gameObjectHit.SetActive(false);
                score += 10;
                timeRemaining = 10f;
                
                if (score > maxScore)
                {
                    maxScore = score;
                    maxScoreText.text = $"{maxScore}";
                }
                
                scoreText.text = $"{score}";
                
                if (targetIdx + 1 < targets.Count)
                {
                    targetIdx += 1;
                }
                else
                {
                    targetIdx = 0;
                }
                targets[targetIdx].SetActive(true);
            }
        }
    }
}
