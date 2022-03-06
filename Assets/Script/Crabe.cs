using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class Crabe : MonoBehaviour
{
    [SerializeField] float crabeSpeed; 
    [SerializeField] int bulleNombreParAttaque;
    [SerializeField] float bulleMinTaille, bulleMMaxTaille;
    [SerializeField] float bulleSpeedMin, bulleSpeedMax;
    [SerializeField] float bulleAngleDirection;
    [SerializeField] float bulleAttaqueCD, mooveCD, chasseCD;
    [SerializeField] float bulleTempsEntreChaqueBulle;
    [SerializeField] GameObject bulleObject;


    private bool isBulleAttaque, isMoove, isChasse;
    private float savedTimeBulle, savedTimeBulleAttaque, savedTimeMoove, savedTimeChasse;
    private int nbBulle;
    private Vector3 targetPosition, goToPosition;
    private float targetX, targetY, targetZ;
    // Start is called before the first frame update
    void Start()
    {
        savedTimeBulle = Time.time;
        savedTimeBulleAttaque = Time.time;
        savedTimeMoove = Time.time;
        savedTimeChasse = Time.time;
        isBulleAttaque = true;
        isChasse = true;
    }

    // Update is called once per frame
    async void Update()
    {        
        AttaqueBulle();
        SetBulleAttaqueCD();
        Chasse();
        Moove();
        
    }
    public void AttaqueBulle()
    {
        if(Time.time > savedTimeBulle + bulleTempsEntreChaqueBulle + Random.Range(-0.1f,0.1f) && isBulleAttaque)
        {
            Vector3 startBullePosition = new Vector3(this.transform.position.x+Random.Range(-1,1), this.transform.position.y, this.transform.position.z);
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
    
}
