using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    public Player playerScript; // reference to player script
    public GameObject player;
    public Transform playerMovePoint;
    public Collider2D playerCollider;
    public AudioManager audioManager;
    public GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        playerCollider = GameObject.FindGameObjectWithTag("Player").GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.isGameRunning) // only do check if game is running
        {
            // check if player is still within water bounds
            if (player.transform.position.y <= 11.4)
            {
                // if player is within the boundry of water
                // and if player is NOT on a log
                if (player.transform.position.y >= 6.5 && !playerScript.isOnLog)
                {
                    StartCoroutine("waitBeforeKill");
                }
                else if (playerScript.isOnLog)
                {
                    player.SetActive(true); // catch 
                }
            }
        }
    }

    IEnumerator waitBeforeKill()
    {
        yield return new WaitForSeconds(0.05f); // given player a chance to get back on log
        if (!playerScript.isOnLog)
        { 
            // minus life
            playerScript.playerLivesRemaining--;
            // play audio
            audioManager.PlayAudio(audioManager.drownSound, 1f);

            if (playerScript.playerLivesRemaining > 0)
            {
                // reset position and move position
                player.transform.position = playerScript.startingPosition;
                playerMovePoint.transform.position = playerScript.startingPosition;
            }
            else // if no lives left
            {
               //stop game!
                playerScript.CheckLivesRemaining();
            }
            StopCoroutine("waitBeforeKill"); // stop coroutine before it loops
        }
    }
}
