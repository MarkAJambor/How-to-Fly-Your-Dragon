using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GPUInstancer;

public class PillarSpawner : MonoBehaviour {

    public bool pillarsEnabled;
    public bool mushroomsEnabled;
    public bool groundCrystalsEnabled;
    public bool levelHasBeenLoaded = false;
    public GameObject[] colorLights;
    public GameObject loadingPanel;
    public Vector2 randomPosition;
    public Vector2 corner1;
    public Vector2 corner2;
    public Vector2 corner3;
    public Vector2 corner4;
    public Vector3 spawnLocation;
    public Vector3 tempScale;
    public float exclusionCircleRadius;
    public float numberOfPillars;
    public float numberOfMushrooms;
    public float numberOfGroundCrystals;
    public float d1;
    public float d2;
    public float d3;
    public float d4;
    public float x;
    public float y;
    public float z;
    public float largestDistance;
    public int color;
    public int overlapProbability;
    public int framesSinceStart;
    public Terrain activeTerrain;
    public DragonFlightController player;
    public List<GPUInstancerPrefab> pillarsGPU = new List<GPUInstancerPrefab>();
    public GPUInstancerPrefabManager GPUIManager;
    public GPUInstancerPrefab[] GPUPillarsArray;
    public GPUInstancerPrefab[] GPUGroundCrystalArray;
    public GPUInstancerPrefab mushroom;
    public GPUInstancerPrefab temp;

	// Use this for initialization
	void Start ()
    {
        corner1 = new Vector2(-14000, 14000);
        corner2 = new Vector2(14000, 14000);
        corner3 = new Vector2(-14000, -14000);
        corner4 = new Vector2(14000, -14000);
        player = FindObjectOfType<DragonFlightController>();
        //Instantiate(colorLights[0], new Vector3(-7000, 1500, -7000), Quaternion.identity);
        //Instantiate(colorLights[1], new Vector3(7000, 1500, -7000), Quaternion.identity);
        //Instantiate(colorLights[2], new Vector3(7000, 1500, 7000), Quaternion.identity);
        //Instantiate(colorLights[3], new Vector3(-7000, 1500, 7000), Quaternion.identity);
    }

    public void Update()
    {
        if (!levelHasBeenLoaded)
        {
            if (framesSinceStart > 1)
            {
                this.spawnGroundCrystals();
                this.spawnMushrooms();
                this.spawnPillars();
                loadingPanel.SetActive(false);
                levelHasBeenLoaded = true;
                player.pauseGame();
            }
            else
            {
                foreach (ShellCrabController crab in player.shellCrabArray)
                {
                    crab.motionSound.Stop();
                }
                player.spider.motionSound.Stop();
                player.stoneMonster.motionSound.Stop();
                framesSinceStart++;
                loadingPanel.SetActive(true);
            }
        }
    }

    public void spawnGroundCrystals()
    {
        //spawn mushrooms
        if (groundCrystalsEnabled)
        {
            for (int i = 0; i < numberOfGroundCrystals; i++)
            {
                x = Random.Range(-14000, 14000);
                z = Random.Range(-14000, 14000);
                randomPosition = new Vector2(x, z);
                if (randomPosition.sqrMagnitude > exclusionCircleRadius * exclusionCircleRadius || (randomPosition.x > -200 && randomPosition.x < 200 && randomPosition.y > 1100 && randomPosition.y < 2000))
                {
                    i--;
                    //Debug.Log("failed");
                }
                else
                {
                    if (x >= 0)
                    {
                        if (z >= 0)
                        {
                            activeTerrain = player.terrains[3];
                        }
                        else
                        {
                            activeTerrain = player.terrains[2];
                        }
                    }
                    else
                    {
                        if (z >= 0)
                        {
                            activeTerrain = player.terrains[1];
                        }
                        else
                        {
                            activeTerrain = player.terrains[0];
                        }
                    }
                    spawnLocation = new Vector3(x, activeTerrain.SampleHeight(new Vector3(x, 0, z)), z);
                    GPUInstancerPrefab allocatedGO = Instantiate(GPUGroundCrystalArray[Random.Range(0, 9)], spawnLocation, Quaternion.Euler(0, Random.Range(0, 360), 0));
                    tempScale = allocatedGO.transform.localScale;
                    tempScale *= Random.Range(1, 4);
                    allocatedGO.transform.localScale = tempScale;
                    pillarsGPU.Add(allocatedGO);
                }
            }
        }
    }

    public void spawnMushrooms()
    {
        //spawn mushrooms
        if (mushroomsEnabled)
        {
            for (int i = 0; i < numberOfMushrooms; i++)
            {
                x = Random.Range(-14000, 14000);
                z = Random.Range(-14000, 14000);
                randomPosition = new Vector2(x, z);
                if (randomPosition.sqrMagnitude < exclusionCircleRadius * exclusionCircleRadius)
                {
                    i--;
                    //Debug.Log("failed");
                }
                else
                {
                    if (x >= 0)
                    {
                        if (z >= 0)
                        {
                            activeTerrain = player.terrains[3];
                        }
                        else
                        {
                            activeTerrain = player.terrains[2];
                        }
                    }
                    else
                    {
                        if (z >= 0)
                        {
                            activeTerrain = player.terrains[1];
                        }
                        else
                        {
                            activeTerrain = player.terrains[0];
                        }
                    }
                    spawnLocation = new Vector3(x, activeTerrain.SampleHeight(new Vector3(x, 0, z))-75, z);
                    GPUInstancerPrefab allocatedGO = Instantiate(mushroom, spawnLocation, Quaternion.Euler(0, Random.Range(0, 360), 0));
                    pillarsGPU.Add(allocatedGO);
                }
            }
        }
    }

    public void spawnPillars()
    {
        //spawn pillars
        if (pillarsEnabled)
        {
            for (int i = 0; i < numberOfPillars; i++)
            {
                x = Random.Range(-14000, 14000);
                y = Random.Range(-14000, 14000);
                randomPosition = new Vector2(x, y);
                if (randomPosition.sqrMagnitude < exclusionCircleRadius * exclusionCircleRadius)
                {
                    i--;
                    //Debug.Log("failed");
                }
                else
                {
                    spawnLocation = new Vector3(randomPosition.x, 0, randomPosition.y);
                    d1 = Vector2.Distance(randomPosition, corner1);
                    d2 = Vector2.Distance(randomPosition, corner2);
                    d3 = Vector2.Distance(randomPosition, corner3);
                    d4 = Vector2.Distance(randomPosition, corner4);
                    d1 += Random.Range(0, overlapProbability);
                    d2 += Random.Range(0, overlapProbability);
                    d3 += Random.Range(0, overlapProbability);
                    d4 += Random.Range(0, overlapProbability);
                    largestDistance = d1;
                    color = 0;
                    if (d2 > largestDistance)
                    {
                        largestDistance = d2;
                        color = 1;
                    }
                    if (d3 > largestDistance)
                    {
                        largestDistance = d3;
                        color = 2;
                    }
                    if (d4 > largestDistance)
                    {
                        largestDistance = d4;
                        color = 3;
                    }
                    GPUInstancerPrefab allocatedGO = Instantiate(GPUPillarsArray[color], new Vector3(randomPosition.x, 2121, randomPosition.y), Quaternion.Euler(0, Random.Range(0, 360), 0));
                    pillarsGPU.Add(allocatedGO);
                }
            }
        }
    }
}
