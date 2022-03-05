using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float playerPropulsionForce; // determine la la force de propulsion du poulpe (en gros la distance jusqu'ou ca le propulsera apres un input)
    [SerializeField] float playerDeceleration; // la vitesse a la quel le poulpe passe de X vitesse a zero quand on ne se propulse pas
    [SerializeField] float ancrePropulsionDistance; // la distance max jusqu'ou l'ancre serra propulsée
    [SerializeField] float ancrePuissanceDePropulsion; // vitesse de depart du jet d'ancre 
    [SerializeField] float ancrePuissanceDecroissement; // vitesse a la quel l'ancre passe de x vitesse a zero
    [SerializeField] bool ancreIsToxic; // si TRUE l'ancre serra toxic et ferra des degats
    [SerializeField] float ancreDegatsDirect, ancreDegatsDegatsParSeconde; // les degats que l'ancre infligera a l'impacte
    [SerializeField] float ancreDureeEmpoisonement; // durée de l'empoisonement
    [SerializeField] float ancreCD; // temps entre chaque jet d'ancre
    [SerializeField] float ancreReserve; // reserve d'ancre
    [SerializeField] float ancreRegenerationPassive; // ancre ajouté tout les x seconde sans manger de proie
    [SerializeField] float ancreRegenerationActive; // ancre ajouté apres avoir mangé une proie
    [SerializeField] float ancreReserveAjoutTime; // temps entre chaque ajout naturel d'ancre dans la reserve
    [SerializeField] float rotationSpeed; // vitesse de rotation du poulpe
    Vector3 v3Force;
    



    
    private float z;
    private Rigidbody rb;
    
    

    // Start is called before the first frame update
    void Start()
    {        
        rb = this.gameObject.GetComponent<Rigidbody>();
        rb.AddForce(new Vector3(0,0,0),ForceMode.VelocityChange);
    }

    // Update is called once per frame
    void Update()
    {
        Rotation();
        Propulsion();
        rb.angularVelocity -= rb.angularVelocity * Time.deltaTime * 1;
    }
    void FixedUpdate()
    {

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
        if (Input.GetButtonDown("Fire"))
        {

        }
    }
    public void DragTentacle()
    {

    }
    public void Die()
    {

    }
}
