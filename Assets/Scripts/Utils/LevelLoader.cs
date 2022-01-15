using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 0.45f;

    private static LevelLoader instance;
    public static LevelLoader Instance //Singleton
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<LevelLoader>();
            }
            return instance;
        }
    }

    public void LoadNextScene() //Load next scene
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }
    public void LoadMainMenu() //Load main menu
    {
        StartCoroutine(LoadLevel(0));
    }
    public void RestartLevel() //Restart level
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex));
    }

    IEnumerator LoadLevel(int levelIndex) //Load entered level
    {
        transition = GetComponentInChildren<Animator>();
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(levelIndex);
    }
}