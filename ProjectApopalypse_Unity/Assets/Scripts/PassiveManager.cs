using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveManager : MonoBehaviour {

    public List<Passive> passives = new List<Passive>();

    Unit unit;
    Unit target;
    Unit[] allies;
    Unit[] enemies;

    void OnEnable()
    {
        unit = GetComponent<Unit>();

        //Setup the allies and enemies array.
        //Get all gameobjects that are not this instance, are in ally layer and are enabled.
        //Get all gameobjects that are not this instance, are in enemy layer and are enabled.

        EnableSelf();
        EnableTarget();
        EnableAllies();
        EnableEnemies();

        SetupPassives();
    }

    void OnDisable()
    {
        DisableSelf();
        DisableTarget();
        DisableAllies();
        DisableEnemies();
    }

    public void SetupPassives()
    {
        if(unit.passiveAbility1 != null)
        {
            passives.Add(unit.passiveAbility1);
        }
        if (unit.passiveAbility2 != null)
        {
            passives.Add(unit.passiveAbility2);
        }
        if (unit.passiveAbility3 != null)
        {
            passives.Add(unit.passiveAbility3);
        }
        if (unit.passiveAbility4 != null)
        {
            passives.Add(unit.passiveAbility4);
        }
    }

    void EnableSelf()
    {
        unit.deployAction += SelfDeploy;
        unit.damagedAction += SelfDamaged;
        unit.attackAction += SelfAttack;
        unit.targetAction += SelfTarget;
        unit.deathAction += SelfDeath;
    }

    void DisableSelf()
    {
        unit.deployAction -= SelfDeploy;
        unit.damagedAction -= SelfDamaged;
        unit.attackAction -= SelfAttack;
        unit.targetAction -= SelfTarget;
        unit.deathAction -= SelfDeath;
    }

    void EnableTarget()
    {

    }

    void DisableTarget()
    {

    }

    void EnableAllies()
    {

    }

    void DisableAllies()
    {

    }

    void EnableEnemies()
    {

    }

    void DisableEnemies()
    {

    }

    public void SelfAttack()
    {
        //Debug.Log("Self attack triggered in manager.");
        for(int i = 0; i < passives.Count; i++)
        {
            ISelfAttack selfAttack = passives[i] as ISelfAttack;
            if (selfAttack != null)
            {
                //NOTE: RIGHT NOW THIS IS NOT TRIGGERING BUT IT SHOULD BE, I HAVE NARROWED IT DOWN TO NOT TRIGGERING RIGHT HERE BUT THE ABOVE DEBUG IS TRIGGERING.
                Debug.Log(passives[i].name + " implements ISelfAttack.");
                selfAttack.SelfAttack();   
            }
        }
    }

    //Not implemented.
    public void SelfDamaged()
    {
        //Debug.Log("Self damaged triggered in manager.");
        for (int i = 0; i < passives.Count; i++)
        {
            ISelfDamaged selfDamaged = passives[i] as ISelfDamaged;
            if (selfDamaged != null)
            {
                //Debug.Log(passives[i].name + " implements ISelfDamaged.");
                selfDamaged.SelfDamaged();
            }
        }
    }

    public void SelfDeath()
    {
        //Debug.Log("Self death triggered in manager.");
        for (int i = 0; i < passives.Count; i++)
        {
            ISelfDeath selfDeath = passives[i] as ISelfDeath;
            if (selfDeath != null)
            {
                //Debug.Log(passives[i].name + " implements ISelfDeath.");
                selfDeath.SelfDeath();
            }
        }
    }

    public void SelfDeploy()
    {
        //Debug.Log("Self deploy triggered in manager.");
        for (int i = 0; i < passives.Count; i++)
        {
            ISelfDeploy selfDeploy = passives[i] as ISelfDeploy;
            if (selfDeploy != null)
            {
                //Debug.Log(passives[i].name + " implements SelfDeploy.");
                selfDeploy.SelfDeploy();
            }
        }
    }

    //Not implemented.
    public void SelfStatusEffect()
    {
        //Debug.Log("Self status effect triggered in manager.");
        for (int i = 0; i < passives.Count; i++)
        {
            ISelfStatusEffect selfStatusEffect = passives[i] as ISelfStatusEffect;
            if (selfStatusEffect != null)
            {
                //Debug.Log(passives[i].name + " implements SelfDeploy.");
                selfStatusEffect.SelfStatusEffect();
            }
        }
    }

    public void SelfTarget()
    {
        //Debug.Log("Self target triggered in manager.");
        for (int i = 0; i < passives.Count; i++)
        {
            ISelfTarget selfTarget = passives[i] as ISelfTarget;
            if (selfTarget != null)
            {
                //Debug.Log(passives[i].name + " implements SelfDeploy.");
                selfTarget.SelfTarget();
            }
        }
    }
}
