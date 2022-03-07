using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bouffe : MonoBehaviour
{
    public bool isToxicFood;
    public float speed, propulsionForce;
    float z;
    float rotationSpeed = 100;
    int dir;
    Vector3 v3Force;
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        isToxicFood = true;
        //Debug.Log("propulse");
        
    }

    // Update is called once per frame
    void Update()
    {
        dir = Random.Range(-10,11);
        z += Time.deltaTime * rotationSpeed * dir;

            if (z > 360.0f)
            {
                z = 0.0f;
                
            }
        transform.localRotation = Quaternion.Euler(0,0,z);
        v3Force = propulsionForce * transform.up*speed*Time.deltaTime;
        rb.AddForce(v3Force,ForceMode.Acceleration);
        //transform.position = Vector3.Lerp(transform.position,transform.up,1);
    }
}
