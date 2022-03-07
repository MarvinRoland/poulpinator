using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float playerPropulsionForce; // determine la la force de propulsion du poulpe (en gros la distance jusqu'ou ca le propulsera apres un input)
    [SerializeField] float playerDeceleration; // la vitesse a la quel le poulpe passe de X vitesse a zero quand on ne se propulse pas    
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
    [SerializeField] int forcePhysique, forceToxic; // skill physique et toxic pour debloquer les skill
    [SerializeField] int lvlSkill1, lvlSkill2, lvlSkill3, lvlSkill4, lvlSkill5;
    [SerializeField] Camera cameraPlayer;
    [SerializeField] public float cameraDistance, cameraHauteur;
    [SerializeField] GameObject ancreObject;
    bool isCameraGoFar = false;
    bool isGoBig = false;
    bool canShoot = false;
    public float disCam;
    public Vector3 sizeUp;
    float savedTimeShoot, savedTimeSpriteAncre, shootCD, spriteCD;
    int nbAncre, nbAncreToSpew;
    bool gameOver;
     
 


    
    private float z;
    private Rigidbody rb;
    Vector3 v3Force;
    

    // Start is called before the first frame update
    void Start()
    {        
        rb = this.gameObject.GetComponent<Rigidbody>();
        savedTimeShoot = Time.time;
        savedTimeSpriteAncre = Time.time;
        nbAncre = 0;
        spriteCD = 0.02f;
        gameOver = true;
        nbAncreToSpew = 5;
    }

    // Update is called once per frame
    void Update()
    {
        Rotation();
        Propulsion();
        CameraSetPosition();
        PlayerSetScale();
        JetDancre();
        SetPlayerAncre();

        rb.angularVelocity -= rb.angularVelocity * Time.deltaTime * 1;
        cameraPlayer.transform.position = new Vector3(transform.position.x, transform.position.y+cameraHauteur,cameraDistance);
        rb.AddForce(new Vector3(0,-1*Time.deltaTime,0),ForceMode.VelocityChange);
        
        
    }
    void FixedUpdate()
    {

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
        if (Input.GetButtonDown("Fire1")&& ancreReserve > ancreConsume)
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
                expPhysique -= 10;
                expToxic += 25; 
            }
            else
            {
                expPhysique += 25;
                expToxic -= 10;
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
            // devient plus gros

            expSkill += 100;
            forcePhysique += 1;
        }
        if (expToxic > expSkill)
        {
            //degats andre
            // nb andre
            // ancre toxic
            //reserve dancre
            //ancre autour
            //devien toxic au touché
            expToxic += 100;
            forceToxic +=1;
        }
    }
}
