using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ARPG.Combat;
using UnityEngine.AI;
using System;

public class DragonBoss : EnemyAbilityController
{
    [SerializeField] float chaseDistance = 20.0f;
    [SerializeField] float meleeRange = 5.0f;
    [SerializeField] float vertexThreshold = 5.0f;

    [SerializeField] GameObject HealthBar;
    [SerializeField] PatrolPath patrolPath;

    Transform PlayerTarget;
    Vector3 PatrolToPosition;

    Coroutine draCoolRoutine;
    //Coroutine draRegen;

    private int CurrentPatrolVertexIndex = 0;
    public AudioManager audioManager;
    private bool fadeOutMusic = true;
    private bool startMusic = false;
    [SerializeField] float musicTransitionRadius = 20.0f;
    protected override void Start()
    {
        base.Start();
        stats[StatTypes.MaxHP] = 1000;
        stats[StatTypes.HP] = stats[StatTypes.MaxHP];
        stats[StatTypes.LVL] = 10;
        stats[StatTypes.MonsterType] = 3;

        PlayerTarget = null;

        if (patrolPath != null)
        {
            PatrolToPosition = patrolPath.GetVertex(CurrentPatrolVertexIndex);
        }
        else
        {
            PatrolToPosition = transform.position;
        }
        audioManager = FindObjectOfType<AudioManager>();
        enemyAbilityOnCool = false;
        AbilityRange = 15f;
        FarAwayRange = 4f;
        cooldownTimer = 6f;

    }

    public string GetClassTypeName()
    {
        return "DragonBoss";
    }
    IEnumerator draCoolDown()
    {
        enemyAbilityOnCool = true;
        timeChecker = cooldownTimer;
        while (timeChecker > 0)
        {
            timeChecker -= Time.deltaTime;
            yield return null;
        }
        enemyAbilityOnCool = false;
        draCoolRoutine = null;
    }
    /*
    private IEnumerator draRegenEnergy()
    {
        //yield return new WaitForSeconds(1);
        while (stats[StatTypes.Mana] < stats[StatTypes.MaxMana])
        {
            stats[StatTypes.Mana] += stats[StatTypes.ManaRegen];
            yield return new WaitForSeconds(3f);
        }
        draRegen = null;
    }
    */
    new private void Update()
    {
        UpdateAnimator();
        if (stats[StatTypes.HP] <= 0 && !GetComponent<Animator>().GetBool("Dead"))
        {
            if (fadeOutMusic)
            {
                audioManager.FadeOut("Boss1BGM", "Dungeon1BGM");
                fadeOutMusic = false;
            }
            animator.SetBool("Dead", true);
            NavMeshPath path = new NavMeshPath();
            agent.CalculatePath(transform.position, path);
            agent.path = path;
            agent.isStopped = true;
            agent.acceleration = 0;
            agent.angularSpeed = 0;
            agent.radius = 0;
            PlayerTarget = null;
        }
        /*
        if (stats[StatTypes.HP] > 0)
        {
            if (draRegen == null)
                draRegen = StartCoroutine(draRegenEnergy());
        }
        */
        
        if (InMusicTransitionRadius() && PlayerTarget == null && stats[StatTypes.HP] > 0)
        {
            if (!startMusic)
            {
                startMusic = true;
                audioManager.FadeOut("Dungeon1BGM", "Boss1BGM");
            }
        }
        if (InSightRadius() && PlayerTarget == null && stats[StatTypes.HP] > 0)
        {
            MakeHostile();
        }
        else if (!InSightRadius() && PlayerTarget != null && stats[StatTypes.HP] > 0)
        {
            MakeNonHostile();
        }

        if (PlayerTarget != null && stats[StatTypes.HP] > 0)
        {
            Quaternion rotationToLookAt = Quaternion.LookRotation(PlayerTarget.transform.position - transform.GetChild(0).position);
            float rotationY = Mathf.SmoothDampAngle(transform.eulerAngles.y,
            rotationToLookAt.eulerAngles.y, ref yVelocity, smooth);
            transform.eulerAngles = new Vector3(0, rotationY, 0);
            AttackTarget = PlayerTarget;
            if (InSightRadius() && InAbilityStopRange()&& !enemyAbilityOnCool && stats[StatTypes.Mana] > 0 && EnemyAttackTypeList.Count == 2)
            {
                if (EnemyAttackTypeList[0].abilityOnCooldown == false && !InFarAwayRange())
                {
                    QueueAbilityCast(EnemyAttackTypeList[0].abilityAssigned);
                    if (draCoolRoutine == null)
                        draCoolRoutine = StartCoroutine(draCoolDown());
                }else if (EnemyAttackTypeList[1].abilityOnCooldown == false && InFarAwayRange())
                {
                    QueueAbilityCast(EnemyAttackTypeList[1].abilityAssigned);
                    if (draCoolRoutine == null)
                        draCoolRoutine = StartCoroutine(draCoolDown());
                }
            }
        

            if (animator.GetBool("AnimationEnded") && !InMeleeRange())
            {
                agent.isStopped = false;
                NavMeshPath path = new NavMeshPath();
                agent.CalculatePath(player.transform.position, path);
                agent.path = path;
            }

            if (InMeleeRange())
            {
                agent.isStopped = true;
                animator.SetTrigger("Attack1");
            }
        }
        else
        {
            if (animator.GetBool("AnimationEnded") && stats[StatTypes.HP] > 0)
            {
                PatrolBehavior();
            }
        }

    }

