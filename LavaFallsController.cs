using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaFallsController : MonoBehaviour
{
    public AudioSource waterfall1;
    public AudioSource waterfall2;
    public DragonFlightController player;

    // Start is called before the first frame update
    void Start()
    {
        
    }


    public void Update()
    {
        if (player.pauseMenu.activeSelf)
        {
            if (waterfall1.isPlaying)
            {
                waterfall1.Pause();
            }
            if (waterfall2.isPlaying)
            {
                waterfall2.Pause();
            }
        }
        else
        {
            if (!waterfall1.isPlaying)
            {
                waterfall1.Play();
            }
            if (!waterfall2.isPlaying)
            {
                waterfall2.Play();
            }
        } 
    }
}
