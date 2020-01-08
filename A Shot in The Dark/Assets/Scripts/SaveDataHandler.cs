using UnityEngine;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
public class SaveDataHandler : MonoBehaviour {

    public static SaveDataHandler saveDataHandler;
    bool loadOnce;

    public int highscore;
    public int[] highscoreLevel = new int[30];
    public int lives;
    public int health;

    public bool isThereSaveData;

    void Awake() {
        if (saveDataHandler != null)
        {
            Destroy(gameObject);
        }
        else {
            saveDataHandler = this;
            DontDestroyOnLoad(gameObject);

            if (!loadOnce) {
                if (File.Exists(Application.persistentDataPath + "/playerSaveData.dat")) {
                    Load();
                }
                loadOnce = true;
            }

        }
    }

    public void Save() {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/playerSaveData.dat");
        PlayerData data = new PlayerData();

        //Data Here
        data.isThereSaveData = isThereSaveData;
        for (int i = 0; i < highscoreLevel.Length; i++) { data.highscoreLevel[i] = highscoreLevel[i]; }
        data.highscore = highscore;
        data.lives = lives;
        data.health = health;
        
        //End Data Here

        bf.Serialize(file, data);
        file.Close();
        print("Data Saved");
    }

    public void Load() {
        if (File.Exists(Application.persistentDataPath + "/playerSaveData.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/playerSaveData.dat", FileMode.Open);
            PlayerData data = (PlayerData)bf.Deserialize(file);
            file.Close();

            //Data Here
            highscore = data.highscore;
            for (int i = 0; i < highscoreLevel.Length; i++){highscoreLevel[i] = data.highscoreLevel[i];}

            isThereSaveData = data.isThereSaveData;
            lives = data.lives;
            health = data.health;

            //End Data here
            print("Data Loaded");
        }
    }

    [Serializable]
    class PlayerData {


        public int highscore;
        public int[] highscoreLevel = new int[30];
        public int lives;
        public int health;

        public bool isThereSaveData;

    }
}
