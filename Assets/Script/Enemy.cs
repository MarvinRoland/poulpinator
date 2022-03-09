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
    public bool isChasse;
    public bool isFlee;
    public GameObject target;
    public Vector3 targetPosition;
    // Start is called before the first frame update
    void Start()
    {
        rb = this.gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        target = GameObject.Find("Player Boddy");
        targetPosition = GameObject.Find("Player Boddy").transform.position;
        EnemySetChasse();

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
    public void EnemySetChasse()
    {        
        if(transform.localScale.magnitude <= target.transform.localScale.magnitude*0.7f)
        {
            isChasse = false;
            isFlee = true;
        } 
    }
}
