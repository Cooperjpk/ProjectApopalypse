using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Unit : Entity {

    //States
    public enum States
    {
        Idle,
        Move,
        Attack
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
    int actBaseDamage;

    int curTroopDamage;
    public Variable totTroopDamage;
    int actTroopDamage;

    int curCoreDamage;
    public Variable totCoreDamage;
    int actCoreDamage;

    int curChestDamage;
    public Variable totChestDamage;
    int actChestDamage;

    int curDirectDamage;
    public Variable totDirectDamage;
    int actDirectDamage;

    int curSplashDamage;
    public Variable totSplashDamage;
    int actSplashDamage;

    int curCooldown;
    public Variable totCooldown;
    int actCooldown;

    int curChargeTime;
    public Variable totChargeTime;
    int actChargeTime;

    int curSplashRadius;
    public Variable totSplashRadius;
    int actSplashRadius;

    int curRange;
    public Variable totRange;
    int actRange;

    int curMoveSpeed;
    public Variable totMoveSpeed;
    int actMoveSpeed;

    public enum MoveType
    {
        Ground,
        Flying
    }
    public MoveType moveType;

    Transform target;
    string priorityTargetTag;

    NavMeshAgent navMeshAgent;

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
        else
        {
            return States.Idle;
        }
    }

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();

        //Find the closest target.
        target = GetClosestEnemy(FindObjectsOfType<Entity>());

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
    }

    //https://forum.unity.com/threads/clean-est-way-to-find-nearest-object-of-many-c.44315/
    Transform GetClosestEnemy(Entity[] targets)
    {
        Transform bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;
        foreach (Entity potentialTarget in targets)
        {
            Vector3 directionToTarget = potentialTarget.transform.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget.transform;
            }   
        }
        return bestTarget;
    }

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
        if(target == null)
        {
            target = GetClosestEnemy(FindObjectsOfType<Entity>());
        }
    }

    void MoveState()
    {
        if (target == null)
        {
            target = GetClosestEnemy(FindObjectsOfType<Entity>());
        }
        navMeshAgent.SetDestination(target.position);
    }

    void ExitMoveState()
    {


    }
    #endregion

    #region Attack
    void EnterAttackState()
    {
        if (target == null)
        {
            target = GetClosestEnemy(FindObjectsOfType<Entity>());
        }
    }

    void AttackState()
    {
        if (target == null)
        {
            target = GetClosestEnemy(FindObjectsOfType<Entity>());
        }
    }

    void ExitAttackState()
    {


    }
    #endregion

}
