using System.Collections;
using DG.Tweening;
using GLTFast.Schema;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    private const string NUMBER_POPUP = "NumberPopUp";

    [SerializeField] TextMeshProUGUI countdownText;
    [SerializeField] TextMeshProUGUI timeLeftText;
    [SerializeField] GameObject gameCountdown;
    [SerializeField] GameObject gameOver;
    [SerializeField] float totalTime = 10f; // 60 seconds 

    [SerializeField] Animator animator;
    public Animator ReemyAnimator;
    float elapsedTime = 0f;

    public bool GameStarted { get; private set; } = false;

    private void Awake()
    {
        Instance = this;


        Time.timeScale = 0f;  // Pause the game at start
        GameStarted = false;

        animator.updateMode = AnimatorUpdateMode.UnscaledTime; // Animators are affected by Time.timeScale
        ReemyAnimator.updateMode = AnimatorUpdateMode.UnscaledTime;
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

    public IEnumerator StartGameCountdown()
    {
        gameCountdown.gameObject.SetActive(true);

        // Countdown: 3, 2, 1, Go!
        string[] countdown = { "3", "2", "1", "Go!" };


        foreach (string step in countdown)
        {
            countdownText.text = step;
            yield return null; // so Unity proccess UI before firing the animation
            animator.SetTrigger(NUMBER_POPUP);
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
        yield return new WaitForSecondsRealtime(.3f); // unaffected by Time.timScale
        gameOver.SetActive(true);
        Time.timeScale = 0;
        Player.Instance.SaveScores();
        yield return new WaitForSecondsRealtime(5f);
        SceneManager.LoadScene(0);
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    private void UpdateVisual()
    {
        float timeRemaining = Mathf.Clamp(totalTime - elapsedTime, 0, totalTime);
        timeLeftText.text = $"{timeRemaining:F0}";
    }
}
