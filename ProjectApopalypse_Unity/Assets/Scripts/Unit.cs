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
    int directDamage;
    int totTargets = 1;

    int myLayer;
    int enemyLayer;
    LayerMask layerMask;

    static int maxLineDistance = 999;
    //Vector3 attackPosition;
    public bool targetAllies = false;
    public bool targetEnemies = true;

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
        if(totTargets <= 0)
        {
            totTargets = 1;
        }

        //If the unit has direct damage, deal that damage to targets[] (curTargets).
        if (curDirectDamage > 0)
        {
            for (int i = 0; i < totTargets; i++)
            {
                targets[i].ChangeHealth(CalculateDamage(targets[i].gameObject.tag,DamageType.Direct), DamageType.Direct, gameObject.tag);
            }
        }

        //If splash, create a raycast line or other shape depending on the mode that was selected.
        if (curSplashRadius > 0 && splashOrigin == SplashOrigin.Self)
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
                Debug.LogWarning("There was a problem assembling the layer mask because allies and enemie were both not targetted.");
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
                        RaycastHit hit;
                        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, maxLineDistance, layerMask))
                        {
                            hit.collider.gameObject.GetComponent<Entity>().ChangeHealth(CalculateDamage(hit.collider.gameObject.tag, DamageType.Direct), DamageType.Direct, gameObject.tag);
                            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.green);
                        }
                        else
                        {
                            Debug.LogWarning("Nothing was hit in the splash shape.");
                        }
                        break;
                    }
                case (SplashType.Box):
                    {
                        break;
                    }
                case (SplashType.Sphere):
                    {
                        /*
                        if (splashOrigin == SplashOrigin.Self)
                        {
                            Collider[] colliders = Physics.OverlapSphere(attackPosition,curSplashRadius);
                        }
                        else if (splashOrigin == SplashOrigin.Targets)
                        {
                            for(int i = 0; i > totTargets; i++)
                            {
                                attackPosition = targets[i].transform.position;
                                Collider[] colliders = Physics.OverlapSphere(attackPosition, curSplashRadius);
                            }
                        }
                        /*
                        Collider[] hitColliders = Physics.OverlapSphere(center, radius);
                        int i = 0;
                        while (i < hitColliders.Length)
                        {
                            hitColliders[i].SendMessage("AddDamage");
                            i++;
                        }
                        */
                        break;
                    }
                case (SplashType.Capsule):
                    {
                        break;
                    }
            }
        }
        //If there is splash damage except that it uses all target locations, do damage here.
        else if(curSplashRadius > 0 && splashOrigin == SplashOrigin.Targets)
        {

        }
        else
        {
            Debug.LogWarning("Either targets[0] is null or the splashOrigin is unspecified.");
        }

        //ATTACK
        //Define the location, direction and size of the shape.
        //When things are hit, check their tag and then apply damage and effects.

        //Attack functionality happens here
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

        //Reset the number of targets.
        totTargets = 1;
    }

    /*
    List<Collider> TargetsHit (SplashType type)
    {
        //RaycastHit hit;
        if (type == SplashType.Line)
        {
            
        }
        else if(type == SplashType.Sphere)
        {
            //Collider[] hitColliders = Physics.OverlapSphere(, curSplashRadius);
        }
        else if(type == SplashType.Box)
        {

        }
        else if(type == SplashType.Capsule)
        {

        }
        /*
        foreach (Collider other in cols)
        {
            if (targetCount < targetLimit && targetTags.Contains(other.tag) && targetLayers.Contains(other.gameObject.layer))
            {
                targetCount++;
                other.GetComponent<Unit>().ChangeHealth(-damage);
            }
            else if (targetCount >= targetLimit)
            {
                Debug.Log("Target count has been surpassed.");
                break;
            }
        }
        targetCount = 0;
        }
        */

    int CalculateDamage(string damageDealer, DamageType damageType)
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
        return Mathf.RoundToInt(finalDamage);
    }
}
