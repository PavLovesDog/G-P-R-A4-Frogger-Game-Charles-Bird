using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// This script is to be attached to a GameObject called GameManager in the scene. It is to be used to manager the settings and overarching gameplay loop.
/// </summary>
public class GameManager : MonoBehaviour
{
    [Header("Scoring")]
    [SerializeField] private int currentScore = 0; //The current score in this round.
    public int highScore = 0; //The highest score achieved either in this session or over the lifetime of the game.

    [Header("Playable Area")]
    public float levelConstraintTop; //The maximum positive Y value of the playable space.
    public float levelConstraintBottom; //The maximum negative Y value of the playable space.
    public float levelConstraintLeft; //The maximum negative X value of the playable space.
    public float levelConstraintRight; //The maximum positive X value of the playablle space.

    [Header("Gameplay Loop")]
    public bool isGameRunning; //Is the gameplay part of the game current active?
    public bool isGamePaused; // has the player paused?
    bool trigger = true; // for intro timer switch
    public float totalGameTime; //The maximum amount of time or the total time avilable to the player.
    public float gameTimeRemaining; //The current elapsed time
    public GameObject[] vehicles; // referenc to all moving gameobjects

    [Header("Script References")]
    public Player player;
    public Vehicle1 vehicleScript;
    public GameObject playerObject;
    public AudioManager audioManager;

    [Header("Text References")]
    public GameObject mainMenu;
    public GameObject pauseMenu;
    public GameObject controlSchemeMenu;
    public GameObject preMessage;
    public TMP_Text timerText;
    public TMP_Text preTimerText;
    public TMP_Text currentScoreUI;
    public TMP_Text highscoreText;
    public TMP_Text pauseHighscoreText;
    public GameObject[] hearts;
    public GameObject coinEffect;


    public float timer = 4;
   


    // Start is called before the first frame update
    void Start()
    {
        // Game loop bool begin
        isGameRunning = false;
        isGamePaused = true;

        //set up timer
        gameTimeRemaining = totalGameTime;

        //set current score to 0?
        UpdateScore(-currentScore);
        currentScoreUI.text = "0";

        //Set a highscore here?
    }

    // Update is called once per frame
    void Update()
    {
        //Update highscore
        if (currentScore > highScore)
        {
            highScore = currentScore;
        }
        else
        {
            highscoreText.text = "HighScore: " + highScore;
            pauseHighscoreText.text = "Current HighScore: \n" + highScore;
        }

        // pause Function
        if (Input.GetKeyDown(KeyCode.P))
        {
            isGamePaused = true;
            pauseMenu.SetActive(true);
            audioManager.StopOverworldAudio();
            audioManager.menuMusic.Play();
        }
        // Quit Function
        Quit();
        //if (Input.GetKeyDown(KeyCode.Q))
        //{
        //    // Handle game loop reset
        //    isGameRunning = false;
        //    isGamePaused = true;
        //    mainMenu.SetActive(true);
        //    audioManager.StopOverworldAudio();
        //    audioManager.menuMusic.Play();
        //    //reset player position & player if dead
        //    player.transform.position = player.startingPosition;
        //    player.movePoint.transform.position = player.startingPosition;
        //    player.gameObject.SetActive(true);
        //    player.playerLivesRemaining = player.playerTotalLives; //reset player lives
        //    UpdateScore(-currentScore); //reset score
        //    //reset timers
        //    preMessage.SetActive(true);
        //    gameTimeRemaining = totalGameTime;
        //    timer = 5;
        //    trigger = true; // reset trigger for gamerunning timer
        //    resetVehicles(); // reset map
        //}

        if (!isGamePaused)
        {
            timer -= Time.deltaTime;
            preTimerText.text = Mathf.Round(timer) + "...";
        }

        //Objective message timer
        if (timer <= 0)
        {
            preMessage.SetActive(false);
            //this run once
            if (trigger)
            {
                isGameRunning = true;
                trigger = false;
            }
        }

        //Look after heart GUI
        HandleHearts();

        // handle level timer
        if (isGameRunning && !isGamePaused)
        {
            gameTimeRemaining -= Time.deltaTime;

            if (gameTimeRemaining <= 0)
            {
                gameTimeRemaining = 0;
                isGameRunning = false;
                // Do More special effects + handle audio
                audioManager.PlayAudio(audioManager.roosterCrow, 1f);
                audioManager.StopOverworldAudio();

                playerObject.SetActive(false);
            }
            // Round float to int Source Code: discussed in week 3-4 class @ read up on http://docs.unity3d.com/ScriptReference/Mathf.Round.html
            timerText.text = Mathf.Round(gameTimeRemaining) + " seconds untill Nightfall...";
        }
    }

