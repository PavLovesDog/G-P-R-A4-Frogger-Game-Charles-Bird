using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObject : MonoBehaviour
{
    public float timer;
  
    void Update()
    {
        timer -= Time.deltaTime;

        // for time dependent objects, set time in editor for specific item
        if (timer<= 0)
        {
            Destroy(gameObject);
        }
    }
}
