using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PlayerController : MonoBehaviour
{
    public GameObject Body;
    //public PickupZone PickupZone;


    float rotationSpeedDegPerSec = 720;
    float AccelUnitPerSec = 40f;
    float maxSpeedUnitsPerSec = 10f;

    float horizontalAxis = 0;
    float verticalAxis = 0;
    float rawHorizontalAxis = 0;
    float rawVerticalAxis = 0;

    float lastInputStrength = 0;
    Vector3 lastSpeed = Vector3.zero;
    Vector3 lastDirection = Vector3.zero;

    bool isHoldingTree = false;

    float standardMovementMultiplier = 1;
    float isHoldingTreeMoveMultiplier = 0.5f;

    float CurrentMoveMultiplier { get => isHoldingTree ? isHoldingTreeMoveMultiplier : standardMovementMultiplier; }
    public float RotationSpeedDegPerSec { get => rotationSpeedDegPerSec * CurrentMoveMultiplier; set => rotationSpeedDegPerSec = value; }
    public float AccelUnitPerSec1 { get => AccelUnitPerSec * CurrentMoveMultiplier; set => AccelUnitPerSec = value; }
    public float MaxSpeedUnitsPerSec { get => maxSpeedUnitsPerSec * CurrentMoveMultiplier; set => maxSpeedUnitsPerSec = value; }
   

    public void OnPickup(Vector3 SnapMove) 
    {
        transform.Translate(SnapMove);
        isHoldingTree = true;
    }

    public void OnDrop()
    {
        isHoldingTree = false;
    }

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
        float inputStrength = Mathf.Max(Mathf.Abs(horizontalAxis), Mathf.Abs(verticalAxis));
        Vector3 direction = new Vector3(horizontalAxis, 0, verticalAxis).normalized;
        Vector3 change = direction * inputStrength * AccelUnitPerSec1 * Time.fixedDeltaTime;
        Vector3 newSpeed = GetSpeedAfterDrag(lastSpeed) + change;

        if (newSpeed.magnitude > MaxSpeedUnitsPerSec)
        {
            newSpeed = (newSpeed.normalized * MaxSpeedUnitsPerSec);
        }

        transform.Translate(newSpeed * Time.fixedDeltaTime);
        if (inputStrength > 0 && inputStrength >= lastInputStrength)
        {
            
            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
            Quaternion newRotation = Quaternion.RotateTowards(Body.transform.rotation, targetRotation, RotationSpeedDegPerSec * Time.fixedDeltaTime);
            Body.transform.rotation = newRotation;
        }

        lastInputStrength = inputStrength;
        lastSpeed = newSpeed;
        lastDirection = direction;
    }

    private void OnDrawGizmos()
    {
        Handles.DrawLine(transform.position, transform.position + lastDirection);
    }

    // Add brakes if no longer going a direction
    private Vector3 GetSpeedAfterDrag(Vector3 startSpeed)
    {

        Vector3 change = Vector3.zero;
        float dragLimit = AccelUnitPerSec1 * Time.fixedDeltaTime;


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
