using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bouffe : MonoBehaviour
{
    float speed;
    float z;
    float rotationSpeed = 100;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        z += Time.deltaTime * rotationSpeed;

            if (z > 360.0f)
            {
                z = 0.0f;
                
            }
        transform.localRotation = Quaternion.Euler(0,0,z);
    }
}
