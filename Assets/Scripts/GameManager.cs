using System.Collections;
using DG.Tweening;
using GLTFast.Schema;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] TextMeshProUGUI timeLeftText;
    [SerializeField] GameObject gameCountdown;
    [SerializeField] GameObject gameOver;
    [SerializeField] float totalTime = 10f; // 60 seconds 
    public float GameEndedTimeOut=5;
    public Animator ReemyAnimator;
    float elapsedTime = 0f;

    public bool GameStarted { get; private set; } = false;

    private void Awake()
    {
        Instance = this;

        GameStarted = false;
    }

    private void Update()
    {
        if (!GameStarted) return;

        elapsedTime += Time.deltaTime;

        UpdateVisual();

        if (elapsedTime >= totalTime)
        {
            StartCoroutine(GameEnded());
            GameStarted = false;
        }
    }
    public void StartGame()
    {
        elapsedTime = 0f;
        GameStarted = true;
        Player.Instance.SetRunningState(true);
    }

    public IEnumerator GameEnded()
    {
        MenuUIHandler.Instance.ShowLeaderboard();
        Player.Instance.SetRunningState(false);
        yield return new WaitForSecondsRealtime(.3f); // unaffected by Time.timScale
        gameOver.SetActive(true);
        Player.Instance.SaveScores();
        yield return new WaitForSecondsRealtime(GameEndedTimeOut);
        SceneManager.LoadScene(0);
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    private void UpdateVisual()
    {
        //float timeRemaining = Mathf.Clamp(totalTime - elapsedTime, 0, totalTime); // top down Timer

        int minutes = Mathf.FloorToInt(elapsedTime / 60f);
        int seconds = Mathf.FloorToInt(elapsedTime % 60f);

        timeLeftText.text = $"{minutes:00}:{seconds:00}";
    }
}
