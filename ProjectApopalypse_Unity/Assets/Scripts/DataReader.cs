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

    //Attacks
    public Attack attackDefault;
    string[] attackKey;
    List<string[]> attackInstances = new List<string[]>();
    string[] attackStrings;

    //Passives
    public Passive passiveDefault;
    string[] passiveKey;
    List<string[]> passiveInstances = new List<string[]>();
    string[] passiveStrings;

    void Awake()
    {
        ReadFile("Units");
        ReadFile("Attacks");
        ReadFile("Passives");
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

        //Now that all values have been set, add the Attack gameObjects.
        if (unit.attack1 != string.Empty)
        {
            AddAttackToUnit(unit, unit.attack1);
        }
        if (unit.attack2 != string.Empty)
        {
            AddAttackToUnit(unit, unit.attack2);
        }
        if (unit.attack3 != string.Empty)
        {
            AddAttackToUnit(unit, unit.attack3);
        }
        if (unit.attack4 != string.Empty)
        {
            AddAttackToUnit(unit, unit.attack4);
        }

        //Now that all values have been set, add the Passive gameObjects.
        if (unit.passive1 != string.Empty)
        {
            AddPassiveToUnit(unit, unit.passive1);
        }
        if (unit.passive2 != string.Empty)
        {
            AddPassiveToUnit(unit, unit.passive2);
        }
        if (unit.passive3 != string.Empty)
        {
            AddPassiveToUnit(unit, unit.passive3);
        }
        if (unit.passive4 != string.Empty)
        {
            AddPassiveToUnit(unit, unit.passive4);
        }
    }

    void AddAttackToUnit(Unit currentUnit, string attackName)
    {
        //Add the Attack as a child gameobject to the current Unit 
        Attack attack = Instantiate<Attack>(attackDefault, currentUnit.transform.position, currentUnit.transform.rotation) as Attack;
        attack.transform.parent = currentUnit.transform;

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
            FieldInfo fieldInfo = attack.GetType().GetField(attackKey[i]);
            //Debug.Log(attackKey[i]);
            Type fieldType = fieldInfo.GetValue(attack).GetType();
            //Debug.Log(fieldType.ToString());

            if (fieldType == typeof(string))
            {
                fieldInfo.SetValue(attack, attackStrings[i]);
                Debug.Log(attackKey[i].ToString() + " set to " + fieldInfo.GetValue(attack));
            }
            else if (fieldType == typeof(int))
            {
                fieldInfo.SetValue(attack, int.Parse(attackStrings[i]));
                Debug.Log(attackKey[i].ToString() + " set to " + fieldInfo.GetValue(attack));
            }
            else if (fieldType == typeof(bool))
            {
                fieldInfo.SetValue(attack, bool.Parse(attackStrings[i]));
                Debug.Log(attackKey[i].ToString() + " set to " + fieldInfo.GetValue(attack));
            }
            else
            {
                Debug.LogError(attackKey[i] + " was not parsed because the type hasn't been specified.");
            }
        }
    }

    void AddPassiveToUnit(Unit currentUnit, string passiveName)
    {
        //Add the Attack as a child gameobject to the current Unit 
        Passive passive = Instantiate<Passive>(passiveDefault, currentUnit.transform.position, currentUnit.transform.rotation) as Passive;
        passive.transform.parent = currentUnit.transform;

        //Search for the correct string array and then declare.
        for (int i = 0; i < attackInstances.Count; i++)
        {
            if (attackInstances[i][0] == passiveName)
            {
                //Debug.Log(attackName);
                attackStrings = attackInstances[i];
                break;
            }
        }

        //Set all values of the attack based in what's in the string array.
        for (int i = 0; i < attackStrings.Length; i++)
        {
            FieldInfo fieldInfo = passive.GetType().GetField(attackKey[i]);
            //Debug.Log(attackKey[i]);
            Type fieldType = fieldInfo.GetValue(passive).GetType();
            //Debug.Log(fieldType.ToString());

            if (fieldType == typeof(string))
            {
                fieldInfo.SetValue(passive, attackStrings[i]);
                Debug.Log(attackKey[i].ToString() + " set to " + fieldInfo.GetValue(passive));
            }
            else if (fieldType == typeof(int))
            {
                fieldInfo.SetValue(passive, int.Parse(attackStrings[i]));
                Debug.Log(attackKey[i].ToString() + " set to " + fieldInfo.GetValue(passive));
            }
            else if (fieldType == typeof(bool))
            {
                fieldInfo.SetValue(passive, bool.Parse(attackStrings[i]));
                Debug.Log(attackKey[i].ToString() + " set to " + fieldInfo.GetValue(passive));
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
            case "Passives":
                {
                    //Signify the first row as the key strings and split by tabs.
                    passiveKey = assetPaths[0].Split('\t');

                    //All other rows are instance strings and split by tabs.
                    for (int i = 1; i < assetPaths.Length; i++)
                    {
                        string[] lineValues = assetPaths[i].Split('\t');
                        passiveInstances.Add(lineValues);
                    }
                    break;
                }
        }
    }
}

