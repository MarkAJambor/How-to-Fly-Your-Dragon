using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GPUInstancer;

public class CeilingRockSpawner : MonoBehaviour
{
    public GameObject[] rocks;
    public GameObject[] colorLights;
    public Vector2 randomPosition;
    public Vector2 corner1;
    public Vector2 corner2;
    public Vector2 corner3;
    public Vector2 corner4;
    public Vector3 spawnLocation;
    public float exclusionCircleRadius;
    public float d1;
    public float d2;
    public float d3;
    public float d4;
    public float x;
    public float y;
    public float largestDistance;
    public int color;
    public List<GPUInstancerPrefab> ceilingRocksGPU = new List<GPUInstancerPrefab>();
    public GPUInstancerPrefabManager GPUIManager;
    public GPUInstancerPrefab[] GPUCeilingRocksArray;

    // Use this for initialization
    void Start()
    {
        
        corner1 = new Vector2(-14000, 14000);
        corner2 = new Vector2(14000, 14000);
        corner3 = new Vector2(-14000, -14000);
        corner4 = new Vector2(14000, -14000);
        //Instantiate(colorLights[0], new Vector3(-7000, 1500, -7000), Quaternion.identity);
        //Instantiate(colorLights[1], new Vector3(7000, 1500, -7000), Quaternion.identity);
        //Instantiate(colorLights[2], new Vector3(7000, 1500, 7000), Quaternion.identity);
        //Instantiate(colorLights[3], new Vector3(-7000, 1500, 7000), Quaternion.identity);

        for (int i = 0; i < 1000; i++)
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
                largestDistance = d1;
                color = Random.Range(3, 7);
                //Instantiate(pillars[color], new Vector3(randomPosition.x, Terrain.activeTerrain.SampleHeight(spawnLocation), randomPosition.y), Quaternion.identity)
                GPUInstancerPrefab allocatedGO = Instantiate(GPUCeilingRocksArray[color], new Vector3(randomPosition.x, 2121, randomPosition.y), Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360)));
                allocatedGO.transform.localScale = new Vector3(Random.Range(100, 600), Random.Range(100, 600), Random.Range(100, 600));
                ceilingRocksGPU.Add(allocatedGO);
            }
        }
        for (int i = -14000; i < 14000; i += 2300)
        {
            color = Random.Range(0, 7);
            GPUInstancerPrefab allocatedGO = Instantiate(GPUCeilingRocksArray[color], new Vector3(i, 1500, 14000), Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360)));
            allocatedGO.transform.localScale = new Vector3(1000, 1000, 1000);
            ceilingRocksGPU.Add(allocatedGO);
            allocatedGO = Instantiate(GPUCeilingRocksArray[color], new Vector3(i, 1500, -14000), Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360)));
            allocatedGO.transform.localScale = new Vector3(1000, 1000, 1000);
            ceilingRocksGPU.Add(allocatedGO);
            allocatedGO = Instantiate(GPUCeilingRocksArray[color], new Vector3(14000, 1500, i), Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360)));
            allocatedGO.transform.localScale = new Vector3(1000, 1000, 1000);
            ceilingRocksGPU.Add(allocatedGO);
            allocatedGO = Instantiate(GPUCeilingRocksArray[color], new Vector3(-14000, 1500, i), Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360)));
            allocatedGO.transform.localScale = new Vector3(1000, 1000, 1000);
            ceilingRocksGPU.Add(allocatedGO);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
