using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;

    public string Name;
    public int PhoneNumber;
    public string Email;

    public List<HighScore> HighScores;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadHighScores();
    }

    public class HighScore
    {
        public HighScore(string name, int score, string email, int phoneNumber)
        {
            this.name = name;
            this.score = score;
            this.email = email;
            this.phoneNumber = phoneNumber;
        }
        public string name { get; set; }
        public int score { get; set; }
        public string email { get; set; }
        public int phoneNumber { get; set; }
    }

    [System.Serializable]
    class HighScoreData
    {
        public HighScoreData()
        {
            Names = new List<string>();
            Scores = new List<int>();
            Emails = new List<string>();
            highScorePhoneNumber = new List<int>();
        }
        public List<string> Names;
        public List<int> Scores;
        public List<string> Emails;
        public List<int> highScorePhoneNumber;
    }

    public void SaveHighScores()
    {
        HighScoreData data = new HighScoreData();
        foreach (HighScore highScore in HighScores)
        {
            data.Names.Add(highScore.name);
            data.Scores.Add(highScore.score);
            data.Emails.Add(highScore.email);
            data.highScorePhoneNumber.Add(highScore.phoneNumber);
        }
        string json = JsonUtility.ToJson(data);
        string path = Application.persistentDataPath + "/savefile.json";    
        Debug.Log(path);
        File.WriteAllText(path, json);
    }

    public void LoadHighScores()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            HighScoreData data = JsonUtility.FromJson<HighScoreData>(json);
            if (data.Names.Count != data.Scores.Count)
            {
                Debug.LogError("Mismatch between the number of high score names and the number of high score values");
            }
            HighScores = new List<HighScore>();
            for (int index = 0; index < data.Names.Count; ++index)
            {
                HighScores.Add(new HighScore(data.Names[index], data.Scores[index], data.Emails[index], data.highScorePhoneNumber[index]));
            }
        }
        else
        {
            HighScores = new List<HighScore>();
        }

        //while (HighScores.Count < 5)
        //{
        //    HighScores.Add(new HighScore("Player", 0, "Email", -1));
        //}
    }

    public void AddScoreToHighScores(string name, int score, string email, int phoneNumber)
    {
        HighScores.Add(new HighScore(name, score, email, phoneNumber));

        //// Tries to add this score to the high scores list. If it is not actually a high score (it doesn't beat the top 5), it does not get added.
        //for (int index = 0; index < HighScores.Count; ++index)
        //{
        //    if (score > HighScores[index].score)
        //    {
        //        HighScores.Insert(index, new HighScore(name, score, email, phoneNumber));
        //        if (HighScores.Count > 5)
        //        {
        //            HighScores.RemoveAt(HighScores.Count - 1); // Remove last item from high scores; we only show the top 5
        //        }
        //        break;
        //    }
        //}
    }
}
