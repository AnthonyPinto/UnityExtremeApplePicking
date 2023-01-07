using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apple : MonoBehaviour
{
    public Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb.AddForce(new Vector3(Random.value, Random.value, Random.value));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
