using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script must be utlised as the core component on the 'vehicle' obstacle in the frogger game.
/// </summary>
public class Log : MonoBehaviour
{
    /// <summary>
    /// -1 = left, 1 = right
    /// </summary>
    public int moveDirection = 0; //This variabe is to be used to indicate the direction the vehicle is moving in.
    public float speed; //This variable is to be used to control the speed of the vehicle.
    public Vector2 startingPosition; //This variable is to be used to indicate where on the map the vehicle starts (or spawns)
    public Vector2 endPosition; //This variablle is to be used to indicate the final destination of the vehicle.
    public Vector2 spriteSize;

    public Vector2[] startingPositions; //to store all lanes of starting positions
    public int secondsToWait;
    public int drawnRayLine; // set on each vehicle a different length  
    public bool logCanMove;
    public int isOnSide; // Variable to determine which side the sprite is being spawned to!

    public Rigidbody2D logRb;

    private void Awake()
    {
        logCanMove = true;

        //Create random starting point for log
        findNewPostition();
    }


    void Start()
    {
        //Check to see if something is impeding
        CanGo();

        //see if overlapping another object
        isOverlap();
    }

    void Update()
    {
        //Track current position of vehicle
        Vector2 currentPosition = transform.position;

        //DEBUGGING
        Debug.DrawLine(transform.position, new Vector2(transform.position.x + drawnRayLine * moveDirection, transform.position.y), Color.red);


        if (logCanMove)
        {
            if (moveDirection == 1)
            {
                //Travel right
                logRb.velocity = new Vector2(moveDirection * speed, logRb.velocity.y);

                //Once hit otherside, disable object/destroy
                if (currentPosition.x > endPosition.x)
                {
                    //this.gameObject.SetActive(false);
                    Destroy(gameObject);
                }
            }
            else // if move direction not 1 (i.e. -1)
            {
                //travel left
                logRb.velocity = new Vector2(moveDirection * speed, logRb.velocity.y);

                //Once hit otherside, disable object/destroy
                if (currentPosition.x < endPosition.x)
                {
                    //this.gameObject.SetActive(false);
                    Destroy(gameObject);
                }
            }
        }
    }

    void findNewPostition()
    {
        //Set a position randomly upon activation of object
        startingPosition = startingPositions[Random.Range(0, startingPositions.Length)];
        transform.position = startingPosition;

        // Set Moving direciton
        if (startingPosition.x == startingPositions[1].x) // Starting on the RIGHT side of screen
        {
            moveDirection = -1; // go left
            isOnSide = 1; // is on the right side
        }
        else if (startingPosition.x == startingPositions[0].x) // starting LEFT
        {
            moveDirection = 1; // go right
            isOnSide = -1; // is on the left side
        }

        // Set Ending positions
        if (startingPosition == startingPositions[0] ||
            startingPosition == startingPositions[2] ||
            startingPosition == startingPositions[4])
        {
            endPosition = new Vector2(startingPositions[1].x + 1, startingPosition.y);
        }
        else
        {
            endPosition = new Vector2(startingPositions[0].x, startingPosition.y);
        }
    }

    void CanGo()
    {
        //RayCast Check if something is in its way
        Debug.DrawLine(transform.position, new Vector2(transform.position.x + drawnRayLine * moveDirection, transform.position.y), Color.red);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(transform.position.x + drawnRayLine * moveDirection, transform.position.y));

        if (hit.collider != null) //|| hitArea.gameObject != null) // if theres a log before it, or already spawned within position
        {
            logCanMove = false; // don't let the vehicle move
            StartCoroutine("delay"); // wait for a delay and resume
        }
    }

    bool isOverlap()
    {
        //raycast its own area??: https://docs.unity3d.com/ScriptReference/Physics2D.OverlapBox.html
        Collider2D hitArea = Physics2D.OverlapBox(startingPosition, spriteSize, 0);
        //Physics2D.BoxCast(transform.position, spriteSize, 90, new Vector2(0, 1));

        if (hitArea.transform.position.y == transform.position.y)
        {
            StartCoroutine("delay");
            return true;
        }

        return false;
    }

    //IEnumerators and WaitForSeconds function code sourced from Unity Documentiation
    // https://docs.unity3d.com/ScriptReference/WaitForSeconds.html
    // https://docs.unity3d.com/ScriptReference/Coroutine.html
    IEnumerator delay()
    {
        yield return new WaitForSeconds(secondsToWait);
        logCanMove = true;
    }
}