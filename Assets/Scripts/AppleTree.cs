using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AppleTree : MonoBehaviour
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

    IEnumerator UpdateTiltRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(angleCheckRate);

            rotationAtLastCheck = transform.rotation;
            Vector3 horizontalPos = new Vector3(transform.position.x, 0, transform.position.z);
            Vector3 prevHorizontalPos = new Vector3(prevPosition.x, 0, prevPosition.z);

            float horizontalDistance = (horizontalPos - prevHorizontalPos).magnitude;
            float horizontalSpeed = horizontalDistance / angleCheckRate;
            float percentOfMaxAngle = Mathf.Min(horizontalSpeed, maxTrunkAngleSpeed) / maxTrunkAngleSpeed;
            float angleToApply = maxTrunkAngle * percentOfMaxAngle;

            nextRotationObj.transform.SetPositionAndRotation(transform.position, transform.rotation);


            nextRotationObj.transform.LookAt(new Vector3(prevPosition.x, transform.position.y, prevPosition.z));
            Vector3 newAngles = nextRotationObj.transform.localEulerAngles;
            newAngles.x = angleToApply;
            nextRotationObj.transform.localEulerAngles = newAngles;


            prevPosition = transform.position;
            timeOfLastCheck = Time.time;

        }
    }

    private void OnDrawGizmos()
    {
        Handles.DrawLine(transform.position, prevPosition);
    }



    // Start is called before the first frame update
    void Start()
    {
        nextRotationObj = new GameObject();

        prevPosition = transform.position;

        StartCoroutine(UpdateTiltRoutine());
    }

    // Update is called once per frame
    void Update()
    {
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
