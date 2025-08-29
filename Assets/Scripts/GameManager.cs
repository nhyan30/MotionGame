using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

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
