using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderController : MonoBehaviour
{
    public float torqueForce;
    public float forwardForce;
    public float hitValue;
    public float killValue;
    public float health;
    public float fireballDamage;
    public float flamethrowerDamage;
    public float spiderShotSpeed;
    public float spiderShotDelay;
    public GameObject spiderShot;
    public GameObject spiderShotSpawnLocation;
    public AudioSource damageSound;
    public AudioSource deathSound;
    public AudioSource motionSound;
    public const string RUN = "Run";
    public const string ATTACK = "Attack";
    public const string WALK = "Walk";
    public const string DEATH = "Death";
    public float originalHealth;
    public float lastShotTime = 0;
    public bool isDead;
    public bool shouldTakeFlamethrowerDamage;
    public GameObject explosionSpawnLocation;
    public Quaternion playerDirectionTorque;
    public DragonFlightController player;
    public Rigidbody rb;
    public Vector3 tempDirection;
    public Vector3 spawnLocation;
    public GameObject tempSpiderShot;
    public float distanceToPlayer;
    public float deathTime;
    public Terrain activeTerrain;
    public bool shouldTrySpawn;
    public float failureCount;
    public LayerMask spawnSphereMask = (1 << 0) | (1 << 9) | (1 << 12) | (1 << 13) | (1 << 20);

    Animation anim;

    void Start()
    {
        player = FindObjectOfType<DragonFlightController>();
        anim = GetComponent<Animation>();
        rb = this.GetComponentInChildren<Rigidbody>();
        originalHealth = health;
        InvokeRepeating("changeDirection", 0, 15);
        //int randomX = Random.Range(-13000, 13000);
        //int randomZ = Random.Range(-13000, 13000);
        //this.gameObject.transform.position = new Vector3(-randomX, Terrain.activeTerrain.SampleHeight(new Vector3(-randomX, 0, -randomZ)), -randomZ);
        this.initialSpawn();
    }

    public void walkAnimation()
    {
        anim.CrossFade(WALK);
    }

    public void deathAnimation()
    {
        anim.CrossFade(DEATH);
    }

    // Update is called once per frame
    void Update()
    {
        if (!player.usingSkyLevel)
        {
            if (!isDead)
            {
                this.walkAnimation();
                //quat0 = this.transform.rotation;
                //quat1 = Quaternion.LookRotation(aimLocation - self.position);
                //quat10 = quat1* Quaternion.Inverse(quat0);
                playerDirectionTorque = Quaternion.LookRotation(tempDirection) * Quaternion.Inverse(this.transform.rotation);
                rb.AddTorque(playerDirectionTorque.x * torqueForce * Time.deltaTime, playerDirectionTorque.y * torqueForce * Time.deltaTime, playerDirectionTorque.z * torqueForce * Time.deltaTime, ForceMode.Acceleration);
                rb.AddForce(this.transform.forward * forwardForce, ForceMode.Acceleration);
                distanceToPlayer = (player.transform.position - this.transform.position).magnitude;
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
            this.transform.position = new Vector3(this.transform.position.x, activeTerrain.SampleHeight(this.transform.position), this.transform.position.z);
            if (distanceToPlayer < 3000)
            {
                tempDirection = new Vector3(player.transform.position.x, this.transform.position.y, player.transform.position.z) - this.transform.position;
            }
            if (distanceToPlayer < 1500)
            {
                if (Time.time - lastShotTime > spiderShotDelay)
                {
                    RaycastHit hit;
                    if (Physics.Raycast(spiderShotSpawnLocation.transform.position, player.transform.position - spiderShotSpawnLocation.transform.position, out hit, Mathf.Infinity))
                    {
                        if (hit.collider.gameObject.layer == 11)
                        {
                            this.shootSpiderShot();
                            lastShotTime = Time.time;
                        }
                    }
                }
            }
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
                this.deathAnimation();
                GameObject explosionTemp = Instantiate(player.explosion);
                explosionTemp.transform.position = explosionSpawnLocation.transform.position;
                Destroy(explosionTemp, 5);
                player.playerData.spiderKills++;
                player.savePlayerData();
            }
            if (Time.time - deathTime >= 1.5f && isDead)
            {
                this.respawn();
                isDead = false;
                health = originalHealth;
                player.baseMultiplierValue += 0.6f;
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

    public void shootSpiderShot()
    {
        tempSpiderShot = Instantiate(spiderShot);
        tempSpiderShot.transform.position = spiderShotSpawnLocation.transform.position;
        tempSpiderShot.GetComponent<Rigidbody>().velocity = (player.transform.position - spiderShotSpawnLocation.transform.position).normalized * spiderShotSpeed;
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
