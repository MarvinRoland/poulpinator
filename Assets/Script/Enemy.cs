using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float propulsionForce, deceleration;
    public bool isKnoc = false;
    public int pointDeVie;
    public float speed;
    public bool isPredateur = true;
    private Rigidbody rb;
    Vector3 v3Force;
    private bool canPropulse;
    // Start is called before the first frame update
    void Start()
    {
        rb = this.gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Propulsion()
    {
        if (canPropulse)
        {
            v3Force = propulsionForce * transform.up;
            Debug.Log("propulse");
            rb.AddForce(v3Force,ForceMode.Impulse);
            
        }
        else
        {
            
            rb.velocity -= deceleration*rb.velocity* Time.deltaTime;
        }
    }
}
