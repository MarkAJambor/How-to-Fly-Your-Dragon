using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbSpawner : MonoBehaviour {

    public int minGap;
    public int maxGap;
    public int maxNumberOfOrbs;
    public int maxNumberOfMultipliers;
    public int numberOfOrbs = 0;
    public int numberOfMultipliers = 0;
    public bool rotateRight;
    public GameObject[] orbs;
    public int currentOrb;
    public Vector3 randomDirection;
    public Vector3 randomLocation;
    public Vector3 worldUpVector = new Vector3(0, 1, 0);
    public GameObject testObject;
    public GameObject tempObject;
    public GameObject multiplier;
    public Quaternion rotation;

	// Use this for initialization
	void Start ()
    {
	}

    public void FixedUpdate()
    {
        if (numberOfOrbs < maxNumberOfOrbs)
        {
            while (numberOfOrbs < maxNumberOfOrbs)
            {
                if (Random.Range(0, 2) == 0)
                {
                    rotateRight = true;
                }
                else
                {
                    rotateRight = false;
                }
                //remove this
                currentOrb = Random.Range(0, orbs.Length);
                randomDirection = Random.onUnitSphere * Random.Range(minGap, maxGap);
                randomLocation = new Vector3(Random.Range(-14000, 14000), 0, Random.Range(-14000, 14000));
                randomLocation.y = Random.Range(Terrain.activeTerrain.SampleHeight(randomLocation), 1700);
                for (int i = 0; i < Random.Range(3, 9); i++)
                {
                    rotation = Quaternion.LookRotation(randomDirection, Vector3.Cross(Vector3.Cross(randomDirection, worldUpVector), randomDirection));
                    tempObject = Instantiate(orbs[currentOrb], randomLocation + randomDirection * i, rotation);
                    //tempObject.transform.forward = randomDirection;

                    numberOfOrbs++;
                    if (currentOrb == orbs.Length - 1 && rotateRight)
                    {
                        currentOrb = 0;
                    }
                    else if (currentOrb == 0 && !rotateRight)
                    {
                        currentOrb = orbs.Length - 1;
                    }
                    else
                    {
                        if (rotateRight)
                        {
                            currentOrb++;
                        }
                        else
                        {
                            currentOrb--;
                        }
                    }
                }
            }
        }

        if (numberOfMultipliers < maxNumberOfMultipliers)
        {
            while ( numberOfMultipliers < maxNumberOfMultipliers)
            {
                randomLocation = new Vector3(Random.Range(-14000, 14000), 0, Random.Range(-14000, 14000));
                randomLocation.y = Random.Range(Terrain.activeTerrain.SampleHeight(randomLocation), 1700);
                Instantiate(multiplier, randomLocation, Quaternion.identity);
                numberOfMultipliers++;
            }
        }
    }
}
