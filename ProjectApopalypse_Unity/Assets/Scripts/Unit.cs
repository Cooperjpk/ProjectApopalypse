using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

[RequireComponent(typeof(NavMeshAgent))]
public class Unit : MonoBehaviour
{
    public string displayName;
    public string technicalName;
    public GameObject model;
    public Image icon;

    public string[] classifications;
    public string description;

    public int powerLevel;
    public int PowerLevel
    {
        get
        {
            //Calculate the power level of the unit here.
            return powerLevel;
        }
    }

    public enum Rank
    {
        S,
        A,
        B,
        C,
        D
    }
    public Rank rank;

    //Stage.Theme theme;

    Animator animator;
    Renderer render;

    public string variationName;
    public Texture variation;

    //FOR TESTING PURPOSES, ALL CUR VARIABLES HACE BEEN SET TO PUBLIC BUT SHOULD BE CHANGED BACK TO PRIVATE LATER!!!

    //Health
    public int curHealth;
    public int actHealth;
    public int actHealthMax;
    public int actHealthMin;
    public bool actHealthLocked;

    public int curBaseArmor;
    public int actBaseArmor;
    public int actBaseArmorMax;
    public int actBaseArmorMin;
    public bool actBaseArmorLocked;

    public int curTroopArmor;
    public int actTroopArmor;
    public int actTroopArmorMax;
    public int actTroopArmorMin;
    public bool actTroopArmorLocked;

    public int curSiegeArmor;
    public int actSiegeArmor;
    public int actSiegeArmorMax;
    public int actSiegeArmorMin;
    public bool actSiegeArmorLocked;

    public int curDirectArmor;
    public int actDirectArmor;
    public int actDirectArmorMax;
    public int actDirectArmorMin;
    public bool actDirectArmorLocked;

    public int curSplashArmor;
    public int actSplashArmor;
    public int actSplashArmorMax;
    public int actSplashArmorMin;
    public bool actSplashArmorLocked;

    public bool isAlive;
    public bool isInvincible;

    public enum DamageType
    {
        Direct,
        Splash
    }

    //Above is all old Entity class stuff

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

    public Passive passiveAbility1;
    public Passive passiveAbility2;
    public Passive passiveAbility3;
    public Passive passiveAbility4;

    public int curBaseDamage;
    public int actBaseDamage;
    public int actBaseDamageMax;
    public int actBaseDamageMin;
    public bool actBaseDamageLocked;

    public int curTroopDamage;
    public int actTroopDamage;
    public int actTroopDamageMax;
    public int actTroopDamageMin;
    public bool actTroopDamageLocked;

    public int curSiegeDamage;
    public int actSiegeDamage;
    public int actSiegeDamageMax;
    public int actSiegeDamageMin;
    public bool actSiegeDamageLocked;

    public int curCoreDamage;
    public int actCoreDamage;
    public int actCoreDamageMax;
    public int actCoreDamageMin;
    public bool actCoreDamageLocked;

    public int curChestDamage;
    public int actChestDamage;
    public int actChestDamageMax;
    public int actChestDamageMin;
    public bool actChestDamageLocked;

    public int curDirectDamage;
    public int actDirectDamage;
    public int actDirectDamageMax;
    public int actDirectDamageMin;
    public bool actDirectDamageLocked;

    public int curSplashDamage;
    public int actSplashDamage;
    public int actSplashDamageMax;
    public int actSplashDamageMin;
    public bool actSplashDamageLocked;

    public float curCooldown;
    public float actCooldown;
    public float actCooldownMax;
    public float actCooldownMin;
    public bool actCooldownLocked;

    public float curChargeTime;
    public float actChargeTime;
    public float actChargeTimeMax;
    public float actChargeTimeMin;
    public bool actChargeTimeLocked;

    public float curSplashRadius;
    public float actSplashRadius;
    public float actSplashRadiusMax;
    public float actSplashRadiusMin;
    public bool actSplashRadiusLocked;

    public float curRange;
    public float actRange;
    public float actRangeMax;
    public float actRangeMin;
    public bool actRangeLocked;

    public float curMoveSpeed;
    public float actMoveSpeed;
    public float actMoveSpeedMax;
    public float actMoveSpeedMin;
    public bool actMoveSpeedLocked;

    public int curCharges;
    public int actCharges;
    public int actChargesMax;
    public int actChargesMin;
    public bool actChargesLocked;

