using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("References")]
    public GameManager gameManager;
    public GameObject player;

    [Header("Sunset")]
    public SpriteRenderer sunset;
    public float sunsetTime = 0f;
    public GameObject[] sunsetColour;
    

    [Header("Camera Settings")]
    public Vector3 offset;
    public float focusSpeed;

    [Header("Focus Area")]
    public float camConstraintTop; //The maximum positive Y value of the playable space.
    public float camConstraintBottom; //The maximum negative Y value of the playable space.
    public float camConstraintLeft; //The maximum negative X value of the playable space.
    public float camConstraintRight; //The maximum positive X value of the playablle space.

    // Start is called before the first frame update
    void Start()
    {
        sunset = sunset.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        // Source code found: https://docs.unity3d.com/ScriptReference/MonoBehaviour.Invoke.html
        Invoke("cameraBoundry", 0); // Used 'Invoke', as calling the function directly caused camera to jitter..

        // camera follow and offset derived from code course I purchased off stacksocial
        // Source: https://stacksocial.com/sales/build-the-legend-of-zenda-game-in-unity3d-and-blender
        transform.position = Vector3.Lerp(transform.position, // x
                                          player.transform.position + offset, // y
                                          Time.deltaTime * focusSpeed); // speed
        if (gameManager.isGameRunning && !gameManager.isGamePaused)
        {
            FadingLight(); // set the sun!
        }
    }

    private void cameraBoundry()
    {
        //Right bounds
        if (transform.position.x > camConstraintRight)
        {
            transform.position = new Vector3(camConstraintRight, transform.position.y, transform.position.z);
        }
        //Left bounds
        if (transform.position.x < camConstraintLeft)
        {
            transform.position = new Vector3(camConstraintLeft, transform.position.y, transform.position.z);
        }
        //Upper bounds
        if (transform.position.y > camConstraintTop)
        {
            transform.position = new Vector3(transform.position.x, camConstraintTop, transform.position.z);
        }
        //Lower bounds
        if (transform.position.y < camConstraintBottom)
        {
            transform.position = new Vector3(transform.position.x, camConstraintBottom, transform.position.z);
        }
    }

    void FadingLight()
    {
        // Fading light
        sunsetTime += Time.deltaTime;
        if (sunsetTime > 40f)
        {
            // early yellow sky
            sunsetColour[0].SetActive(true);

            if (sunsetTime > 50f)
            {
                // late yellow sky
                sunsetColour[1].SetActive(true);
                // set other colours inactive
                sunsetColour[0].SetActive(false);

                if (sunsetTime > 60f)
                {
                    // early orange sky
                    sunsetColour[2].SetActive(true);
                    // set other colours inactive
                    sunsetColour[0].SetActive(false);
                    sunsetColour[1].SetActive(false);

                    if (sunsetTime > 90f)
                    {
                        // orange sky
                        sunsetColour[3].SetActive(true);
                        // set other colours inactive
                        sunsetColour[0].SetActive(false);
                        sunsetColour[1].SetActive(false);
                        sunsetColour[2].SetActive(false);

                        if (sunsetTime > 100f)
                        {
                            // dark orange sky
                            sunsetColour[4].SetActive(true);
                            // set other colours inactive
                            sunsetColour[0].SetActive(false);
                            sunsetColour[1].SetActive(false);
                            sunsetColour[2].SetActive(false);
                            sunsetColour[3].SetActive(false);

                            if (sunsetTime > 120f)
                            {
                                // dark orange sky
                                sunsetColour[5].SetActive(true);
                                // set other colours inactive
                                sunsetColour[0].SetActive(false);
                                sunsetColour[1].SetActive(false);
                                sunsetColour[2].SetActive(false);
                                sunsetColour[3].SetActive(false);
                                sunsetColour[4].SetActive(false);
                            }
                        }
                    }
                }
            }
        }
    }
}
