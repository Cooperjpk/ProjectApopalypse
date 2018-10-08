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
    public int actHealth;
    public int actHealthMax;
    public int actHealthMin;
    public bool actHealthLocked;

    int curBaseArmor;
    public int actBaseArmor;
    public int actBaseArmorMax;
    public int actBaseArmorMin;
    public bool actBaseArmorLocked;

    int curTroopArmor;
    public int actTroopArmor;
    public int actTroopArmorMax;
    public int actTroopArmorMin;
    public bool actTroopArmorLocked;

    int curStructureArmor;
    public int actStructureArmor;
    public int actStructureArmorMax;
    public int actStructureArmorMin;
    public bool actStructureArmorLocked;

    int curDirectArmor;
    public int actDirectArmor;
    public int actDirectArmorMax;
    public int actDirectArmorMin;
    public bool actDirectArmorLocked;

    int curSplashArmor;
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

    int damageFraction = 0;

    void Start()
    {
        animator = GetComponent<Animator>();
        render = GetComponent<Renderer>();
    }

    public void ChangeHealth(int damage, DamageType damageType, string damageSource)
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
            default:
                {
                    Debug.Log("The damageSource did not have a recorded tag");
                    break;
                }
            case "Troop":
                {
                    damageFraction += curTroopArmor;
                    break;
                }
            case "Structure":
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
        }
        else if (curHealth >= actHealth)
        {
            curHealth = actHealth;
        }

        damageFraction = 0;
    }

    public virtual void SetCurrentVariables()
    {
        curHealth = actHealth;
        curBaseArmor = actBaseArmor;
        curTroopArmor = actTroopArmor;
        curStructureArmor = actTroopArmor;
        curDirectArmor = actDirectArmor;
        curSplashArmor = actSplashArmor;
    }
}
