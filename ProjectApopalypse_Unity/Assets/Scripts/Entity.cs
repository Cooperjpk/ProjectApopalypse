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

    public enum Rank
    {
        S,
        A,
        B,
        C,
        D
    }
    public Rank rank;

    //Stage.Theme theme;

    Animator animator;
    Renderer render;

    public string variationName;
    public Texture variation;

    //Health
    int curHealth;
    public Variable totHealth;
    public int actHealth;
    public int actHealthMax;
    public int actHealthMin;
    public bool actHealthLocked;

    int curBaseArmor;
    public Variable totBaseArmor;
    public int actBaseArmor;
    public int actBaseArmorMax;
    public int actBaseArmorMin;
    public bool actBaseArmorLocked;

    int curTroopArmor;
    public Variable totTroopArmor;
    public int actTroopArmor;
    public int actTroopArmorMax;
    public int actTroopArmorMin;
    public bool actTroopArmorLocked;

    int curStructureArmor;
    public Variable totStructureArmor;
    public int actStructureArmor;
    public int actStructureArmorMax;
    public int actStructureArmorMin;
    public bool actStructureArmorLocked;

    int curDirectArmor;
    public Variable totDirectArmor;
    public int actDirectArmor;
    public int actDirectArmorMax;
    public int actDirectArmorMin;
    public bool actDirectArmorLocked;

    int curSplashArmor;
    public Variable totSplashArmor;
    public int actSplashArmor;
    public int actSplashArmorMax;
    public int actSplashArmorMin;
    public bool actSplashArmorLocked;

    public bool isAlive;
    public bool isInvincible;

    public enum DamageType
    {
        Direct,
        Splash
    }

    public enum DamageSource
    {
        Troop,
        Structure
    }

    int damageFraction = 0;

    void Awake()
    {
        LoadVariables();    
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        render = GetComponent<Renderer>();
    }

    public void ChangeHealth(int damage, DamageType damageType, DamageSource damageSource)
    {
        switch (damageType)
        {
            case DamageType.Direct:
                {
                    damageFraction += curDirectArmor;
                    break;
                }
            case DamageType.Splash:
                {
                    damageFraction += curSplashArmor;
                    break;
                }
        }

        switch (damageSource)
        {
            case DamageSource.Troop:
                {
                    damageFraction += curTroopArmor;
                    break;
                }
            case DamageSource.Structure:
                {
                    damageFraction += curStructureArmor;
                    break;
                }
        }

        float damageMultiplier = damage / (damage + damageFraction + curBaseArmor);
        float finalDamage = damage * damageMultiplier;
        curHealth -= Mathf.RoundToInt(finalDamage);

        if (curHealth <= 0)
        {
            curHealth = 0;
            isAlive = false;
            //Death happens here.
        }
        else if (curHealth >= totHealth.integer)
        {
            curHealth = totHealth.integer;
        }

        damageFraction = 0;
    }

    public virtual void LoadVariables()
    {
        totHealth.integer = actHealth;
        totHealth.maxInt = actHealthMax;
        totHealth.minInt = actHealthMin;
        totHealth.isLocked = actHealthLocked;

        totBaseArmor.integer = actBaseArmor;
        totBaseArmor.maxInt = actBaseArmorMax;
        totBaseArmor.minInt = actBaseArmorMin;
        totBaseArmor.isLocked = actBaseArmorLocked;

        totTroopArmor.integer = actTroopArmor;
        totTroopArmor.maxInt = actTroopArmorMax;
        totTroopArmor.minInt = actTroopArmorMin;
        totTroopArmor.isLocked = actTroopArmorLocked;

        totStructureArmor.integer = actStructureArmor;
        totStructureArmor.maxInt = actStructureArmorMax;
        totStructureArmor.minInt = actStructureArmorMin;
        totStructureArmor.isLocked = actStructureArmorLocked;

        totDirectArmor.integer = actDirectArmor;
        totDirectArmor.maxInt = actDirectArmorMax;
        totDirectArmor.minInt = actDirectArmorMin;
        totDirectArmor.isLocked = actDirectArmorLocked;

        totSplashArmor.integer = actSplashArmor;
        totSplashArmor.maxInt = actSplashArmorMax;
        totSplashArmor.minInt = actSplashArmorMin;
        totSplashArmor.isLocked = actSplashArmorLocked;
    }
}