    public int curSalvo;
    public int actSalvo;
    public int actSalvoMax;
    public int actSalvoMin;
    public bool actSalvoLocked;

    public int curTargets;
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

    public List<Unit> targets = new List<Unit>();
    string priorityTargetTag;

    NavMeshAgent navMeshAgent;
    int stoppingDistance = 2;
    static int meleeCutOff = 2;
    int directDamage;
    int totTargets = 1;

    public float damageDelay;

    //VARIABLES FOR A SCALING SPLASH DAMAGE SYSTEM, THIS WILL ADD NEEDED COMPLEXITY TO THE UNITS
    public bool degradingSplashDamage = false;
    public float peakDamagePercent = 1;

    int myLayer;
    int enemyLayer;
    LayerMask layerMask;

    static int maxLineDistance = 999;
    //Vector3 attackPosition;
    public bool targetAllies = false;
    public bool targetEnemies = true;
    string unitTag;

    public PassiveManager passiveManager;

    [Header("Particle Effects")]
    public float effectImpactTime = 1;
    public ParticleSystem effectSplashTarget;
    public Vector4 effectSplashTargetColor;
    //public ParticleSystem effectSplashTargetOrigin;

    //Damage Flashing
    Color damageColor = new Color(255, 0, 0, 0);
    float flashTime = 0.1f;

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
        if (targets[0] == null)
        {
            return States.Idle;
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
        animator = GetComponent<Animator>();
        render = GetComponent<Renderer>();

        myLayer = gameObject.layer;
        if (myLayer == 8)
        {
            enemyLayer = 9;
        }
        else if (myLayer == 9)
        {
            enemyLayer = 8;
        }

        //Set your tag
        if (!String.IsNullOrEmpty(unitTag))
        {
            //NEED TO ADD THIS TO THE EXCEL SHEET WHEN ITS NOT BROKEN
            gameObject.tag = unitTag;
        }

        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.stoppingDistance = stoppingDistance;

        //Find the closest target.
        List<Unit> units = GetAllUnits(FindObjectsOfType<Unit>());
        if (units.Count > 0)
        {
            targets = units;
            targets.Sort(ByDistance);
        }

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

        //Add passive componnents based on strings of passive1-4.
        AddPassiveComponents();

        //Now add all passives to the passive manager.
        passiveManager = GetComponent<PassiveManager>();
        if (passiveManager != null)
        {
            passiveManager.SetupPassives();
        }
        else
        {
            Debug.LogError("The passive manager is returning null.");
        }

        passiveManager.SelfDeploy();
    }

    void AddPassiveComponents()
    {
        if (!string.IsNullOrEmpty(passive1))
        {
            //Debug.Log(passive1);
            Type type = Type.GetType(passive1);
            passiveAbility1 = gameObject.AddComponent(type) as Passive;
        }

        if (!string.IsNullOrEmpty(passive2))
        {
            //Debug.Log(passive2);
            Type type = Type.GetType(passive2);
            gameObject.AddComponent(type);
            passiveAbility2 = gameObject.AddComponent(type) as Passive;
        }

        if (!string.IsNullOrEmpty(passive3))
        {
            //Debug.Log(passive3);
            Type type = Type.GetType(passive3);
            gameObject.AddComponent(type);
            passiveAbility3 = gameObject.AddComponent(type) as Passive;
        }

        if (!string.IsNullOrEmpty(passive4))
        {
            //Debug.Log(passive4);
            Type type = Type.GetType(passive4);
            gameObject.AddComponent(type);
            passiveAbility4 = gameObject.AddComponent(type) as Passive;
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
            passiveManager.SelfTarget();
            targets = GetAllUnits(FindObjectsOfType<Unit>());
        }
    }

