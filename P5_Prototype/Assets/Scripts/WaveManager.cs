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
    public GameObject EndCanvas;
    public GameObject ProjectileDad;
    public GameObject Arrow;
    public GameObject Spear;
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

    int wavenumber=0; // husk at første wave er nummer 0
    public int[] blockamount = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    public int[] missamount = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    public float[] WaveFlow= { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
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
        float x = 0;
        for (int i = 0; i < wavePic.width; i++)
        {
            x=i;
            
                Color pixel = wavePic.GetPixel(i, 1);
                Color pixelY = wavePic.GetPixel(i, 0);
                string target="";
                string starting=""; 


                if (pixel == Color.red)
                {
                    starting="top";
                    coloredPixels++;
                }

                else if (pixel == Color.green)
                {
                    starting= "mid";
                    coloredPixels++;
                }


                else if (pixel == Color.blue)
                {
                    starting= "bot";
                    coloredPixels++;
                }
                if (pixelY == Color.red)
                {
                    
                    target="right";
                    coloredPixels++;
                }

                else if (pixelY == Color.green)
                {
                    target= "center";
                    coloredPixels++;
                }


                else if (pixelY == Color.blue)
                {
                    target= "left";
                    coloredPixels++;
                }


                else
                {
                    weirdPixels++;
                }
            spawnProjectile(starting, target, x);
        }
        //Debug.Log(string.Format("weird pixels {0}, colored pixels {1}", weirdPixels, coloredPixels));

    }
    void spawnProjectile(string start, string target, float x)
    {
        GameObject pro = null;
        
        Flow();
 
     
           
                pro = Instantiate(Arrow, transform.position, transform.rotation);
                pro.GetComponent<Arrow>().setEnum(target);
                pro.GetComponent<Arrow>().setEnum(start);
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
        if (currentBlockCombo >= 1)
        {
            if (CurrentFlow > flowMin)
            {
                CurrentFlow -= 0.4f*Time.deltaTime;
                currentBlockCombo = 0;
                currentMissCombo=0;
              
            }
        }
        if (currentMissCombo >= 1)
        {
            if (CurrentFlow < flowMax)
            {

            CurrentFlow += .2f*Time.deltaTime;
            currentMissCombo=0;
             currentBlockCombo = 0;
            }
        }
    }

    IEnumerator Waves()
    {

        if (wavenumber == 0)
        {
            yield return new WaitForSeconds(waitTimeBeforeStart);
        }


        Debug.Log("Send out wave " + wavenumber + " based on flow of " + CurrentFlow);
        if (Lowerflow && wavenumber<maxWaves)
        {
            
            
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
            if(wavenumber == 0){
                    if (currentBlockCombo >= 1)
        {
            if (CurrentFlow > flowMin)
            {
                CurrentFlow -= 5f*Time.deltaTime;
                currentBlockCombo = 0;
                currentMissCombo=0;
              
            }
        }
        if (currentMissCombo >= 1)
        {
            if (CurrentFlow < flowMax)
            {

            CurrentFlow += 10f*Time.deltaTime;
            currentMissCombo=0;
             currentBlockCombo = 0;
            }
        }
            }
            buildWave(waveTextures[wavenumber]);
            WaveFlow[wavenumber] = CurrentFlow;
            wavenumber++;
        } 
        else if (wavenumber < maxWaves && !Lowerflow)
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

       
        yield return new WaitForSeconds(CurrentFlow*10);
        StartCoroutine(Waves());
           

        }
    }

