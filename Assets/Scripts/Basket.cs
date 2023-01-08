using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Basket : MonoBehaviour
{
    public UnityEvent onAppleCollectedEvent;

    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("apple"))
        {
            Destroy(other.gameObject);
            onAppleCollectedEvent.Invoke();
        }
    }
}
