using UnityEngine;
using System.Collections.Generic;
public class PlayerController : MonoBehaviour
{
    [SerializeField] private InputManager inputManager;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Rigidbody playerRigidbody;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float steerSpeed = 180f;
    [SerializeField] private float bodySpeed = 5f;
    [SerializeField] private int gap = 10;
    [SerializeField] private GameObject bodyPrefab;
    [SerializeField] private int maxHistorySize = 100;

    private List<GameObject> bodyParts = new List<GameObject>();
    private List<Vector3> positionsHistory = new List<Vector3>();
    private Vector2 currentMoveInput; 

    private void Start()
    {
        // Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnEnable()
    {
        inputManager.onMove += OnMove;
    }

    private void OnDisable()
    {
        inputManager.onMove -= OnMove;
    }

    private void FixedUpdate()
    {
        HandleMovement();
        transform.position += transform.forward * (moveSpeed * Time.deltaTime);
    }

    private void OnMove(Vector2 inputValue)
    {
        currentMoveInput = inputValue; 

        positionsHistory.Insert(0, transform.position);

        if (positionsHistory.Count > maxHistorySize)
        {
            positionsHistory.RemoveAt(positionsHistory.Count - 1);
        }

        int index = 0;
        foreach (var body in bodyParts)
        {
            Vector3 point = positionsHistory[Mathf.Clamp(index * gap, 0, positionsHistory.Count - 1)];
            Vector3 moveDirection = point - body.transform.position;
            body.transform.position += moveDirection * (bodySpeed * Time.deltaTime);
            body.transform.LookAt(point);

            index++;
        }
    }

    private void HandleMovement()
    {
        float forwardMovement = currentMoveInput.y * moveSpeed * Time.deltaTime;
        transform.position += transform.forward * forwardMovement;

        float steerDirection = currentMoveInput.x;
        transform.Rotate(Vector3.up * (steerDirection * steerSpeed * Time.deltaTime));
    }

    public void GrowSnake()
    {
        GameObject body = Instantiate(bodyPrefab);
        if (bodyParts.Count > 0)
        {
            body.transform.position = bodyParts[bodyParts.Count - 1].transform.position;
        }
        else
        {
            body.transform.position = transform.position;
        }
        bodyParts.Add(body);
    }
}
