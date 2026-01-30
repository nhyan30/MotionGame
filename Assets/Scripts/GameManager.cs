using System.Collections;
using DG.Tweening;
using GLTFast.Schema;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance { get; private set; }

    [SerializeField] TextMeshProUGUI timeLeftText;
    [SerializeField] GameObject gameCountdown;
    [SerializeField] GameObject gameOver;
    [SerializeField] float totalTime = 10f; // 60 seconds 
    public float GameEndedTimeOut = 5f;
    public Animator ReemyAnimator;
    float elapsedTime = 0f;

    public bool GameStarted { get; private set; } = false;
    public bool isGameEnded { get; private set; } = false;
    public Transform finishLookTarget;
    public CinemachineCamera cam1;
    public CinemachineCamera cam2;
    public CinemachineCamera cam3;

    public ParticleSystem confetti;

    AudioSource audioSource;

    private void Awake()
    {
        Instance = this;
        audioSource = GetComponent<AudioSource>();

        GameStarted = false;

        confetti.Stop();
    }

    private void Update()
    {
        if (!GameStarted) return;

        elapsedTime += Time.deltaTime;

        UpdateVisual();
    }

    public void FinishGame()
    {
        StartCoroutine(GameEnded());
        isGameEnded = true;
    }
    public void StartGame()
    {
        elapsedTime = 0f;
        GameStarted = true;
        Player.Instance.SetRunningState(true);
    }

    public IEnumerator GameEnded()
    {
        audioSource.DOFade(0f, 1f).OnComplete(() =>
        {
            audioSource.Stop();
            confetti.Play();

            Player.Instance.PlayWinSound();
        });

        cam3.Priority = 3;

        yield return new WaitForSeconds(2f);

        Player.Instance.SetRunningState(false);

        yield return new WaitForSeconds(4f);

        MenuUIHandler.Instance.ShowLeaderboard();
        Player.Instance.SaveScores();
        yield return new WaitForSeconds(GameEndedTimeOut);
        SceneManager.LoadScene(0);
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    private void UpdateVisual()
    {
        if(isGameEnded) return;

        //float timeRemaining = Mathf.Clamp(totalTime - elapsedTime, 0, totalTime); // top down Timer
        int minutes = Mathf.FloorToInt(elapsedTime / 60f);
        int seconds = Mathf.FloorToInt(elapsedTime % 60f);

        timeLeftText.text = $"<mspace=0.6em>{minutes:00}<mspace=0.4em>:<mspace=0.6em>{seconds:00}<mspace=0.4em>";
    }
    public void StartGameMusic()
    {
        audioSource.volume = 0f; // start muted
        audioSource.Play();
        audioSource.DOFade(0.3f, 1.5f);
    }
}
