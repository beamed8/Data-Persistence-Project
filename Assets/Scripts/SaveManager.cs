using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;
    public string currentPlayerName;
    public string playerName;
    public int playerHighscore;
    public InputField nameInput;
    public Text currentHighscore;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        LoadHighscore();
    }

    private void Start()
    {
        SetCurrentHighscoreText();
    }

    public void StartGame()
    {
        SaveName();
        SceneManager.LoadScene("main");
    }

    public void SaveName()
    {
        currentPlayerName = nameInput.text;
    }

    [System.Serializable]
    class SaveData
    {
        public string highScoreName;
        public int highScore;
    }

    public void SaveHighscore()
    {
        SaveData data = new SaveData();
        data.highScore = playerHighscore;
        data.highScoreName = playerName;

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void LoadHighscore()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            playerHighscore = data.highScore;
            playerName = data.highScoreName;

        }
    }

    public void SetCurrentHighscoreText()
    {
        LoadHighscore();
        if (playerHighscore == 0)
        {
            currentHighscore.text = "---";
            return;
        }

        currentHighscore.text = playerName + " - " + playerHighscore;
    }

    public void ResetHighscoreData()
    {
        SaveData data = new SaveData();
        data.highScore = 0;
        data.highScoreName = "";

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
        SetCurrentHighscoreText();
    }
}
