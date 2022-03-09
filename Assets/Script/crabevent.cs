using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class crabevent : MonoBehaviour
{
    [SerializeField] public GameObject crabe;
    [SerializeField] public Rigidbody[] rocheRb;
    [SerializeField] public GameObject call;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other) 
    {
        if (other.tag == "Player")
        {
            crabe.GetComponent<Crabe>().estActif = true;
            //Destroy(this.gameObject);
            this.gameObject.GetComponent<BoxCollider>().enabled = false;
            
            
        }
    }
    public void Eboulement()
    {
        for (int i = 0; i < 8; i++)
        {
            rocheRb[i].isKinematic = false;
            call.SetActive(false);
        }
    }
}
