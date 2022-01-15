using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataStorage
{
    public static Highscores FetchHighscores()
    {
        string jsonString = PlayerPrefs.GetString("highscoreTable");
        return JsonUtility.FromJson<Highscores>(jsonString);
    }
    public static bool CheckIfOnTop5(int score)
    {
        Highscores highscores = FetchHighscores();

        Debug.Log("> Score - " + score);

        if (highscores == null)
        {
            Debug.Log("> highscores - null");
            return true;
        }

        for(int i = highscores.highscoreEntryList.Count - 1; i >= 0; i--)
        {
            if(highscores.highscoreEntryList[i].score < score)
            {
                Debug.Log("> highscores[i] -> " + highscores.highscoreEntryList[i].score);
                RemoveHighscoreEntry(i);
                return true;
            }
        }

        Debug.Log("> return false");
        return false;
    }
    public static void AddHighscoreEntry(int score, string name)
    {
        // Create HighscoreEntry
        HighscoreEntry highscoreEntry = new HighscoreEntry { score = score, name = name };

        // Load saved Highscores
        Highscores highscores = FetchHighscores();

        if (highscores == null)
        {
            // There's no stored table, initialize
            highscores = new Highscores()
            {
                highscoreEntryList = new List<HighscoreEntry>()
            };
        }

        // Add new entry to Highscores
        highscores.highscoreEntryList.Add(highscoreEntry);

        // Save updated Highscores
        string json = JsonUtility.ToJson(highscores);
        PlayerPrefs.SetString("highscoreTable", json);
        PlayerPrefs.Save();
    }
    public static void RemoveHighscoreEntry(int index)
    {
        // Load saved Highscores
        Highscores highscores = FetchHighscores();

        // Add new entry to Highscores
        highscores.highscoreEntryList.Remove(highscores.highscoreEntryList[index]);

        // Save updated Highscores
        string json = JsonUtility.ToJson(highscores);
        PlayerPrefs.SetString("highscoreTable", json);
        PlayerPrefs.Save();
    }
}
