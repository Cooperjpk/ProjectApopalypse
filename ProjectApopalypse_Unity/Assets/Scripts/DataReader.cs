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

    void Awake()
    {
        string path = Application.streamingAssetsPath + "/StreamingAssets.txt";
        assetPaths = File.ReadAllLines(path);

        foreach(string assetPath in assetPaths)
        {
            Debug.Log(assetPath);
            string filePath = Application.streamingAssetsPath + "/" + assetPath + ".txt";
            string[] filePaths = File.ReadAllLines(filePath);

            //https://answers.unity.com/questions/1354236/ioexception-sharing-violation-on-path-trying-to-sa.html
        }
    }
}
