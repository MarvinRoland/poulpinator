using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class Crabe : MonoBehaviour
{
    [SerializeField] public bool estActif;
    [SerializeField] int crabePointDeVie;
    [SerializeField] float crabeSpeed; 
    [SerializeField] float propulsionForce;
    [SerializeField] int bulleNombreParAttaque;
    [SerializeField] float bulleMinTaille, bulleMMaxTaille;
    [SerializeField] float bulleSpeedMin, bulleSpeedMax;
    [SerializeField] float bulleAngleDirection;
    [SerializeField] float bulleAttaqueCD, mooveCD, chasseCD, jumpCD;
    [SerializeField] float bulleTempsEntreChaqueBulle;
    [SerializeField] GameObject bulleObject;
    [SerializeField] float stunTimer;
    [SerializeField] public GameObject crabEvent;
    Vector3 v3Force;
    
    Rigidbody rb;
    private bool isBulleAttaque, isMoove, isChasse, isJump;
    public bool isKnoc = false;
    private float savedTimeBulle, savedTimeBulleAttaque, savedTimeMoove, savedTimeChasse, savedTimeJump, savedTimeKnoc, savedTimePv, pvCD;
    private int nbBulle;
    private Vector3 targetPosition, goToPosition;
    private float targetX, targetY, targetZ;
    // Start is called before the first frame update
    void Start()
    {
        savedTimePv = Time.time;
        pvCD = 1;
        rb = this.GetComponent<Rigidbody>();
        savedTimeKnoc = Time.time;
        savedTimeBulle = Time.time;
        savedTimeBulleAttaque = Time.time;
        savedTimeMoove = Time.time;
        savedTimeChasse = Time.time;
        savedTimeJump = Time.time;
        isBulleAttaque = true;
        isChasse = true;
        estActif = false;
    }

    // Update is called once per frame
    public void Update()
    {      
        if (estActif)
        {
            Jump();          
            AttaqueBulle();
            SetBulleAttaqueCD();
            Chasse();
            Moove();
            Knoc();
            
        }
        else 
        {
            savedTimeJump = Time.time;
        }
        if (crabePointDeVie <= 0)
        {
            estActif = false;
            crabEvent.GetComponent<crabevent>().Eboulement();
        }
        rb.AddForce(new Vector3(0,-8*Time.deltaTime,0),ForceMode.VelocityChange);
    }
    public void AttaqueBulle()
    {
        if(Time.time > savedTimeBulle + bulleTempsEntreChaqueBulle + Random.Range(-0.1f,0.1f) && isBulleAttaque)
        {
            Vector3 startBullePosition = new Vector3(this.transform.position.x+Random.Range(-1,1), this.transform.position.y+5, this.transform.position.z);
            GameObject bulle = Instantiate(bulleObject,startBullePosition,Quaternion.identity);
            bulle.GetComponent<Bulle>().speed = Random.Range(bulleSpeedMin,bulleSpeedMax);
            bulle.transform.localRotation = Quaternion.Euler(0,0,Random.Range(-bulleAngleDirection,bulleAngleDirection));
            float randsize = Random.Range(bulleMinTaille, bulleMMaxTaille);
            bulle.transform.localScale = new Vector3(bulle.transform.localScale.x * randsize,bulle.transform.localScale.y * randsize, bulle.transform.localScale.z * randsize);//Random.Range(bulleMinTaille, bulleMMaxTaille);
            nbBulle += 1;
            savedTimeBulle = Time.time;
            if(nbBulle >= bulleNombreParAttaque)
            {
                isBulleAttaque = false;
            }
        }
        
    }
    public void SetBulleAttaqueCD()
    {
        if (Time.time > savedTimeBulleAttaque + bulleAttaqueCD)
        {
            nbBulle = 0;
            isBulleAttaque = true;
            savedTimeBulleAttaque = Time.time;

        }
    }
    public void Moove()
    {
        if(Time.time > savedTimeMoove + mooveCD && isMoove == true)
        {
            goToPosition = new Vector3(targetX,transform.position.y,transform.position.z);
            transform.position = Vector3.Lerp(transform.position, goToPosition, Time.deltaTime*crabeSpeed);
            savedTimeMoove = Time.time;
            //Debug.Log("is in moove" + "**" + targetPosition + "**" + goToPosition);
            if (Time.time > savedTimeChasse + chasseCD && isChasse == false)
            {
                isMoove = false;
                savedTimeMoove = Time.time;  
                isChasse = true   ; 
                Debug.Log(isChasse);          
            }
        }
    }
    public void Chasse()
    {
        if(Time.time > savedTimeChasse + chasseCD && isChasse == true)
        {
            targetPosition = GameObject.Find("Player Boddy").transform.position;
            targetX = targetPosition.x;
            targetY = targetPosition.y;
            targetZ = targetPosition.z;
            isMoove = true;
            isChasse = false;            
            savedTimeChasse = Time.time;
            //Debug.Log("is in chasse" + "**" + targetPosition + "**");
        }
    }
    public void Jump()
    {
        if (Time.time > savedTimeJump + jumpCD && !isKnoc)
        {
            isJump = true;
        }
        if(isJump)
        {
            isMoove = false;
            isChasse = false;
            isBulleAttaque = false;

            v3Force = propulsionForce * transform.up;
            Debug.Log("propulse");
            rb.AddForce(v3Force,ForceMode.Impulse);
            isJump = false;
            savedTimeJump = Time.time;
            isChasse = true;
        }
        
    }
    public void Knoc()
    {
        if (isKnoc)
        {
            isMoove = false;
            isChasse = false;
            isBulleAttaque = false;
            isJump = false;
            if (savedTimeKnoc + stunTimer < Time.time)
            {
                isKnoc = false;
                isChasse = true;
                savedTimeJump = Time.time;
                Debug.Log(isKnoc + "knock");
            }            
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "WallStun" && savedTimePv + pvCD < Time.time)
        {
            isKnoc = true;
            savedTimeKnoc = Time.time;
            crabePointDeVie -= 1;
            savedTimePv = Time.time;
        }
    }
}
