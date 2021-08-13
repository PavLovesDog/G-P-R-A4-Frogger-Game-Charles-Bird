using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShortcutManagement;
using UnityEngine;

/// <summary>
/// This script must be used as the core Player script for managing the player character in the game.
/// </summary>
public class Player1 : MonoBehaviour
{
    public string playerName = ""; //The players name for the purpose of storing the high score

    public int playerTotalLives; //Players total possible lives.
    public int playerLivesRemaining; //PLayers actual lives remaining.

    public bool playerIsAlive = true; //Is the player currently alive?
    public bool playerCanMove = false; //Can the player currently move?
    public int facingDirection;

    public Vector2 projectPos = new Vector2();
    public string enemy = "";

    public int[] highscores;  

    public GameManager myGameManager; //A reference to the GameManager in the scene.


    // Start is called before the first frame update
    void Start()
    {
        myGameManager = GameObject.FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) && transform.position.y < myGameManager.levelConstraintTop)
        {
            //transform.position = transform.position + new Vector3(0, 1, 0);
            transform.Translate(new Vector3(0, 1, 0));
        }
        else if (Input.GetKeyDown(KeyCode.S) && transform.position.y > myGameManager.levelConstraintBottom)
        {
            //transform.position = transform.position + new Vector3(0, -1, 0);
            transform.Translate(new Vector3(0, -1, 0));
        }
        else if (Input.GetKeyDown(KeyCode.A) && transform.position.x > myGameManager.levelConstraintLeft)
        {
            //transform.position = transform.position + new Vector3(-1, 0, 0);
            transform.Translate(new Vector3(-1, 0, 0));
        }
        else if (Input.GetKeyDown(KeyCode.D) && transform.position.x < myGameManager.levelConstraintRight)
        {
            //transform.position = transform.position + new Vector3(1, 0, 0);
            transform.Translate(new Vector3(1, 0, 0));
        }
    }
}