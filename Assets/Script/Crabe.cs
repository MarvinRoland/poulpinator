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


    private bool isBulleAttaque;
    private float savedTimeBulle, savedTimeBulleAttaque, savedTimeMoove, savedTimeChasse;
    private int nbBulle;
    // Start is called before the first frame update
    void Start()
    {
        savedTimeBulle = Time.time;
        savedTimeBulleAttaque = Time.time;
        savedTimeMoove = Time.time;
        savedTimeChasse = Time.time;
        isBulleAttaque = true;
    }

    // Update is called once per frame
    void Update()
    {        
        AttaqueBulle();
        SetBulleAttaqueCD();
        
    }
    public void AttaqueBulle()
    {
        if(Time.time > savedTimeBulle + bulleTempsEntreChaqueBulle + Random.Range(-0.1f,0.1f) && isBulleAttaque)
        {
            Vector3 startBullePosition = new Vector3(this.transform.position.x+Random.Range(-1,1), this.transform.position.y, this.transform.position.z);
            GameObject bulle = Instantiate(bulleObject,startBullePosition,Quaternion.identity);
            bulle.GetComponent<Bulle>().speed = Random.Range(bulleSpeedMin,bulleSpeedMax);
            bulle.transform.localRotation = Quaternion.Euler(0,0,Random.Range(-bulleAngleDirection,bulleAngleDirection));
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
        if(Time.time > savedTimeMoove + mooveCD)
        {
            savedTimeMoove = Time.time;
        }
    }
    public void Chasse()
    {
        if(Time.time > savedTimeChasse + mooveCD)
        {
            savedTimeChasse = Time.time;
        }
    }
    
}
