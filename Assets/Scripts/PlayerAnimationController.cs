using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    public Animator playerAnim;
    public SpriteRenderer playerSprite;
    public Player player;

    // Start is called before the first frame update
    void Start()
    {
        playerAnim = GetComponent<Animator>();
        playerSprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Horizontal") > 0) // if we're pressing right
        {
            playerAnim.SetFloat("right", 1);
        }
        else
        {
            playerAnim.SetFloat("right", 0);
        }
        //-----------------------------------
        if (Input.GetAxis("Horizontal") < 0) // if we're pressing left
        {
            playerAnim.SetFloat("left", -1);
            playerSprite.flipX = true;
        }
        else
        {
            playerAnim.SetFloat("left", 0);
            playerSprite.flipX = false;
        }
        //------------------------------------
        if (Input.GetAxis("Vertical") > 0) // if we're pressing up
        {
            playerAnim.SetFloat("up", 1);
        }
        else
        {
            playerAnim.SetFloat("up", 0);
        }
        //------------------------------------
        if (Input.GetAxis("Vertical") < 0) // if we're pressing down
        {
            playerAnim.SetFloat("down", -1);
        }
        else
        {
            playerAnim.SetFloat("down", 0);
        }

        if(!player.playerIsAlive)
        {
            playerAnim.SetBool("isDead", true);
        }
        else
        {
            playerAnim.SetBool("isDead", false);
        }

        //set bool for death anim
        if (player.playerLivesRemaining > 0)
        {
            player.playerIsAlive = true;
        }
        else
        {
            player.playerIsAlive = false;
        }

    }
}
