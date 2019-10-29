
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spear : MonoBehaviour
{
    
    public float waitTime;
    public int WaveNumber;
    public GameObject startpos;
    public GameObject  Peak; 
    public GameObject target;
    public Transform box;
    public float speed = 0.4f;
    bool go = false;    
    float timeToReachTarget;
    public float t;

   
    
    enum ProjectileType { none, top, mid, bot };
    ProjectileType pt;

    public bool randomStartpos;
    public float LaunchAngle = 60f;
    Rigidbody rigid;

    public AudioClip[] SoundClips;
    public AudioSource ArrowHitSource;
    public AudioSource FleshHitSource;
    public AudioSource ShieldHitSource;



    // Start is called before the first frame update
    void Start()
    {

        SoundClips = WaveManager.instance.SoundClips;
        
        int R = Random.Range(0, 3);


        ArrowHitSource = gameObject.AddComponent<AudioSource>();
        ArrowHitSource.playOnAwake = false;
        ArrowHitSource.clip = SoundClips[R];

        FleshHitSource = gameObject.AddComponent<AudioSource>();
        FleshHitSource.playOnAwake = false;
        FleshHitSource.clip = SoundClips[3];

        ShieldHitSource = gameObject.AddComponent<AudioSource>();
        ShieldHitSource.playOnAwake = false;
        ShieldHitSource.clip = SoundClips[4];


        if (pt == ProjectileType.top)
        {
            target = GameObject.FindGameObjectWithTag("PTop");
        }
        else if (pt == ProjectileType.mid)
        {
            target = GameObject.FindGameObjectWithTag("PMid");
        }
        else if (pt == ProjectileType.bot)
        {
            target = GameObject.FindGameObjectWithTag("PBot");
        } else {
            Debug.Log("problems with findings targets!");
        }
        startpos = GetStartPos();
        timeToReachTarget = WaveManager.instance.timeToReachTarget;
        transform.position = startpos.transform.position;
        Peak = startpos.transform.GetChild(0).gameObject;
        GameObject.FindGameObjectWithTag("Peak");
        rigid = GetComponent<Rigidbody>();
        // transform.rotation = Quaternion.LookRotation(rigid.velocity) * transform.rotation;
        //transform.rotation = Quaternion.LookRotation(transform.position);
        //transform.rotation = Quaternion.LookRotation(-rigid.velocity);
        
       // transform.rotation = Quaternion.LookRotation(-Peak.transform.position);
        StartCoroutine(GoTimer());
    }

    // Update is called once per frame
    void Update()
    {
  
        if (go)
        {
           //audiomanager.instance.PlayArrowStarting();
            t += Time.deltaTime / timeToReachTarget;
           transform.position = Vector3.Lerp(startpos.transform.position, target.transform.position, t);
            transform.rotation = Quaternion.LookRotation(transform.position,  target.transform.position);

            /*Launch();
            if (t < 0.22f)
            {
                Vector3 sdirection = Peak.transform.position - transform.position;
                Quaternion rotation = Quaternion.LookRotation(-sdirection);
                transform.rotation = Quaternion.Lerp(transform.rotation, rotation, speed);
            }
            if (t >0.23f)
            {
                Vector3 direction = target.transform.position - transform.position;
                Quaternion rotation = Quaternion.LookRotation(-direction);
                transform.rotation = Quaternion.Lerp(transform.rotation, rotation, (t+0.2f)*Time.deltaTime);
            }
            if (t > 0.60f)
            {
                Vector3 direction = target.transform.position - transform.position;
                Quaternion rotation = Quaternion.LookRotation(-direction);
                transform.rotation = Quaternion.Lerp(transform.rotation, rotation, (t + 0.3f) * Time.deltaTime);
            }*/
        }
        
       
    }

    IEnumerator GoTimer()
    {
        yield return new WaitForSeconds(waitTime);
        go = true;
        //MeshRenderer m = GetComponent<MeshRenderer>();
        //m.enabled = true;
    }

    public void setEnum(string enumSet)
    {
        if (enumSet == "top"){
            pt = ProjectileType.top;
        }
        else if (enumSet == "mid"){
            pt = ProjectileType.mid;
        }
        else if (enumSet == "bot"){
            pt = ProjectileType.bot;
        }
        else{
            pt = ProjectileType.none;
        }
    }

    void Launch()
    {

        //rigid.velocity = -startpos.transform.forward*5;
        transform.position = (1.0f - t) * (1.0f - t) * startpos.transform.position + 2 * (1 - t) * t * Peak.transform.position + (t * t) * target.transform.position;
        // Vector3 direction = new Vector3(0,0,0);
        //direction.x = transform.position.x - target.transform.position.x;
        //direction.y = transform.position.y - target.transform.position.y;
        //direction.z = transform.position.z - target.transform.position.z;

        //float angle = Mathf.Atan2 (-direction.y, direction.x) * Mathf.Rad2Deg;
        //transform.rotation = (0, angle, 0);
        // Vector3 ops = new Vector3(0, angle, 0);
        // transform.rotation = Quaternion.LookRotation( ops );
    }

    GameObject GetStartPos()
    {
        if (randomStartpos)
        {
            try
            {
                GameObject[] startPositions = GameObject.FindGameObjectsWithTag("startPosition");
                int r = Random.Range(0, startPositions.Length);
                //Debug.Log("spawning from " + startPositions[r].name);
                return startPositions[r];
            }
            catch
            {
                Debug.LogError("Projectiles failed to find starting positions");
                return null;
            }
        } else
        {
            // alternativ ide: pil1->pos1, pil2->pos2 -> pil3->pos3 indtil igennem alle startpos, derefter pilx->pos1, pilx+1->pos2
            GameObject[] startPositions= GameObject.FindGameObjectsWithTag("startPosition");
            if (pt == ProjectileType.top)
            {
                return startPositions[2]; // top start pos
            }
            else if (pt == ProjectileType.mid)
            {
                return startPositions[1]; // mid start pos
            }
            else if (pt == ProjectileType.bot)
            {
                return startPositions[0]; // bottom start pos
            }
            else
            {
                Debug.Log("problems with findings starting posistions!");
                return null;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        
        if (collision.gameObject.tag == "shield"){

            ShieldHitSource.Play();
            go = false;
            
            WaveManager.instance.blockamount[WaveNumber] ++;
            WaveManager.instance.currentBlockCombo ++;
            WaveManager.instance.currentMissCombo = 0;
            transform.SetParent(collision.gameObject.transform);
            
            rigid.isKinematic = true;
            Destroy(gameObject, 10);
        
        }
        else if (collision.gameObject.tag == "PTop" || collision.gameObject.tag == "PMid" || collision.gameObject.tag == "PBot")
        {
            ArrowHitSource.Play();
            FleshHitSource.Play();
            WaveManager.instance.missamount[WaveNumber] ++;
            WaveManager.instance.currentBlockCombo = 0;
            WaveManager.instance.currentMissCombo ++;
            go = false;
            Destroy(gameObject,1);
        }


    }
}


