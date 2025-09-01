using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuUIHandler : MonoBehaviour
{
    [SerializeField] private TMP_InputField nameInput;
    [SerializeField] private TMP_InputField emailInput;
    [SerializeField] private TMP_InputField phoneNumberInput;
    [SerializeField] private Button startButton;
    [SerializeField] private TMP_Text highScoreListNames;
    [SerializeField] private TMP_Text highScoreListScores;

    void Start()
    {
        List<DataManager.HighScore> highScores = DataManager.Instance.HighScores;
        FillInHighScoreText();
        nameInput.onValueChanged.AddListener(OnNameInputChanged);
        phoneNumberInput.onValueChanged.AddListener(OnNumberInputChanged);
        startButton.interactable = false;
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
        SceneManager.LoadScene(1);
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
