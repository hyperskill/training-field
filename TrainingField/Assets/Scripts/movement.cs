using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

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

    private float timeRemaining = 3f;

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
                Bounds bounds = gameObjectHit.GetComponent<Collider>().bounds;

                float radius = 0;
                
                if (bounds.size.x > bounds.size.y && bounds.size.z > bounds.size.y)
                {
                    radius = bounds.size.x/2;
                }else if (bounds.size.y > bounds.size.x && bounds.size.z > bounds.size.x)
                {
                    radius = bounds.size.z/2;
                }else if (bounds.size.y > bounds.size.z && bounds.size.x > bounds.size.z)
                {
                    radius = bounds.size.x/2;
                }
                
                RaycastHit[] hitArray = Physics.RaycastAll(Camera.main.transform.position, Camera.main.transform.forward);
                RaycastHit exact = Array.Find(hitArray, e => e.collider.gameObject.layer == LayerMask.NameToLayer("Target"));
                
                float dist = Vector3.Distance(exact.point, bounds.center);
                
                int points;
                
                if (dist > radius)
                {
                    points = 1;
                }else if(dist/radius>2f/3f)
                {
                    points = 3;
                }else if(dist/radius>1f/6f)
                {
                    points = 5;
                }else
                {
                    points = 10;
                }
                
                score += points;
                timeRemaining = 3f;
                
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
                
                gameObjectHit.SetActive(false);
                targets[targetIdx].SetActive(true);
            }
        }
    }
}
