using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderShotController : MonoBehaviour
{
    public float spawnTime;
    public GameObject spiderShotDestruction;

    // Start is called before the first frame update
    void Start()
    {
        spawnTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.GetComponentInChildren<DragonFlightController>() != null)
        {
            collision.collider.GetComponentInChildren<DragonFlightController>().endGame();
        }
        this.destroyShot();
    }

    public void destroyShot()
    {
        GameObject temp = Instantiate(spiderShotDestruction);
        temp.transform.position = this.transform.position;
        Destroy(temp, 4);
        Destroy(this.gameObject);
    }
}