    public bool InFarAwayRange()
    {
        return Vector3.Distance(transform.position, AttackTarget.position) < FarAwayRange;
    }
    public bool InAbilityStopRange()
    {
        return Vector3.Distance(transform.position, AttackTarget.position) < AbilityRange;
    }
    private bool InMusicTransitionRadius()
    {
        return Vector3.Distance(player.transform.position, transform.position) < musicTransitionRadius;
    }
    private bool InSightRadius()
    {
        return Vector3.Distance(player.transform.position, transform.position) < chaseDistance;
    }

    private bool InMeleeRange()
    {
        return Vector3.Distance(player.transform.position, transform.position) < meleeRange;
    }

    private void MakeHostile()
    {
        PlayerTarget = player.transform;
        animator.SetTrigger("Roar");
    }

    private void MakeNonHostile()
    {
        agent.isStopped = true;
        PlayerTarget = null;
        animator.SetTrigger("Roar");
    }

    private void PatrolBehavior()
    {
        if (patrolPath != null)
        {
            //print("For index: " + CurrentPatrolVertexIndex + " " + AtPatrolVertex());
            if (AtPatrolVertex())
            {
                SetNextVertexIndex();
                PatrolToPosition = patrolPath.GetVertex(CurrentPatrolVertexIndex);
            }
            //print("For index: " + CurrentPatrolVertexIndex + " " + AtPatrolVertex());

        }
        NavMeshPath path = new NavMeshPath();
        agent.CalculatePath(PatrolToPosition, path);
        agent.path = path;
    }

    private void SetNextVertexIndex()
    {
        CurrentPatrolVertexIndex = patrolPath.GetNextIndex(CurrentPatrolVertexIndex);
    }

    private bool AtPatrolVertex()
    {
        return Vector3.Distance(transform.position, PatrolToPosition) < vertexThreshold;
    }

    private void UpdateAnimator()
    {
        Vector3 velocity = agent.velocity;
        Vector3 localVelocity = transform.InverseTransformDirection(velocity);
        float speed = localVelocity.z;
        animator.SetFloat("forwardSpeed", speed);
    }

    // Animation Event
    void AnimationStarted()
    {
        animator.SetBool("AnimationEnded", false);
        agent.isStopped = true;
    }

    // Animation Event
    void AnimationEnded()
    {
        agent.isStopped = false;
        animator.SetBool("AnimationEnded", true);
    }

    // Animation Event
    void HitPlayer()
    {
        QueueBasicAttack(basicAttackAbility, player.GetComponent<Character>(), this);
    }

    // Animation Event
    void DieAnimationEnded()
    {
        HealthBar.SetActive(false);
        PlayerTarget = null;
        base.RaiseEnemyKillExpEvent(this, stats[StatTypes.LVL], stats[StatTypes.MonsterType], transform.GetChild(0).name);
        GetComponent<CapsuleCollider>().enabled = false;
        Destroy(this);
    }

    // Gizmos for sight range (purple) and melee range (red)
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(155, 0, 255);
        Gizmos.DrawWireSphere(transform.position, chaseDistance);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, meleeRange);
    }


}
