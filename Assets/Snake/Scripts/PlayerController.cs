using System;
using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private InputManager inputManager;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Rigidbody playerRigidbody;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float steerSpeed = 180f;
    [SerializeField] private float bodySpeed = 5f;
    [SerializeField] private int gap = 10;
    [SerializeField] private GameObject bodyPrefab;
    [SerializeField] private int maxHistorySize = 100;
    [SerializeField] private float jumpForce = 5f; 
    [SerializeField] private bool isGrounded;
    [SerializeField] private ParticleSystem particleEffect;
    [SerializeField] private ParticleSystem particleEffect1;
    [SerializeField] private float Scaletime;
    [SerializeField] private Ease Easebody;
    [SerializeField] public TextMeshProUGUI CollectedText;
    [SerializeField] private FoodSpawner foodSpawner;
    
    private List<GameObject> bodyParts = new List<GameObject>();
    private List<Vector3> positionsHistory = new List<Vector3>();
    private Vector2 currentMoveInput; 
    private List<Quaternion> rotationsHistory = new List<Quaternion>();
    public int maxRotationHistorySize = 10;
    
    [SerializeField] private GameObject TailPart;
  
    private void Start()
    {
        positionsHistory.Add(playerTransform.position);
        GameObject body = Instantiate(TailPart, playerTransform.position, Quaternion.identity);
        bodyParts.Insert(0, body);
    }
    private void OnEnable()
    {
        inputManager.onMove += OnMove;
        inputManager.onJump += Onjump;
    }
    private void OnDisable()
    {
        inputManager.onMove -= OnMove;
        inputManager.onJump -= Onjump;
    }
    private void FixedUpdate()
    {
        HandleMovement();
        transform.position += transform.forward * (moveSpeed * Time.deltaTime);
        UpdatePositionsHistory();
        UpdateRotationsHistory();
        MoveBodyParts();
    }
    private void Onjump(bool isJumping)
    {
        if (isJumping && isGrounded)
        {
            Jump();
        }
    }
    private void Jump()
    {
        playerRigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        isGrounded = false;
        
        particleEffect.Play();
        particleEffect1.Play();
    }
    private void OnMove(Vector2 inputValue)
    {
        currentMoveInput = inputValue; 
    }
    private void HandleMovement()
    {
        float forwardMovement = currentMoveInput.y * moveSpeed * Time.deltaTime;
        transform.position += transform.forward * forwardMovement;

        float steerDirection = currentMoveInput.x;
        transform.Rotate(Vector3.up * (steerDirection * steerSpeed * Time.deltaTime));
    }
    private void UpdatePositionsHistory()
    {
        positionsHistory.Insert(0, transform.position);

        if (positionsHistory.Count > maxHistorySize)
            positionsHistory.RemoveAt(positionsHistory.Count - 1);
    }
    private void UpdateRotationsHistory()
    {
        rotationsHistory.Insert(0, transform.rotation);

        if (rotationsHistory.Count > maxRotationHistorySize)
            rotationsHistory.RemoveAt(rotationsHistory.Count - 1);
    }
    private void MoveBodyParts()
    {
        for (int i = 0; i < bodyParts.Count; i++)
        {
            int historyIndex = Mathf.Clamp(i * gap, 0, positionsHistory.Count - 1);
        
            Vector3 targetPosition = positionsHistory[historyIndex];
            Quaternion targetRotation = rotationsHistory[historyIndex];

            bodyParts[i].transform.position = Vector3.Lerp(bodyParts[i].transform.position,
                targetPosition, bodySpeed * Time.deltaTime);
            bodyParts[i].transform.rotation = Quaternion.Lerp(bodyParts[i].transform.rotation, 
                targetRotation, bodySpeed * Time.deltaTime);
        }
    }

    private void GrowSnake()
    {
        GameObject body = Instantiate(bodyPrefab, playerTransform.position, Quaternion.identity);
        bodyParts.Insert(0, body);
        body.transform.DOScale(Vector3.one,Scaletime).SetEase(Easebody);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Food"))
        {
            GrowSnake();
            GameManager.food++;
            Destroy(other.gameObject);

            if (CollectedText != null)
            {
                CollectedText.text = "SCORE: " + GameManager.food.ToString();
            }

            GameObject foodSpawner = GameObject.Find("FoodSpawner");
            if (foodSpawner != null)
            {
                foodSpawner.GetComponent<FoodSpawner>().SpawnFood();
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true; 
            if (particleEffect != null&& particleEffect1 != null && particleEffect.isPlaying)
            {
                particleEffect.Stop();
                particleEffect1.Stop();
            }
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false; 
            
          
        }
    }
}
