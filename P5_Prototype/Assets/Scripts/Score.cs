
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
  
    private Image BarImage;
    private Text MissText;
    private Text BlockText;

    private Outline MissOutLine;
    private Outline BlockOutline;
    private Outline BarImageOutLine;
    public Color OutlineColor1;
    public Color OutlineColor2;
    public float glowSpeed;

    void Start()
    {

        BarImage = gameObject.transform.GetChild(0).GetComponent<Image>(); 
        BlockText = gameObject.transform.GetChild(1).GetComponent<Text>();
        MissText = gameObject.transform.GetChild(2).GetComponent<Text>(); 

        BarImage.fillAmount = 0;

        BarImageOutLine = gameObject.transform.GetChild(0).GetComponent<Outline>();
        MissOutLine = gameObject.transform.GetChild(3).GetComponent<Outline>();
        BlockOutline = gameObject.transform.GetChild(4).GetComponent<Outline>();
    }

    private void Update()
    {

        UpdateScores();
        outlineGlow();


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
  
        MissText.text= missSum.ToString();
        BlockText.text= blockSum.ToString();
        UpdateProgress(missSum, blockSum);

    }

    void UpdateProgress(int missSum, int blockSum)
    {
        // updating with each wave
        BarImage.fillAmount = WaveManager.instance.wavenumber * 0.025f;


        // updating with each arrow
        // we know 100% is 5 arrows per wave * 40 waves =200
        // the current procent is missed arrows + blocked arrrows
        // testing showed 200 is not enterily accurate, arrows can hit and miss at the same time
        // atleast in a desktop setting, maybe its different in vr

        //int sumOfSum = missSum + blockSum;
        //float percent = sumOfSum / 200f;  
        //BarImage.fillAmount = percent;
    }

    void outlineGlow()
    {
        Color lerpedColor = Color.Lerp(OutlineColor1, OutlineColor2, Mathf.PingPong(Time.time, glowSpeed));
        MissOutLine.effectColor = lerpedColor;
        BlockOutline.effectColor = lerpedColor;
        BarImageOutLine.effectColor = lerpedColor;
    }
}