using UnityEngine;

public class FruitController : MonoBehaviour
{
    public int fruitScore = 1000;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager gameManager = UnityEngine.Object.FindFirstObjectByType<GameManager>();
            if (gameManager != null)
            {
                gameManager.AddScore(fruitScore);
            }

            Destroy(gameObject);
        }
    }
}
