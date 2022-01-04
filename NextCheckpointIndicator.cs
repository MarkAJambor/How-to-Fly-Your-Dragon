using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextCheckpointIndicator : MonoBehaviour
{

    public DragonFlightController player;
    public GameObject[] checkpoints;
    public int currentCheckpointGoal = 0;
    public int checkpointsLength;
    public Rigidbody selfRB;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<DragonFlightController>();
        Destroy(this.gameObject, 10);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FixedUpdate()
    {
        if (currentCheckpointGoal > checkpointsLength - 1)
        {
            Destroy(this.gameObject);
        }
        else
        {
            //selfRB.velocity = (checkpoints[currentCheckpointGoal].transform.position - this.transform.position).normalized * 400;
            selfRB.AddForce((checkpoints[currentCheckpointGoal].transform.position - this.transform.position).normalized * 1000, ForceMode.Acceleration);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == checkpoints[currentCheckpointGoal])
        {
            currentCheckpointGoal++;
        }
    }
}
