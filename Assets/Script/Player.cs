using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float playerPropulsionForce; // determine la la force de propulsion du poulpe (en gros la distance jusqu'ou ca le propulsera apres un input)
    [SerializeField] float playerDeceleration; // la vitesse a la quel le poulpe passe de X vitesse a zero quand on ne se propulse pas    
    [SerializeField] int playerDamage;    
    [SerializeField] bool tentacleIsToxic;
    [SerializeField] float spikeCD, attackCD;    
    [SerializeField] float ancrePuissanceDePropulsion; // vitesse de depart du jet d'ancre 
    [SerializeField] float ancreNbSpew; // nombre de tache
    [SerializeField] float ancreTempsExiste; // le temps que la tache restera
    [SerializeField] float ancrePuissanceDecroissement; // vitesse a la quel l'ancre passe de x vitesse a zero
    [SerializeField] bool ancreIsToxic; // si TRUE l'ancre serra toxic et ferra des degats
    [SerializeField] float ancreDegatsDirect, ancreDegatsDegatsParSeconde, ancreSpeed; // les degats que l'ancre infligera a l'impacte
    [SerializeField] float ancreDureeEmpoisonement; // durée de l'empoisonement
    [SerializeField] float ancreCD; // temps entre chaque jet d'ancre
    [SerializeField] float ancreReserve; // reserve d'ancre
    [SerializeField] float ancreReserveMax;
    [SerializeField] float ancreConsume;  // ancre a chaque tir
    [SerializeField] float ancreRegenerationPassive; // ancre ajouté tout les x seconde sans manger de proie
    [SerializeField] float ancreRegenerationActive; // ancre ajouté apres avoir mangé une proie
    [SerializeField] float ancreReserveAjoutTime; // temps entre chaque ajout naturel d'ancre dans la reserve
    [SerializeField] float rotationSpeed; // vitesse de rotation du poulpe
    [SerializeField] float expPhysique, expToxic, expSkillPhysicNeeded, expSkillToxicNeeded; // exp phys, exp toxic, exp avant prochain lvl
    [SerializeField] float expToAdd, expToRemove; //exp ajouté 
    [SerializeField] int forcePhysique, forceToxic; // skill physique et toxic pour debloquer les skill
    [SerializeField] int lvlSkill1, lvlSkill2, lvlSkill3, lvlSkill4, lvlSkill5;
    [SerializeField] Camera cameraPlayer;
    [SerializeField] public float cameraDistance, cameraHauteur;
    [SerializeField] GameObject ancreObject;
    [SerializeField] GameObject tentacle;
    bool isDead = false;
    bool isCameraGoFar = false;
    bool isSpike = false;
    bool canSpike = false;
    bool isGoBig = false;
    bool canShoot = false;
    public float disCam;
    public Vector3 sizeUp;
    float savedTimeShoot, savedTimeSpriteAncre, savedTimeSpikeAttaque, savedTimeAttack, shootCD, spriteCD;
    int nbAncre, nbAncreToSpew;
    bool gameOver = false;
    float myrotAngle;
     
    float randZMin,randZMax;
    Vector3 mousePosition;

    
    private float z;
    private Rigidbody rb;
    Vector3 v3Force;
    Quaternion myrot;

    // Start is called before the first frame update
    void Start()
    {    
        SetToxicStat();
        SetPhysicStat() ;
        myrot = this.transform.rotation; 
        randZMin = -0.3f;
        randZMax = 0.3f;  
        rb = this.gameObject.GetComponent<Rigidbody>();
        savedTimeShoot = Time.time;
        savedTimeSpriteAncre = Time.time;
        savedTimeSpikeAttaque = Time.time;
        nbAncre = 0;
        spriteCD = 0.02f;
        gameOver = true;
        nbAncreToSpew = 5;
        //cameraPlayer. = new Vector3(0,0,0);
    }

    // Update is called once per frame
    void Update()
    {
        mousePosition = Input.mousePosition;
        
        
        Rotation();
        Propulsion();
        CameraSetPosition();
        PlayerSetScale();
        JetDancre();
        SetPlayerAncre();
        FixNegatifExp();
        rb.angularVelocity -= rb.angularVelocity * Time.deltaTime * 1;
        cameraPlayer.transform.position = new Vector3(transform.position.x, transform.position.y+cameraHauteur,cameraDistance);
        rb.AddForce(new Vector3(0,-1*Time.deltaTime,0),ForceMode.VelocityChange);  
        
    }
    /*public void SetPlayerPuissance()
    {
        myrot = this.transform.rotation; 
        
            }
    }*/
    void FixedUpdate()
    {

    }
    public void FixNegatifExp()
    {
        if(expPhysique < 0)
        {
            expPhysique = 0;
        }
        if(expToxic < 0)
        {
            expToxic = 0;
        }
    }
    public void SetPlayerAncre()
    {
        if (ancreReserve < ancreReserveMax)
        {
            ancreReserve += ancreRegenerationPassive * Time.deltaTime * ancreReserveAjoutTime;
        }
        if (ancreReserve > ancreReserveMax)
        {
            ancreReserve = ancreReserveMax;
        }
        Debug.Log(ancreReserve + "ancre");
    }
    public void Propulsion()
    {
        if (Input.GetButtonDown("Jump"))
        {
            v3Force = playerPropulsionForce * transform.up;
            Debug.Log("propulse");
            rb.AddForce(v3Force,ForceMode.Impulse);
            
        }
        else
        {
            
            rb.velocity -= playerDeceleration*rb.velocity* Time.deltaTime;
        }
    }
    public void Rotation()
    {
        if (Input.GetButton("Horizontal"))
        {
            z += Time.deltaTime * rotationSpeed * Input.GetAxis("Horizontal");

            if (z > 360.0f)
            {
                z = 0.0f;
                
            }
            
        }
        transform.localRotation = Quaternion.Euler(0, 0, z);
    }
    public void JetDancre()
    {
        if (Input.GetButtonDown("Fire3")&& ancreReserve > ancreConsume)
        {
            canShoot = true;
            ancreReserve -= ancreConsume;
            //ancre.transform.position = Vector3.Lerp(ancre.transform.position, ancre.transform.position-ancre.transform.up*Time.deltaTime*ancreSpeed, 1);
        }
        if (canShoot && nbAncre < nbAncreToSpew && Time.time > spriteCD + savedTimeSpriteAncre )
        {
            
                   
            GameObject ancre = Instantiate(ancreObject, transform.position, transform.rotation);
            ancre.GetComponent<Ancre>().propulsionForce = ancrePuissanceDePropulsion;
            ancre.GetComponent<Ancre>().isToxic = ancreIsToxic;
            ancre.transform.localScale *= Random.Range(0.1f,0.4f);
            ancre.GetComponent<Ancre>().timeExist = ancreTempsExiste;
            ancre.GetComponent<Ancre>().decraseSpeed = ancrePuissanceDecroissement;
            nbAncre += 1;
            
            savedTimeSpriteAncre = Time.time;
        }
        if (canShoot && nbAncre >= 5)
        {
            canShoot = false;
            nbAncre = 0;
        }

    }
    public Quaternion GetAngle()
    {
        //Quaternion angle = this.transform.localRotation;
        float angleZ = 0;
        if (forceToxic >= 2)
        {
            angleZ = transform.rotation.z + Random.Range(-90.0f,90.0f);
        }
        if (forceToxic <2)
        {
            angleZ = transform.rotation.z + Random.Range(0.3f,0.3f);
        }     
        Quaternion angle = Quaternion.Euler(0,0,angleZ);//transform.rotation + Quaternion.Euler(0.0f,0.0f,0.0f);
        return angle;
    }
    public void DragTentacle()
    {

    }
    public void Die()
    {
        //play animation mort
        // gameover is on
        // retourne au checkpoint
        
        
        
    }
    private void OnTriggerEnter(Collider other) 
    {
        if (other.tag == "Bouffe passive")
        {
            Destroy(other.gameObject);
            isCameraGoFar = true;
            isGoBig = true;
            sizeUp = transform.localScale += new Vector3(0.1f,0.1f,0.1f);
            disCam = cameraDistance += 0.1f;            
            PlayerUpgrade(other.GetComponent<Bouffe>().isToxicFood, expToAdd, expToRemove);           
            
        }            
        
        if(other.tag == "Enemy" && other.GetComponent<Enemy>().isPredateur)
        {
            if(isSpike)
            {
                other.GetComponent<Enemy>().pointDeVie -= playerDamage;
            }            
            else if (!isSpike  && !other.GetComponent<Enemy>().isKnoc)
            {
                isDead = true;
            }
        }
        if(other.tag == "Crabe" && other.GetComponent<Crabe>().estActif)
        {
            if(!other.GetComponent<Crabe>().isKnoc)
            {
                isDead = true;
            }            
            else
            {
               
            }
        }


        Debug.Log("hit");
    }
    public void CameraSetPosition()
    {
        if (isCameraGoFar)
        {
            cameraDistance = Mathf.Lerp(cameraDistance, disCam*1.1f, 1*Time.deltaTime);
        }
    }
    public void PlayerSetScale()
    {
        if (isGoBig)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, sizeUp, 1*Time.deltaTime);
        }
        
    }
    public void PlayerUpgrade(bool toxic, float expadd, float expremove)
    {
        if (toxic)
        {
            expToxic += expadd;
            expPhysique -= expremove;    
        }
        if (!toxic)
        {
            expToxic -= expremove;
            expPhysique += expadd;    
        } 
        CheckExp();
    }
    public void CheckExp()
    {
        if (expPhysique > expSkillPhysicNeeded)
        {            
            expSkillPhysicNeeded *= 2;
            forcePhysique += 1;
            if(forcePhysique < 4)
            {
                SetPhysicStat();
            }
            
        }
        if (expToxic > expSkillToxicNeeded)
        {
            expSkillToxicNeeded *= 2;
            forceToxic += 1;
            if(forceToxic < 4)
            {
                SetToxicStat();
            }           
            
        }
    }
    public void SetToxicStat()
    {
        switch (forceToxic)
        {
        case 0:
        Debug.Log("setstat");
            
            tentacleIsToxic = false;
            ancrePuissanceDePropulsion = 10;
            ancreDegatsDirect = 0;
            ancreDegatsDegatsParSeconde = 0;
            ancreDureeEmpoisonement = 0;                    
            ancreNbSpew = 5;
            ancreTempsExiste = 5;
            ancreIsToxic = false;                    
        break;
        case 1:
            
            tentacleIsToxic = false;
            ancrePuissanceDePropulsion = 15;
            ancreDegatsDirect = 0;
            ancreDegatsDegatsParSeconde = 0;
            ancreDureeEmpoisonement = 0;
            ancreRegenerationPassive = 1;
            ancreNbSpew = 5;
            ancreTempsExiste = 7;
            ancreIsToxic = false;
        break;
        case 2:
            
            tentacleIsToxic = false;
            ancrePuissanceDePropulsion = 10;
            ancreDegatsDirect = 0;
            ancreDegatsDegatsParSeconde = 0;
            ancreDureeEmpoisonement = 0;
            ancreRegenerationPassive = 1;
            ancreNbSpew = 5;
            ancreTempsExiste = 10;
            ancreIsToxic = false;
        break;
        case 3:
            
            tentacleIsToxic = false;
            ancrePuissanceDePropulsion = 10;
            ancreDegatsDirect = 0;
            ancreDegatsDegatsParSeconde = 0;
            ancreDureeEmpoisonement = 0;
            ancreRegenerationPassive = 1;
            ancreNbSpew = 5;
            ancreTempsExiste = 15;
            ancreIsToxic = true;
        break;
        default:
        break;
        }
    }
    public void SetPhysicStat()
    {
        switch (forceToxic)
        {
        case 0:
            playerDamage = 0;
            playerPropulsionForce = 10;
            canSpike = false;
        break;
        default:

        break;
        }
    }
    public void TentacleAttaque()
    {
        if (Input.GetButtonDown("Ancre"))
        {
            Quaternion myrot = this.transform.rotation;
            myrot.z += 180;
            GameObject _tentacle = Instantiate(tentacle,transform.position + transform.right*Time.deltaTime * 1.0f,myrot);
            //_tentacle.transform.localScale += Vector3.Distance(transform.position, mousePosition);
        }
        
    }
    public void SpikeAttaque()
    {
        if(canSpike && Time.time > savedTimeSpikeAttaque + spikeCD)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                isSpike = true;
                //joue animationspike
            }
            
        }
    }
}
