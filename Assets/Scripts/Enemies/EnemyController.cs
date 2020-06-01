﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public enum AttackAnimationState
{
    Start, Force, End, None
}

[RequireComponent(typeof(Animator))]
public class EnemyController : MonoBehaviour
{
    public int AttackRange = 3;
    public int[] AttackDamage;
    public FightJudger Judger;

    private NavMeshAgent agent;
    private Transform player;
    private Animator animator;
    private int attackType;
    private bool setForce = false;
    private Vector3 direciton;

    public bool ActiveOnAwake = false;

    public static string deadName = "isDead";
    public static string attackName = "isAttacking";
    public static string runName = "isRunning";
    private Rigidbody _playerRigidbody;
    private PlayerHealth _playerHealth;
    private AttackAnimationState state;

    void Start()
    {
        player = GameObject.FindGameObjectsWithTag("Player")[0].transform;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        gameObject.SetActive(ActiveOnAwake);
        state = AttackAnimationState.None;
        _playerHealth = player.gameObject.GetComponent<PlayerHealth>();
        _playerRigidbody = player.gameObject.GetComponent<Rigidbody>();
    }
    
    void Update()
    {
        if (!animator.GetBool(deadName))
        {
            direciton = player.position - transform.position;
            if (state != AttackAnimationState.None)
            {
                agent.isStopped = true;
                return;
            }
            if (attackType == 0)
                attackType = Random.Range(1, AttackRange + 1);
            
            transform.LookAt(new Vector3(player.position.x, player.position.y, player.position.z));
            agent.isStopped = false;
            agent.SetDestination(player.position);
            if (direciton.magnitude > 2f)
                transform.LookAt(new Vector3(player.position.x, player.position.y, player.position.z));
        }
        else
        {
            agent.isStopped = true;
            Judger.OnDeath(gameObject);
        }
    }

    void FixedUpdate()
    {
        Debug.Log(state);
        if (!animator.GetBool(deadName))
        {
            if (direciton.magnitude > 3f && state == AttackAnimationState.None)
            {
                animator.SetBool(runName, true);
            }
            else
            {
                switch (state)
                {
                    case AttackAnimationState.None:
                        animator.SetBool(runName, false);
                        animator.SetInteger(attackName, attackType);
                        state = AttackAnimationState.Start;
                        break;
                    case AttackAnimationState.Start:
                        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.1f)
                            state = AttackAnimationState.Force;
                        break;
                    case AttackAnimationState.Force:
                        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f)
                        {
                            _playerRigidbody
                                .AddExplosionForce(1f, transform.position, 6.0f, 4.0f, ForceMode.Impulse);
                            if (direciton.magnitude < 3f && attackType != 0)
                            {
                                Debug.Log($"Attack type: {attackType}");
                                Debug.Log($"attack damage: {AttackDamage[attackType - 1]}");
                        
                                _playerHealth.ApplyDamage(AttackDamage[attackType - 1]);
                            }

                            state = AttackAnimationState.End;
                        }
                        break;
                    case AttackAnimationState.End:

                        if (Math.Abs(animator.GetCurrentAnimatorStateInfo(0).normalizedTime - 1f) < 0.05)
                        {
                            Debug.Log("end animation");

                            animator.SetBool(runName, false);
                            animator.SetInteger(attackName, 0);
                            attackType = 0;
                            state = AttackAnimationState.None;
                        }
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
}