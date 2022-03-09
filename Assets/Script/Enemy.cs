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
    public float aggroRange;
    private Rigidbody rb;
    Vector3 v3Force;
    private bool canPropulse;
    public bool isChasse;
    public bool isFlee;
    public GameObject target;
    public Vector3 targetPosition;
    public Collider coll;
    float savedTimeProp, propCD;
    bool isBlind;
    [SerializeField]public float blindtime;
    float savedBlindTime;
    float savedTimeKnok;
    public float knokCD;
    
    int chases;
    // Start is called before the first frame update
    void Start()
    {
        rb = this.gameObject.GetComponent<Rigidbody>();
        savedTimeProp = Time.time;
        savedBlindTime = Time.time;
        isBlind = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isKnoc)
        {
            propCD = 0.5f;
            target = GameObject.Find("Player Boddy");
            targetPosition = GameObject.Find("Player Boddy").transform.position;

            //rb.AddForce(new Vector3(0,-1*Time.deltaTime,0),ForceMode.VelocityChange);  
            rb.velocity -= deceleration * rb.velocity * Time.deltaTime;
            if (!isBlind)
            {
                EnemySetChasse();
            }
            if (isBlind)
            {
                PropulsionEnemy();
            }
            if (Time.time > savedBlindTime + blindtime)
            {
                isBlind = false;
            }
            Debug.Log(chases);
        }
        else
        {
            if(Time.time > savedTimeKnok + knokCD) 
            {
                isKnoc = false;
                savedTimeKnok = Time.time;
                
            }
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Ancre")
        {
            Vector3 origineScal = other.transform.localScale;
            isBlind = true;
            savedBlindTime = Time.time;
            //other.gameObject.transform.position = this.transform.position;
            //other.transform.localScale = new Vector3(1,1,1);
        }
        if (other.tag == "WallStun")
        {
            isKnoc = true;
            savedTimeKnok = Time.time;
        }
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
        if(transform.localScale.magnitude <= target.transform.localScale.magnitude)
        {
            isChasse = false;
            isFlee = true;
            chases = -1;
            if (Vector3.Distance(transform.position, target.transform.position) < aggroRange)
            {
                Chasse();
                PropulsionEnemy();
            }
        } 
        else
        {
            isChasse = true;
            isFlee = false;
            chases = 1;
            if (Vector3.Distance(transform.position, target.transform.position) < aggroRange)
            {
                Chasse();
                PropulsionEnemy();
            }
        }
    }
    public void Chasse()
    {
        transform.LookAt(target.transform, transform.forward); //* Time.deltaTime * speed*chases
        
    }
    public void PropulsionEnemy()
    {
        if (Time.time > savedTimeProp + propCD)
        {
            v3Force = propulsionForce * transform.forward*chases;        
            rb.AddForce(v3Force,ForceMode.Impulse);
            savedTimeProp = Time.time;
        }
        
    }

}
