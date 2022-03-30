using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.IO;

public class CsvReadWrite : MonoBehaviour
{
    Controller wheelController;

    private List<string[]> rowData = new List<string[]>();

    private void Awake() 
    {
        wheelController = GameObject.Find("Lucky wheel controller").GetComponent<Controller>();
    }
    
    void Start () {
        Save();
    }
    void Save(){

        string[] rowDataTemp = new string[5];
        rowDataTemp[0] = "spin index";
        rowDataTemp[1] = "aktuální kredit";
        rowDataTemp[2] = "bet";
        rowDataTemp[3] = "vyhra";
        rowDataTemp[4] = "nasobok";
        rowData.Add(rowDataTemp);

        for(int i = 0; i < 9999; i++)
        {
            wheelController.creditAmount -= wheelController.BetUpdate(100);
           int chance = wheelController.ChanceForWiningSpin();
           int bet = wheelController.BetUpdate(100);
           int winAmount = chance * bet;

            rowDataTemp = new string[5];
            rowDataTemp[0] = " "+i;
            rowDataTemp[1] =  wheelController.creditAmount.ToString();
            rowDataTemp[2] =  bet.ToString();
            rowDataTemp[3] =  winAmount.ToString();
            rowDataTemp[4] = chance.ToString();
            rowData.Add(rowDataTemp);
        }

        string[][] output = new string[rowData.Count][];

        for(int i = 0; i < output.Length; i++){
            output[i] = rowData[i];
        }

        int length = output.GetLength(0);
        string delimiter = ",";

        StringBuilder sb = new StringBuilder();
        
        for (int index = 0; index < length; index++)
            sb.AppendLine(string.Join(delimiter, output[index]));
        
        
        string filePath = getPath();

        StreamWriter outStream = System.IO.File.CreateText(filePath);
        outStream.WriteLine(sb);
        outStream.Close();
    }
    private string getPath(){
        #if UNITY_EDITOR
        return Application.dataPath +"/CSV/"+"Saved_data.csv";
        #elif UNITY_ANDROID
        return Application.persistentDataPath+"Saved_data.csv";
        #elif UNITY_IPHONE
        return Application.persistentDataPath+"/"+"Saved_data.csv";
        #else
        return Application.dataPath +"/"+"Saved_data.csv";
        #endif
    }
}
