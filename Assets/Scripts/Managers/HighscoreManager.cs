using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HighscoreManager : MonoBehaviour
{

    [SerializeField] private Transform entryContainer;
    [SerializeField] private Transform entryTemplate;
    [SerializeField] private float templateHeight = 40f;
    private List<Transform> highscoreEntryTransformList;

    private void Awake()
    {
        entryTemplate.gameObject.SetActive(false);
        Highscores highscores = DataStorage.FetchHighscores();

        if (highscores == null)
        {
            // There's no stored table, initialize
            Debug.Log("Initializing table with default values...");
            DataStorage.AddHighscoreEntry(10, "CPU_1");
            DataStorage.AddHighscoreEntry(5, "CPU_2");
            DataStorage.AddHighscoreEntry(12, "CPU_3");
            DataStorage.AddHighscoreEntry(20, "CPU_4");
            DataStorage.AddHighscoreEntry(7, "CPU_5");
            // Reload
            highscores = DataStorage.FetchHighscores();
        }

        // Sort entry list by Score
        for (int i = 0; i < highscores.highscoreEntryList.Count; i++)
        {
            for (int j = i + 1; j < highscores.highscoreEntryList.Count; j++)
            {
                if (highscores.highscoreEntryList[j].score > highscores.highscoreEntryList[i].score)
                {
                    // Swap
                    HighscoreEntry tmp = highscores.highscoreEntryList[i];
                    highscores.highscoreEntryList[i] = highscores.highscoreEntryList[j];
                    highscores.highscoreEntryList[j] = tmp;
                }
            }
        }

        highscoreEntryTransformList = new List<Transform>();
        foreach (HighscoreEntry highscoreEntry in highscores.highscoreEntryList)
        {
            CreateHighscoreEntryTransform(highscoreEntry, entryContainer, highscoreEntryTransformList);
        }
    }

    private void CreateHighscoreEntryTransform(HighscoreEntry highscoreEntry, Transform container, List<Transform> transformList)
    {
        Transform entryTransform = Instantiate(entryTemplate, container);
        RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
        entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * transformList.Count);
        entryTransform.gameObject.SetActive(true);

        int rank = transformList.Count + 1;
        string rankString;
        switch (rank)
        {
            default:
                rankString = rank + "TH"; break;

            case 1: rankString = "1ST"; break;
            case 2: rankString = "2ND"; break;
            case 3: rankString = "3RD"; break;
        }

        entryTransform.Find("Position_Text").GetComponent<TextMeshProUGUI>().text = rankString;

        int score = highscoreEntry.score;

        entryTransform.Find("Score_Text").GetComponent<TextMeshProUGUI>().text = score.ToString();

        string name = highscoreEntry.name;
        entryTransform.Find("Name_Text").GetComponent<TextMeshProUGUI>().text = name;

        // Set background visible odds and evens, easier to read
        entryTransform.Find("Background").gameObject.SetActive(rank % 2 == 1);

        // Highlight First
        if (rank == 1)
        {
            entryTransform.Find("Position_Text").GetComponent<TextMeshProUGUI>().color = Color.green;
            entryTransform.Find("Score_Text").GetComponent<TextMeshProUGUI>().color = Color.green;
            entryTransform.Find("Name_Text").GetComponent<TextMeshProUGUI>().color = Color.green;
        }

        transformList.Add(entryTransform);
    }
}
