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
    

 
    void Start()
    {
        part = transform.GetChild(1).gameObject.GetComponent<ParticleSystem>(); // get child number 1 to this gameobjects particlesystem
        source = GetComponent<AudioSource>();
    
        part.Stop();
        setupThunder();
    }


    void setupThunder()
    {
        int r2 = Random.Range(0, thunderSounds.Length);
        source.clip = thunderSounds[r2];
        //Debug.Log(r2);
        float r = Random.Range(minWaitTime, maxWaitTime);
        StartCoroutine(manageThunder(r));
    }


    IEnumerator manageThunder(float waitingTime)
    {
        yield return new WaitForSeconds(waitingTime);
        part.Play();
        //yield return new WaitForSeconds(0.5f);
        source.Play();
        
        yield return new WaitForSeconds(4);
       
        part.Stop();
        setupThunder();
    }
}
