using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    float rotationSpeedDegPerSec = 180;
    float speedUnitPerSec = 5;

    float horizontalAxis = 0;
    float verticalAxis = 0;

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
        transform.Rotate(horizontalAxis * Vector3.up * rotationSpeedDegPerSec * Time.fixedDeltaTime);
        transform.Translate(verticalAxis * Vector3.forward * speedUnitPerSec * Time.fixedDeltaTime);
    }
}
