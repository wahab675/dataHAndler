using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class DataHandler : SingeltonBase<DataHandler>
{
    public string fileName;
    public DATA inGameData;
    public string path;
    private BinaryFormatter _binaryFormatter;
    public FileStream fileStream;

    // Start is called before the first frame update
    public override void Awake()
    {
        base.Awake();
        path = Path.Combine(Application.persistentDataPath, fileName);
        loadData();
    }

    public void saveData()
    {
        save();
    }

    private void loadData()
    {
        if (File.Exists(path))
        {
            //load Here
            _binaryFormatter = new BinaryFormatter();
            fileStream = new FileStream(path,FileMode.Open);
            try
            {
                inGameData = _binaryFormatter.Deserialize(fileStream) as DATA;
                fileStream.Close();
            }
            catch (Exception e)
            {
                fileStream.Close();
                Console.WriteLine(e);
                throw;
            }
        }
        else
        {
            inGameData = new DATA();
        }
    }

    private void save()
    {
        _binaryFormatter = new BinaryFormatter();
        fileStream = new FileStream(path,FileMode.Create);
        try
        {
            _binaryFormatter.Serialize(fileStream, inGameData);
            fileStream.Close();
        }
        catch (Exception e)
        {
            fileStream.Close();
            Debug.LogWarning(e);
            throw;
        }
            
    }

    private void OnApplicationQuit()
    {
        save();
    }
    public void resetData()
    {
        inGameData = new DATA();
    }

}

[System.Serializable]
public class DATA
{
    public bool music;
    public bool sound;
    public bool haptic;
    public int levels;
    public bool GameLaunchEventSent;
    public Dictionary<int, int> allLevelTries;
    public DATA()
    {
        music = true;
        sound = true;
        haptic = true;
        levels = 1;
        GameLaunchEventSent = false;
        allLevelTries = new Dictionary<int, int>();
    }
}