using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject Body;
    //public PickupZone PickupZone;

    bool isEnabled = false;

    float rotationSpeedDegPerSec = 720;
    float AccelUnitPerSec = 40f;
    float maxSpeedUnitsPerSec = 10f;

    float horizontalAxis = 0;
    float verticalAxis = 0;
    float rawHorizontalAxis = 0;
    float rawVerticalAxis = 0;

    float lastInputStrength = 0;
    Vector3 lastVelocity = Vector3.zero;
    Vector3 lastDirection = Vector3.zero;

    bool isHoldingTree = false;
    bool isShaking = false;

    float standardMovementMultiplier = 1;
    float isHoldingTreeMoveMultiplier = 0.5f;
    float isShakingMoveMultiplier = 0.75f;

    float CurrentMoveMultiplier { 
        get => isHoldingTree ?
            isShaking ?
                isHoldingTreeMoveMultiplier * isShakingMoveMultiplier
                : isHoldingTreeMoveMultiplier
            : isShaking ?
                isShakingMoveMultiplier
                : standardMovementMultiplier;
    }
    public float RotationSpeedDegPerSec { get => rotationSpeedDegPerSec * CurrentMoveMultiplier; set => rotationSpeedDegPerSec = value; }
    public float AccelUnitPerSec1 { get => AccelUnitPerSec * CurrentMoveMultiplier; set => AccelUnitPerSec = value; }
    public float MaxSpeedUnitsPerSec { get => maxSpeedUnitsPerSec * CurrentMoveMultiplier; set => maxSpeedUnitsPerSec = value; }
   

    public void OnGameStart()
    {
        isEnabled = true;
    }

    public void OnGameOver()
    {
        isEnabled = false;
    }

    public void OnPickup(Vector3 SnapMove) 
    {
        transform.Translate(new Vector3(SnapMove.x, 0, SnapMove.z));
        isHoldingTree = true;
    }

    public void OnThrow(Vector3 direction)
    {
        isHoldingTree = false;
        lastVelocity -= direction * maxSpeedUnitsPerSec; // Knockback
    }

    public void SetIsShaking(bool newIsShaking)
    {
        isShaking = newIsShaking;
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
        if (!isEnabled)
        {
            return;
        }

        float inputStrength = Mathf.Max(Mathf.Abs(horizontalAxis), Mathf.Abs(verticalAxis));
        Vector3 direction = new Vector3(horizontalAxis, 0, verticalAxis).normalized;
        Vector3 change = direction * inputStrength * AccelUnitPerSec1 * Time.fixedDeltaTime;
        Vector3 newVelocity = GetSpeedAfterDrag(lastVelocity) + change;

        if (newVelocity.magnitude > MaxSpeedUnitsPerSec)
        {
            newVelocity = (newVelocity.normalized * MaxSpeedUnitsPerSec);
        }

        transform.Translate(newVelocity * Time.fixedDeltaTime);
        if (inputStrength > 0 && inputStrength >= lastInputStrength)
        {
            
            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
            Quaternion newRotation = Quaternion.RotateTowards(Body.transform.rotation, targetRotation, RotationSpeedDegPerSec * Time.fixedDeltaTime);
            Body.transform.rotation = newRotation;
        }

        lastInputStrength = inputStrength;
        lastVelocity = newVelocity;
        lastDirection = direction;
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
