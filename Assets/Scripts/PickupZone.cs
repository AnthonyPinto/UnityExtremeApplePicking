using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupZone : MonoBehaviour
{
    public PlayerController playerController;

    List<AppleTree> treesInZone = new List<AppleTree>();

    AppleTree heldTree = null;


    const float liftHeight = 0.5f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (heldTree != null) // Launch
            {
                Vector3 launchDirection = (transform.position - playerController.gameObject.transform.position).normalized;


                heldTree.SetIsHeld(false);
                heldTree.transform.SetParent(null);
                heldTree.OnLaunched(launchDirection);
                heldTree = null;
                playerController.OnDrop();

            }

            else if (treesInZone.Count > 0) // Pickup
            {


                heldTree = treesInZone[0];
                Vector3 snapMove = heldTree.transform.position - transform.position;
                playerController.OnPickup(snapMove);

                heldTree.transform.Translate(Vector3.up * liftHeight);
                heldTree.SetIsHeld(true);
                heldTree.transform.SetParent(transform);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
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
