using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnFood : MonoBehaviour
{
    [SerializeField] GameObject food;
    public float savedTime, cdTime;
    void Awake()
    {
        for (int i = 0; i < 100; i++)
        {
        
            Instantiate(food, new Vector3(Random.Range(-90.0f,10.0f),Random.Range(-30.0f,35.0f),-15.0f), Quaternion.Euler(0,0,0));
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        savedTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time>savedTime+cdTime)
        {
            Instantiate(food, new Vector3(Random.Range(-90.0f,10.0f),Random.Range(-30.0f,35.0f),-15.0f), Quaternion.Euler(0,0,0));
            savedTime = Time.time;
        }
    }
}
