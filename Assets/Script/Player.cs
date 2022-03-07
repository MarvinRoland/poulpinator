using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float playerPropulsionForce; // determine la la force de propulsion du poulpe (en gros la distance jusqu'ou ca le propulsera apres un input)
    [SerializeField] float playerDeceleration; // la vitesse a la quel le poulpe passe de X vitesse a zero quand on ne se propulse pas    
    [SerializeField] int playerDamage;    
    [SerializeField] float spikeCD, attackCD;    
    [SerializeField] float ancrePuissanceDePropulsion; // vitesse de depart du jet d'ancre 
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
    [SerializeField] float expPhysique, expToxic, expSkill; // exp phys, exp toxic, exp avant prochain lvl
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
    bool isGoBig = false;
    bool canShoot = false;
    public float disCam;
    public Vector3 sizeUp;
    float savedTimeShoot, savedTimeSpriteAncre, savedTimeSpikeAttaque, savedTimeAttack, shootCD, spriteCD;
    int nbAncre, nbAncreToSpew;
    bool gameOver = false;
     
    float randZMin,randZMax;
    Vector3 mousePosition;

    
    private float z;
    private Rigidbody rb;
    Vector3 v3Force;
    

    // Start is called before the first frame update
    void Start()
    {      
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
    }

    // Update is called once per frame
    void Update()
    {
        mousePosition = Input.mousePosition;
        if (!isDead)
        {
            Rotation();
            Propulsion();
            CameraSetPosition();
            PlayerSetScale();
            JetDancre();
            SetPlayerAncre();
            FixNegatifExp();
        }
        if (isDead)
        {
            Die();
        }
        
        rb.angularVelocity -= rb.angularVelocity * Time.deltaTime * 1;
        cameraPlayer.transform.position = new Vector3(transform.position.x, transform.position.y+cameraHauteur,cameraDistance);
        rb.AddForce(new Vector3(0,-1*Time.deltaTime,0),ForceMode.VelocityChange);  
    }
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
        if (Input.GetButtonDown("Ancre")&& ancreReserve > ancreConsume)
        {
            canShoot = true;
            ancreReserve -= ancreConsume;
            //ancre.transform.position = Vector3.Lerp(ancre.transform.position, ancre.transform.position-ancre.transform.up*Time.deltaTime*ancreSpeed, 1);
        }
        if (canShoot && nbAncre < nbAncreToSpew && Time.time > spriteCD + savedTimeSpriteAncre )
        {
            Quaternion myrot = this.transform.rotation;
            myrot.z += Random.Range(-0.1f,0.1f);
            GameObject ancre = Instantiate(ancreObject, transform.position, myrot);
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
            if(other.GetComponent<Bouffe>().isToxicFood)
            {
                expPhysique -= expToRemove;
                expToxic += expToAdd; 
            }
            else
            {
                expPhysique += expToAdd;
                expToxic -= expToRemove;
            }
            
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
    public void PlayerUpgrade()
    {
        if (expPhysique > expSkill)
        {
            //augment degat physique
            //skill pointes
            //propulsion plus puissante
            // armure
            

            //lvl 1
            //propulsion plus puissante
            //lvl2
            //augment degat physique
            //lvl3
            //skill pointes
            
            expSkill += 100;
            forcePhysique += 1;
            if (forcePhysique == 1)
            {
                playerPropulsionForce += 10;
            }
            if (forcePhysique == 2)
            {
                playerDamage = 5;
            }
            if (forcePhysique == 3)
            {
                // pointes isactive
            }
        }
        if (expToxic > expSkill)
        {
            // lvl 1
            //reserve dancre ameliore, ancre plus longue, taper avec le fouet ajoute un poison
            //lvl2
            //reserve ameliorée, ancre sur les coté
            //lvl 3
            //ancre devien toxic
            expToxic += 100;
            forceToxic +=1;
            if (forceToxic == 1)
            {
                ancreReserveMax += 50;
                ancreReserve = ancreReserveMax;
                //fouet degat poison
            }
            if (forceToxic == 2)
            {
                ancreReserveMax += 100;
                ancreReserve = ancreReserveMax;
                randZMin = -100.0f;
                randZMax = 100.0f;
            }
            if (forceToxic == 3)
            {
                //
            }
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
        if(forcePhysique >= 3 && Time.time > savedTimeSpikeAttaque + spikeCD)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                isSpike = true;
                //joue animationspike
            }
            
        }
    }
}
