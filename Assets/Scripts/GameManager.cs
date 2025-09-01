using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] TextMeshProUGUI countdownText;
    [SerializeField] TextMeshProUGUI timeLeftText;
    [SerializeField] GameObject gameCountdown;
    [SerializeField] GameObject gameOver;
    [SerializeField] float totalTime = 10f; // 60 seconds 
    float elapsedTime = 0f;

    public bool GameStarted { get; private set; } = false;


    private void Awake()
    {
        Instance = this;

        gameCountdown.gameObject.SetActive(true);

        Time.timeScale = 0f;  // Pause the game at start
        GameStarted = false;
    }

    private void Start()
    {
        StartCoroutine(StartGameCountdown());
    }
    private void Update()
    {
        if (!GameStarted) return;

        elapsedTime += Time.deltaTime;

        UpdateVisual();

        if (elapsedTime >= totalTime)
        {
            StartCoroutine (GameEnded());
            GameStarted = false;
        }
        
    }

    IEnumerator StartGameCountdown()
    {
        // Countdown: 3, 2, 1, Go!
        string[] countdown = { "3", "2", "1", "Go!" };
        yield return new WaitForSecondsRealtime(1f);
        foreach (string step in countdown)
        {
            countdownText.text = step;
            yield return new WaitForSecondsRealtime(1f);
        }

        gameCountdown.gameObject.SetActive(false);

        StartGame();  // Proceed to start game
    }

    private void StartGame()
    {
        elapsedTime = 0f;
        GameStarted = true;
        Time.timeScale = 1f;
    }

    public IEnumerator GameEnded()
    {
        yield return new WaitForSeconds(.4f);
        gameOver.SetActive(true);
        Time.timeScale = 0;
        Player.Instance.SaveScores();
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    private void UpdateVisual()
    {
        float timeRemaining = Mathf.Clamp(totalTime - elapsedTime, 0, totalTime);
        timeLeftText.text = $"Time left: {timeRemaining:F2}";
    }
}
