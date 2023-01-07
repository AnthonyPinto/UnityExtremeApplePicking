using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject Body;

    float rotationSpeedDegPerSec = 180;
    float speedUnitPerSec = 5;

    float horizontalAxis = 0;
    float verticalAxis = 0;

    float lastInputStrength = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        horizontalAxis = Input.GetAxis("Horizontal");
        verticalAxis = Input.GetAxis("Vertical");
    }

    private void FixedUpdate()
    {
        //transform.Rotate(horizontalAxis * Vector3.up * rotationSpeedDegPerSec * Time.fixedDeltaTime);
        //transform.Translate(verticalAxis * Vector3.forward * speedUnitPerSec * Time.fixedDeltaTime);

        float inputStrength = Mathf.Max(Mathf.Abs(horizontalAxis), Mathf.Abs(verticalAxis));


        Vector3 direction = new Vector3(horizontalAxis, 0, verticalAxis).normalized;
        transform.Translate(direction * inputStrength * speedUnitPerSec * Time.fixedDeltaTime);
        if (inputStrength >= lastInputStrength)
        {
            Body.transform.LookAt(transform.position + direction);
        }

        lastInputStrength = inputStrength;
    }
}
