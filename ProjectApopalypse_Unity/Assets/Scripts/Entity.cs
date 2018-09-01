using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Entity : MonoBehaviour
{
    public string displayName;
    public string technicalName;
    public GameObject model;
    public Image icon;

    public string[] classifications;
    public string description;

    public int powerLevel;
    public int PowerLevel
    {
        get
        {
            //Calculate the power level of the entity here.
            return powerLevel;
        }
    }

    public enum Rarity
    {
        Common,
        Rare,
        Epic,
        Legendary
    }
    Rarity rarity;

    //Stage.Theme theme;

    Animator animator;
    Renderer renderer;
    Texture variation;

    void Start()
    {
    }
}
