using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System;

public class StartupFunctions : MonoBehaviour
{
    public ReferenceLib toInitialize;
    public AudioClip defaultIntro;
    public AudioClip defaultLoop;
    public int intToSave;
    private void Start(){ 
        Application.targetFrameRate = 60;
        AudioManager.Instance.PlayMusic(defaultIntro, defaultLoop, false);
        //LoadSave();
    }

    void Awake(){
        PlayerPrefs.SetFloat("musicVolume", 1f);
        PlayerPrefs.SetFloat("effectVolume", 1f);

        toInitialize.Awake();
    }

    /*private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DoSave(intToSave);
            Debug.Log("saved");
        }
    }*/

    public static void DoSave(int myInt)
    {
        //write save data and checksum
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/icon.jpg";
        FileStream stream = new FileStream(path, FileMode.Create);
        formatter.Serialize(stream, myInt);
        stream.Close();

        FileStream stream2 = new FileStream(path, FileMode.Open);
        string path1 = Application.persistentDataPath + "/CrashHandler.exe";
        FileStream stream1 = new FileStream(path1, FileMode.Create);
        SHA256 hasher = SHA256.Create();
        formatter.Serialize(stream1, hasher.ComputeHash(stream2));
        stream1.Close();
        stream2.Close();
    }

    public static void LoadSave()
    {
        string path = Application.persistentDataPath + "/icon.jpg";
        if (!File.Exists(path))
        {
            Debug.Log("no save data");
            return;
        }
        if (CheckSaveIntegrity())
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            int dataFromFile = (int)formatter.Deserialize(stream);  //will need to implement as try/catch... what if bozo edits save file and it no longer successfully casts to desired type?
            Debug.Log(dataFromFile);
            stream.Close();
        }
        else
        {
            Debug.Log("save data was illegally modified");
        }
    }

    public static bool CheckSaveIntegrity()
    {
        string path = Application.persistentDataPath + "/icon.jpg";
        if (!File.Exists(path))
        {
            return false;
        }
        FileStream stream = new FileStream(path, FileMode.Open);
        SHA256 hasher = SHA256.Create();
        byte[] digest = hasher.ComputeHash(stream);
        stream.Close();
        path = Application.persistentDataPath + "/CrashHandler.exe";
        if (!File.Exists(path))
        {
            return false;
        }
        FileStream stream1 = new FileStream(path, FileMode.Open);
        BinaryFormatter formatter = new BinaryFormatter();
        byte[] checksum = formatter.Deserialize(stream1) as byte[];
        stream1.Close();
        for(int i = 0; i < checksum.Length; i++)
        {
            if(checksum[i] != digest[i])
            {
                return false;
            }
        }
        return true;
    }
}
