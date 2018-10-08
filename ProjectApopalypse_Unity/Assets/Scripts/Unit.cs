using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Unit : Entity
{

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
    AttackType attackType;

    public enum SplashType
    {
        Line,
        Sphere,
        Box,
        Capsule
    }
    public SplashType splashType;

    public enum AttackOrigin
    {
        Self,
        Targets
    }
    public AttackOrigin attackOrigin;

    float cooldownStamp;
    float chargeStamp;

    public float salvoRate;

    public string passive1;
    public string passive2;
    public string passive3;
    public string passive4;

    int curBaseDamage;
    public int actBaseDamage;
    public int actBaseDamageMax;
    public int actBaseDamageMin;
    public bool actBaseDamageLocked;

    int curTroopDamage;
    public int actTroopDamage;
    public int actTroopDamageMax;
    public int actTroopDamageMin;
    public bool actTroopDamageLocked;

    int curCoreDamage;
    public int actCoreDamage;
    public int actCoreDamageMax;
    public int actCoreDamageMin;
    public bool actCoreDamageLocked;

    int curChestDamage;
    public int actChestDamage;
    public int actChestDamageMax;
    public int actChestDamageMin;
    public bool actChestDamageLocked;

    int curDirectDamage;
    public int actDirectDamage;
    public int actDirectDamageMax;
    public int actDirectDamageMin;
    public bool actDirectDamageLocked;

    int curSplashDamage;
    public int actSplashDamage;
    public int actSplashDamageMax;
    public int actSplashDamageMin;
    public bool actSplashDamageLocked;

    int curCooldown;
    public int actCooldown;
    public int actCooldownMax;
    public int actCooldownMin;
    public bool actCooldownLocked;

    int curChargeTime;
    public int actChargeTime;
    public int actChargeTimeMax;
    public int actChargeTimeMin;
    public bool actChargeTimeLocked;

    int curSplashRadius;
    public int actSplashRadius;
    public int actSplashRadiusMax;
    public int actSplashRadiusMin;
    public bool actSplashRadiusLocked;

    int curRange;
    public int actRange;
    public int actRangeMax;
    public int actRangeMin;
    public bool actRangeLocked;

    int curMoveSpeed;
    public int actMoveSpeed;
    public int actMoveSpeedMax;
    public int actMoveSpeedMin;
    public bool actMoveSpeedLocked;

    int curCharges;
    public int actCharges;
    public int actChargesMax;
    public int actChargesMin;
    public bool actChargesLocked;

    int curSalvo;
    public int actSalvo;
    public int actSalvoMax;
    public int actSalvoMin;
    public bool actSalvoLocked;

    int curTargets;
    public int actTargets;
    public int actTargetsMax;
    public int actTargetsMin;
    public bool actTargetsLocked;

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
    static int meleeCutOff = 2;

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
        //If the unit is not alive, it is dead. First priority state.
        if (!isAlive)
        {
            return States.Death;
        }
        //If the unit can attack, is in range and is fully charged, then attack.
        else if (canAttack && curRange >= Vector3.Distance(targets[0].transform.position, transform.position) && chargeStamp <= Time.time)
        {
            return States.Attack;
        }
        //If the unit is mobile, it should move.
        else if (canMove)
        {
            return States.Move;
        }
        //If the unit has nothing to do, just stand idly.
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

        //Decide whether the unit is melee or ranged depending on their range and then lock their range in.
        if (actRange <= meleeCutOff)
        {
            attackType = AttackType.Melee;
            actRangeLocked = true;
        }
        else
        {
            attackType = AttackType.Ranged;
        }
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
        if (!targets[0] || !targets[0].gameObject.activeSelf)
        {
            targets = GetAllEntities(FindObjectsOfType<Entity>());
        }
    }

    List<Entity> GetAllEntities(Entity[] entities)
    {
        List<Entity> potentialTargets = new List<Entity>();
        foreach (Entity entity in entities)
        {
            if (entity.gameObject.layer != myLayer)
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

    public override void SetCurrentVariables()
    {
        base.SetCurrentVariables();

        curBaseDamage = actBaseDamage;
        curTroopDamage = actTroopDamage;
        curCoreDamage = actCoreDamage;
        curChestDamage = actChestDamage;
        curDirectDamage = actDirectDamage;
        curSplashDamage = actSplashDamage;
        curCooldown = actCooldown;
        curChargeTime = actChargeTime;
        curSplashRadius = actSplashRadius;
        curRange = actRange;
        curMoveSpeed = actMoveSpeed;
        curSalvo = actSalvo;
        curCharges = actCharges;
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
        if (targets[0] != null)
        {
            navMeshAgent.SetDestination(targets[0].transform.position);
        }
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
        UseAttack();
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

    void UseAttack()
    {
        if (cooldownStamp <= Time.time)
        {
            Debug.Log("Use the ability!");

            //Reset the cooldown.
            cooldownStamp = Time.time + curCooldown;

            //Reset the charge time.
            chargeStamp = Time.time + curChargeTime;

            //RELEASE ATTACKS AT salvoRate FOR SALVO NUMBER
            for (int i = 0; i > curSalvo; i++)
            {
                AttackFunctionality();
            }

            //ATTACK
            //If direct, apply damage and effects to target(s)
            //If splash, create a raycast line or other shape depending on the mode that was selected.
            //Define the location, direction and size of the shape.
            //When things are hit, check their tag and then apply damage and effects.

            //Attack functionality happens here
            //curBaseDamage
            //curTroopDamage
            //curCoreDamage
            //curChestDamage
            //curDirectDamage
            //curSplashDamage
            //curCooldown
            //curChargeTime
            //curSplashRadius
            //curRange
            //curMoveSpeed
            //curSalvo
            //curCharges

        }
    }

    void AttackFunctionality()
    {
        //If the unit has direct damage, deal that damage to targets[] (curTargets).
        if (curDirectDamage > 0)
        {
            for (int i = 0; i > curTargets; i++)
            {
                targets[i].ChangeHealth(curDirectDamage + curBaseDamage, DamageType.Direct, gameObject.tag);
            }
        }

        if (curSplashRadius > 0)
        {

        }
    }

}
