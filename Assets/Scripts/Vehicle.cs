using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script must be utlised as the core component on the 'vehicle' obstacle in the frogger game.
/// </summary>
public class Vehicle : MonoBehaviour
{
    /// <summary>
    /// -1 = left, 1 = right
    /// </summary>
    public int moveDirection = 0; //This variabe is to be used to indicate the direction the vehicle is moving in.
    public float speed; //This variable is to be used to control the speed of the vehicle.
    public Vector2 startingPosition; //This variable is to be used to indicate where on the map the vehicle starts (or spawns)
    public Vector2 endPosition; //This variablle is to be used to indicate the final destination of the vehicle.

    public Vector2[] startingPositions; //to store all lanes of starting positions
    public int secondsToWait;
    public int drawnRayLine; // set on each vehicle a different length  
    public bool vehicleCanMove;
    public int isOnSide; // Variable to determine which side the sprite is being spawned to!

    public GameObject cart;
    public Rigidbody2D vehicleRb;

    private void Awake()
    {
        vehicleCanMove = true;
        //Create random starting point for vehicle
        setUpVehicle();

        //Check to see if something is impeding
        CanGo();
       
        //set sprites to correct positions/directions
        setUpSprite();

        //Set the endPosition for cart to disappear at, judged on startPostion
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


    void Start()
    {
        //not using this??
    }

    void Update()
    {
        //Track current position of vehicle
        Vector2 currentPosition = transform.position;

        //DEBUGGING
        Debug.DrawLine(transform.position, new Vector2(transform.position.x + drawnRayLine * moveDirection, transform.position.y), Color.red);
        

        if(vehicleCanMove)
        {
            if (moveDirection == 1)
            {
                //Travel right
                vehicleRb.velocity = new Vector2(moveDirection * speed, vehicleRb.velocity.y);

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
                vehicleRb.velocity = new Vector2(moveDirection * speed, vehicleRb.velocity.y);
                
                //Once hit otherside, disable object/destroy
                if (currentPosition.x < endPosition.x)
                {
                    //this.gameObject.SetActive(false);
                    Destroy(gameObject);
                }
            }
        }
    }

    void setUpVehicle()
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
    }

    void CanGo()
    {
        //RayCast Check if something is in its way
        Debug.DrawLine(transform.position, new Vector2(transform.position.x + drawnRayLine * moveDirection, transform.position.y), Color.red);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(transform.position.x + drawnRayLine * moveDirection, transform.position.y));

        if (hit.collider != null)
        {
            //TODO: MAYBE JUST MOVE THEIR X POSITION UP OR DOWN!
            vehicleCanMove = false; // don't let the vehicle move
            StartCoroutine(delay()); // wait for a delay and resume
        }
    }

    void setUpSprite()
    {
        //IF player is on LEFT side of screen
        if (isOnSide == -1)
        {
            GetComponent<SpriteRenderer>().flipX = true;
            cart.GetComponent<SpriteRenderer>().flipX = true;

            // Swap sprite locations, if odd
            Vector2 temp = transform.position;
            transform.position = cart.transform.position;
            cart.transform.position = temp;
        } else if (isOnSide == 1) //If player is on RIGHT side of screen
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }
    }

    //IEnumerators and WaitForSeconds function code sourced from Unity Documentiation
    // https://docs.unity3d.com/ScriptReference/WaitForSeconds.html
    // https://docs.unity3d.com/ScriptReference/Coroutine.html
    IEnumerator delay()
    {
        yield return new WaitForSeconds(secondsToWait);
        vehicleCanMove = true; 
    }
}




