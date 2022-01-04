using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellCrabController : MonoBehaviour
{
    public float torqueForce;
    public float forwardForce;
    public float hitValue;
    public float killValue;
    public float health;
    public float fireballDamage;
    public float flamethrowerDamage;
    public float originalHealth;
    public AudioSource damageSound;
    public AudioSource deathSound;
    public AudioSource motionSound;
    public bool isDead;
    public bool shouldTakeFlamethrowerDamage;
    public GameObject explosionSpawnLocation;
    public Quaternion playerDirectionTorque;
    public DragonFlightController player;
    public Rigidbody rb;
    public Vector3 tempDirection;
    public Vector3 spawnLocation;
    public Terrain activeTerrain;
    public float deathTime;
    public float failureCount;
    public bool shouldTrySpawn;
    public LayerMask spawnSphereMask = (1 << 0) | (1 << 9) | (1 << 12) | (1 << 13) | (1 << 20);

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<DragonFlightController>();
        rb = this.GetComponentInParent<Rigidbody>();
        originalHealth = health;
        InvokeRepeating("changeDirection", 0, 15);
        //int randomX = Random.Range(-13000, 13000);
        //int randomZ = Random.Range(-13000, 13000);
        //this.gameObject.transform.position = new Vector3(-randomX, Terrain.activeTerrain.SampleHeight(new Vector3(-randomX, 0, -randomZ)), -randomZ);
        this.initialSpawn();
    }

    // Update is called once per frame
    void Update()
    {
        if (!player.usingSkyLevel)
        {
            if (!isDead)
            {
                //quat0 = this.transform.rotation;
                //quat1 = Quaternion.LookRotation(aimLocation - self.position);
                //quat10 = quat1* Quaternion.Inverse(quat0);
                playerDirectionTorque = Quaternion.LookRotation(tempDirection) * Quaternion.Inverse(this.transform.rotation);
                rb.AddTorque(playerDirectionTorque.x * torqueForce * Time.deltaTime, playerDirectionTorque.y * torqueForce * Time.deltaTime, playerDirectionTorque.z * torqueForce * Time.deltaTime, ForceMode.Acceleration);
                rb.AddForce(this.transform.forward * forwardForce, ForceMode.Acceleration);
            }
        }
    }

    public void FixedUpdate()
    {
        if (!player.usingSkyLevel)
        {
            if (!motionSound.isPlaying)
            {
                motionSound.Play();
            }
            if (this.transform.position.x >= 0)
            {
                if (this.transform.position.z >= 0)
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
                if (this.transform.position.z >= 0)
                {
                    activeTerrain = player.terrains[1];
                }
                else
                {
                    activeTerrain = player.terrains[0];
                }
            }
            this.transform.position = new Vector3(this.transform.position.x, activeTerrain.SampleHeight(new Vector3(this.transform.position.x, 0, this.transform.position.z)), this.transform.position.z);
            if (shouldTakeFlamethrowerDamage && !isDead)
            {
                health -= flamethrowerDamage * Time.deltaTime;
                shouldTakeFlamethrowerDamage = false;
                if (!damageSound.isPlaying)
                {
                    damageSound.Play();
                }
            }
            if (health <= 0 && !isDead)
            {
                isDead = true;
                deathTime = Time.time;
                deathSound.Play();
                GameObject explosionTemp = Instantiate(player.explosion);
                explosionTemp.transform.position = explosionSpawnLocation.transform.position;
                Destroy(explosionTemp, 5);
                player.playerData.crabKills++;
                player.savePlayerData();
            }
            if (Time.time - deathTime >= 1.5f && isDead)
            {
                this.respawn();
                isDead = false;
                health = originalHealth;
                player.baseMultiplierValue += 0.2f;
                player.points += killValue * player.multiplierValue;
            }
        }
    }

    public void initialSpawn()
    {
        shouldTrySpawn = true;
        failureCount = 0;
        while (shouldTrySpawn)
        {
            spawnLocation = new Vector3(Random.Range(-12000, 12000), 0, Random.Range(-12000, 12000));
            if (!Physics.CheckSphere(spawnLocation, 100, spawnSphereMask))
            {
                this.gameObject.transform.position = spawnLocation;
                shouldTrySpawn = false;
            }
            else
            {
                failureCount++;
            }
            if (failureCount > 20)
            {
                this.gameObject.transform.position = spawnLocation;
                shouldTrySpawn = false;
            }
        }
    }

    public void respawn()
    {
        shouldTrySpawn = true;
        failureCount = 0;
        while (shouldTrySpawn)
        {
            if (player.transform.position.x >= 0)
            {
                if (player.transform.position.z >= 0)
                {
                    spawnLocation = new Vector3(-Random.Range(1000, 5000), activeTerrain.SampleHeight(new Vector3(-5000, 0, -5000)) + 100, -Random.Range(1000, 5000));
                }
                else
                {
                    spawnLocation = new Vector3(-Random.Range(1000, 5000), activeTerrain.SampleHeight(new Vector3(-5000, 0, 5000)) + 100, Random.Range(1000, 5000));
                }
            }
            else
            {
                if (player.transform.position.z >= 0)
                {
                    spawnLocation = new Vector3(Random.Range(1000, 5000), activeTerrain.SampleHeight(new Vector3(5000, 0, -5000)) + 100, -Random.Range(1000, 5000));
                }
                else
                {
                    spawnLocation = new Vector3(Random.Range(1000, 5000), activeTerrain.SampleHeight(new Vector3(5000, 0, 5000)) + 100, Random.Range(1000, 5000));
                }
            }
            if (!Physics.CheckSphere(spawnLocation, 100, spawnSphereMask))
            {
                this.gameObject.transform.position = spawnLocation;
                shouldTrySpawn = false;
            }
            else
            {
                failureCount++;
            }
            if (failureCount > 20)
            {
                this.gameObject.transform.position = spawnLocation;
                shouldTrySpawn = false;
            }
        }
    }

    public void changeDirection()
    {
        tempDirection = new Vector3(Random.Range(-14000, 14000), this.transform.position.y, Random.Range(-14000, 14000)) - this.transform.position;
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Fireball" && !isDead)
        {
            player.points += hitValue * player.multiplierValue;
            health -= fireballDamage;
            damageSound.Play();
            //isDead = true;
        }
    }
}
