using System.Collections;
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
      

        MissText.text= missSum.ToString();
        BlockText.text = missSum.ToString();
    }

    void UpdateProgress()
    {
        BarImage.fillAmount = WaveManager.instance.wavenumber * 2.5f;
    }
}
