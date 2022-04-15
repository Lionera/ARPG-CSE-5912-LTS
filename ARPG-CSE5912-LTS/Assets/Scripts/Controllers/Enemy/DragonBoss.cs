using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ARPG.Combat;
using UnityEngine.AI;
using System;

public class DragonBoss : Enemy
{
    [SerializeField] float chaseDistance = 15.0f;
    [SerializeField] float longRange = 10.0f;
    [SerializeField] float meleeRange = 5.0f;
    [SerializeField] float vertexThreshold = 5.0f;

    [SerializeField] GameObject HealthBar;
    [SerializeField] PatrolPath patrolPath;

    Transform PlayerTarget;
    Vector3 PatrolToPosition;

    private int CurrentPatrolVertexIndex = 0;

    //private void Awake()
    //{
    //    stats = GetComponent<Stats>();
    //}

    protected override void Start()
    {
        base.Start();
        stats[StatTypes.MaxHP] = 500;
        stats[StatTypes.HP] = 500;
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
    }

    new private void Update()
    {
        UpdateAnimator();
        if (stats[StatTypes.HP] <= 0)
        {
            animator.SetBool("Dead", true);
            //get rid of enemy canvas
            PlayerTarget = null;
        }

        if (InSightRadius() && PlayerTarget == null && stats[StatTypes.HP] >= 0)
        {
            MakeHostile();
        }
        else if (!InSightRadius() && PlayerTarget != null && stats[StatTypes.HP] >= 0)
        {
            MakeNonHostile();
        }

        if (PlayerTarget != null && stats[StatTypes.HP] > 0)
        {
            Quaternion rotationToLookAt = Quaternion.LookRotation(PlayerTarget.transform.position - transform.GetChild(0).position);
            float rotationY = Mathf.SmoothDampAngle(transform.eulerAngles.y,
            rotationToLookAt.eulerAngles.y, ref yVelocity, smooth);
            transform.eulerAngles = new Vector3(0, rotationY, 0);

            if (animator.GetBool("AnimationEnded") && !InMeleeRange())
            {
                agent.isStopped = false;
                agent.destination = PlayerTarget.position;
            }

            if (InMeleeRange())
            {
                agent.isStopped = true;
                agent.destination = transform.position;
                animator.SetTrigger("Attack1");
            }
        }
        else 
        {
            if (animator.GetBool("AnimationEnded") && stats[StatTypes.HP] >= 0)
            {
                PatrolBehavior();
            }
        }

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
        agent.destination = PatrolToPosition;
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
        print("Damaged player");
    }

    // Animation Event
    void DieAnimationEnded()
    {
        HealthBar.SetActive(false);
        base.RaiseEnemyKillExpEvent(this, stats[StatTypes.LVL], stats[StatTypes.MonsterType]);
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
