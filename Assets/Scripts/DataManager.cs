using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using xPoke.CustomLog;

public class DataManager : Singleton<DataManager>
{
    private string _powerUpDataListFileName = "/power-up-data.dat";

    public void SaveTrashCount(int count)
    {
        if(count<0)
            return;
        PlayerPrefs.SetInt("TrashCount", count);
        PlayerPrefs.Save();
    }

    public int LoadTrashCount()
    {
        return PlayerPrefs.GetInt("TrashCount", 0);
    }

    public void SaveBestScore(int score)
    {
        if(score <0)
            return;

        int bestScore = PlayerPrefs.GetInt("BestScore", 0);
        if(score > bestScore)
            PlayerPrefs.SetInt("BestScore",score);
        PlayerPrefs.Save();
    }

    public void SaveTotalGearCount(int count)
    {
        if(count < 0)
            return;
        PlayerPrefs.SetInt("GearCount", count);
        PlayerPrefs.Save();
    }

    public int LoadTotalGearCount()
    {
        return PlayerPrefs.GetInt("GearCount");
    }

    public int LoadBestScore()
    {
        return PlayerPrefs.GetInt("BestScore", 0);
    }

    public void WriteTutorialFlag()
    {
        PlayerPrefs.SetInt("TutorialPlayed", 1);
        PlayerPrefs.Save();
    }

    public int ReadTutorialFlag()
    {
        return PlayerPrefs.GetInt("TutorialPlayed");
    }
    
    public void WriteShowTutorialFlag(int value)
    {
        if (value != 0 && value != 1)
            return;
        PlayerPrefs.SetInt("ShowTutorial", value);
        PlayerPrefs.Save();
    }

    public int ReadShowTutorialFlag()
    {
        return PlayerPrefs.GetInt("ShowTutorial");
    }

    public void SavePowerUpData(List<PowerUpData> dataList)
    {
        // JSON Creation
        SerializableList<PowerUpData> listObj = new SerializableList<PowerUpData>(dataList);
        string json = JsonUtility.ToJson(listObj.serializableList);
        CustomLog.Log(CustomLog.CustomLogType.SYSTEM, "JSON Created: "+json);

        // Save json string to file
        if(File.Exists(@"" + Application.persistentDataPath + _powerUpDataListFileName))
        {
            File.Delete(@""+ Application.persistentDataPath + _powerUpDataListFileName);
        }
        StreamWriter writer = new StreamWriter(Application.persistentDataPath + _powerUpDataListFileName);
        writer.WriteLine(json);
        writer.Close();
        CustomLog.Log(CustomLog.CustomLogType.SYSTEM, "JSON Saved");
    }

    public List<PowerUpData> LoadPowerUpData()
    {
        // Read from file the json string
        if (!File.Exists(@""+ Application.persistentDataPath + _powerUpDataListFileName))
            return null;
        StreamReader reader = new StreamReader(Application.persistentDataPath + _powerUpDataListFileName);
        string json = reader.ReadToEnd();
        CustomLog.Log(CustomLog.CustomLogType.SYSTEM, "JSON Read: "+json);
        reader.Close();
        return JsonUtility.FromJson<List<PowerUpData>>(json);
    }
}
