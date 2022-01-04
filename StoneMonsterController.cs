using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneMonsterController : MonoBehaviour
{
    public float torqueForce;
    public float forwardForce;
    public float hitValue;
    public float killValue;
    public float health;
    public float fireballDamage;
    public float flamethrowerDamage;
    public float backUpDuration;
    public const string IDLE = "Anim_Idle";
    public const string RUN = "Anim_Run";
    public const string ATTACK = "Anim_Attack";
    public const string DAMAGE = "Anim_Damage";
    public const string DEATH = "Anim_Death";
    public float originalHealth;
    public AudioSource damageSound;
    public AudioSource deathSound;
    public AudioSource motionSound;
    public bool isDead;
    public bool shouldTakeFlamethrowerDamage;
    public GameObject explosionSpawnLocation;
    public Quaternion playerDirectionTorque;
    public Vector3 spawnLocation;
    public Vector3 collisionAvoidanceDirection;
    public DragonFlightController player;
    public Rigidbody rb;
    public Terrain activeTerrain;
    public float deathTime;
    public float backUpTime;
    public bool shouldTrySpawn;
    public bool shouldBackUp;
    public float failureCount;
    public LayerMask spawnSphereMask = (1 << 0) | (1 << 9) | (1 << 12) | (1 << 13) | (1 << 20);

    Animation anim;

    void Start()
    {
        player = FindObjectOfType<DragonFlightController>();
        anim = GetComponent<Animation>();
        rb = this.GetComponentInParent<Rigidbody>();
        originalHealth = health;
        //int randomX = Random.Range(-13000, 13000);
        //int randomZ = Random.Range(-13000, 13000);
        //this.gameObject.transform.position = new Vector3(-randomX, Terrain.activeTerrain.SampleHeight(new Vector3(-randomX, 0, -randomZ)) + 100, -randomZ);
        this.respawn();
    }

    public void Update()
    {
        if (!player.usingSkyLevel)
        {
            if (!isDead)
            {
                this.RunAni();
                //quat0 = this.transform.rotation;
                //quat1 = Quaternion.LookRotation(aimLocation - self.position);
                //quat10 = quat1* Quaternion.Inverse(quat0);
                playerDirectionTorque = Quaternion.LookRotation(player.transform.position - this.transform.position) * Quaternion.Inverse(this.transform.rotation);
                rb.AddTorque(playerDirectionTorque.x* torqueForce * Time.deltaTime, playerDirectionTorque.y* torqueForce * Time.deltaTime, playerDirectionTorque.z* torqueForce * Time.deltaTime, ForceMode.Acceleration);
                if (!shouldBackUp)
                {
                    rb.AddForce(this.transform.forward * forwardForce, ForceMode.Force);
                }
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
            this.setTerrain();
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
                player.playerData.demonKills++;
                player.savePlayerData();
            }
            if (Time.time - deathTime >= 1.5f && isDead)
            {
                this.respawn();
                isDead = false;
                health = originalHealth;
                player.baseMultiplierValue += 0.4f;
                player.points += killValue * player.multiplierValue;
            }
            if (shouldBackUp)
            {
                if (Time.time > backUpTime + backUpDuration)
                {
                    shouldBackUp = false;
                }
                rb.AddForce(collisionAvoidanceDirection.normalized * forwardForce * 2f, ForceMode.Force);
                //Debug.Log("stone monster backing up");
            }
        }
    }

    public void setTerrain()
    {
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
                    spawnLocation = new Vector3(-Random.Range(1000, 5000), 0, -Random.Range(1000, 5000));
                }
                else
                {
                    spawnLocation = new Vector3(-Random.Range(1000, 5000), 0, Random.Range(1000, 5000));
                }
            }
            else
            {
                if (player.transform.position.z >= 0)
                {
                    spawnLocation = new Vector3(Random.Range(1000, 5000), 0, -Random.Range(1000, 5000));
                }
                else
                {
                    spawnLocation = new Vector3(Random.Range(1000, 5000), 0, Random.Range(1000, 5000));
                }
            }
            if (!Physics.CheckSphere(spawnLocation, 100, spawnSphereMask))
            {
                this.gameObject.transform.position = spawnLocation;
                this.setTerrain();
                this.gameObject.transform.position = new Vector3(this.transform.position.x, activeTerrain.SampleHeight(new Vector3(this.transform.position.x, 0, this.transform.position.z)), this.transform.position.z);
                shouldTrySpawn = false;
            }
            else
            {
                failureCount++;
            }
            if (failureCount > 20)
            {
                this.gameObject.transform.position = spawnLocation;
                this.setTerrain();
                this.gameObject.transform.position = spawnLocation;
                shouldTrySpawn = false;
            }
        }
    }

    public void IdleAni()
    {
        anim.CrossFade(IDLE);
    }

    public void RunAni()
    {
        anim.CrossFade(RUN);
    }

    public void AttackAni()
    {
        anim.CrossFade(ATTACK);
    }

    public void DamageAni()
    {
        anim.CrossFade(DAMAGE);
    }

    public void DeathAni()
    {
        anim.CrossFade(DEATH);
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
        else
        {
            if (!shouldBackUp)
            {
                backUpTime = Time.time;
                shouldBackUp = true;
                collisionAvoidanceDirection = this.transform.position - collision.collider.transform.position;
            }
        }
    }
}
