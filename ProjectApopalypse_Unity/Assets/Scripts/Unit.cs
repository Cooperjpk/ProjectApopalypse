﻿using System.Collections;
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
        None
    }
    public SplashType splashType;

    public enum SplashOrigin
    {
        Self,
        Targets
    }
    public SplashOrigin splashOrigin;

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

    float curCooldown;
    public float actCooldown;
    public float actCooldownMax;
    public float actCooldownMin;
    public bool actCooldownLocked;

    float curChargeTime;
    public float actChargeTime;
    public float actChargeTimeMax;
    public float actChargeTimeMin;
    public bool actChargeTimeLocked;

    float curSplashRadius;
    public float actSplashRadius;
    public float actSplashRadiusMax;
    public float actSplashRadiusMin;
    public bool actSplashRadiusLocked;

    float curRange;
    public float actRange;
    public float actRangeMax;
    public float actRangeMin;
    public bool actRangeLocked;

    float curMoveSpeed;
    public float actMoveSpeed;
    public float actMoveSpeedMax;
    public float actMoveSpeedMin;
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
    int directDamage;
    int totTargets = 1;

    int myLayer;
    int enemyLayer;
    LayerMask layerMask;

    static int maxLineDistance = 999;
    //Vector3 attackPosition;
    public bool targetAllies = false;
    public bool targetEnemies = true;

    [Header("Particle Effects")]
    public ParticleSystem effectSplashTarget;
    public Vector4 effectSplashTargetColor;
    //public ParticleSystem effectSplashTargetOrigin;

    //Splash Particle Effect Targets Origin

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
            case States.Death:
                {
                    EnterDeathState();
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
            case States.Death:
                {
                    DeathState();
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
            case States.Death:
                {
                    ExitDeathState();
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
        else if (canAttack && targets[0].gameObject.activeSelf && curRange >= Vector3.Distance(targets[0].transform.position, transform.position) && chargeStamp <= Time.time)
        {
            return States.Attack;
        }
        //If the unit is mobile, it should move.
        else if (canMove && targets[0].gameObject.activeSelf)
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
        if (myLayer == 8)
        {
            enemyLayer = 9;
        }
        else if (myLayer == 9)
        {
            enemyLayer = 8;

        }

        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.stoppingDistance = stoppingDistance;

        //Find the closest target.
        targets = GetAllEntities(FindObjectsOfType<Entity>());
        targets.Sort(ByDistance);

        //Invoke the function that runs the behaviour for entering the intial currentState
        Invoke("Enter" + currentState.ToString() + "State", 0);

        SetCurrentVariables();

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
            if (entity.gameObject.layer != myLayer && entity.gameObject.activeSelf)
            {
                potentialTargets.Add(entity);
            }
        }

        if (potentialTargets != null)
        {
            return potentialTargets;
        }
        else
        {
            Debug.Log("No existing targets.");
            return null;
        }
    }

    List<Entity> GetAllEntitiesInRange(Entity[] entities)
    {
        List<Entity> potentialTargets = new List<Entity>();
        foreach (Entity entity in entities)
        {
            if (entity.gameObject.layer != myLayer && curRange >= Vector3.Distance(entity.transform.position, transform.position))
            {
                potentialTargets.Add(entity);
            }
        }

        if (potentialTargets != null)
        {
            return potentialTargets;
        }
        else
        {
            Debug.Log("No targets are in range.");
            return null;
        }
    }

    int ByDistance(Entity entityA, Entity entityB)
    {
        float distanceToA = Vector3.Distance(transform.position, entityA.transform.position);
        float distanceToB = Vector3.Distance(transform.position, entityB.transform.position);
        return distanceToA.CompareTo(distanceToB);
    }

    int ByColliderDistance(Collider colA, Collider colB)
    {
        float distanceToA = Vector3.Distance(transform.position, colA.transform.position);
        float distanceToB = Vector3.Distance(transform.position, colB.transform.position);
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
        curTargets = actTargets;
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

    }

    void MoveState()
    {
        navMeshAgent.SetDestination(targets[0].transform.position);
    }

    void ExitMoveState()
    {
        navMeshAgent.SetDestination(transform.position);
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
        Debug.Log(gameObject.name + " has died :(");
        gameObject.SetActive(false);
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
            //Reset the cooldown.
            cooldownStamp = Time.time + curCooldown;

            //Reset the charge time.
            chargeStamp = Time.time + curChargeTime;

            //Invoke the attack at salvoRate, curSalvo number of times.
            for (int i = 0; i < curSalvo; i++)
            {
                //Debug.Log("Attack happening");
                Invoke("AttackFunctionality", i * salvoRate);
            }
        }
    }

    public void AttackFunctionality()
    {
        //Find out how many targets to apply the attack to.
        if (curTargets <= targets.Count)
        {
            totTargets = curTargets;
        }
        else if (targets.Count < curTargets)
        {
            totTargets = targets.Count;
        }

        //Just in case targets is 0, set targets to 1.
        if (totTargets <= 0)
        {
            totTargets = 1;
        }

        //If the unit has direct damage, deal that damage to targets[] (curTargets).
        if (curDirectDamage > 0)
        {
            for (int i = 0; i < totTargets; i++)
            {
                targets[i].ChangeHealth(CalculateDamage(targets[i].gameObject.tag, DamageType.Direct), DamageType.Direct, gameObject.tag);
            }
        }

        //If splash, create a raycast line or other shape depending on the mode that was selected.
        if (curSplashRadius > 0 && curSplashDamage > 0)
        {
            //Sets the layer mask to only target the layers that enemies or allies are on.
            if (targetAllies && targetEnemies)
            {
                layerMask = LayerMask.GetMask(LayerMask.LayerToName(myLayer), LayerMask.LayerToName(enemyLayer));
            }
            else if (targetAllies && !targetEnemies)
            {
                layerMask = LayerMask.GetMask(LayerMask.LayerToName(myLayer));
            }
            else if (targetEnemies && !targetAllies)
            {
                layerMask = LayerMask.GetMask(LayerMask.LayerToName(enemyLayer));
            }
            else
            {
                Debug.LogWarning("There was a problem assembling the layer mask because allies and enemies were both not targetted.");
            }

            switch (splashType)
            {
                default:
                    {
                        Debug.LogWarning("The splash type has not been specified.");
                        break;
                    }
                case (SplashType.Line):
                    {
                        Debug.Log("Line splash.");
                        for (int j = 0; j < totTargets; j++)
                        {
                            var heading = targets[j].transform.position - transform.position;
                            var distance = heading.magnitude;
                            var direction = heading / distance;

                            RaycastHit[] hits;
                            hits = Physics.RaycastAll(transform.position, direction, maxLineDistance, layerMask);
                            Debug.DrawRay(transform.position, direction * maxLineDistance, Color.green);

                            for (int i = 0; i < hits.Length; i++)
                            {
                                hits[i].collider.gameObject.GetComponent<Entity>().ChangeHealth(CalculateDamage(hits[i].collider.gameObject.tag, DamageType.Splash), DamageType.Splash, gameObject.tag);
                            }
                        }
                        break;
                    }
                case (SplashType.Sphere):
                    {
                        Debug.Log("Sphere splash.");
                        Vector3 origin;
                        if (splashOrigin == SplashOrigin.Self)
                        {
                            origin = transform.position;

                            Collider[] selfHits;
                            selfHits = Physics.OverlapSphere(origin,curSplashRadius);

                            for (int k = 0; k < selfHits.Length; k++)
                            {
                                selfHits[k].gameObject.GetComponent<Entity>().ChangeHealth(CalculateDamage(selfHits[k].gameObject.tag, DamageType.Splash), DamageType.Splash, gameObject.tag);
                                Debug.Log(selfHits[k].gameObject.name + " has been hit for " + CalculateDamage(selfHits[k].gameObject.tag, DamageType.Splash));
                            }
                        }
                        else if (splashOrigin == SplashOrigin.Targets)
                        {
                            for (int j = 0; j < totTargets; j++)
                            {
                                origin = targets[j].transform.position;

                                Collider[] targetHits;
                                targetHits = Physics.OverlapSphere(origin,curSplashRadius);

                                for(int l = 0; l < targetHits.Length; l++)
                                {
                                    targetHits[l].gameObject.GetComponent<Entity>().ChangeHealth(CalculateDamage(targetHits[l].gameObject.tag, DamageType.Splash), DamageType.Splash, gameObject.tag);
                                    Debug.Log(targetHits[l].gameObject.name + " has been hit for " + CalculateDamage(targetHits[l].gameObject.tag, DamageType.Splash));
                                    //Splash Particle Effect Targets Origin
                                    var emitParams = new ParticleSystem.EmitParams();
                                    //This needs to be from a data entry in the sheet that takes in 3 values (I'm thinking RGB).
                                    //emitParams.startColor = effectSplashTargetColor;
                                    //This needs to be in accordance with the actual size of the splash radius.
                                    emitParams.startSize = curSplashRadius;
                                    effectSplashTarget.Emit(emitParams,1);
                                }
                            }
                        }
                        break;
                    }
                case (SplashType.None):
                    {
                        break;
                    }
            }
        }

        //Reset the number of targets.
        totTargets = 1;
    }

    public int CalculateDamage(string damageDealer, DamageType damageType)
    {
        int damage = 0;
        int damageFraction = 0;

        switch (damageDealer)
        {
            default:
                {
                    Debug.Log("damageDealer string was not a recognizable string.");
                    break;
                }
            case "Troop":
                {
                    damageFraction += curTroopDamage;
                    break;
                }
            case "Core":
                {
                    damageFraction += curCoreDamage;
                    break;
                }
            case "Chest":
                {
                    damageFraction += curChestDamage;
                    break;
                }
        }

        switch (damageType)
        {
            default:
                {
                    Debug.Log("damageType was not a recognizable value.");
                    break;
                }
            case DamageType.Direct:
                {
                    damageFraction += curDirectDamage;
                    break;
                }
            case DamageType.Splash:
                {
                    damageFraction += curSplashDamage;
                    break;
                }
        }

        float accumulative = damage + damageFraction;
        float damageMultiplier = damage / accumulative;
        damageMultiplier += 1;
        float finalDamage = damage * damageMultiplier;

        Debug.Log(finalDamage);
        //For some reason damage is ending up being 0 when base damage is 1 and splash damage is 1 and everything else is 0.

        return Mathf.RoundToInt(finalDamage);
    }
}
