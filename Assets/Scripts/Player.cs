using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShortcutManagement;
using UnityEngine;
using TMPro;

/// <summary>
/// This script must be used as the core Player script for managing the player character in the game.
/// </summary>
public class Player : MonoBehaviour
{
    [Header("Player")]
    public string playerName = "";
    public int playerTotalLives;
    public int playerLivesRemaining;
    public bool playerIsAlive = true;
    public bool playerCanMove = false;
    public int facingDirection;
    public float moveSpeed = 5f;
    public float moveVolume = 0.5f;
    public Vector2 startingPosition;
    public Vector3[] endPos;
 
    [Header("References")]
    public Transform movePoint;
    public LayerMask StopsMovement; // layer mask to check for colliderable objects the player cant move on
    public GameManager gameManager;
    public GameObject deathParticles;
    public GameObject bonusArea;

    [Header("End Zone References")]
    public int gatesLeft = 5;
    public GameObject doorPrefab;
    public GameObject flagPrefab;
    public GameObject soldierPrefab;
    public GameObject[] soldiers;
    public GameObject winScreen;
    public GameObject loseScreen;
    public TMP_Text winScoreText;
    public TMP_Text loseScoreText;
    public bool end1Open;// = true;
    public bool end2Open;// = true;
    public bool end3Open;// = true;
    public bool end4Open;// = true;
    public bool end5Open;// = true;

    [Header("Water Crossing")]
    public Water water;
    public bool isOnLog;

    [Header("Audio")]
    public AudioManager audioManager;

    private void Awake()
    {
        OpenFinishGates(); // ensure bools are set to true
    }

    // Start is called before the first frame update
    void Start()
    {
        // set the myGameManager
        gameManager = GameObject.FindObjectOfType<GameManager>();

        //set the parent of our movepoint
        movePoint.parent = null;

        //set lives
        playerLivesRemaining = playerTotalLives;
    }

    // Update is called once per frame
    void Update()
    {   
        // invoke player boundry
        playerBoundry();
       
        //-----------------------------------------------------------------------Player movement!
        // set up move variables
        float xMovement = 0f;
        float yMovement = 0f;

        // locking input into if statement ensures one step per button press
        // Idea found: https://answers.unity.com/questions/376587/how-to-treat-inputgetaxis-as-inputgetbuttondown.html
        // by user Julianobsg
        if (Input.anyKeyDown)
        {
            // movement of player by 1 or -1
            xMovement = Input.GetAxisRaw("Horizontal");
            yMovement = Input.GetAxisRaw("Vertical");
        }
        else
        {
            xMovement = 0f;
            yMovement = 0f;
        }

        // Increment player 1 unit on grid
        Vector3 incrementX = new Vector3(xMovement, 0, 0);
        Vector3 incrementY = new Vector3(0, yMovement, 0);

        // Grid Based Movement Code lines 109 - 148
        // Source by gamesplusjames: https://www.youtube.com/watch?v=mbzXIOKZurA&t=96s

        //Move player towards its movepoint, smoothly
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);

