using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    #region Variables
    [SerializeField] private GameObject background_1, background_2, background_3;
    [SerializeField] private float xEdgePosition = -20f;
    [SerializeField] private float xDistanceBetweenImages = 14.55f;
    private bool isPlayerAlive = true;
    #endregion

    #region Monobehaviour
    private void Start() //Subscribe to OnGameEnded event
    {
        GameManager.Instance.OnGameEnded += GameEnded;
    }

    void Update() //Handles the background images if the player is alive
    {
        if(isPlayerAlive)
            HandleBackgroundImagesPosition();
    }
    private void OnDestroy() //Unsubscribes event
    {
        GameManager.Instance.OnGameEnded -= GameEnded;
    }
    #endregion

    #region Methods
    private void HandleBackgroundImagesPosition() //Moves the background images so there's always only 3 in use
    {
        transform.position += -Vector3.right * Time.deltaTime;

        if (background_1.transform.position.x < xEdgePosition)
        {
            background_1.transform.position = new Vector3(background_3.transform.position.x + xDistanceBetweenImages,
                                                          background_1.transform.position.y,
                                                          background_1.transform.position.z);
        }
        if (background_2.transform.position.x < xEdgePosition)
        {
            background_2.transform.position = new Vector3(background_1.transform.position.x + xDistanceBetweenImages,
                                                          background_2.transform.position.y,
                                                          background_2.transform.position.z);
        }
        if (background_3.transform.position.x < xEdgePosition)
        {
            background_3.transform.position = new Vector3(background_2.transform.position.x + xDistanceBetweenImages,
                                                          background_3.transform.position.y,
                                                          background_3.transform.position.z);
        }
    }

    public void GameEnded()
    {
        isPlayerAlive = false;
    }
    #endregion

}
