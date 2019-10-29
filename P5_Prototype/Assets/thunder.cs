using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class thunder : MonoBehaviour
{
    ParticleSystem part;
    AudioSource source;
    public float maxWaitTime;
    public float minWaitTime;
    public AudioClip[] thunderSounds;
    
    // known issues:
    // lyden spiller ikke altid, måske fordi collision sker 2 gange
    // lynet er altid det samme sted
    // lynet er temmelig grimt og bouncer lidt 
    void Start()
    {
        part = GetComponent<ParticleSystem>();
        source = GetComponent<AudioSource>();
    
        part.Stop();
        setupThunder();
    }

    
    void OnParticleCollision(GameObject other)
    {
        
        source.Play();
        part.Stop();
        
        setupThunder();


    }

    void setupThunder()
    {
        int r2 = Random.Range(0, thunderSounds.Length);
        source.clip = thunderSounds[r2];

        float r = Random.Range(minWaitTime, maxWaitTime);
        StartCoroutine(manageThunder(r));
    }


    IEnumerator manageThunder(float waitingTime)
    {
        yield return new WaitForSeconds(waitingTime);
        part.Play();
    }
}
