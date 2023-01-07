using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupZone : MonoBehaviour
{
    PlayerController playerController;

    List<AppleTree> treesInZone = new List<AppleTree>();

    AppleTree heldTree = null;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (heldTree != null)
            {

                heldTree.SetIsHeld(false);
                heldTree.transform.SetParent(null);
                heldTree = null;
            }

            else if (treesInZone.Count > 0)
            {
                heldTree = treesInZone[0];
                heldTree.SetIsHeld(true);
                heldTree.transform.SetParent(transform);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);

        if (other.CompareTag("appletree"))
        {
            treesInZone.Add(other.GetComponent<AppleTree>());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("appletree"))
        {
            treesInZone.Remove(other.GetComponent<AppleTree>());
        }
    }
}
