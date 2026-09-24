using UnityEngine;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

// THIS IS ACTIVE INFO DURING GAMEPLAY
public class GameManager : MonoBehaviour
{
    //Private Variables

    //Public Variables
    public static GameManager manager;
    public ScoreData scoreData;
    public List<ScoreEntry> scoreList;
    public int score;
    public string initials;
    public int maxEntries = 8;
    public string filepath;

    private void Awake()
    {
        if (manager == null)
        {
            //If there is no manager, make this instance a manager; ensure it isn't destroyed
            manager = this;
            DontDestroyOnLoad(gameObject);
        } 
        else if (manager != this) 
        {
            //If there's a manager already, delete this one
            Destroy(gameObject);
        }

        //File Saving Code
        filepath = Path.Combine(Application.persistentDataPath, "scores.dat");

        //Load in Score on Awake()
        Load();
    }

    //Add a New Score
    public void AddNewScore()
    {
        // Create New Entry
        ScoreEntry entry = new ScoreEntry(initials, score);

        // Add to List
        scoreData.scoreList.Add(entry);

        //Sort List Descending (Highest Score First)
        scoreData.scoreList.Sort((x, y) => y.score.CompareTo(x.score));

        //Trim the List if it Exceeds Max Size
        if (scoreData.scoreList.Count > maxEntries)
        {
            scoreData.scoreList.RemoveRange(maxEntries, scoreData.scoreList.Count - maxEntries);
        }

        //Save Changes to the File
        Save();
    }

    //Save the File
    public void Save()
    {
        //Create a File and Push Data to It
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(filepath);

        //Instantiate the Class With Current Data
        ScoreData data = new ScoreData();
        data = scoreData;
        
        //Write Player Data to File
        bf.Serialize(file, data);

        //Close the File When Finished
        file.Close();
    }

    //Load the File
    public void Load()
    {
        //If a file exists, open and decode it, save it to the data, and close the file
        if (File.Exists(filepath))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(filepath, FileMode.Open);
            ScoreData data = (ScoreData)bf.Deserialize(file);
            file.Close();
            scoreData = data;
        }
    }

    //Return Score List
    public List<ScoreEntry> GetScoreList()
    {
        return scoreData.scoreList;
    }
}

//THIS SECTION IS THE DATA WE ARE SAVING
[System.Serializable]
public class ScoreEntry{
    public string initials;
    public int score;

    public ScoreEntry(string _initials, int _score)
    {
        initials = _initials;
        score = _score;
    }
}

[System.Serializable]
public class ScoreData
{
    public List<ScoreEntry> scoreList = new List<ScoreEntry>();
}