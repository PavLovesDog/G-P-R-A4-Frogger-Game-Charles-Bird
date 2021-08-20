using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script must be utlised as the core component on the 'vehicle' obstacle in the frogger game.
/// </summary>
public class Vehicle1 : MonoBehaviour
{
    public int moveDirection = 0; //This variabe is to be used to indicate the direction the vehicle is moving in.
    public float speed; //This variable is to be used to control the speed of the vehicle.
    public Vector2 spawnPosition; //This variable is to be used to indicate where on the map the vehicle starts (or spawns)
    public Vector2 endPosition; //This variablle is to be used to indicate the final destination of the vehicle.

    //An array of starting positions, matched with log positions visually, used to reset game
    public Vector2[] startingPositions = { new Vector2(-25, 11.5f), new Vector2(-13.5f, 11.5f),
                                           new Vector2(13.5f, 10.5f), new Vector2(22.5f, 10.5f), new Vector2(31.5f, 10.5f),
                                           new Vector2(-29.5f, 9.5f), new Vector2(-21.5f, 9.5f), new Vector2(-13.5f, 9.5f),
                                           new Vector2(13.5f, 8.5f), new Vector2(26, 8.5f),
                                           new Vector2(-33.5f, 7.5f), new Vector2(-28.5f, 7.5f), new Vector2(-18.5f, 7.5f), new Vector2(-13.5f, 7.5f),
                                           new Vector2(13.5f, 6.5f), new Vector2(17.5f, 6.5f), new Vector2(21.5f, 6.5f), new Vector2(25.5f, 6.5f), new Vector2(29.5f, 6.5f), new Vector2(33.5f, 6.5f), new Vector2(37.5f,6.5f),
                                           new Vector2(-23.5f, 0.5f), new Vector2(-17.5f, 0.5f), new Vector2(-11.5f, 0.5f),
                                           new Vector2(11.5f, -0.5f), new Vector2(18.5f, -0.5f), new Vector2(25.5f, -0.5f),
                                           new Vector2(-22.5f, -1.5f), new Vector2(-11.5f, -1.5f),
                                           new Vector2(-11.5f, -2.5f),
                                           new Vector2(11.5f, -3.5f), new Vector2(19.5f, -3.5f), new Vector2(28.5f, -3.5f),
                                           new Vector2(-23.5f, -4.5f), new Vector2(-20.5f, -4.5f), new Vector2(-14.5f, -4.5f), new Vector2(-11.5f, -4.5f),
    };

    // ---- Visual Representation of mapped locations above ----
    //          INDEX:
    // lane 12        0, 1
    // lane 11        2, 3, 4
    // lane 10        5, 6, 7
    // lane 9         8, 9
    // lane 8         10, 11, 12, 13
    // lane 7         14, 15, 16, 17, 18, 19, 20
    // lane 6         21, 22, 23
    // lane 5         24, 25, 26
    // lane 4         27, 28
    // lane 3         29
    // lane 2         30, 31, 32
    // lane 1         33, 34, 35, 36

    [Header("References")]
    public Rigidbody2D vehicleRb;
    public GameManager gameManager;
    public Player playerSript;

    void Start()
    {
        if(spawnPosition.x < 0)
        {
            moveDirection = 1;
        }
        else if (spawnPosition.x > 1)
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
            if (!gameManager.isGamePaused) // if the game is NOT paused
            {
                if (moveDirection == 1)
                {
                    // Find speed and travel right
                    GetDifficulty();

                    //Once hit otherside, disable object/destroy
                    if (currentPosition.x > endPosition.x)
                    {
                        transform.position = spawnPosition; // reset position
                    }
                }
                else // if move direction not 1 (i.e. -1)
                {
                    // find speed and travel left
                    GetDifficulty();

                    //Once hit otherside, disable object/destroy
                    if (currentPosition.x < endPosition.x)
                    {
                        transform.position = spawnPosition; // reset position
                    }
                }
            } 
        }
        else if (!gameManager.isGameRunning && playerSript.playerLivesRemaining == 0) // if player is dead
        {
            if (moveDirection == 1)
            {
                // have vehicles run off screen
                vehicleRb.velocity = new Vector2(moveDirection * speed, vehicleRb.velocity.y);
        
                if (currentPosition.x >= endPosition.x) // if out of bounds
                {
                    gameObject.SetActive(false);
                }
            } 
            else if (moveDirection == -1 && !gameManager.isGamePaused)
            {
                // Vehicles leave screen
                vehicleRb.velocity = new Vector2(moveDirection * speed, vehicleRb.velocity.y);
            
                if (currentPosition.x <= endPosition.x)
                {
                    gameObject.SetActive(false);
                }
            }
        }
    }

    // A function which sets vehicle/log speed dependent on
    // move direction and difficulty setting.
    public void GetDifficulty()
    {
        // travel right
        if (gameManager.difficulty == 1 && moveDirection == 1) // easy
        {
            transform.Translate(Vector3.right * (speed * 0.75f) * Time.deltaTime, Space.World);
        }
        else if (gameManager.difficulty == 2 && moveDirection == 1) // medium
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime, Space.World);
        }
        else if (gameManager.difficulty == 3 && moveDirection == 1) // hard
        {
            transform.Translate(Vector3.right * (speed * 1.5f) * Time.deltaTime, Space.World);
        }

        //travel left
        if (gameManager.difficulty == 1 && moveDirection == -1) // easy
        {
            transform.Translate(-Vector3.right * (speed * 0.75f) * Time.deltaTime, Space.World);
        }
        else if (gameManager.difficulty == 2 && moveDirection == -1) // medium
        {
            transform.Translate(-Vector3.right * speed * Time.deltaTime, Space.World);
        }
        else if (gameManager.difficulty == 3 && moveDirection == -1) // hard
        {
            transform.Translate(-Vector3.right * (speed * 1.5f) * Time.deltaTime, Space.World);
        }
    }
}
