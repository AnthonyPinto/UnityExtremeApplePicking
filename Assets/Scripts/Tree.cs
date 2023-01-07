using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{
    public GameObject ApplePrefab;
    public Transform AppleSpawnPoint;

    bool isHeld = false;
    List<Transform> touchingArmTransforms = new List<Transform>();

    float nextAppleWait = 0;
    float minAppleDelay =  0.1f;
    float maxAppleDelay = 1;
    int maxSimultaneousApples = 4;




    Vector3 prevPosition;
    Vector3 latestPostion;

    float angleCheckRate = 0.1f;
    float maxTrunkAngle = 90f;
    float maxTrunkAngleSpeed = 5;

    float timeOfLastCheck = 0;
    Quaternion rotationAtLastCheck;
    GameObject nextRotationObj;




    IEnumerator UpdateTiltRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(angleCheckRate);

            rotationAtLastCheck = transform.rotation;

            float distanceTraveled = (latestPostion - prevPosition).magnitude;
            float speed = distanceTraveled / angleCheckRate;
            //Debug.Log("dist: " + distanceTraveled + " | speed: " + speed);
            //Debug.Log("prevPos :" + prevPosition + " | newPos: " + transform.position);

            float percentOfMaxAngle = Mathf.Max(speed / maxTrunkAngleSpeed) / maxTrunkAngleSpeed;
            float angleToApply = maxTrunkAngle * percentOfMaxAngle;


            nextRotationObj.transform.SetPositionAndRotation(transform.position, transform.rotation);


            nextRotationObj.transform.LookAt(prevPosition);
            Vector3 newAngles = nextRotationObj.transform.localEulerAngles;
            newAngles.x = angleToApply;
            nextRotationObj.transform.localEulerAngles = newAngles;


            prevPosition = latestPostion;
            latestPostion = transform.position;
            timeOfLastCheck = Time.time;

        }
    }



    // Start is called before the first frame update
    void Start()
    {
        nextRotationObj = new GameObject();

        prevPosition = transform.position;
        latestPostion = transform.position;

        StartCoroutine(UpdateTiltRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isHeld)
            {

                isHeld = false;
                transform.SetParent(null);
            }

            else if (touchingArmTransforms.Count > 0)
            {
                isHeld = true;
                transform.SetParent(touchingArmTransforms[0].parent);
            }
        }

        if (isHeld)
        {
            nextAppleWait -= Time.deltaTime;

            if (nextAppleWait <= 0)
            {
                SpawnApples();
                nextAppleWait = Random.Range(minAppleDelay, maxAppleDelay);
            }
        }

        float timeSinceLastCheck = Time.time - timeOfLastCheck;
        float percentComplete = (timeSinceLastCheck / angleCheckRate);


        transform.rotation = Quaternion.Slerp(rotationAtLastCheck, nextRotationObj.transform.rotation, percentComplete);

    }

    void SpawnApples()
    {
        int appleCount = Random.Range(1, maxSimultaneousApples + 1);

        for (int i = 0; i < appleCount; i++)
        {
            Instantiate(ApplePrefab, AppleSpawnPoint.position, ApplePrefab.transform.rotation);
        }
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
