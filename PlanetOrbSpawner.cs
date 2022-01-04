using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetOrbSpawner : MonoBehaviour
{
    public GameObject orbSpawnRoot;
    public GameObject planetOrbs;
    public GameObject spawnPoint;
    public GameObject tempObject;
    public GameObject multiplier;
    public GameObject[] clouds;
    public DragonFlightController player;
    public Vector3 spawnLocation;
    public RaycastHit hit;
    public LayerMask planetOrbMask;
    public Quaternion rotation;
    public int goalOrbs;
    public int goalMultipliers;
    public int numberOfClouds;
    public int remainingTurns = 0;
    public float gap;
    public float turnAngle;
    public float cloudHeight;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<DragonFlightController>();
        cloudHeight = player.seaLevel + 2000; // +(player.karmanLine / 5);
        int count = 0;
        for (int i = 0; i < goalOrbs; i++)
        {
            this.spawnNextOrb();
        }
        for (int i = 0; i < goalMultipliers; i++)
        {
            this.spawnNextMultiplier();
            count++;
        }
        for (int i = 0; i < numberOfClouds; i++)
        {
            this.spawnNextCloud();
            count++;
        }
    }

    public void FixedUpdate()
    {
        if (remainingTurns == 0)
        {
            remainingTurns = (int)Random.Range(300, 1000);
            //gap = Random.Range(0.001, 0.005);
            gap = 0.005f;
            turnAngle = Random.Range(-0.002f, 0.002f);
        }
        rotation = orbSpawnRoot.transform.rotation;
        rotation *= Quaternion.Euler(gap, turnAngle, 0);
        orbSpawnRoot.transform.rotation = rotation;
        remainingTurns--;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void spawnNextCloud()
    {
        spawnLocation = Random.onUnitSphere * cloudHeight + orbSpawnRoot.transform.position;
        tempObject = Instantiate(clouds[(int)Random.Range(0, clouds.Length)]);
        tempObject.transform.position = spawnLocation;
        tempObject.transform.parent = orbSpawnRoot.transform;
        tempObject.transform.up = spawnLocation - orbSpawnRoot.transform.position;
    }

    public void spawnNextMultiplier()
    {
        spawnLocation = Random.onUnitSphere * cloudHeight + orbSpawnRoot.transform.position; //cloudheight was used to replace 13000
        tempObject = Instantiate(multiplier);
        tempObject.transform.position = spawnLocation;
        tempObject.transform.up = spawnLocation - orbSpawnRoot.transform.position;
  
    }

    public void spawnNextOrb()
    {
        spawnLocation = Random.onUnitSphere * 15000 + orbSpawnRoot.transform.position;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(spawnLocation, orbSpawnRoot.transform.position - spawnLocation, out hit, 10000, planetOrbMask))
        {
            tempObject = Instantiate(planetOrbs);
            tempObject.transform.position = hit.point + (spawnLocation - orbSpawnRoot.transform.position).normalized * planetOrbs.transform.localScale.x;
        }


        ////**************code for a smooth path**************
        //if (remainingTurns == 0)
        //{
        //    remainingTurns = (int)Random.Range(1, 11);
        //    gap = Random.Range(10, 20);
        //    turnAngle = Random.Range(-40, 40);
        //}
        //rotation = orbSpawnRoot.transform.rotation;
        //rotation *= Quaternion.Euler(gap, turnAngle, 0);
        //orbSpawnRoot.transform.rotation = rotation;
        //remainingTurns--;
        //tempObject = Instantiate(planetOrbs);
        //tempObject.transform.position = spawnPoint.transform.position;

    }
}
