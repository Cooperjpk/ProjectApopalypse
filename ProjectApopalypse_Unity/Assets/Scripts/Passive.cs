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

public interface ISAllyDeploy
{
    void AllyDeploy();
}

public interface IAllyAttack
{
    void AllyAttack();
}

public interface IAllyDamaged
{
    void AllyDamaged();
}

public interface IAllyDeath
{
    void AllyDeath();
}

public interface IAllyTarget
{
    void AllyTarget();
}

public interface IAllyStatusEffect
{
    void AllyStatusEffect();
}

public interface IEnemyDeploy
{
    void EnemyDeploy();
}

public interface IEnemyAttack
{
    void EnemyAttack();
}

public interface IEnemyDamaged
{
    void EnemyDamaged();
}

public interface IEnemyDeath
{
    void EnemyDeath();
}

public interface IEnemyTarget
{
    void EnemyTarget();
}

public interface IEnemyStatusEffect
{
    void EnemyStatusEffect();
}

public interface ITargetDeploy
{
    void TargetDeploy();
}

public interface ITargetAttack
{
    void TargetAttack();
}

public interface ITargetDamaged
{
    void TargetDamaged();
}

public interface ITargetDeath
{
    void TargetDeath();
}

public interface ITargetTarget
{
    void TargetTarget();
}

public interface ITargetStatusEffect
{
    void TargetStatusEffect();
}
