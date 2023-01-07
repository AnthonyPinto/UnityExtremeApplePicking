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

    // Start is called before the first frame update
    void Start()
    {
        
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