        // Handle player movement
        if (gameManager.isGameRunning && !gameManager.isGamePaused)
        {
            //if player is at movepoint, or almost at
            if (Vector3.Distance(transform.position, movePoint.position) <= 0.025f)
            {
                //Check if button is being pressed
                if (Mathf.Abs(xMovement) == 1f) // X axis, use absolute value to check for left or right (1 or -1)
                {
                    // Play AudioSource
                    audioManager.PlayAudio(audioManager.moveSound, moveVolume);

                    //Check if colliders block path ////////////MAY NEED TO REMOVE
                    if (!Physics2D.OverlapCircle(movePoint.position + incrementX, 0.2f, StopsMovement)) // if there is NOT a collider ahead, we may move
                    {
                        //Move movePoint to new location,
                        movePoint.position += incrementX;

                        //speed up on log to compensate for moving parent object
                        if (isOnLog)
                        {
                            moveSpeed = 10f;
                        }
                        else
                        {
                            moveSpeed = 5f; // reset to regular value when not on log
                        }
                    }
                } // CAN ADD AN ELSE IF HERE TO STOP DIAGONAL MOVEMENT

                if (Mathf.Abs(yMovement) == 1f) // Y axis
                {
                    // Play AudioSource
                    audioManager.PlayAudio(audioManager.moveSound, moveVolume);

                    if (!Physics2D.OverlapCircle(movePoint.position + incrementY, 0.2f, StopsMovement))
                    {
                        movePoint.position += incrementY;
                    }
                }
            }
        }
    }
    //-----------------------------------------------------------------------Player movement END

    // COLLISIONS ------------------------------------------------------------------------------

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // What happens if player collide with Vehicles
        if (collision.gameObject.CompareTag("Vehicle"))
        {
            playerLivesRemaining--;
            Instantiate(deathParticles, transform.position, Quaternion.identity);
            audioManager.PlayAudio(audioManager.deathSound, 1f); // Audio is set up through the Audio Manager gameobject because audio cannot play from unactive gameobjects
            CheckLivesRemaining(); // perform lives remaining check
        }

        // What happens when players pick up coins!
        if (collision.gameObject.CompareTag("Coin"))
        {
            gameManager.CollectBonus(25, collision.transform.position);
            Destroy(collision.gameObject);
        }

        // What happens when player finds the bonus
        if (collision.gameObject.CompareTag("Bonus"))
        {
            StartCoroutine("BonusDelay");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //collision with log
        if (collision.gameObject.CompareTag("Log"))
        {
            isOnLog = true;
            // transition parent/child objects idea Source Code: https://www.youtube.com/watch?v=DQYj8Wgw3O0
            movePoint.transform.parent = collision.transform; // frog becomes child of log and moves with it
        }

        // Ending Position collisions -----------------------------------------------------------------START

        //DOOR 1
        if (end1Open && collision.gameObject.CompareTag("Finish 1"))
        {
            //Decrement finish spots left
            gatesLeft--;
            Instantiate(doorPrefab, endPos[0], transform.rotation); // shut door and save
            //Add score
            gameManager.UpdateScore(20);
            //check gates left
            FinishGateCheck();
            // shut bool, player can no longer win through this gate
            end1Open = false; 
        }
 

        //DOOR 2
        if (end2Open && collision.gameObject.CompareTag("Finish 2"))
        {
            gatesLeft--;
            Instantiate(doorPrefab, endPos[1], transform.rotation);
            gameManager.UpdateScore(20);
            FinishGateCheck();
            end2Open = false;
        }

        //DOOR 3
        if (end3Open && collision.gameObject.CompareTag("Finish 3"))
        {
            gatesLeft--;
            Instantiate(doorPrefab, endPos[2], transform.rotation);
            gameManager.UpdateScore(20);
            FinishGateCheck();
            end3Open = false;
        }

        //DOOR 4
        if (end4Open && collision.gameObject.CompareTag("Finish 4"))
        {
            gatesLeft--;
            Instantiate(flagPrefab, endPos[3], transform.rotation); // raise flag and save
            gameManager.UpdateScore(20);
            FinishGateCheck();
            // instantiate frog night atop the tower LEFT
            Instantiate(soldierPrefab, new Vector3(-3.5f, 18.25f, 10), transform.rotation);
            end4Open = false;   
        }

        //DOOR 5
        if (end5Open && collision.gameObject.CompareTag("Finish 5"))
        {
            //Decrement finish spots left
            gatesLeft--;
            Instantiate(flagPrefab, endPos[4], transform.rotation); // raise flag and save
            //Add score
            gameManager.UpdateScore(20);
            //check gates left
            FinishGateCheck();
            // instantiate frog night atop the tower RIGHT
            Instantiate(soldierPrefab, new Vector3(3.5f, 18.25f, 10), transform.rotation);
            end5Open = false;
        }

        // Ending Position collisions -------------------------------------------------------------------END
    }

    // collision check to keep bool active while on log
    private void OnTriggerStay2D(Collider2D collision)
    {
        //collision with log
        if (collision.gameObject.CompareTag("Log"))
        {
            isOnLog = true;
        }
    }

    // Collision check to detach player from log
    // and switch bool on exit of collider
    private void OnTriggerExit2D(Collider2D collision)
    {
        //collision with log
        if (collision.gameObject.CompareTag("Log"))
        {
            isOnLog = false;
            movePoint.transform.parent = null; // frog detaches from child
        }
    }

    // Function to ensure player stays within the game borders
    private void playerBoundry()
    {
        //Right bounds
        if (transform.position.x > gameManager.levelConstraintRight)
        {
            movePoint.transform.position = new Vector3(gameManager.levelConstraintRight, transform.position.y, transform.position.z);
        }
        //Left bounds
        if (transform.position.x < gameManager.levelConstraintLeft)
        {
            movePoint.transform.position = new Vector3(gameManager.levelConstraintLeft, transform.position.y, transform.position.z);
        }
        //Upper bounds
        if (transform.position.y > gameManager.levelConstraintTop)
        {
            movePoint.transform.position = new Vector3(transform.position.x, gameManager.levelConstraintTop, transform.position.z);
        }
        //Lower bounds
        if (transform.position.y < gameManager.levelConstraintBottom)
        {
            movePoint.transform.position = new Vector3(transform.position.x, gameManager.levelConstraintBottom, transform.position.z);
        }
    }

    // Function handles what happens when player collides with
    // finish point, checks for if the player is passing the final gate
    void FinishGateCheck()
    {
        audioManager.PlayAudio(audioManager.gateClosed, 2f); //Audio
        gameManager.UpdateScore(50); // Score

        // Check for last door
        if (gatesLeft == 0)
        {
            //End game, YOU WIN!
            winScreen.SetActive(true);
            winScoreText.text = "Your Score: " + gameManager.currentScore;
            audioManager.PlayAudio(audioManager.victory, 2f);
            audioManager.StopOverworldAudio();
            gameManager.UpdateScore(1000); // everyone safely home score bonus!
            gameManager.isGameRunning = false;

            // update score for unused time
            if (gameManager.gameTimeRemaining > 0)
            {
                gameManager.UpdateScore(Mathf.FloorToInt(gameManager.gameTimeRemaining * 10));
            }
        }

        // reset player with delay. this "feels" better than immediately teleporting
        StartCoroutine("FinishDelay");

        //remove a soldier from start area
        if (gatesLeft >= 1)
        {
            soldiers[-1 + gatesLeft].SetActive(false);
                                                           
        }
        else
        {
            gameObject.SetActive(false); // removes player as he is the last!
        }
    }

    // Checks how many lives player has left and either
    // resets player position or ends the game
    public void CheckLivesRemaining()
    {
        if (playerLivesRemaining > 0)
        {
            //reset position if lives left
            transform.position = startingPosition;
            movePoint.position = startingPosition;
            playerIsAlive = true;
        }
        else if (playerLivesRemaining == 0)// if no lives left
        {
            //End game, You Lose
            loseScreen.SetActive(true);
            loseScoreText.text = "Your Score: " + gameManager.currentScore;
            playerIsAlive = false;
            gameObject.SetActive(false);
            audioManager.PlayAudio(audioManager.GameOver, 1f);
            audioManager.StopOverworldAudio();
            gameManager.isGameRunning = false;
        }
    }

    // Function to reopenbools for finish gates
    public void OpenFinishGates()
    {
        // set gate bools
        end1Open = true;
        end2Open = true;
        end3Open = true;
        end4Open = true;
        end5Open = true;
    }

    // Source Code derived from: https://docs.unity3d.com/ScriptReference/MonoBehaviour.StartCoroutine.html
    // a coroutine to delay players reset upon returning frog home
    IEnumerator FinishDelay()
    {
        yield return new WaitForSeconds(0.5f);
        transform.position = startingPosition;
        movePoint.position = startingPosition;
    }

    // a coroutine to delay the rate at which the player
    // can recieve the bonus points
    IEnumerator BonusDelay()
    {
        gameManager.CollectBonus(50, transform.position); // update score wherever he at
        bonusArea.SetActive(false);
        yield return new WaitForSeconds(5f);
        bonusArea.SetActive(true);
        StopCoroutine("BonusDelay");
    }
}
