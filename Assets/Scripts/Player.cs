using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShortcutManagement;
using UnityEngine;

/// <summary>
/// This script must be used as the core Player script for managing the player character in the game.
/// </summary>
public class Player : MonoBehaviour
{
    [Header("Player")]
    public string playerName = ""; //The players name for the purpose of storing the high score
    public int playerTotalLives; //Players total possible lives.
    public int playerLivesRemaining; //PLayers actual lives remaining.
    public bool playerIsAlive = true; //Is the player currently alive?
    public bool playerCanMove = false; //Can the player currently move?
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

    [Header("End Zone References")]
    public int gatesLeft = 5;
    public GameObject doorPrefab;
    public GameObject flagPrefab;
    public GameObject soldierPrefab;
    public GameObject[] soldiers;
    public List<GameObject> frogSoldiers = new List<GameObject>(); 

    [Header("Water Crossing")]
    public Water water;
    public bool isOnLog;
    public GameObject[] logs;

    [Header("Audio")]
    public AudioManager audioManager;
    

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
        //-----------------------------------------------------------------------Player movemnet!
        playerBoundry();

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

        // Grid Based Movement Code lines 54 - 74
        // Source by gamesplusjames: https://www.youtube.com/watch?v=mbzXIOKZurA&t=96s

        //Move player towards its movepoint, smoothly
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);

        if (gameManager.isGameRunning)
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Vehicle"))
        {
            //Minus life here and handle auio + visual
            playerLivesRemaining--;
            Instantiate(deathParticles, transform.position, Quaternion.identity);
            audioManager.PlayAudio(audioManager.deathSound, 1f); // Audio is set up through the Audio Manager gameobject because audio cannot play from unactive gameobjects

            CheckLivesRemaining();
        }

        if (collision.gameObject.CompareTag("Coin"))
        {
            gameManager.CollectBonus(25, collision.transform.position);
            Destroy(collision.gameObject);
            //spawn effect
            //play sound
        }

        if (collision.gameObject.CompareTag("Bonus"))
        {
            gameManager.CollectBonus(50, collision.transform.position);
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

        //DOOR 1
        if (collision.gameObject.CompareTag("Finish 1"))
        {
            //Decrement finish spots left
            gatesLeft--;
            Instantiate(doorPrefab, endPos[0], transform.rotation); // shut door and save
            //Add score
            gameManager.UpdateScore(20);
            //check gates left
            FinishGateCheck();
        }

        //DOOR 2
        if (collision.gameObject.CompareTag("Finish 2"))
        {
            //Decrement finish spots left
            gatesLeft--;
            Instantiate(doorPrefab, endPos[1], transform.rotation); // shut door and save
            //Add score
            gameManager.UpdateScore(20);
            //check gates left
            FinishGateCheck();
        }

        //DOOR 3
        if (collision.gameObject.CompareTag("Finish 3"))
        {
            //Decrement finish spots left
            gatesLeft--;
            Instantiate(doorPrefab, endPos[2], transform.rotation); // shut door and save
            //Add score
            gameManager.UpdateScore(20);
            //check gates left
            FinishGateCheck();
        }

        //DOOR 4
        if (collision.gameObject.CompareTag("Finish 4"))
        {
            //Decrement finish spots left
            gatesLeft--;
            Instantiate(flagPrefab, endPos[3], transform.rotation); // raise flag and save
            //Add score
            gameManager.UpdateScore(20);
            //check gates left
            FinishGateCheck();
  
            // instantiate frog night atop the tower LEFT
            Instantiate(soldierPrefab, new Vector3(-3.5f, 18, 10), transform.rotation);
        }

        //DOOR 5
        if (collision.gameObject.CompareTag("Finish 5"))
        {
            //Decrement finish spots left
            gatesLeft--;
            Instantiate(flagPrefab, endPos[4], transform.rotation); // raise flag and save
            //Add score
            gameManager.UpdateScore(20);
            //check gates left
            FinishGateCheck();
           
            // instantiate frog night atop the tower RIGHT
            Instantiate(soldierPrefab, new Vector3(3.5f, 18, 10), transform.rotation);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //collision with log
        if (collision.gameObject.CompareTag("Log"))
        {
            isOnLog = true;

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //collision with log
        if (collision.gameObject.CompareTag("Log"))
        {
            isOnLog = false;
            movePoint.transform.parent = null; // frog detaches from child
        }
    }

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

    void FinishGateCheck()
    {
        //Audio
        audioManager.PlayAudio(audioManager.gateClosed, 2f);

        // Score
        gameManager.UpdateScore(50);

        // Check for last door
        if (gatesLeft == 0)
        {
            //End game, YOU WIN!
            //TODO CREATE WIN MESSAGE AND DISPLAY! and display score
            audioManager.PlayAudio(audioManager.victory, 2f);
            audioManager.StopOverworldAudio();
            gameManager.UpdateScore(1000); // game win score
        }
        // reset player
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

    public void CheckLivesRemaining()
    {
        if (playerLivesRemaining > 0)
        {
            //reset position if lives left
            transform.position = startingPosition;
            movePoint.position = startingPosition;
        }
        else if (playerLivesRemaining == 0)// if no lives left
        {
            //TODO CREATE LOSE MESSAGE AND DISPLAY! also display score and 'q' to quit
            playerIsAlive = false;
            gameObject.SetActive(false);
            audioManager.PlayAudio(audioManager.GameOver, 1f);
            audioManager.StopOverworldAudio();
            gameManager.isGameRunning = false;
        }
    }

    IEnumerator FinishDelay()
    {
        yield return new WaitForSeconds(0.5f);
        transform.position = startingPosition;
        movePoint.position = startingPosition;
    }
}
