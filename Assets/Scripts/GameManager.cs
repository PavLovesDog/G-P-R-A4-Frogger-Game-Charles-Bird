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
    public float totalGameTime; //The maximum amount of time or the total time avilable to the player.
    public float gameTimeRemaining; //The current elapsed time

    [Header("Script References")]
    public Player player;
    public GameObject playerObject;
    public AudioManager audioManager;

    [Header("Text References")]
    public GameObject mainMenu;
    public TMP_Text timerText;
    public TMP_Text currentScoreUI;
    public GameObject[] hearts;
    public GameObject coinEffect;


   


    // Start is called before the first frame update
    void Start()
    {
        // Game loop bool begin
        isGameRunning = false;

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
        //Testing
        if (Input.GetMouseButtonDown(0))
        {
            isGameRunning = true;
            mainMenu.SetActive(false);
        }


        HandleHearts();

        // handle timer
        if (isGameRunning)
        {
            gameTimeRemaining -= Time.deltaTime;

            if (gameTimeRemaining <= 0)
            {
                gameTimeRemaining = 0;
                isGameRunning = false;
                // Do More special effects + handle audio
                audioManager.PlayAudio(audioManager.roosterCrow, 1f);
                audioManager.StopAudio();

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
}

