using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultPassive : Passive, ISelfAttack, ISelfDamaged, ISelfDeath, ISelfDeploy, ISelfStatusEffect, ISelfTarget {

    void Awake()
    {
        displayName = "Default Passive";
        description = "The default passive ability.";
        isLocked = true;
    }

    public void SelfAttack()
    {
        Debug.Log("Self attack triggered.");
    }

    public void SelfDamaged()
    {
        Debug.Log("Self damaged triggered.");
    }

    public void SelfDeath()
    {
        Debug.Log("Self death triggered.");
    }

    public void SelfDeploy()
    {
        Debug.Log("Self deploy triggered.");
    }

    public void SelfStatusEffect()
    {
        Debug.Log("Self status effect triggered.");
    }

    public void SelfTarget()
    {
        Debug.Log("Self target triggered.");
    }
}