    List<Unit> GetAllUnits(Unit[] units)
    {
        List<Unit> potentialTargets = new List<Unit>();
        foreach (Unit unit in units)
        {
            if (unit.gameObject.layer != myLayer && unit.gameObject.activeSelf)
            {
                potentialTargets.Add(unit);
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

    List<Unit> GetAllUnitsInRange(Unit[] units)
    {
        List<Unit> potentialTargets = new List<Unit>();
        foreach (Unit unit in units)
        {
            if (unit.gameObject.layer != myLayer && curRange >= Vector3.Distance(unit.transform.position, transform.position))
            {
                potentialTargets.Add(unit);
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

    int ByDistance(Unit unitA, Unit unitB)
    {
        float distanceToA = Vector3.Distance(transform.position, unitA.transform.position);
        float distanceToB = Vector3.Distance(transform.position, unitB.transform.position);
        return distanceToA.CompareTo(distanceToB);
    }
    int ByColliderDistance(Collider colA, Collider colB)
    {
        float distanceToA = Vector3.Distance(transform.position, colA.transform.position);
        float distanceToB = Vector3.Distance(transform.position, colB.transform.position);
        return distanceToA.CompareTo(distanceToB);
    }

    public void SetCurrentVariables()
    {
        curHealth = actHealth;
        curBaseArmor = actBaseArmor;
        curTroopArmor = actTroopArmor;
        curSiegeArmor = actSiegeArmor;
        curDirectArmor = actDirectArmor;
        curSplashArmor = actSplashArmor;
        //Above was entitiy work
        curBaseDamage = actBaseDamage;
        curTroopDamage = actTroopDamage;
        curSiegeDamage = actSiegeDamage;
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
        passiveManager.SelfDeath();
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

            passiveManager.SelfAttack();

            StartCoroutine(DamageDelay(damageDelay));
        }
    }

    public void AttackFunctionality()
    {
        Debug.Log("Attack happening");

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
        for (int i = 0; i < totTargets; i++)
        {
            int calculatedDamage = CalculateDamage(targets[i].gameObject.tag, DamageType.Direct);
            targets[i].ChangeHealth(calculatedDamage, DamageType.Direct, gameObject.tag);
        }

        //If splash, create a raycast line or other shape depending on the mode that was selected.
        if (curSplashRadius > 0)
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
                Debug.LogError("There was a problem assembling the layer mask because allies and enemies were both not targetted.");
            }

            switch (splashType)
            {
                default:
                    {
                        Debug.LogError("The splash type has not been specified.");
                        break;
                    }
                case (SplashType.Line):
                    {
                        //Debug.Log("Line splash.");
                        for (int j = 0; j < totTargets; j++)
                        {
                            var heading = targets[j].transform.position - transform.position;
                            var distance = heading.magnitude;
                            var direction = heading / distance;

                            RaycastHit[] hits;
                            hits = Physics.RaycastAll(transform.position, direction, curSplashRadius, layerMask);
                            Debug.DrawRay(transform.position, direction * curSplashRadius, Color.green);

                            for (int i = 0; i < hits.Length; i++)
                            {
                                int calculatedDamage = CalculateDamage(hits[i].collider.gameObject.tag, DamageType.Splash);
                                hits[i].collider.gameObject.GetComponent<Unit>().ChangeHealth(calculatedDamage, DamageType.Splash, gameObject.tag);
                            }
                        }
                        break;
                    }
                case (SplashType.Sphere):
                    {
                        Vector3 origin;
                        if (splashOrigin == SplashOrigin.Self)
                        {
                            origin = transform.position;

                            Collider[] selfHits;
                            selfHits = Physics.OverlapSphere(origin, curSplashRadius, layerMask);

                            for (int k = 0; k < selfHits.Length; k++)
                            {
                                int calculatedDamage = CalculateDamage(selfHits[k].gameObject.tag, DamageType.Splash);
                                selfHits[k].gameObject.GetComponent<Unit>().ChangeHealth(calculatedDamage, DamageType.Splash, gameObject.tag);
                                //Debug.Log(selfHits[k].gameObject.name + " has been hit for " + calculatedDamage);
                            }
                        }
                        else if (splashOrigin == SplashOrigin.Targets)
                        {
                            for (int j = 0; j < totTargets; j++)
                            {
                                origin = targets[j].transform.position;

                                Collider[] targetHits;
                                targetHits = Physics.OverlapSphere(origin, curSplashRadius, layerMask);

                                for (int l = 0; l < targetHits.Length; l++)
                                {
                                    int calculatedDamage = CalculateDamage(targetHits[l].gameObject.tag, DamageType.Splash);
                                    targetHits[l].gameObject.GetComponent<Unit>().ChangeHealth(calculatedDamage, DamageType.Splash, gameObject.tag);
                                    Debug.Log(targetHits[l].gameObject.name + " has been hit for " + calculatedDamage);

                                    //Splash Particle Effect Targets Origin
                                    //var emitParams = new ParticleSystem.EmitParams();
                                    //This needs to be from a data entry in the sheet that takes in 3 values (I'm thinking RGB).
                                    //emitParams.startColor = effectSplashTargetColor;
                                    //This needs to be in accordance with the actual size of the splash radius.
                                    //emitParams.startSize = curSplashRadius;
                                    //effectSplashTarget.Emit(emitParams, 1);
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

    void OnDrawGizmosSelected()
    {
        if (curSplashRadius > 0 && splashType == SplashType.Sphere)
        {
            Gizmos.color = Color.green;
            if (splashOrigin == SplashOrigin.Self)
            {
                Gizmos.DrawWireSphere(transform.position, curSplashRadius);
            }
            else
            {
                for (int t = 0; t < totTargets; t++)
                {
                    Gizmos.DrawWireSphere(targets[t].transform.position, curSplashRadius);
                }
            }
        }

    }

    string GetUnitType()
    {
        if (!canMove)
        {
            return "Siege";
        }
        else
        {
            return "Troop";
        }
    }

    public int CalculateDamage(string targetTag, DamageType damageType)
    {
        int bonusDamage = 0;

        switch (targetTag)
        {
            default:
                {
                    Debug.Log("targetTag " + targetTag + " was not a recognizable tag.");
                    break;
                }
            case "Troop":
                {
                    bonusDamage += curTroopDamage;
                    break;
                }
            case "Siege":
                {
                    bonusDamage += curSiegeDamage;
                    break;
                }
            case "Core":
                {
                    bonusDamage += curCoreDamage;
                    break;
                }
            case "Chest":
                {
                    bonusDamage += curChestDamage;
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
                    bonusDamage += curDirectDamage;
                    break;
                }
            case DamageType.Splash:
                {
                    bonusDamage += curSplashDamage;
                    break;
                }
        }

        float accumulative = curBaseDamage + bonusDamage;
        float finalDamage = accumulative;

        //Debug.Log("Final damage is " + finalDamage);
        return Mathf.RoundToInt(finalDamage);
    }

    public void ChangeHealth(int damage)
    {
        curHealth -= damage;

        if (curHealth <= 0)
        {
            curHealth = 0;
            isAlive = false;
        }
        else if (curHealth >= actHealth)
        {
            curHealth = actHealth;
        }

        passiveManager.SelfDamaged();
    }

    public void ChangeHealth(int damage, DamageType damageType, string damageSource)
    {
        int bonusArmor = 0;

        switch (damageType)
        {
            case DamageType.Direct:
                {
                    //Debug.Log("Direct damage being dealt.");
                    bonusArmor += curDirectArmor;
                    break;
                }
            case DamageType.Splash:
                {
                    //Debug.Log("Splash damage being dealt.");
                    bonusArmor += curSplashArmor;
                    break;
                }
        }

        switch (damageSource)
        {
            default:
                {
                    Debug.LogWarning("The damageSource did not have a recorded tag");
                    break;
                }
            case "Troop":
                {
                    //Debug.Log("Troop damage being dealt.");
                    bonusArmor += curTroopArmor;
                    break;
                }
            case "Siege":
                {
                    //Debug.Log("Troop damage being dealt.");
                    bonusArmor += curSiegeArmor;
                    break;
                }
        }

        //Debug.Log("Bonus armor is " + bonusArmor);
        float accumulative = damage + bonusArmor + curBaseArmor;
        float damageMultiplier = damage / accumulative;
        float finalDamage = damage * damageMultiplier;

        if (finalDamage < 1)
        {
            //Debug.Log("Damage was 0, changed to 1.");
            finalDamage = 1;
        }

        //Debug.Log("Final damage is " + Mathf.RoundToInt(finalDamage));
        curHealth -= Mathf.RoundToInt(finalDamage);

        if (curHealth <= 0)
        {
            curHealth = 0;
            isAlive = false;
        }
        else if (curHealth >= actHealth)
        {
            curHealth = actHealth;
        }

        passiveManager.SelfDamaged();

        //Flash unit color to signify damage being dealt.
        StartCoroutine(DamageFlash());
    }

    IEnumerator DamageFlash()
    {
        Color regularColor = render.material.color;
        render.material.color = damageColor;
        yield return new WaitForSeconds(flashTime);
        //renderer.material.color = regularColor;
        render.material.color = Color.white;
    }

    IEnumerator DamageDelay(float damageDelay)
    {
        yield return new WaitForSeconds(damageDelay);
        AttackFunctionality();

        //Invoke the attack at salvoRate, curSalvo number of times.
        for (int i = 1; i < curSalvo; i++)
        {
            Invoke("AttackFunctionality", i * salvoRate);
        }
    }
}