    void HandleHearts()
    {
        //handle hearts
        if (player.playerLivesRemaining == 5)
        {
            // set all to active
        }

        if (player.playerLivesRemaining == 4)
        {
            hearts[4].SetActive(false);
        }

        if (player.playerLivesRemaining == 3)
        {
            hearts[3].SetActive(false);
        }

        if (player.playerLivesRemaining == 2)
        {
            hearts[2].SetActive(false);
        }

        if (player.playerLivesRemaining == 1)
        {
            hearts[1].SetActive(false);
        }

        if (player.playerLivesRemaining == 0)
        {
            hearts[0].SetActive(false);
        }
    }

    public void UpdateScore(int scoreValue)
    {
        currentScore += scoreValue;
        currentScoreUI.text = currentScore.ToString(); // convert in to string
        // make fun sound
        // sparkles on score?
    }

    public void CollectBonus(int amount, Vector2 position)
    {
        UpdateScore(amount);
        Instantiate(coinEffect, position, transform.rotation);
        audioManager.PlayAudio(audioManager.coinPickUp, 1f);
        
    }

    public void resetVehicles()
    {
        // ensure active
        for (int i = 0; i < vehicles.Length; i++)
        {
            vehicles[i].SetActive(true);
        }

        //reset positions
        for (int i = 0; i < vehicles.Length; i++)
        {
            vehicles[i].transform.position = vehicleScript.startingPositions[i]; // resets all vehicles to original position
        }
    }

    public void Quit()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            // Handle game loop reset
            isGameRunning = false;
            isGamePaused = true;
            mainMenu.SetActive(true);
            audioManager.StopOverworldAudio();
            audioManager.menuMusic.Play();
            //reset player position & player if dead
            player.transform.position = player.startingPosition;
            player.movePoint.transform.position = player.startingPosition;
            player.gameObject.SetActive(true);
            player.playerLivesRemaining = player.playerTotalLives; //reset player lives
            UpdateScore(-currentScore); //reset score
            //reset timers
            preMessage.SetActive(true);
            gameTimeRemaining = totalGameTime;
            timer = 5;
            trigger = true; // reset trigger for gamerunning timer
            resetVehicles(); // reset map
        }
    }


    // Use of buttons: https://www.tutorialspoint.com/unity/unity_the_button.htm
    public void OnPlayButtonPress()
    {

        //isGameRunning = true;
        isGamePaused = false;
        mainMenu.SetActive(false);
        pauseMenu.SetActive(false);
        audioManager.PlayOverworldAudio();
        audioManager.menuMusic.Stop();
        // reset vehicles
        //resetVehicles(); // is this needed?

    }

    public void ReturnPauseButton()
    {
        isGameRunning = true;
        isGamePaused = false;
        mainMenu.SetActive(false);
        pauseMenu.SetActive(false);
        audioManager.PlayOverworldAudio();
        audioManager.menuMusic.Stop();
    }

    public void OnControlsButtonPress()
    {
        controlSchemeMenu.SetActive(true);
    }

    public void ReturnButtonPress()
    {
        controlSchemeMenu.SetActive(false);
    }

    // application.Quit method Source Code, by GameDevTraum: https://www.youtube.com/watch?v=6nenEHhcNwQ
    public void OnQuitButtonPress()
    {
        Application.Quit(); // quit appllication
    }
}

