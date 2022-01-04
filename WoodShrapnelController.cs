using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodShrapnelController : MonoBehaviour
{
    public GameObject attractionPoint;
    public Rigidbody selfRB;

    // Start is called before the first frame update
    void Start()
    {
        selfRB = this.GetComponent<Rigidbody>();
        selfRB.velocity = new Vector3(Random.Range(-80, 80), Random.Range(75, 100), Random.Range(-80, 80));
        selfRB.angularVelocity = new Vector3(Random.Range(-10, 10), Random.Range(-10, 10), Random.Range(-10, 10));
        Destroy(this.gameObject, 20f);
    }

    // Update is called once per frame
    void Update()
    {
        selfRB.AddForce((attractionPoint.transform.position - this.transform.position).normalized * 50f, ForceMode.Acceleration);
    }
}
