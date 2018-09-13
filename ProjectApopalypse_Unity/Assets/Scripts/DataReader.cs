using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class DataReader : MonoBehaviour
{
    //Check StreamingAssets and get a list of strings for each of the files to find in the StreamingAssets folder.
    //This class looks at a list of JSON files and reads them all.
    //Each of the files are then read and the name of the file decides what type of class will be created.
    //Each of files are ran for each row that's in them. All public instances are created.

    public Entity defaultEntity;
    public Transform spawnTransform;
    public string[] assetPaths;

    public string[] keyString;
    List<string[]> instanceStrings = new List<string[]>();

    void Awake()
    {
        CreateInstance("Unit", "defaultUnit");
    }

    void CreateInstance(string className, string instanceName)
    {
        //Open the file from the streaming assets folder.
        string path = Application.streamingAssetsPath + "/" + className + ".txt";
        assetPaths = File.ReadAllLines(path);

        //Signify the first row as the key strings and split by tabs.
        keyString = assetPaths[0].Split('\t');

        //All other rows are instance strings and split by tabs.
        for (int i = 1; i < assetPaths.Length; i++)
        {
            string[] lineValues = assetPaths[i].Split('\t');
            instanceStrings.Add(lineValues);
        }

        //Instantiate the gameobject based on className.
        //var instance = Instantiate < (StringToType(className) > (defaultEntity,spawnTransform.position,spawnTransform.rotation) as (typeof(StringToType(className));


        /*Rigidbody rocketInstance;
            rocketInstance = Instantiate(rocketPrefab, barrelEnd.position, barrelEnd.rotation) as Rigidbody;
            rocketInstance.AddForce(barrelEnd.forward * 5000);*/
    }

    public Type StringToType(string typeAsString)
    {
        Type typeAsType = Type.GetType(typeAsString);
        return typeAsType;
    }

}

/*
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
*/

