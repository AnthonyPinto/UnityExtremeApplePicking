using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppleTree : MonoBehaviour
{
    public GameObject ApplePrefab;
    public Transform AppleSpawnPoint;
    public GameObject Trunk;
    public List<GameObject> AllLeaves;

    bool isHeld = false;
    bool isShaking = false;
    bool isLaunched = false;
    Vector3 launchRotationPerSecond = new Vector3(720, 0, 0);
    Vector3 launchDeltaPerSecond = new Vector3(0, 15, 45);

    List<Transform> touchingArmTransforms = new List<Transform>();

    float nextAppleWait = 0;
    float minAppleDelay =  0.1f;
    float maxAppleDelay = 1;
    int maxSimultaneousApples = 4;



    float distanceToNextAppleDrop = 1;
    float minDistanceBeforeDrop = .1f;
    float maxDistanceBeforeDrop = 3f;

    int applesDropped = 0;

    float dropSlowdownStep = 1f;

    float shakingDropSpeedMultiplier = 10;



    Vector3 posAtLastAngleCheck;
    Vector3 posLastFrame;

    float angleCheckRate = 0.1f;
    float maxTrunkAngle = 15f;
    float maxTrunkAngleSpeed = 5;

    float timeOfLastCheck = 0;
    Quaternion rotationAtLastCheck;
    GameObject nextRotationObj;


    public void SetIsHeld(bool newIsHeld)
    {
        this.isHeld = newIsHeld;
    }

    public void OnLaunched(Vector3 launchDirection)
    {
        isLaunched = true;
        transform.LookAt((transform.position + launchDirection));
        foreach (GameObject leaves in AllLeaves)
        {
            leaves.transform.SetParent(Trunk.transform);
        }
    }

    public void SetIsShaking(bool newIsShaking)
    {
        isShaking = newIsShaking;
    }

    IEnumerator UpdateTiltRoutine()
    {
        while (!isLaunched)
        {
            yield return new WaitForSeconds(angleCheckRate);

            rotationAtLastCheck = transform.rotation;
            Vector3 horizontalPos = new Vector3(transform.position.x, 0, transform.position.z);
            Vector3 prevHorizontalPos = new Vector3(posAtLastAngleCheck.x, 0, posAtLastAngleCheck.z);

            float horizontalDistance = (horizontalPos - prevHorizontalPos).magnitude;
            float horizontalSpeed = horizontalDistance / angleCheckRate;
            float percentOfMaxAngle = Mathf.Min(horizontalSpeed, maxTrunkAngleSpeed) / maxTrunkAngleSpeed;
            float angleToApply = maxTrunkAngle * percentOfMaxAngle;

            nextRotationObj.transform.SetPositionAndRotation(transform.position, transform.rotation);


            nextRotationObj.transform.LookAt(new Vector3(posAtLastAngleCheck.x, transform.position.y, posAtLastAngleCheck.z));
            Vector3 newAngles = nextRotationObj.transform.localEulerAngles;
            newAngles.x = angleToApply;
            nextRotationObj.transform.localEulerAngles = newAngles;


            posAtLastAngleCheck = transform.position;
            timeOfLastCheck = Time.time;

        }
    }



    // Start is called before the first frame update
    void Start()
    {
        nextRotationObj = new GameObject();

        posAtLastAngleCheck = transform.position;
        posLastFrame = transform.position;

        StartCoroutine(UpdateTiltRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        if (isLaunched)
        {
            transform.Translate(launchDeltaPerSecond * Time.deltaTime);
            Trunk.transform.Rotate(launchRotationPerSecond * Time.deltaTime);

            return;
        }

        float distanceToReduce = Vector3.Distance(posLastFrame, transform.position);
        if (isShaking)
        {
            distanceToReduce *= shakingDropSpeedMultiplier;
        }
        distanceToNextAppleDrop -= distanceToReduce;
        if (distanceToNextAppleDrop <= 0)
        {
            SpawnApple();
        }

        posLastFrame = transform.position;

        float timeSinceLastCheck = Time.time - timeOfLastCheck;
        float percentComplete = (timeSinceLastCheck / angleCheckRate);


        transform.rotation = Quaternion.Slerp(rotationAtLastCheck, nextRotationObj.transform.rotation, percentComplete);
    }

    void SpawnApple()
    {
        Vector3 offset = (new Vector3(Random.value, 0, Random.value).normalized) * Random.value * 2;

        Instantiate(ApplePrefab, AppleSpawnPoint.position + offset, ApplePrefab.transform.rotation);
        float delay = dropSlowdownStep * applesDropped;
        distanceToNextAppleDrop = Random.Range(minDistanceBeforeDrop, maxDistanceBeforeDrop) + delay;
        applesDropped++;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("arm"))
        {
            touchingArmTransforms.Add(other.transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("arm"))
        {
            touchingArmTransforms.Remove(other.transform);
        }
    }
}
