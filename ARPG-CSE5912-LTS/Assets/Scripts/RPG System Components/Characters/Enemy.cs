using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace ARPG.Combat
{
    public abstract class Enemy : Character
    {
        private GameObject GeneralClass;
        public NavMeshAgent enemy;

        public virtual float AttackRange { get; set; }
        public virtual float Range { get; set; }
        public virtual float BodyRange { get; set; }
        public virtual float SightRange { get; set; }
        public virtual float Speed { get; set; }

        public static event EventHandler<InfoEventArgs<(int, int)>> EnemyKillExpEvent;

        protected override void Start()
        {
            base.Start();
            GeneralClass = GameObject.Find("EnemyClass");
        }
        protected override void Update()
        {

            base.Update();
            if (stats[StatTypes.HP] <= 0)
            {
                if (GetComponent<Animator>().GetBool("Dead")== false){
                    Dead();
                }
                GetComponent<Animator>().SetBool("Dead", true);
                GetComponent<Transform>().GetChild(2).gameObject.SetActive(false);

            }
            else
            {
                SeePlayer();
            }
            if (AttackTarget != null)
            {
                Vector3 realDirection = GeneralClass.transform.forward;
                Vector3 direction = AttackTarget.position - GeneralClass.transform.position;
                float angle = Vector3.Angle(direction, realDirection);
                if (angle < SightRange && InStopRange())
                {
                    Quaternion rotationToLookAt = Quaternion.LookRotation(AttackTarget.transform.position - transform.position);
                    float rotationY = Mathf.SmoothDampAngle(transform.eulerAngles.y,
                    rotationToLookAt.eulerAngles.y, ref yVelocity, smooth);
                    transform.eulerAngles = new Vector3(0, rotationY, 0);

                }
            }

        }

        public  void SeePlayer()
        {

            if (InTargetRange()) //need set health.
            {
                //Debug.Log("yeah");
                Vector3 realDirection = GeneralClass.transform.forward;
                Vector3 direction = AttackTarget.position - GeneralClass.transform.position;
                float angle = Vector3.Angle(direction, realDirection);
                if (AttackTarget.GetComponent<Stats>()[StatTypes.HP] <= 0) //When player is dead, stop hit.
                {
                    StopRun();
                }
                else if (angle < SightRange && !InStopRange())
                {
                    //Debug.Log("run");
                    RunToPlayer();
                }
                else if (angle < SightRange && InStopRange())
                {
                    StopRun();
                    //Quaternion rotationToLookAt = Quaternion.LookRotation(AttackTarget.transform.position - transform.position);
                    GetComponent<Animator>().SetTrigger("AttackTrigger");
                    //float rotationY = Mathf.SmoothDampAngle(transform.eulerAngles.y, rotationToLookAt.eulerAngles.y, ref yVelocity, smooth);

                }
                else
                {
                    StopRun();
                }
            }
            //Debug.Log("okk");
        }

        public  void RunToPlayer()
        {
            if (InTargetRange())
            {
                enemy.isStopped = false;

                enemy.speed = Speed;
                enemy.SetDestination(AttackTarget.position);
            }
        }

        public void StopRun()
        {
            enemy.isStopped = true;
        }

        public virtual float CurrentEnemyHealth { get; set; }
        public virtual float MaxEnemyHealth { get; set; }
        public void Attack(EnemyTarget target)
        {
            AttackTarget = target.transform;
        }

        public void Cancel()
        {
            AttackTarget = null;
        }
        public  bool InTargetRange()
        {
            if (AttackTarget == null) return false;
            return Vector3.Distance(GeneralClass.transform.position, AttackTarget.position) < AttackRange;
        }
        public  bool InStopRange()
        {
            if (AttackTarget == null) return false;
            return Vector3.Distance(GeneralClass.transform.position, AttackTarget.position) < BodyRange;
        }
        public void Hit()
        {


            if (AttackTarget != null)
            {
                float distance = Vector3.Distance(this.transform.position, AttackTarget.transform.position);
                if (distance < 2)
                {
                    AttackTarget.GetComponent<Stats>()[StatTypes.HP] -= stats[StatTypes.PHYATK];
                }
            }
        }

        public void Dead()
        {
            //nead a dead animation before destroy.
            if (stats[StatTypes.HP] <= 0)
            {
                Debug.Log("enemy kkkkill");
                EnemyKillExpEvent?.Invoke(this, new InfoEventArgs<(int, int)>((stats[StatTypes.LVL], stats[StatTypes.MonsterType])));
                StartCoroutine(Die(10));
            }
        }
        IEnumerator Die(int seconds)
        {
            //Print the time of when the function is first called.
            //Debug.Log("Started Coroutine at timestamp : " + Time.time);

            //yield on a new YieldInstruction that waits for 5 seconds.
            yield return new WaitForSeconds(seconds);

            /////After we have waited  seconds print the time again.
            //Debug.Log("Finished Coroutine at timestamp : " + Time.time);
            //Destroy(gameObject);

        }
        public void ProduceItem()
        {
            Debug.Log("Item dropped");
        }

        public override Type GetCharacterType()
        {
            return typeof(Enemy);
        }
    }
}
