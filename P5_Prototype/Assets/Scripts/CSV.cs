using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System;

public class CSV : MonoBehaviour
{
    public static CSV instance = null;
    public int ParticipantNumber;
    public bool MR;
    private List<string[]> rowData = new List<string[]>();
    int[] Wavenumber = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19 };
   


    // Use this for initialization
    void Start()
    {
        //Save();
        if (instance == null)

            instance = this;
    }

    public void Save(int[] Waveblocked, int[] WaveMissed, float[] WaveFlow)
    {
     
        // Creating First row of titles manually
        string[] rowDataTemp = new string[6];
        rowDataTemp[0] = "Participant ID";
        rowDataTemp[1] = "MR";
        rowDataTemp[2] = "Wavenumber";
        rowDataTemp[3] = "Blocked";
        rowDataTemp[4] = "Missed";
        rowDataTemp[5] = "Average Flow";
        rowData.Add(rowDataTemp);

        
        for (int i = 0; i < Wavenumber.Length; i++)
        {
            
            
            rowDataTemp = new string[6];
            rowDataTemp[0] = ParticipantNumber.ToString();
            rowDataTemp[1] = MR.ToString();
            rowDataTemp[2] = Wavenumber[i].ToString();
            rowDataTemp[3] = Waveblocked[i].ToString();
            rowDataTemp[4] = WaveMissed[i].ToString();
            rowDataTemp[5] = WaveFlow[i].ToString();
            rowData.Add(rowDataTemp);
        }

        // Creating Last row manually
        //string[] rowDataTemp2 = new string[6];

        /*float sumOfMiss = 0;
        float sumOfPercent = 0;
        float sumOfBlock = 0;
        for (int i = 0; i < Wavenumber.Length; i++)
        {
            sumOfMiss += WaveMissed[i];
            sumOfBlock += Waveblocked[i];
            sumOfPercent += BlockPercent[i];
        }
        sumOfPercent = Mathf.FloorToInt(  sumOfPercent / Wavenumber.Length);
        
        rowDataTemp2[0] = " ";
        rowDataTemp2[1] = " ";
        rowDataTemp2[2] = " ";
        rowDataTemp2[3] = sumOfBlock.ToString();
        rowDataTemp2[4] = sumOfMiss.ToString();
        rowDataTemp2[5] = sumOfPercent.ToString();
        rowData.Add(rowDataTemp2);
        */


        string[][] output = new string[rowData.Count][];

        for (int i = 0; i < output.Length; i++)
        {
            output[i] = rowData[i];
        }

        int length = output.GetLength(0);
        string delimiter = ";";

        StringBuilder sb = new StringBuilder();

        for (int index = 0; index < length; index++)
        {
            sb.AppendLine(string.Join(delimiter, output[index]));
        }


        string filePath = getPath();

        StreamWriter outStream = System.IO.File.CreateText(filePath);
        outStream.WriteLine(sb);
        outStream.Close();
        Debug.Log("saved scores");
    }


    private string getPath()
    {
        if (MR)
        {
            return Application.dataPath + "/CSV/" + "MRdataP" +ParticipantNumber+".csv";
        }
        else
        {
            return Application.dataPath + "/CSV/" + "VRdataP" + ParticipantNumber + ".csv";
        }
        
     
    }


}