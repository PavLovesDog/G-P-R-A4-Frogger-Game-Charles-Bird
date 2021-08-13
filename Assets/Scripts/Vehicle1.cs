using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script must be utlised as the core component on the 'vehicle' obstacle in the frogger game.
/// </summary>
public class Vehicle1 : MonoBehaviour
{
    /// <summary>
    /// -1 = left, 1 = right
    /// </summary>
    public int moveDirection = 0; //This variabe is to be used to indicate the direction the vehicle is moving in.
    public float speed; //This variable is to be used to control the speed of the vehicle.
    public Vector2 startingPosition; //This variable is to be used to indicate where on the map the vehicle starts (or spawns)
    public Vector2 endPosition; //This variablle is to be used to indicate the final destination of the vehicle.

   // public bool vehicleCanMove;

    [Header("References")]
    public Rigidbody2D vehicleRb;
    public GameManager gameManager;

    void Start()
    {
        //vehicleCanMove = true;

        if(startingPosition.x < 0)
        {
            moveDirection = 1;
        }
        else if (startingPosition.x > 1)
        {
            moveDirection = -1;
        }
    }

    void Update()
    {
        //Track current position of vehicle
        Vector2 currentPosition = transform.position;

        if (gameManager.isGameRunning) // if game loop is running
        {
            if (moveDirection == 1)
            {
                //Travel right
                vehicleRb.velocity = new Vector2(moveDirection * speed, vehicleRb.velocity.y);

                //Once hit otherside, disable object/destroy
                if (currentPosition.x > endPosition.x)
                {
                    transform.position = startingPosition; // reset position
                }
            }
            else // if move direction not 1 (i.e. -1)
            {
                //travel left
                vehicleRb.velocity = new Vector2(moveDirection * speed, vehicleRb.velocity.y);

                //Once hit otherside, disable object/destroy
                if (currentPosition.x < endPosition.x)
                {
                    transform.position = startingPosition; // reset position
                }
            }
        }
        else
        {
            if (moveDirection == 1 && currentPosition.x > endPosition.x)
            {
                gameObject.SetActive(false); // set unactive for gameover
            } 
            else if (moveDirection == -1 && currentPosition.x < endPosition.x)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
