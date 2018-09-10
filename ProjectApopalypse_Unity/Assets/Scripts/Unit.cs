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

    public bool canMove;
    public bool canAttack;

    public enum AttackType
    {
        Melee,
        Ranged
    }
    public AttackType attackType;

    int curBaseDamage;
    public Variable totBaseDamage;
    public int actBaseDamage;

    int curTroopDamage;
    public Variable totTroopDamage;
    public int actTroopDamage;

    int curCoreDamage;
    public Variable totCoreDamage;
    public int actCoreDamage;

    int curChestDamage;
    public Variable totChestDamage;
    public int actChestDamage;

    int curDirectDamage;
    public Variable totDirectDamage;
    public int actDirectDamage;

    int curSplashDamage;
    public Variable totSplashDamage;
    public int actSplashDamage;

    int curCooldown;
    public Variable totCooldown;
    public int actCooldown;

    int curChargeTime;
    public Variable totChargeTime;
    public int actChargeTime;

    int curSplashRadius;
    public Variable totSplashRadius;
    public int actSplashRadius;

    int curRange;
    public Variable totRange;
    public int actRange;

    int curMoveSpeed;
    public Variable totMoveSpeed;
    public int actMoveSpeed;

    public enum MoveType
    {
        Ground,
        Flying
    }
    public MoveType moveType;

    public List<Entity> targets = new List<Entity>();
    string priorityTargetTag;

    NavMeshAgent navMeshAgent;

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
        if (canAttack && curRange <= Vector3.Distance(target.transform.position, transform.position))
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
        if(!target || !target.gameObject.activeSelf)
        {
            target = GetClosestTarget(FindObjectsOfType<Entity>());
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
        navMeshAgent.SetDestination(target.position);
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
