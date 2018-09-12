using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Unit : Entity {

    //States
    public enum States
    {
        Idle,
        Move,
        Attack,
        Death
    }
    public States currentState;

    public bool canMove = true;
    public bool canAttack = true;

    public enum AttackType
    {
        Melee,
        Ranged
    }
    public AttackType attackType;

    int curBaseDamage;
    public Variable totBaseDamage;
    public int actBaseDamage;
    public int actBaseDamageMax;
    public int actBaseDamageMin;
    public bool actBaseDamageLocked;

    int curTroopDamage;
    public Variable totTroopDamage;
    public int actTroopDamage;
    public int actTroopDamageMax;
    public int actTroopDamageMin;
    public bool actTroopDamageLocked;

    int curCoreDamage;
    public Variable totCoreDamage;
    public int actCoreDamage;
    public int actCoreDamageMax;
    public int actCoreDamageMin;
    public bool actCoreDamageLocked;

    int curChestDamage;
    public Variable totChestDamage;
    public int actChestDamage;
    public int actChestDamageMax;
    public int actChestDamageMin;
    public bool actChestDamageLocked;

    int curDirectDamage;
    public Variable totDirectDamage;
    public int actDirectDamage;
    public int actDirectDamageMax;
    public int actDirectDamageMin;
    public bool actDirectDamageLocked;

    int curSplashDamage;
    public Variable totSplashDamage;
    public int actSplashDamage;
    public int actSplashDamageMax;
    public int actSplashDamageMin;
    public bool actSplashDamageLocked;

    int curCooldown;
    public Variable totCooldown;
    public int actCooldown;
    public int actCooldownMax;
    public int actCooldownMin;
    public bool actCooldownLocked;

    int curChargeTime;
    public Variable totChargeTime;
    public int actChargeTime;
    public int actChargeTimeMax;
    public int actChargeTimeMin;
    public bool actChargeTimeLocked;

    int curSplashRadius;
    public Variable totSplashRadius;
    public int actSplashRadius;
    public int actSplashRadiusMax;
    public int actSplashRadiusMin;
    public bool actSplashRadiusLocked;

    int curRange;
    public Variable totRange;
    public int actRange;
    public int actRangeMax;
    public int actRangeMin;
    public bool actRangeLocked;

    int curMoveSpeed;
    public Variable totMoveSpeed;
    public int actMoveSpeed;
    public int actMoveSpeedMax;
    public int actMoveSpeedMin;
    public bool actMoveSpeedLocked;

    public enum MoveType
    {
        Ground,
        Flying
    }
    public MoveType moveType;

    public List<Entity> targets = new List<Entity>();
    string priorityTargetTag;

    NavMeshAgent navMeshAgent;
    int stoppingDistance = 2;

    int myLayer;

    //Called every time a State is entered
    void EnterState(States state)
    {
        switch (state)
        {
            case States.Idle:
                {
                    EnterIdleState();
                    break;
                }
            case States.Move:
                {
                    EnterMoveState();
                    break;
                }
            case States.Attack:
                {
                    EnterAttackState();
                    break;
                }
        }
    }

    //Called every update in a State
    void UpdateState(States state)
    {
        switch (state)
        {
            case States.Idle:
                {
                    IdleState();
                    break;
                }
            case States.Move:
                {
                    MoveState();
                    break;
                }
            case States.Attack:
                {
                    AttackState();
                    break;
                }
        }
    }

    //Called every time a State is exitted
    void ExitState(States state)
    {
        switch (state)
        {
            case States.Idle:
                {
                    ExitIdleState();
                    break;
                }
            case States.Move:
                {
                    ExitMoveState();
                    break;
                }
            case States.Attack:
                {
                    ExitAttackState();
                    break;
                }
        }
    }

    States DecideState()
    {
        //Logic goes here...
        if (canAttack && curRange <= Vector3.Distance(targets[0].transform.position, transform.position))
        {
            return States.Attack;
        }
        else if (canMove)
        {
            return States.Move;
        }
        else if (!isAlive)
        {
            return States.Death;
        }
        else
        {
            return States.Idle;
        }
    }

    void Start()
    {
        myLayer = gameObject.layer;

        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.stoppingDistance = stoppingDistance;

        //Find the closest target.
        targets = GetAllEntities(FindObjectsOfType<Entity>());
        targets.Sort(ByDistance);

        //Invoke the function that runs the behaviour for entering the intial currentState
        Invoke("Enter" + currentState.ToString() + "State", 0);
    }

    void Update()
    {
        //Use logic to find out the current state
        States newState = DecideState();

        //If the newState was not the same as the old state then play the exit state of the old state and enter state of the new state 
        if (newState != currentState)
        {
            ExitState(currentState);
            currentState = newState;
            EnterState(currentState);
        }

        //Play update state of the current state
        UpdateState(currentState);

        //Check if target is null or innactive
        if(!targets[0] || !targets[0].gameObject.activeSelf)
        {
            targets = GetAllEntities(FindObjectsOfType<Entity>());
        }
    }

    List<Entity> GetAllEntities(Entity[] entities)
    {
        List<Entity> potentialTargets = new List<Entity>();
        foreach (Entity entity in entities)
        {
            if(entity.gameObject.layer != myLayer)
            {
                potentialTargets.Add(entity);
            }
        }
        return potentialTargets;
    }

    int ByDistance(Entity entityA, Entity entityB)
    {
        float distanceToA = Vector3.Distance(transform.position, entityA.transform.position);
        float distanceToB = Vector3.Distance(transform.position, entityB.transform.position);
        return distanceToA.CompareTo(distanceToB);
    }

    public override void LoadVariables()
    {
        base.LoadVariables();

        totBaseDamage.integer = actBaseDamage;
        totBaseDamage.maxInt = actBaseDamageMax;
        totBaseDamage.minInt = actBaseDamageMin;
        totBaseDamage.isLocked = actBaseDamageLocked;

        totTroopDamage.integer = actTroopDamage;
        totTroopDamage.maxInt = actTroopDamageMax;
        totTroopDamage.minInt = actTroopDamageMin;
        totTroopDamage.isLocked = actTroopDamageLocked;

        totCoreDamage.integer = actCoreDamage;
        totCoreDamage.maxInt = actCoreDamageMax;
        totCoreDamage.minInt = actCoreDamageMin;
        totCoreDamage.isLocked = actCoreDamageLocked;

        totChestDamage.integer = actChestDamage;
        totChestDamage.maxInt = actChestDamageMax;
        totChestDamage.minInt = actChestDamageMin;
        totChestDamage.isLocked = actCoreDamageLocked;

        totDirectDamage.integer = actDirectDamage;
        totDirectDamage.maxInt = actDirectDamageMax;
        totDirectDamage.minInt = actDirectDamageMin;
        totDirectDamage.isLocked = actDirectDamageLocked;

        totSplashDamage.integer = actSplashDamage;
        totSplashDamage.maxInt = actSplashDamageMax;
        totSplashDamage.minInt = actSplashDamageMin;
        totSplashDamage.isLocked = actSplashDamageLocked;

        totCooldown.integer = actCooldown;
        totCooldown.maxInt = actCooldownMax;
        totCooldown.minInt = actCooldownMin;
        totCooldown.isLocked = actCooldownLocked;

        totChargeTime.integer = actChargeTime;
        totChargeTime.maxInt = actChargeTimeMax;
        totChargeTime.minInt = actChargeTimeMin;
        totChargeTime.isLocked = actChargeTimeLocked;

        totSplashRadius.integer = actSplashRadius;
        totSplashRadius.maxInt = actSplashRadiusMax;
        totSplashRadius.minInt = actSplashRadiusMin;
        totSplashRadius.isLocked = actSplashRadiusLocked;

        totRange.integer = actRange;
        totRange.maxInt = actRangeMax;
        totRange.minInt = actRangeMin;
        totRange.isLocked = actRangeLocked;

        totMoveSpeed.integer = actMoveSpeed;
        totMoveSpeed.maxInt = actMoveSpeedMax;
        totMoveSpeed.minInt = actMoveSpeedMin;
        totMoveSpeed.isLocked = actMoveSpeedLocked;
    }

    /*
    //https://forum.unity.com/threads/clean-est-way-to-find-nearest-object-of-many-c.44315/
    Transform GetClosestTarget(Entity[] targets)
    {
        /*
        Transform bestTarget = null;
        float closestDistance = Mathf.Infinity;
        Vector3 currentPosition = transform.position;

        foreach (Entity potentialTarget in targets)
        {
            Vector3 directionToTarget = potentialTarget.transform.position - currentPosition;
            float distanceToTarget = directionToTarget.sqrMagnitude;
            if (distanceToTarget < closestDistance)
            {
                closestDistance = distanceToTarget;
                bestTarget = potentialTarget.transform;
            }   
        }
        return bestTarget;

         function ByDistance(a : GameObject, b : GameObject) : int
 {
     var dstToA = Vector3.Distance(transform.position, a.transform.position);
     var dstToB = Vector3.Distance(transform.position, b.transform.position);
     return dstToA.CompareTo(dstToB);
 }
        

        Entity[] entityArray = FindObjectsOfType<Entity>();
        List<Entity> targets = new List<Entity>();

        entities.Sort(ByDistance);

    }
*/

    #region Idle
    void EnterIdleState()
    {

    }

    void IdleState()
    {


    }

    void ExitIdleState()
    {

    }
    #endregion

    #region Move
    void EnterMoveState()
    {

    }

    void MoveState()
    {
        navMeshAgent.SetDestination(targets[0].transform.position);
    }

    void ExitMoveState()
    {

    }
    #endregion

    #region Attack
    void EnterAttackState()
    {

    }

    void AttackState()
    {

    }

    void ExitAttackState()
    {

    }
    #endregion

    #region Death
    void EnterDeathState()
    {

    }

    void DeathState()
    {

    }

    void ExitDeathState()
    {


    }
    #endregion

}
