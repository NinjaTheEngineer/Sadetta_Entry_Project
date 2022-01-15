using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Variables
    private GameManager gameManager;

    private Vector2 direction;
    private Vector3 currentAngle;
    private Collider2D lastHitObstacleUp;
    private Collider2D lastHitObstacleDown;

    private bool isFlyingUp;
    private float flyTimecounter;
    private float fallDelay = 0.25f;

    [SerializeField] private float gravity = -9.8f;
    [SerializeField] private float maxFlyTime;
    [SerializeField] private float flyStrength = 5f;
    [SerializeField] private float upRotationMultiplier = 1f;
    [SerializeField] private float downRotationMultiplier = 2.5f;

    [SerializeField] private LayerMask whatIsObstacle;

    private bool playerIsAlive = true;
    #endregion

    #region Monobehaviour
    private void Start() //Initializes variables
    {
        gameManager = GameManager.Instance;
        flyTimecounter = maxFlyTime;
        currentAngle = transform.eulerAngles;
    }

    private void Update() //Handles player vertical movement if player is alive
    {
        if(playerIsAlive)
            HandleVerticalMovement();
    }

    private void FixedUpdate() //Handles player rotation and obstacle check if player is alive
    {
        if (playerIsAlive)
        {
            HandlePlayerRotation();
            HandlePassingObstacles();
        }
    }
    #endregion

    #region Methods
    private void HandlePassingObstacles() //Checks whenever the players as passed an obstacle
    {
        RaycastHit2D upHit = Physics2D.Raycast(
            new Vector2(transform.position.x, transform.position.y + 1f), Vector2.up); 

        if (upHit.collider != null && lastHitObstacleUp != upHit.collider)
        {
            if (upHit.collider.gameObject.CompareTag("Obstacle"))
            {
                lastHitObstacleUp = upHit.collider;
                gameManager.IncrementScore();
            }
        }

        RaycastHit2D downHit = Physics2D.Raycast(
            new Vector2(transform.position.x, transform.position.y - 1f), Vector2.down);

        if (downHit.collider != null && lastHitObstacleDown != downHit.collider)
        {
            if (downHit.collider.gameObject.CompareTag("Obstacle"))
            {
                lastHitObstacleDown = downHit.collider;
                gameManager.IncrementScore();
            }
        }
    }

    private void HandleVerticalMovement() //Handle Input for players vertical movement
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)
            || Input.GetKeyDown(KeyCode.UpArrow))
        {
            isFlyingUp = true;
            flyTimecounter = maxFlyTime;
            direction = Vector2.up * flyStrength;
        }

        if ((Input.GetKey(KeyCode.Space) || Input.GetMouseButton(0)
            || Input.GetKey(KeyCode.UpArrow)) && isFlyingUp)
        {
            if (flyTimecounter > 0)
            {
                direction = Vector2.up * flyStrength;
                flyTimecounter -= Time.deltaTime;
            }
            else
            {
                isFlyingUp = false;
            }
        }

        if (Input.GetKeyUp(KeyCode.Space) || Input.GetMouseButtonUp(0)
            || Input.GetKeyUp(KeyCode.UpArrow))
        {
            isFlyingUp = false;
        }

        direction.y += gravity * Time.deltaTime;
        transform.position += (Vector3)direction * Time.deltaTime;
    }

    private void HandlePlayerRotation() //Handles up and down rotation depending on the players input
    {
        if (isFlyingUp)
        {
            fallDelay = 0.25f;
            Vector3 targetAngle = new Vector3(0f, 0f, 100f);
            currentAngle = new Vector3(0, 0,
                Mathf.LerpAngle(currentAngle.z, targetAngle.z, Time.deltaTime * upRotationMultiplier));
        }
        else
        {
            if(fallDelay > 0f)
            {
                fallDelay -= Time.deltaTime;
                Vector3 targetAngle = new Vector3(0f, 0f, -80f);
                currentAngle = new Vector3(0, 0,
                Mathf.LerpAngle(currentAngle.z, targetAngle.z, Time.deltaTime / (downRotationMultiplier * 2)));
            }
            else
            {
                Vector3 targetAngle = new Vector3(0f, 0f, -80f);
                currentAngle = new Vector3(0, 0,
                Mathf.LerpAngle(currentAngle.z, targetAngle.z, Time.deltaTime / downRotationMultiplier));
            }
        }
        transform.eulerAngles = currentAngle;
    }

    private void OnCollisionEnter2D(Collision2D collision) //Checks for collision with obstacles
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            playerIsAlive = false;
            GetComponent<Animator>().SetBool("Dead", true);
            gameManager.PlayerCrashed();
        }
    }
    #endregion
}
