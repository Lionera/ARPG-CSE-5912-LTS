using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GithUb : EnemyAbilityController
{
    [SerializeField] float chaseDistance = 25.0f;
    [SerializeField] float meleeRange = 5.0f;
    [SerializeField] float vertexThreshold = 5.0f;

    [SerializeField] GameObject HealthBar;
    [SerializeField] PatrolPath patrolPath;

    Transform PlayerTarget;
    Vector3 PatrolToPosition;
    Coroutine draCoolRoutine;
    private NavMeshAgent navAgent;

    private int i = 1;
    private int CurrentPatrolVertexIndex = 0;
    private int AttackCycle = 0;
    public AudioManager audioManager;
    private bool fadeOutMusic = true;
    private bool startMusic = false;
    [SerializeField] float musicTransitionRadius = 30.0f;
    protected override void Start()
    {
        base.Start();

        navAgent = GetComponent<NavMeshAgent>();
        navAgent.speed = 3.5f;
        stats[StatTypes.AtkSpeed] = 1;
        stats[StatTypes.MaxHP] = 1500;
        stats[StatTypes.HP] = 1500;
        stats[StatTypes.LVL] = 30;
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

        GetComponent<Animator>().updateMode = AnimatorUpdateMode.AnimatePhysics;
        GetComponent<Animator>().updateMode = AnimatorUpdateMode.Normal;
        enemyAbilityOnCool = false;
        AbilityRange = 22f;
        FarAwayRange = 5f;
        cooldownTimer = 6f;
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
    public string GetClassTypeName()
    {
        return "GithUb";
    }

    new private void Update()
    {
        UpdateAnimator();
        if (stats[StatTypes.HP] <= 0 && !GetComponent<Animator>().GetBool("Dead"))
        {
            if (fadeOutMusic)
            {
                audioManager.FadeOut("Boss3BGM", "Dungeon3BGM");
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

        if (InMusicTransitionRadius() && PlayerTarget == null && stats[StatTypes.HP] > 0)
        {
            if (!startMusic)
            {
                startMusic = true;
                audioManager.FadeOut("Dungeon3BGM", "Boss3BGM");
            }
        }
        if (InSightRadius() && PlayerTarget == null && stats[StatTypes.HP] > 0)
        {
            audioManager.FadeOut("Dungeon2BGM", "Boss2BGM");
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
            if (InSightRadius() && InAbilityStopRange() && !enemyAbilityOnCool && stats[StatTypes.Mana] > 0 && EnemyAttackTypeList.Count > 0)
            {
                if (EnemyAttackTypeList[0].abilityOnCooldown == false && !InFarAwayRange())
                {
                    QueueAbilityCast(EnemyAttackTypeList[0].abilityAssigned);
                    if (draCoolRoutine == null)
                        draCoolRoutine = StartCoroutine(draCoolDown());
                }

                else if (EnemyAttackTypeList[i].abilityOnCooldown == false && InFarAwayRange())
                {

                    QueueAbilityCast(EnemyAttackTypeList[i].abilityAssigned);
                    if (draCoolRoutine == null)
                        draCoolRoutine = StartCoroutine(draCoolDown());
                    i++;
                    if (i == EnemyAttackTypeList.Count)
                    {
                        i = 1;
                    }
                }
            }
            if (GetComponent<Animator>().GetBool("AnimationEnded") && !InMeleeRange())
            {
                navAgent.isStopped = false;
                //navAgent.destination = PlayerTarget.position;

                NavMeshPath path = new NavMeshPath();
                agent.CalculatePath(player.transform.position, path);
                agent.path = path;
            }

            if (InMeleeRange())
            {
                navAgent.isStopped = true;
                switch (AttackCycle)
                {
                    case 0:
                        GetComponent<Animator>().SetTrigger("Attack1");
                        break;
                    case 1:
                        GetComponent<Animator>().SetTrigger("Attack3");
                        break;
                    case 2:
                        GetComponent<Animator>().SetTrigger("Attack3");
                        break;
                    default:
                        GetComponent<Animator>().SetTrigger("Attack3");
                        break;
                }
            }
        }
        else
        {
            if (GetComponent<Animator>().GetBool("AnimationEnded") && stats[StatTypes.HP] > 0)
            {
                PatrolBehavior();
            }
        }

    }

    private bool InSightRadius()
    {
        return Vector3.Distance(player.transform.position, transform.position) < chaseDistance;
    }
    private bool InMusicTransitionRadius()
    {
        return Vector3.Distance(player.transform.position, transform.position) < musicTransitionRadius;
    }

    private bool InMeleeRange()
    {
        return Vector3.Distance(player.transform.position, transform.position) < meleeRange;
    }

    private void MakeHostile()
    {
        PlayerTarget = player.transform;
        GetComponent<Animator>().SetTrigger("Intimidate");
        navAgent.speed = 6f;
    }

    private void MakeNonHostile()
    {
        navAgent.isStopped = true;
        PlayerTarget = null;
        GetComponent<Animator>().SetTrigger("Intimidate");
        navAgent.speed = 3.5f;
    }

    private void PatrolBehavior()
    {
        if (patrolPath != null)
        {
            if (AtPatrolVertex())
            {
                SetNextVertexIndex();
                PatrolToPosition = patrolPath.GetVertex(CurrentPatrolVertexIndex);
            }
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
        Vector3 velocity = navAgent.velocity;
        Vector3 localVelocity = transform.InverseTransformDirection(velocity);
        float speed = localVelocity.z;
        GetComponent<Animator>().SetFloat("forwardSpeed", speed);
    }

    // Animation Event
    void AnimationStarted()
    {
        GetComponent<Animator>().SetBool("AnimationEnded", false);
        navAgent.isStopped = true;
    }

    // Animation Event
    void AnimationEnded()
    {
        navAgent.isStopped = false;
        GetComponent<Animator>().SetBool("AnimationEnded", true);
    }

    // Animation Event
    void HitPlayer()
    {
        if (++AttackCycle > 2) { AttackCycle = 0; }
        QueueBasicAttack(basicAttackAbility, player.GetComponent<Character>(), this);
    }

    // Animation Event
    void DieAnimationEnded()
    {
        HealthBar.SetActive(false);
        base.RaiseEnemyKillExpEvent(this, GetComponent<Stats>()[StatTypes.LVL], GetComponent<Stats>()[StatTypes.MonsterType], transform.GetChild(0).name);
        PlayerTarget = null;
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
