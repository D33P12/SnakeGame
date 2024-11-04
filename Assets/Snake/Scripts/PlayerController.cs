using System;
using UnityEngine;
using System.Collections.Generic;
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
    
    private List<GameObject> bodyParts = new List<GameObject>();
    private List<Vector3> positionsHistory = new List<Vector3>();
    private Vector2 currentMoveInput; 

    private void Start()
    {
        positionsHistory.Add(playerTransform.position);
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
    private void MoveBodyParts()
    {
        for (int i = 0; i < bodyParts.Count; i++)
        {
            int historyIndex = Mathf.Clamp(i * gap, 0, positionsHistory.Count - 1);
            Vector3 targetPosition = positionsHistory[historyIndex];

            bodyParts[i].transform.position = Vector3.Lerp(bodyParts[i].transform.position, targetPosition, bodySpeed * Time.deltaTime);
        }
    }
    public void GrowSnake()
    {
        GameObject body = Instantiate(bodyPrefab);

        if (bodyParts.Count > 0)
        {
            Vector3 lastBodyPosition = bodyParts[bodyParts.Count - 1].transform.position;

            Vector3 directionToNewPart = (lastBodyPosition - playerTransform.position).normalized;

            body.transform.position = lastBodyPosition + directionToNewPart * gap;
        }
        else
        {
            body.transform.position = playerTransform.position;
        }
        bodyParts.Add(body);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Food"))
        {
            GrowSnake();
            GameManager.food++;
            Destroy(other.gameObject);
        }
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true; 
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
