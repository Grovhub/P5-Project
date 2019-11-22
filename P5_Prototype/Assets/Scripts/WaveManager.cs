using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
   
    public static WaveManager instance = null;
  
    public int maxWaves;
    public float timeToReachTarget;

    public float waitTimeBeforeStart;
    public float waitTimeBeforeEnd;
    public GameObject StartPositions;
    public GameObject TargetPositions;
    public GameObject EndCanvas;
    public GameObject ProjectileDad;
    public GameObject Arrow;
    public GameObject Spear;
    public GameObject Viking;
    public bool Lowerflow;
    public Texture2D[] waveTextures;

   
    public float flowMax;
    public float CurrentFlow = 1f;
    public float flowMin;
    private List<float> AverageFlowList = new List<float>();
   // [HideInInspector] public float AverageFlow;
    public float currentBlockCombo;
    public float currentMissCombo;
    
    
    int waitTime;

    public int wavenumber=0; // husk at første wave er nummer 0
    public int[] blockamount = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};
    public int[] missamount = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    public float[] WaveFlow= { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    public AudioClip[] SoundClips;

    void Awake()
    {
        if (instance == null)

            instance = this;

        else if (instance != this)

            Destroy(gameObject);


        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        StartCoroutine(Waves());
    }

    private void Update()
    {
        Flow();
    }


    void buildWave(Texture2D wavePic)
    {
        var weirdPixels = 0;
        var coloredPixels = 0;
      
        for (int i = 0; i < wavePic.width; i++)
        {
 
                Color pixel = wavePic.GetPixel(i, 1);
                Color pixelY = wavePic.GetPixel(i, 0);
                GameObject startPos = null;
                GameObject TargetPos = null;
               
            if (pixel == Color.red)
                {
                   // starting="top";
                    startPos = StartPositions.transform.GetChild(1).gameObject;
                    coloredPixels++;
                }

            else if (pixel == Color.green)
                {
                   // starting= "mid";
                    startPos = StartPositions.transform.GetChild(0).gameObject;
                    coloredPixels++;
                }
            else if (pixel == Color.blue)
                {
                   // starting= "bot";
                    startPos = StartPositions.transform.GetChild(2).gameObject;
                    coloredPixels++;
                }

            if (pixelY == Color.red)
                {
                    
                    //target="right";
                    TargetPos = TargetPositions.transform.GetChild(1).gameObject;
                    coloredPixels++;
                }

            else if (pixelY == Color.green)
                {
                // target= "center";
                    TargetPos = TargetPositions.transform.GetChild(0).gameObject;
                    coloredPixels++;
                }


            else if (pixelY == Color.blue)
                {
                    //target= "left";
                    TargetPos = TargetPositions.transform.GetChild(2).gameObject;
                    coloredPixels++;
                }
            else
                {
                    weirdPixels++;
                }
        spawnProjectile(startPos, TargetPos, i);
        }
        //Debug.Log(string.Format("weird pixels {0}, colored pixels {1}", weirdPixels, coloredPixels));

    }
    void spawnProjectile(GameObject start, GameObject target, float x)
    {
        GameObject pro = null;
        pro = Instantiate(Arrow, transform.position, transform.rotation);
        pro.GetComponent<Arrow>().target = target;
        pro.GetComponent<Arrow>().startpos = start;
        pro.GetComponent<Arrow>().waitTime = x * CurrentFlow;
        pro.GetComponent<Arrow>().WaveNumber = wavenumber;
        pro.transform.SetParent(ProjectileDad.transform);

    
    }

    void gameEnd()
    {
        CSV.instance.Save( blockamount, missamount, WaveFlow);
    }

    void Flow()
    {
        if (!Lowerflow){
            if (currentBlockCombo >= 2)
            {
                    if (CurrentFlow > flowMin)
                         {
                            CurrentFlow -= CurrentFlow/2*Time.deltaTime;
                            currentBlockCombo = 0;
                            currentMissCombo=0;
              
                         }
            }
            if (currentMissCombo >= 1)
            {
                    if (CurrentFlow < flowMax)
                    {

                            CurrentFlow += 1f*Time.deltaTime;
                            currentMissCombo=0;
                            currentBlockCombo = 0;
                    }

            }
        }else if(Lowerflow){
            if (currentBlockCombo >= 2)
            {
                    if (CurrentFlow > flowMin)
                         {
                            CurrentFlow -= CurrentFlow*Time.deltaTime;
                            currentBlockCombo = 0;
                            currentMissCombo=0;
              
                         }
            }
            if (currentMissCombo >= 1)
            {
                    if (CurrentFlow < flowMax)
                    {

                            CurrentFlow += 1f*Time.deltaTime;
                            currentMissCombo=0;
                            currentBlockCombo = 0;
                    }

            }
    
        }
    
    }

    IEnumerator Waves()
    {

        if (wavenumber == 0)
        {

            yield return new WaitForSeconds(3);
            AudioSource as87 = Viking.GetComponent<AudioSource>();
            as87.Play();
            yield return new WaitForSeconds(1);
            spawnProjectile(StartPositions.transform.GetChild(2).gameObject, Viking, 1);
            spawnProjectile(StartPositions.transform.GetChild(0).gameObject, Viking, 2);
            spawnProjectile(StartPositions.transform.GetChild(1).gameObject, Viking, 3);
            yield return new WaitForSeconds(10);
            missamount[wavenumber]=0;
            Destroy(Viking.gameObject, 3);
        }


     
        Debug.Log("Send out wave " + wavenumber + " based on flow of " + CurrentFlow);
      
            
            
           /* for (int i = 0; i < 10; i++)
            {
                int r = Random.Range(0, 2);
                if (r == 0)
                {
                    spawnProjectile(i, "top");
                }
                else if (r == 1)
                {
                    spawnProjectile(i, "mid");
                }
                else if (r == 2)
                {
                    spawnProjectile(i, "mid");
                }
            }*/
    
        if (wavenumber < maxWaves)
        {

           buildWave(waveTextures[wavenumber]);
            WaveFlow[wavenumber] = CurrentFlow;
            wavenumber++;
        }else
        {
            yield return new WaitForSeconds(waitTimeBeforeEnd);
            EndCanvas.SetActive(true);
            gameEnd();
            StopCoroutine(Waves());
        }

       
        yield return new WaitForSeconds(CurrentFlow*5);
        StartCoroutine(Waves());
           

        }
    }

