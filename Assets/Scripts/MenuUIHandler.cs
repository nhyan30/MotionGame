using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MenuUIHandler : MonoBehaviour
{
    public static MenuUIHandler Instance;
    [SerializeField] private TMP_InputField nameInput;
    [SerializeField] private TMP_InputField emailInput;
    [SerializeField] private TMP_InputField phoneNumberInput;
    [SerializeField] private Button startButton;
    [SerializeField] private TMP_Text highScoreListNames;
    [SerializeField] private TMP_Text highScoreListScores;
    [SerializeField] private GameObject menuList;
    [SerializeField] private GameObject touchToStart;

    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI gameOverText;

    bool touched = false;


    public CanvasGroup IdleUI;
    public CanvasGroup RegistrationUI;
    public CanvasGroup CountdownUI;
    public CanvasGroup GameplayUI;
    public CanvasGroup GameOverUI;

    private const string NUMBER_POPUP = "NumberPopUp";
    [SerializeField] Animator countdownAnimator;
    [SerializeField] TextMeshProUGUI countdownText;

    private void Awake()
    {
        Instance = this;
        Fade(IdleUI, true);
    }
    void Start()
    {
        List<DataManager.HighScore> highScores = DataManager.Instance.HighScores;
        FillInHighScoreText();
        nameInput.onValueChanged.AddListener(OnNameInputChanged);
        phoneNumberInput.onValueChanged.AddListener(OnNumberInputChanged);
        startButton.interactable = false;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && touched == false)
        {
            touched = true;
            Fade(IdleUI, false, () => Fade(RegistrationUI, true));
        }

        DataManager.Instance.LoadHighScores();
        FillInHighScoreText();
    }

    public void UpdateVisual(int coinCollected)
    {
        scoreText.text = $"{coinCollected}";
        gameOverText.text = $"Your Score : {coinCollected}";
    }

    void OnNameInputChanged(string nameInput)
    {
        // Make the start button active only if the name field is not blank (or just whitespace)
        startButton.interactable = !string.IsNullOrWhiteSpace(nameInput);
    }
    void OnNumberInputChanged(string phoneNumberInput)
    {
        // Make the start button active only if the name field is not blank (or just whitespace)
        startButton.interactable = !string.IsNullOrWhiteSpace(phoneNumberInput);
    }

    void FillInHighScoreText()
    {
        List<DataManager.HighScore> highScores = DataManager.Instance.HighScores;
        string names = "";
        string scores = "";
        int index = 1;
        foreach (DataManager.HighScore highScoreData in highScores)
        {
            names += $"{index.ToString()}. {highScoreData.name}\n";
            scores += $"{highScoreData.score.ToString()}\n";
            ++index;
        }
        highScoreListNames.text = names;
        highScoreListScores.text = scores;
    }

    public void StartGame()
    {
        string name = nameInput.text;  // Gets the TextMeshPro input field for the name, the only input field on the canvas
        string email = emailInput.text;
        DataManager.Instance.Email = email.Trim();
        DataManager.Instance.Name = name.Trim();
        if (int.TryParse(phoneNumberInput.text, out int phone))
        {
            DataManager.Instance.PhoneNumber = phone;
        }
        else
        {
            DataManager.Instance.PhoneNumber = 0; // fallback if input is invalid
        }
        //SceneManager.LoadScene(1);
        Fade(RegistrationUI, false, () =>
        {
            StartCoroutine(StartGameCountdown());
        });

    }
    public IEnumerator StartGameCountdown()
    {
        Fade(CountdownUI, true);

        // Countdown: 3, 2, 1, Go!
        string[] countdown = { "3", "2", "1", "Go!" };


        foreach (string step in countdown)
        {
            countdownText.text = step;
            yield return null; // so Unity proccess UI before firing the animation
            countdownAnimator.SetTrigger(NUMBER_POPUP);
            yield return new WaitForSecondsRealtime(1f);
        }
        CountdownUI.alpha = 0;
        GameManager.Instance.StartGame();
    }
    public void ClearData()
    {
        DataManager.Instance.HighScores.Clear();
        DataManager.Instance.SaveHighScores();
        Debug.Log("[ScoreTester] Cleared all high scores.");
    }

    public void ListData()
    {
        DataManager.Instance.LoadHighScores();
        Debug.Log("[ScoreTester] Reloaded Scores from File:");
        foreach (var hs in DataManager.Instance.HighScores)
        {
            Debug.Log($"{hs.name} : {hs.score} : {hs.email} :  {hs.phoneNumber}");
        }
    }
    void Fade(CanvasGroup canvasGroup, bool visible, UnityAction callback = null)
    {
        canvasGroup.blocksRaycasts = false;
        canvasGroup.DOFade(visible ? 1 : 0, 0.3f).SetEase(Ease.InOutQuad)
            .OnComplete(() =>
            {
                if (visible)
                    canvasGroup.blocksRaycasts = true;
                callback?.Invoke();
            });
    }
    public void Quit()
    {
        DataManager.Instance.SaveHighScores();
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }

}
