using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject Body;

    float rotationSpeedDegPerSec = 180;
    float AccelUnitPerSec = 40f;
    float maxSpeedUnitsPerSec = 10f;

    float horizontalAxis = 0;
    float verticalAxis = 0;
    float rawHorizontalAxis = 0;
    float rawVerticalAxis = 0;

    float lastInputStrength = 0;
    Vector3 lastSpeed = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        horizontalAxis = Input.GetAxis("Horizontal");
        verticalAxis = Input.GetAxis("Vertical");
        rawHorizontalAxis = Input.GetAxisRaw("Horizontal");
        rawVerticalAxis = Input.GetAxisRaw("Vertical");
    }

    private void FixedUpdate()
    {
        //transform.Rotate(horizontalAxis * Vector3.up * rotationSpeedDegPerSec * Time.fixedDeltaTime);
        //transform.Translate(verticalAxis * Vector3.forward * speedUnitPerSec * Time.fixedDeltaTime);

        float inputStrength = Mathf.Max(Mathf.Abs(horizontalAxis), Mathf.Abs(verticalAxis));
        Vector3 direction = new Vector3(horizontalAxis, 0, verticalAxis).normalized;
        Vector3 change = direction * inputStrength * AccelUnitPerSec * Time.fixedDeltaTime;
        Vector3 newSpeed = GetSpeedAfterDrag(lastSpeed) + change;

        if (newSpeed.magnitude > maxSpeedUnitsPerSec)
        {
            newSpeed = (newSpeed.normalized * maxSpeedUnitsPerSec);
        }

        transform.Translate(newSpeed * Time.fixedDeltaTime);
        if (inputStrength >= lastInputStrength)
        {
            Body.transform.LookAt(transform.position + direction);
        }

        lastInputStrength = inputStrength;
        lastSpeed = newSpeed;
    }

    // Add brakes if no longer going a direction
    private Vector3 GetSpeedAfterDrag(Vector3 startSpeed)
    {

        Vector3 change = Vector3.zero;
        float dragLimit = AccelUnitPerSec * Time.fixedDeltaTime;


        if (
            (startSpeed.x > 0 && rawHorizontalAxis <= 0) ||
            (startSpeed.x < 0 && rawHorizontalAxis >= 0)
            )
        {
            change.x = -startSpeed.x;
        }

        if (
            (startSpeed.z > 0 && rawVerticalAxis <= 0) ||
            (startSpeed.z < 0 && rawVerticalAxis >= 0)
            )
        {
            change.z = -startSpeed.z;
        }

        if (change.magnitude > dragLimit)
        {
            change = change.normalized * dragLimit;
        }



        return startSpeed + change;

    }
}
