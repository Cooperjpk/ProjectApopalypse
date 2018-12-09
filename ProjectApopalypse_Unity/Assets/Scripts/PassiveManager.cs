using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveManager : MonoBehaviour {

    public List<Passive> passives = new List<Passive>();

    Unit unit;

    public void SetupPassives()
    {
        unit = GetComponent<Unit>();

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

    public void SelfAttack()
    {
        //Debug.Log("Self attack triggered in manager.");
        for(int i = 0; i < passives.Count; i++)
        {
            ISelfAttack selfAttack = passives[i] as ISelfAttack;
            if (selfAttack != null)
            {
                //Debug.Log(passives[i].name + " implements ISelfAttack.");
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
