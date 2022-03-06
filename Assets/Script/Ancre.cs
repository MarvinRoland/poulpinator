using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ancre : MonoBehaviour
{
    public float speed, propulsionForce, timeExist, decraseSpeed;
    public float degat;
    public float degatsCD, savedTimeDegat, savedTimeExist;
    public bool isToxic;
    private Rigidbody rb;
    Vector3 v3Force;
    Vector3 localscale;
    SpriteRenderer sprite;
    Color color;
    // Start is called before the first frame update
    void Start()
    {
        savedTimeExist = Time.time;
        rb = this.gameObject.GetComponent<Rigidbody>();
        savedTimeDegat = Time.time;
        v3Force = propulsionForce * -transform.up;
        Debug.Log("propulse");
        rb.AddForce(v3Force,ForceMode.Impulse);
        localscale = transform.localScale;
        sprite = this.gameObject.GetComponent<SpriteRenderer>();
        color = sprite.color;
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity -= decraseSpeed*rb.velocity* Time.deltaTime;
        if (transform.localScale.y < 3.0f)
        {
            transform.localScale *= 1.01f;
        }
        if (Time.time > savedTimeExist + timeExist)
        {
            color.a = Mathf.Lerp (color.a, 0.0f, 1.0f*Time.deltaTime);
            sprite.color = color;
        }
        
    }
}
