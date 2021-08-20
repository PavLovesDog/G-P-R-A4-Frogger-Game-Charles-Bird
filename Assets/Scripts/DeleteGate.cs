using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteGate : MonoBehaviour
{
    public GameManager gameManager;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        // Deletes the object once the game ends
        // This script is soley for continuity on game restart
        if (!gameManager.isGameRunning)
        {
            Destroy(gameObject);
        }
    }
}
