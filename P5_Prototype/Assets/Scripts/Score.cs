
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
  
    private Image BarImage;
    private Text MissText;
    private Text BlockText;

    void Start()
    {

        BarImage = gameObject.transform.GetChild(0).GetComponent<Image>(); 
        BlockText = gameObject.transform.GetChild(1).GetComponent<Text>();
        MissText = gameObject.transform.GetChild(2).GetComponent<Text>(); 

        BarImage.fillAmount = 0;
    }

    private void Update()
    {
        UpdateScores();
        UpdateProgress();
    }

    void UpdateScores()
    {
        int missSum = 0;
        int blockSum = 0;
        for (int i = 0; i < WaveManager.instance.missamount.Length; i++)
        {
            missSum += WaveManager.instance.missamount[i];
            blockSum += WaveManager.instance.blockamount[i];
        }
     /*   for (int i = 0; i < WaveManager.instance.blockamount.Length; i++)
        {

            blockSum += WaveManager.instance.blockamount[i];
        }
      
*/ 
        MissText.text= missSum.ToString();
        BlockText.text= blockSum.ToString();
    }

    void UpdateProgress()
    {
        BarImage.fillAmount = WaveManager.instance.wavenumber * 0.025f;
    }
}