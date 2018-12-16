using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Passive : MonoBehaviour {

    public string displayName;
    public string description;
    public bool isLocked = false;
    public bool isHidden = false;
}

public interface ISelfDeploy
{
    void SelfDeploy();
}

public interface ISelfAttack
{
    void SelfAttack();
}

public interface ISelfDamaged
{
    void SelfDamaged();
}

public interface ISelfDeath
{
    void SelfDeath();
}

public interface ISelfTarget
{
    void SelfTarget();
}

public interface ISelfStatusEffect
{
    void SelfStatusEffect();
}

/*
 * AllyDeploy
 * AllyDamaged
 * AllyDeath
 * TargetDeath
 * TargetStatusEffect
 * EnemyDamaged
 * EnemyDeath
 */ 
