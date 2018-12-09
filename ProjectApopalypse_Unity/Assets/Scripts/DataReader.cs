using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Reflection;

public class DataReader : MonoBehaviour
{
    string[] assetPaths;

    //Units
    public Unit unitDefault;
    string[] unitKey;
    List<string[]> unitInstances = new List<string[]>();
    string[] unitStrings;

    void Awake()
    {
        ReadFile("Units");
        InstantiateUnit("defaultSplashUnit", transform.position, transform.rotation);
        InstantiateUnit("defaultSplashUnit", transform.position, transform.rotation);
        InstantiateUnit("defaultSplashUnit", transform.position, transform.rotation);

    }

    void InstantiateUnit(string instanceName, Vector3 spawnLocation, Quaternion spawnRotation)
    {
        //Instantiate the unit using the unitDefault and then applying changes.
        Unit unit = Instantiate<Unit>(unitDefault, spawnLocation, spawnRotation) as Unit;

        //Set unit to the player layer.
        unit.gameObject.layer = 8;

        //Search for the correct string array and then declare.
        for (int i = 0; i < unitInstances.Count; i++)
        {
            if (unitInstances[i][0] == instanceName)
            {
                //Debug.Log(instanceName);
                unitStrings = unitInstances[i];
                break;
            }
        }

        //Set all values of the unit based on what's in the string array.
        for (int i = 0; i < unitStrings.Length; i++)
        {
            FieldInfo fieldInfo = unit.GetType().GetField(unitKey[i]);
            Type fieldType = fieldInfo.GetValue(unit).GetType();
            //Debug.Log(fieldType.ToString());

            if (unitKey[i] == "technicalName")
            {
                unit.name = unitStrings[i];
                fieldInfo.SetValue(unit, unitStrings[i]);
                //Debug.Log(unitStrings[i].ToString() + " set as unit's name");
            }
            else if (fieldType == typeof(string))
            {
                fieldInfo.SetValue(unit, unitStrings[i]);
                //Debug.Log(unitKey[i].ToString() + " set to " + fieldInfo.GetValue(unit));
            }
            else if (fieldType == typeof(int))
            {
                fieldInfo.SetValue(unit, int.Parse(unitStrings[i]));
                //Debug.Log(unitKey[i].ToString() + " set to " + fieldInfo.GetValue(unit));
            }
            else if (fieldType == typeof(float))
            {
                fieldInfo.SetValue(unit, float.Parse(unitStrings[i]));
                //Debug.Log(unitKey[i].ToString() + " set to " + fieldInfo.GetValue(unit));
            }
            else if (fieldType == typeof(bool))
            {
                fieldInfo.SetValue(unit, bool.Parse(unitStrings[i]));
                //Debug.Log(unitKey[i].ToString() + " set to " + fieldInfo.GetValue(unit));
            }
            else if (fieldType == typeof(Unit.SplashOrigin))
            {
                Unit.SplashOrigin splashOrigin = (Unit.SplashOrigin)System.Enum.Parse( typeof( Unit.SplashOrigin ), unitStrings[i]);
                fieldInfo.SetValue(unit, splashOrigin);
                //Debug.Log(unitKey[i].ToString() + " set to " + fieldInfo.GetValue(unit));
            }
            else if (fieldType == typeof(Unit.SplashType))
            {
                Unit.SplashType splashType = (Unit.SplashType)System.Enum.Parse(typeof(Unit.SplashType), unitStrings[i]);
                fieldInfo.SetValue(unit, splashType);
                //Debug.Log(unitKey[i].ToString() + " set to " + fieldInfo.GetValue(unit));
            }
            else if (fieldType == typeof(Unit.MoveType))
            {
                Unit.MoveType moveType = (Unit.MoveType)System.Enum.Parse(typeof(Unit.MoveType), unitStrings[i]);
                fieldInfo.SetValue(unit, moveType);
                //Debug.Log(unitKey[i].ToString() + " set to " + fieldInfo.GetValue(unit));
            }
            else if (fieldType == typeof(Unit.Rank))
            {
                Unit.Rank rank = (Unit.Rank)System.Enum.Parse(typeof(Unit.Rank), unitStrings[i]);
                fieldInfo.SetValue(unit, rank);
                //Debug.Log(unitKey[i].ToString() + " set to " + fieldInfo.GetValue(unit));
            }
            else
            {
                Debug.LogError(unitKey[i] + " was not parsed because the type hasn't been specified.");
            }
        }
        
        //Now that the unit is pretty much setup, load and set the variables.
        unit.SetCurrentVariables();
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
            case "Units":
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
}

