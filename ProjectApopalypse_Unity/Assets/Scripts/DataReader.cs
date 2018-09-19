using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Reflection;

public class DataReader : MonoBehaviour
{
    //Check StreamingAssets and get a list of strings for each of the files to find in the StreamingAssets folder.
    //This class looks at a list of JSON files and reads them all.
    //Each of the files are then read and the name of the file decides what type of class will be created.
    //Each of files are ran for each row that's in them. All public instances are created.

    string[] assetPaths;

    //Units
    public Unit unitDefault;
    string[] unitKey;
    List<string[]> unitInstances = new List<string[]>();

    string[] unitString;

    void Awake()
    {
        //ReadFile("Unit");
        //InstantiateUnit("defaultUnit", transform.position, transform.rotation);
    }

    void InstantiateUnit(string instanceName, Vector3 spawnLocation, Quaternion spawnRotation)
    {
        //Instantiate the unit using the unitDefault and then applying changes.
        Unit unit = Instantiate<Unit>(unitDefault, spawnLocation, spawnRotation) as Unit;

        //Search for the correct string array and then declare
        for (int i = 0; i < unitInstances.Count; i++)
        {
            if (unitInstances[i][0] == instanceName)
            {
                //Debug.Log(instanceName);
                unitString = unitInstances[i];
                break;
            }
        }

        //Set all values of the unit based in what's in the string array.
        for (int i = 0; i < unitString.Length; i++)
        {
            FieldInfo fieldInfo = unit.GetType().GetField(unitKey[i]);
            Type fieldType = fieldInfo.GetValue(unit).GetType();
            //Debug.Log(fieldType.ToString());

            if (fieldType == typeof(string) && unitKey[i].Contains("attack"))
            {
                //This is straight up where the component that it needs to be added to the unit.
                Debug.Log("What's poppin ITS ME, YA BOi COOOPP");
                fieldInfo.SetValue(unit, unitString[i]);
                Debug.Log(unitKey[i].ToString() + " set to " + fieldInfo.GetValue(unit));
            }
            else if (fieldType == typeof(string))
            {
                fieldInfo.SetValue(unit, unitString[i]);
                Debug.Log(unitKey[i].ToString() + " set to " + fieldInfo.GetValue(unit));
            }
            else if (fieldType == typeof(int))
            {
                fieldInfo.SetValue(unit, int.Parse(unitString[i]));
                Debug.Log(unitKey[i].ToString() + " set to " + fieldInfo.GetValue(unit));
            }
            else if (fieldType == typeof(bool))
            {
                fieldInfo.SetValue(unit, bool.Parse(unitString[i]));
                Debug.Log(unitKey[i].ToString() + " set to " + fieldInfo.GetValue(unit));
            }
            else
            {
                Debug.LogError(unitKey[i] + " was not parsed because the type hasn't been specified.");
            }
        }
    }

    void ReadFile(string fileName)
    {
        //Open the file from the streaming assets folder.
        string path = Application.streamingAssetsPath + "/" + fileName + ".txt";
        assetPaths = File.ReadAllLines(path);

        //Depending on the file name, get the data from the file and store it.
        switch (fileName)
        {
            default:
                {
                    Debug.LogError("The fileName is not set up for reading.");
                    break;
                }
            case "Unit":
                {
                    //Signify the first row as the key strings and split by tabs.
                    unitKey = assetPaths[0].Split('\t');

                    //All other rows are instance strings and split by tabs.
                    for (int i = 1; i < assetPaths.Length; i++)
                    {
                        string[] lineValues = assetPaths[i].Split('\t');
                        unitInstances.Add(lineValues);
                    }
                    break;
                }
        }
    }

    public Type StringToType(string typeAsString)
    {
        Type typeAsType = Type.GetType(typeAsString);
        return typeAsType;
    }

}

