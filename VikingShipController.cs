using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VikingShipController : MonoBehaviour
{
    public GameObject fireballExplosion;
    public GameObject flame1;
    public GameObject flame2;
    public GameObject flame3;
    public GameObject explosion;
    public GameObject woodPiece;
    public GameObject cannon1;
    public GameObject cannon2;
    public GameObject crowsNest;
    public GameObject cannonball;
    public GameObject cannonAimLocation;
    public GameObject cannonFiringEffect;
    public DragonFlightController player;
    public AudioSource fireSound;
    public AudioSource destructionSound;
    public AudioSource announceDragon;
    public AudioSource cannonFiring;
    public AudioClip[] alertClips;
    public float cannonFireDelay;
    public float cannonballSpeed;
    public float originalHealth;
    public float health;
    public float flamethrowerDamage;
    public float fireballDamage;
    public float killValue;
    public float hitValue;
    public float sightDistance;
    public float lastCannon1Fire = 0;
    public float lastCannon2Fire = 0;
    public bool dragonWithinRange;
    public bool dragonWasAnnounced;
    public bool shouldTakeFlamethrowerDamage;
    public bool shouldDie;
    public LayerMask dragonSightlineMask;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<DragonFlightController>();
        originalHealth = health;
        announceDragon.clip = alertClips[0];
        cannonAimLocation = player.cannonAimLocation;
    }

    public void FixedUpdate()
    {
        if (shouldTakeFlamethrowerDamage)
        {
            health -= flamethrowerDamage * Time.deltaTime;
            shouldTakeFlamethrowerDamage = false;
        }
        if (health < originalHealth)
        {
            if (!fireSound.isPlaying)
            {
                fireSound.Play();
            }

            if (health < originalHealth * 0.75f)
            {
                flame1.SetActive(true);
            }
            if (health < originalHealth * 0.5f)
            {
                flame2.SetActive(true);
            }
            if (health < originalHealth * 0.25f)
            {
                flame3.SetActive(true);
            }
        }
        if (health <= 0)
        {
            if (destructionSound.isPlaying)
            {

            }
            else if (!shouldDie)
            {
                //GameObject explosionTemp = Instantiate(explosion);
                //explosionTemp.transform.position = this.transform.position;
                //Destroy(explosionTemp, 5);
                for (int i = 0; i < 25; i++)
                {
                    GameObject temp = Instantiate(woodPiece);
                    temp.transform.position = this.transform.position;
                    temp.GetComponentInChildren<WoodShrapnelController>().attractionPoint = player.skyLevelAttractionPoint;
                }
                destructionSound.Play();
                shouldDie = true;
            }
            if (shouldDie && !destructionSound.isPlaying)
            {
                player.baseMultiplierValue += 1f;
                player.points += killValue * player.multiplierValue;
                GameObject temp = Instantiate(fireballExplosion);
                temp.transform.position = this.transform.position;
                shouldDie = false;
                this.gameObject.SetActive(false);
            }
        }

        //fire cannonballs at dragon
        if ((crowsNest.transform.position - player.transform.position).sqrMagnitude < Mathf.Pow(sightDistance, 2))
        {
            //dragon is within sight distance
            RaycastHit hit;
            if (Physics.Raycast(crowsNest.transform.position, player.transform.position - crowsNest.transform.position, out hit, sightDistance, dragonSightlineMask))
            {
                //raycast hit something
                if (hit.collider.gameObject.layer == 11)
                {
                    //player was seen
                    if (!dragonWasAnnounced)
                    {
                        //dragon was sighted for the first time
                        int clipNumber = (int)Random.Range(0, 602);
                        if (clipNumber < 100)
                        {
                            announceDragon.clip = alertClips[0];
                        }
                        else if (clipNumber < 200)
                        {
                            announceDragon.clip = alertClips[1];
                        }
                        else if (clipNumber < 300)
                        {
                            announceDragon.clip = alertClips[2];
                        }
                        else if (clipNumber < 400)
                        {
                            announceDragon.clip = alertClips[3];
                        }
                        else if (clipNumber < 500)
                        {
                            announceDragon.clip = alertClips[4];
                        }
                        else if (clipNumber < 600)
                        {
                            announceDragon.clip = alertClips[5];
                        }
                        else if (clipNumber == 600)
                        {
                            announceDragon.clip = alertClips[6];
                        }
                        else if (clipNumber == 601)
                        {
                            announceDragon.clip = alertClips[7];
                        }
                        else
                        {
                            announceDragon.clip = alertClips[0];
                        }
                        announceDragon.Play();
                        dragonWasAnnounced = true;
                    }
                    else if (dragonWasAnnounced && !announceDragon.isPlaying)
                    {
                        //try to fire each cannons
                        if (Vector3.Angle(player.transform.position - cannon1.transform.position, cannon1.transform.forward) < 60)
                        {
                            //try to fire cannon 1
                            if (Time.time - lastCannon1Fire > cannonFireDelay)
                            {
                                //fire cannon 1
                                lastCannon1Fire = Time.time;
                                cannonFiring.Play();
                                GameObject temp = Instantiate(cannonball);
                                temp.transform.position = cannon1.transform.position;
                                temp.GetComponent<Rigidbody>().velocity = (cannonAimLocation.transform.position - cannon1.transform.position).normalized * cannonballSpeed;
                                Destroy(temp, 15);
                                GameObject effect = Instantiate(cannonFiringEffect);
                                effect.transform.position = cannon1.transform.position;
                                effect.GetComponent<ParticleSystem>().Play();
                                Destroy(effect, 4);
                            }
                        }
                        if (Vector3.Angle(player.transform.position - cannon2.transform.position, cannon2.transform.forward) < 60)
                        {
                            //try to fire cannon 2
                            if (Time.time - lastCannon2Fire > cannonFireDelay)
                            {
                                //fire cannon 2
                                lastCannon2Fire = Time.time;
                                cannonFiring.Play();
                                GameObject temp = Instantiate(cannonball);
                                temp.transform.position = cannon2.transform.position;
                                temp.GetComponent<Rigidbody>().velocity = (cannonAimLocation.transform.position - cannon2.transform.position).normalized * cannonballSpeed;
                                Destroy(temp, 15);
                                GameObject effect = Instantiate(cannonFiringEffect);
                                effect.transform.position = cannon2.transform.position;
                                effect.GetComponent<ParticleSystem>().Play();
                                Destroy(effect, 4);
                            }
                        }
                    }
                }
                else
                {
                    if (dragonWasAnnounced)
                    {
                        dragonWasAnnounced = false;
                    }
                }
            }
            else
            {
                dragonWasAnnounced = false;
            }
        }
        else
        {
            dragonWasAnnounced = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
