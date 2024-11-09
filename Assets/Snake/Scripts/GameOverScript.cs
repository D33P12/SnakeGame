using UnityEngine;

public class GameOverScript : MonoBehaviour
{
    [SerializeField] private Canvas gameOverCanvas;
    private void Start()
    {
        gameOverCanvas.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            Time.timeScale = 0;
            gameOverCanvas.gameObject.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
