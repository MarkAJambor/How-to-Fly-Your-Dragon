using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballController : MonoBehaviour {

    public GameObject fireballExplosion;
    public GameObject woodenShieldExplosionEffect;
    public DragonFlightController mainDragon;
    public Rigidbody selfRB;
    public PlanetOrbSpawner planetOrbSpawner;

	// Use this for initialization
	void Start ()
    {
        mainDragon = FindObjectOfType<DragonFlightController>();
        planetOrbSpawner = FindObjectOfType<PlanetOrbSpawner>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void FixedUpdate()
    {
        selfRB.AddForce(-this.transform.forward * 0.05f * Mathf.Pow(selfRB.transform.InverseTransformDirection(selfRB.velocity).z, 1.7f) * mainDragon.elevationLiftValue, ForceMode.Force);
        if (mainDragon.usingSkyLevel)
        {
            selfRB.useGravity = false;
            selfRB.AddForce((mainDragon.skyLevelAttractionPoint.transform.position - this.transform.position).normalized * 9.8f, ForceMode.Acceleration);
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Shield")
        {
            GameObject explosion = Instantiate(woodenShieldExplosionEffect);
            explosion.transform.position = collision.collider.transform.position;
            Destroy(explosion, 4);
            collision.collider.gameObject.SetActive(false);
        }
        GameObject temp = Instantiate(fireballExplosion);
        temp.transform.position = this.transform.position;
        Destroy(this.gameObject);

        if (collision.collider.tag == "Ship")
        {
            VikingShipController ship = collision.collider.GetComponentInParent<VikingShipController>();
            if (ship != null)
            {
                ship.player.points += ship.hitValue * ship.player.multiplierValue;
                ship.health -= ship.fireballDamage;
            }
        }
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Point Claw")
        {
            mainDragon.greenValue = other.GetComponentInChildren<MeshRenderer>().material.color.g;
            mainDragon.blueValue = other.GetComponentInChildren<MeshRenderer>().material.color.b;
            mainDragon.shouldAddPoints = true;
            Destroy(other.gameObject);
            FindObjectOfType<OrbSpawner>().numberOfOrbs--;
            mainDragon.playPointsSound(mainDragon.greenValue, mainDragon.blueValue);
        }

        if (other.tag == "Planet Orb")
        {
            mainDragon.greenValue = 1;
            mainDragon.blueValue = 0;
            mainDragon.shouldAddPoints = true;
            Destroy(other.gameObject);
            planetOrbSpawner.spawnNextOrb();
            mainDragon.playPointsSound(1, 0);
        }

        if (other.tag == "Multiplier")
        {
            if (mainDragon.usingSkyLevel)
            {
                mainDragon.baseMultiplierValue += 0.5f;
                planetOrbSpawner.spawnNextMultiplier();
            }
            else if (mainDragon.usingCaveLevel)
            {
                mainDragon.shouldAddMultiplier = true;
                mainDragon.orbSpawner.numberOfMultipliers--;
            }
            Destroy(other.transform.parent.gameObject);
            mainDragon.playMultiplierSound();
        }
        if (other.gameObject.layer != 22)
        {
            GameObject temp = Instantiate(fireballExplosion);
            temp.transform.position = this.transform.position;
            Destroy(this.gameObject);
        }
    }
}
