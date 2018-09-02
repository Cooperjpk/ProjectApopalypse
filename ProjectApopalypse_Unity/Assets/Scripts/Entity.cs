using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    Renderer render;

    public string variationName;
    public Texture variation;

    void Start()
    {
        animator = GetComponent<Animator>();
        render = GetComponent<Renderer>();
    }
}
