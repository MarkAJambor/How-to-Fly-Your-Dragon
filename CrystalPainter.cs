using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalPainter : MonoBehaviour {

    public GameObject pillar;
    public Vector3 raycastPosition;
    public GameObject crystal;
    public GameObject tempCrystal;

	// Use this for initialization
	void Start ()
    {
        int count = 0;
        int numberOfCrystals = 25;
        for (int i = 0; i < numberOfCrystals; i++)
        {
            pillar.transform.rotation = Quaternion.Euler(0, i * 360 / numberOfCrystals, 0);
            raycastPosition = new Vector3(1000, Random.Range(8400, 10000), -500);
            RaycastHit hit;
            if (Physics.Raycast(raycastPosition, transform.forward, out hit, 1000))
            {
                count++;
                tempCrystal = Instantiate(crystal, hit.point, Quaternion.LookRotation(hit.normal));
                tempCrystal.transform.parent = pillar.transform;
            }
        }
        Debug.Log(count);
	}
	
	// Update is called once per frame
	void Update ()
    {


	}
}
