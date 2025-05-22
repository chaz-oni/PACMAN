using UnityEngine;
using UnityEngine.SceneManagement;

public class NewMonoBehaviourScript : MonoBehaviour
{
    private bool Score = false;
    public GameObject scorePanel;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Play();
        }
        else if (Input.GetKeyDown(KeyCode.Tab))
        {
            FetchScore();
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            Exit();
        }
    }

    public void FetchScore()
    {
        Debug.Log("entering score");
        Score = !Score;

        Time.timeScale = Score ? 0f : 1f;

        if (Score != null)
        {
            scorePanel.SetActive(scorePanel);
        }

        if (Input.GetKeyDown(KeyCode.Delete))
        {
            // Disable panel
            Debug.Log("exit score");
        }
    }

    public void Play()
    {
        SceneManager.LoadScene(1);
    }

    public void Exit()
    {
        Application.Quit();

        // Si es que el editor esta abierto, cierralo
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
