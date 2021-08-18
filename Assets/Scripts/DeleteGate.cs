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
        if (!gameManager.isGameRunning)
        {
            Destroy(gameObject);
        }
    }
}
