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
    string[] unitStrings;

    //Attacks
    string[] attackKey;
    List<string[]> attackInstances = new List<string[]>();
    string[] attackStrings;

    void Awake()
    {
        ReadFile("Units");
        ReadFile("Attacks");
        InstantiateUnit("defaultUnit", transform.position, transform.rotation);
    }

    void InstantiateUnit(string instanceName, Vector3 spawnLocation, Quaternion spawnRotation)
    {
        //Instantiate the unit using the unitDefault and then applying changes.
        Unit unit = Instantiate<Unit>(unitDefault, spawnLocation, spawnRotation) as Unit;

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

        //Set all values of the unit based in what's in the string array.
        for (int i = 0; i < unitStrings.Length; i++)
        {
            FieldInfo fieldInfo = unit.GetType().GetField(unitKey[i]);
            Type fieldType = fieldInfo.GetValue(unit).GetType();
            //Debug.Log(fieldType.ToString());

            if(unitKey[i] == "technicalName")
            {
                unit.name = unitStrings[i];
                //Debug.Log(unitKey[i].ToString() + " set as unit's name);
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
            else if (fieldType == typeof(bool))
            {
                fieldInfo.SetValue(unit, bool.Parse(unitStrings[i]));
                //Debug.Log(unitKey[i].ToString() + " set to " + fieldInfo.GetValue(unit));
            }
            else
            {
                Debug.LogError(unitKey[i] + " was not parsed because the type hasn't been specified.");
            }
        }

        //Now that all values have been set, add the Attack component.
        AddAttackToUnit(unit, unit.attackName);
    }

    void AddAttackToUnit(Unit unit, string attackName)
    {
        //Add the Attack script component to the unit.
        Attack attackingUnit = unit.gameObject.AddComponent<Attack>();

        //Search for the correct string array and then declare.
        for (int i = 0; i < attackInstances.Count; i++)
        {
            if (attackInstances[i][0] == attackName)
            {
                //Debug.Log(attackName);
                attackStrings = attackInstances[i];
                break;
            }
        }

        //Set all values of the attack based in what's in the string array.
        for (int i = 0; i < attackStrings.Length; i++)
        {
            /*
            FieldInfo fieldInfo = attackingUnit.GetType().GetField(attackKey[i]);
            //Debug.Log(attackKey[i]);
            //The line below is getting an NRE and I'm not sure why. This is blocking the Attack variables.
            Type fieldType = fieldInfo.GetValueDirect(attackingUnit).GetType();
            Debug.Log(fieldType.ToString());
            */

            FieldInfo fieldInfo = attackingUnit.GetType().GetField(attackKey[i]);
            Debug.Log(fieldInfo.GetValue(attackingUnit).ToString());
            Type fieldType = fieldInfo.GetValue(attackingUnit).GetType();
            //Debug.Log(fieldType.ToString());

            if (fieldType == typeof(string))
            {
                fieldInfo.SetValue(attackingUnit, attackStrings[i]);
                Debug.Log(attackKey[i].ToString() + " set to " + fieldInfo.GetValue(attackingUnit));
            }
            else if (fieldType == typeof(int))
            {
                fieldInfo.SetValue(attackingUnit, int.Parse(attackStrings[i]));
                Debug.Log(attackKey[i].ToString() + " set to " + fieldInfo.GetValue(attackingUnit));
            }
            else if (fieldType == typeof(bool))
            {
                fieldInfo.SetValue(attackingUnit, bool.Parse(attackStrings[i]));
                Debug.Log(attackKey[i].ToString() + " set to " + fieldInfo.GetValue(attackingUnit));
            }
            else
            {
                Debug.LogError(attackKey[i] + " was not parsed because the type hasn't been specified.");
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
            case "Attacks":
                {
                    //Signify the first row as the key strings and split by tabs.
                    attackKey = assetPaths[0].Split('\t');

                    //All other rows are instance strings and split by tabs.
                    for (int i = 1; i < assetPaths.Length; i++)
                    {
                        string[] lineValues = assetPaths[i].Split('\t');
                        attackInstances.Add(lineValues);
                    }
                    break;
                }
        }
    }
}

