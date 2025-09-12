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
    public float GameEndedTimeOut=5;
    public Animator ReemyAnimator;
    float elapsedTime = 0f;

    public bool GameStarted { get; private set; } = false;
    public bool isGameEnded { get; private set; } = false;
    public Transform finishLookTarget;
    public CinemachineCamera cam1;
    public CinemachineCamera cam2;

    private void Awake()
    {
        Instance = this;

        GameStarted = false;
    }

    private void Start()
    {
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
        cam1.Priority = 2;

        elapsedTime = 0f;
        GameStarted = true;
        Player.Instance.SetRunningState(true);
    }

    public IEnumerator GameEnded()
    {
        yield return new WaitForSeconds(2f);

        Player.Instance.SetRunningState(false);

        //Player.Instance.transform
        //    .DOLookAt(finishLookTarget.position, .3f) 
        //    .OnComplete(() =>
        //    {
        //        Player.Instance.SetWinningState(true);
        //    });



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

        timeLeftText.text = $"<mspace=0.6em>{minutes:00}:{seconds:00}";
    }
}
