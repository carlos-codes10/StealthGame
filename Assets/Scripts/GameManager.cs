using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // refrences
    [SerializeField] HealthSO health;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Time.timeScale = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GameOver()
    {
        // stop time show gameover screen
        // COPY THE NAME BELOW: gameOverUI then uncomment!
        //gameOverUI.SetActive(true);
        Time.timeScale = 0;
    }

    public void Restart()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene("SampleScene");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
