using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    #region Variables
    [SerializeField] private float movementSpeedMin = 2f;
    [SerializeField] private float movementSpeedMax = 4f;
    [SerializeField] private float maxMovementSpeed = 10f;
    [SerializeField] private float movementSpeedIncremenet = 0.5f;
    [SerializeField] private bool changesMovementSpeed = true;

    private float movementSpeed = 3f;
    private float difficultTimer = 3f;
    private float difficultTimerCounter = 30f;

    private bool gameEnded = false;
    #endregion

    #region Monobehaviour

    private void Start() //Subscribe to GameEnded event and starts movement speed for certain obstacles
    {
        GameManager.Instance.OnGameEnded += GameEnded;
        if(changesMovementSpeed)
            movementSpeed = Random.Range(movementSpeedMin, movementSpeedMax);
    }
    private void Update() //Handle difficulty method
    {
        HandleDifficulty();
    }

    private void FixedUpdate() //If the game isn't over, adds movement to the obstacle
    {
        if (!gameEnded)
            transform.position += Vector3.left * Time.deltaTime * movementSpeed;
    }
    private void OnDestroy() //Unsubscribe event
    {
        GameManager.Instance.OnGameEnded -= GameEnded;
    }
    #endregion

    #region Methods
    private void GameEnded() //End game
    {
        gameEnded = true;
    }
    private void HandleDifficulty() //Handle the obstacle movement speed for each x seconds in game
    {
        if (difficultTimerCounter > 0f)
        {
            difficultTimerCounter -= Time.deltaTime;
        }
        else
        {
            difficultTimerCounter = difficultTimer;
            if (changesMovementSpeed && movementSpeed < maxMovementSpeed)
                movementSpeed += movementSpeedIncremenet;
        }
    }
    #endregion

}
