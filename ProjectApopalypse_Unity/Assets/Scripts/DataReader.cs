using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DataReader : MonoBehaviour
{
    //Check StreamingAssets and get a list of strings for each of the files to find in the StreamingAssets folder.
    //This class looks at a list of JSON files and reads them all.
    //Each of the files are then read and the name of the file decides what type of class will be created.
    //Each of files are ran for each row that's in them. All public instances are created.

    public string[] assetPaths;
    string[] keyString;
    List<string[]> instanceStrings = new List<string[]>();

    public List<string[]> units = new List<string[]>();

    void Awake()
    {
        string path = Application.streamingAssetsPath + "/StreamingAssets.txt";
        assetPaths = File.ReadAllLines(path);

        foreach (string assetPath in assetPaths)
        {
            //Debug.Log(assetPath);
            string[] fileText = File.ReadAllLines(Application.streamingAssetsPath + "/" + assetPath + ".txt");

            for (int i = 0; i < fileText.Length; i++)
            {
                string[] lineValues = fileText[i].Split('\t');
                instanceStrings.Add(lineValues);
            }

            switch (assetPath)
            {
                case "Unit":
                    {
                        units = instanceStrings;
                        break;
                    }
            }

            instanceStrings.Clear();
        }
    }

    void CreateInstance(string assetName,string technicalName)
    {
        //Create an instance of the name of the text file = which should be a preexisitng class
        //Use the key string to determine what variable to affect and then use the string to determine the value.
        //Create all the instances and ur done!

        List<string[]> currentStrings = new List<string[]>();
        string currentClass;

        switch (assetName)
        {
            case "Unit":
                {
                    currentStrings = units;
                    currentClass = "Unit";
                    break;
                }
        }

        //GameObject gameObject = Instantiate<>

        //First split up the first line by tabs so you have your key for all other lines.
        keyString = currentStrings[0];

        //Now for each line after the first, break it up and add it to the list of string arrays.
        for (int i = 1; i <  currentStrings.Count; i++)
        {
              
        }
        
    }
}
