using UnityEngine;
using UnityEngine.SceneManagement;

public class MainManager : MonoBehaviour
{
    private void Start()
    {
        int randomScore = Random.Range(10, 500);
        DataManager.Instance.AddScoreToHighScores(DataManager.Instance.Name, randomScore, DataManager.Instance.Email, DataManager.Instance.PhoneNumber);

        Debug.Log($"Score: {randomScore} ,Name: {DataManager.Instance.Name}, Phone number: {DataManager.Instance.PhoneNumber}");

        DataManager.Instance.SaveHighScores();
    }

    private void Update()
    {
        // Press "L" to reload scores from file
        if (Input.GetKeyDown(KeyCode.L))
        {
            DataManager.Instance.LoadHighScores();
            Debug.Log("[ScoreTester] Reloaded Scores from File:");
            foreach (var hs in DataManager.Instance.HighScores)
            {
                Debug.Log($"{hs.name} : {hs.score} : {hs.phoneNumber}");
            }
        }

        // Press "C" to clear scores (reset test)
        if (Input.GetKeyDown(KeyCode.C))
        {
            DataManager.Instance.HighScores.Clear();
            DataManager.Instance.SaveHighScores();
            Debug.Log("[ScoreTester] Cleared all high scores.");
        }
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
