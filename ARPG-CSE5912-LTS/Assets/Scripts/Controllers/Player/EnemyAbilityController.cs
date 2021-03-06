using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using ARPG.Combat;
using System;

public class EnemyAbilityController : Enemy
{
    protected override void OnDisable()
    {
        base.OnDisable();
        CastTimerCastType.AbilityCastTimeWasCompletedEvent -= OnCompletedCast;
        InstantCastType.AbilityInstantCastWasCompletedEvent -= OnCompletedCast;
    }
    protected override void Start()
    {
        base.Start();
    }
    protected override void Update()
    {
        base.Update();
    }

    [HideInInspector] public bool enemyInAOEAbilityTargetSelectionMode;
    [HideInInspector] public bool enemyInSingleTargetAbilitySelectionMode;

    int groundLayerMask = 1 << 6;

    protected override void OnEnable()
    {
        base.OnEnable();
        CastTimerCastType.AbilityCastTimeWasCompletedEvent += OnCompletedCast;
        InstantCastType.AbilityInstantCastWasCompletedEvent += OnCompletedCast;
    }

   


    void OnCompletedCast(object sender, InfoEventArgs<AbilityCast> e)
    {
        if (e.info.caster == this)
        {
            Debug.Log("Bandit debug: e.info.caster is " + e.info.caster + " and this is " + this);

            if (e.info.caster is Bandit)
            {
                Debug.Log("Bandit completed cast event was heard");
            }
            Debug.Log("Enemy Cast was completed");
            DeductCastingCost(e.info);
            GetColliders(e.info);
        }
    }

    public void EnemyQueueAbilityCastSelectionRequired(AbilityCast abilityCast)
    {
        if (abilityCast.requiresCharacterUnderCursor)
        {
            EnemyClick(abilityCast);
        }
        else
        {
            EnemyClickAOE(abilityCast);
        }
    }

    private void EnemyClick(AbilityCast abilityCast)
    {
        Ray ray = new Ray(AttackTarget.position + Vector3.up, Vector3.down);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            GameObject go = hit.collider.gameObject;
            Character target = go.GetComponent<Character>();
            //TODO: If ability requires enemy or ally to be clicked, excluding the other, check that first
            if (target != null)
            {
                abilityCast.hit = hit;
                bool targetInRange = CheckCharacterInRange(target);
                if (!targetInRange)
                {
                    Debug.Log("Character Not in range");
                    OnSingleTargetSelected(abilityCast);
                }
            }



        }
    }

    private void EnemyClickAOE(AbilityCast abilityCast)
    {
        if (abilityCast.abilityArea != null)
        {
            abilityCast.abilityArea.DisplayAOEArea();
        }

        if (abilityCast.abilityArea != null)
        {
            Ray ray = new Ray(AttackTarget.position + Vector3.up, Vector3.down);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, groundLayerMask))
            {
                abilityCast.hit = hit;
                OnGroundTargetSelected(abilityCast);
            }
            abilityCast.abilityArea.abilityAreaNeedsShown = false;

        }
    }
}
