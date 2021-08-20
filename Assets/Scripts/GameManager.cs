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
    public int currentScore = 0;
    public int highScore = 0;

    [Header("Playable Area")]
    public float levelConstraintTop; //The maximum positive Y value of the playable space.
    public float levelConstraintBottom; //The maximum negative Y value of the playable space.
    public float levelConstraintLeft; //The maximum negative X value of the playable space.
    public float levelConstraintRight; //The maximum positive X value of the playablle space.

    [Header("Gameplay Loop")]
    public int difficulty;
    public bool isGameRunning;
    public bool isGamePaused;
    bool trigger = true; // for intro timer switch
    public float timer = 4; // for intro message
    public float totalGameTime;
    public float gameTimeRemaining;
    public GameObject[] vehicles; // referenc to all moving gameobjects

    [Header("Script References")]
    public Player player;
    public Vehicle1 vehicleScript;
    public GameObject playerObject;
    public AudioManager audioManager;
    public CameraController cameraScript;

    [Header("Text References")]
    public GameObject mainMenu;
    public GameObject pauseMenu;
    public GameObject controlSchemeMenu;
    public GameObject difficultyMenu;
    public GameObject preMessage;
    public TMP_Text timerText;
    public TMP_Text preTimerText;
    public TMP_Text currentScoreUI;
    public TMP_Text highscoreText;
    public TMP_Text pauseHighscoreText;
    public GameObject[] hearts;
    public GameObject coinEffect;

   
    void Start()
    {
        // Game loop bool begin
        isGameRunning = false;
        isGamePaused = true;

        //set up timer
        gameTimeRemaining = totalGameTime;

        //set current score
        UpdateScore(-currentScore);
        currentScoreUI.text = "0";
    }

    void Update()
    {
        //Update highscore
        UpdateHighscore();

        // pause Function
        GamePause();

        // Quit Function
        Quit();

        if (!isGamePaused)
        {
            timer -= Time.deltaTime;
            preTimerText.text = Mathf.Round(timer) + "...";
        }

        //Look after level and begin timers
        HandleTimers();

        //Look after heart GUI
        HandleHearts();
    }

    // Function to handle the level timer and objectives menu timer
    // seen at beginning of level
    void HandleTimers()
    {
        //Objectives message timer
        if (timer <= 0)
        {
            preMessage.SetActive(false);
            //this runs once
            if (trigger)
            {
                isGameRunning = true;
                audioManager.PlayAudio(audioManager.startWhistle, 1);
                trigger = false;
            }
        }

        // handle level timer
        if (isGameRunning && !isGamePaused)
        {
            gameTimeRemaining -= Time.deltaTime;

            if (gameTimeRemaining <= 0)
            {
                gameTimeRemaining = 0;
                isGameRunning = false;
                audioManager.PlayAudio(audioManager.roosterCrow, 1f);
                audioManager.StopOverworldAudio();
                playerObject.SetActive(false);
            }
            // Round float to int Source Code: discussed in week 3-4 class @ read up on http://docs.unity3d.com/ScriptReference/Mathf.Round.html
            timerText.text = Mathf.Round(gameTimeRemaining) + " seconds untill Nightfall...";
        }
    }

    // Handles the hearts displayed in the GUI.
    // sets them active and inactive accordingly.
    void HandleHearts()
    {
        //handle hearts
        if (player.playerLivesRemaining == 5)
        {
            // set all to active
            for (int i = 0; i < 5; i++)
            {
                hearts[i].SetActive(true);
            }
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

    // function used to update score
    // takes in the score value to add as an integer 
    public void UpdateScore(int scoreValue)
    {
        currentScore += scoreValue;
        currentScoreUI.text = currentScore.ToString(); // convert in to string
    }

    // function to update the highscore
    public void UpdateHighscore()
    {
        if (currentScore > highScore)
        {
            highScore = currentScore;
        }
        else
        {
            highscoreText.text = "HighScore: " + highScore;
            pauseHighscoreText.text = "Current HighScore: \n" + highScore;
        }
    }

    // a funtion which handles player input to pause
    // the current game, displays a pause menu
    public void GamePause()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            isGamePaused = true;
            pauseMenu.SetActive(true);
            audioManager.StopOverworldAudio();
            audioManager.menuMusic.Play();
        }
    }

    // Handles when bonuses are collected by player
    // takes in a value to update score as an integer
    // & a position to instantiate a special effect
    public void CollectBonus(int amount, Vector2 position)
    {
        UpdateScore(amount);
        Instantiate(coinEffect, position, transform.rotation);
        audioManager.PlayAudio(audioManager.coinPickUp, 1f);
    }

    // A function to handle resetting vehicles when the game resets
    // deactivates every vehicle, then reactivates them in their 
    // original start positions
    public void resetVehicles()
    {
        //deactivate vehicles for reset
        for (int i = 0; i < vehicles.Length; i++)
        {
            vehicles[i].SetActive(false);
        }

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

    // Function to handle player input for 'Quitting' mid-game
    // listens for player input and brings players back to main menu
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
            timer = 5; // reset objectives timer 
            trigger = true; // reset trigger for gamerunning timer
            cameraScript.sunsetTime = 0f; // reset sunset timer
        }
    }

    // Use of buttons Sourced from: https://www.tutorialspoint.com/unity/unity_the_button.htm
    // function handles the return to menu button which displays
    // in win/lose message at game end
    // resets necessary audio, timers, score and player
    public void OnQuitButton()
    {
        // Handle game loop reset
        isGameRunning = false;
        isGamePaused = true;
        mainMenu.SetActive(true);
        audioManager.StopOverworldAudio();
        audioManager.menuMusic.Play();

        //Reset Game operations 
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
        cameraScript.sunsetTime = 0f; // reset sunset timer
        player.winScreen.SetActive(false); // turn off win screen, if any
    }

    // Function for 'Play' button in main menu
    // sets the game active whilst also ensuring vehicles
    // are in start position
    public void OnPlayButtonPress()
    {
        isGamePaused = false;
        mainMenu.SetActive(false);
        pauseMenu.SetActive(false);
        audioManager.PlayOverworldAudio();
        audioManager.menuMusic.Stop();
        // reset vehicles
        resetVehicles();
        //Reset exits
        player.OpenFinishGates();
        player.loseScreen.SetActive(false); // turn off lose screen, if any

        // Handle if players choose "Play Again" feature from win/lose message
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
        cameraScript.sunsetTime = 0f; // reset sunset timer
        player.winScreen.SetActive(false); // turn off win screen, if any

    }

    // Function for 'Return' button in pause menu
    // returns the gamestate to active from paused
    public void ReturnPauseButton()
    {
        isGameRunning = true;
        isGamePaused = false;
        mainMenu.SetActive(false);
        pauseMenu.SetActive(false);
        audioManager.PlayOverworldAudio();
        audioManager.menuMusic.Stop();
    }

    // Function for 'Controls' button in Main Menu
    // sets the control window active
    public void OnControlsButtonPress()
    {
        controlSchemeMenu.SetActive(true);
    }

    // Function for 'Return' Button in Controls menu
    // sets the controls windown inactive
    public void ReturnButtonPress()
    {
        controlSchemeMenu.SetActive(false);
    }

    // application.Quit method Source Code, by GameDevTraum: https://www.youtube.com/watch?v=6nenEHhcNwQ
    // function for 'Quit' button in main menu
    // should exit the program!
    public void OnExitButtonPress()
    {
        Application.Quit(); // quit appllication
    }

    // Function for "Difficulty" button
    // displays the difficulty selection screen 
    public void OnDifficultyPress()
    {
        // set difficulty menu active
        difficultyMenu.SetActive(true);
    }

    // Difficulty settings ----------------------------
    // sets the game difficulty integer to 1
    public void SetEasy()
    {
        difficulty = 1;
        // set difficulty inactive
        difficultyMenu.SetActive(false);
    }
    // sets the game difficulty integer to 2
    public void SetMedium()
    {
        difficulty = 2;
        // set difficulty inactive
        difficultyMenu.SetActive(false);
    }
    // sets the game difficulty integer to 3
    public void SetHard()
    {
        difficulty = 3;
        // set difficulty inactive
        difficultyMenu.SetActive(false);
    }
}

