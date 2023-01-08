using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Basket : MonoBehaviour
{
    public UnityEvent onAppleCollectedEvent;

    public List<AudioSource> audioSources;
    public AudioClip clip;

    float pitchVariance = 0.1f;
    float basePitch;
    int nextIdx = 0;

    // Start is called before the first frame update
    void Start()
    {
        basePitch = audioSources[0].pitch;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("apple"))
        {
            Destroy(other.gameObject);
            onAppleCollectedEvent.Invoke();



            AudioSource audioSource = audioSources[nextIdx];

            nextIdx++;
            if (nextIdx >= audioSources.Count)
            {
                nextIdx = 0;
            }
            audioSource.pitch = basePitch + Random.Range(0, pitchVariance);
            audioSource.PlayOneShot(clip);
        }
    }
}
