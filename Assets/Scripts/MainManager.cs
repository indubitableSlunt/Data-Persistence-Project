using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    private List<SaveDatum> HighScores = new List<SaveDatum>();
    //public string HighScoreName;
    //public int HighScore;

    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public Text txtHighScore;
    public GameObject GameOverText;
    
    private bool m_Started = false;
    public string PlayerName;
    private int m_Points;
    
    private bool m_GameOver = false;

    private const string saveDataFileName = "savefile1.json";
    private const int highScoreCount = 10;

    [System.Serializable]
    class SaveDatum
    {
        public string PlayerName;
        public int HighScore;
    }

    [System.Serializable]
    class SaveData
    {
        public SaveDatum[] data;
    }

    public void SavePlayerData()
    {
        SaveDatum lowestHighScore;
        if (HighScores.Count > 0)
        {
            HighScores = HighScores.OrderByDescending((hs) => hs.HighScore).ToList();
            lowestHighScore = HighScores.Last();
        }
        else
        {
            lowestHighScore = new SaveDatum { HighScore = 0 };
        }

        if (m_Points > lowestHighScore.HighScore || HighScores.Count < highScoreCount)
        {
            SaveDatum datum = new SaveDatum { PlayerName = PlayerName, HighScore = m_Points};


            if (HighScores.Count >= highScoreCount)
            {
                HighScores.RemoveAt(HighScores.Count - 1);
            }

            HighScores.Add(datum);

            SaveData saveData = new SaveData { data = HighScores.ToArray() };
            //data.PlayerName = PlayerName;
            //data.HighScore = m_Points;

            string json = JsonUtility.ToJson(saveData);

            File.WriteAllText(Application.persistentDataPath + $"/{saveDataFileName}", json);
        }
    }

    public bool LoadHighScores()
    {
        string path = Application.persistentDataPath + $"/{saveDataFileName}";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData saveData = JsonUtility.FromJson<SaveData>(json);
            HighScores = saveData.data.ToList();
            HighScores = HighScores.OrderByDescending((hs) => hs.HighScore).ThenBy((hs) => hs.PlayerName).ToList();

            //HighScoreName = data.PlayerName;
            //HighScore = data.HighScore;

            return true;
        }

        return false;
    }


    // Start is called before the first frame update
    void Start()
    {
        PlayerName = MenuManager.Instance.playerName;
        if (LoadHighScores())
        {
            txtHighScore.text = "High Scores" + Environment.NewLine;

            foreach(SaveDatum highScore in HighScores)
            {
                txtHighScore.text += $"   {highScore.PlayerName} : {highScore.HighScore}" + Environment.NewLine; ;
            }
        }


        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = UnityEngine.Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SavePlayerData();
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";
    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);
    }
}
