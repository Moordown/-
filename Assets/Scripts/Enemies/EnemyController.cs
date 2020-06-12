using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public enum AttackAnimationState
{
    Start,
    Force,
    End,
    None
}

[RequireComponent(typeof(NavMeshObstacle))]
[RequireComponent(typeof(Animator))]
public class EnemyController : MonoBehaviour
{
    public int AttackRange = 3;
    
    public int[] AttackDamage;
    public float[] AttackImpulse;
    
    public FightJudger Judger;

    private NavMeshAgent agent;
    private Transform player;
    private Animator animator;
    private int attackType;

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
                        attackType = Random.Range(1, AttackRange + 1);
                        animator.SetBool(runName, false);
                        animator.SetInteger(attackName, attackType);
                        state = AttackAnimationState.Start;
                        break;
                    case AttackAnimationState.Start:
                        // TODO: эта тупая анимация может потом по второму кругу пойти, или стейтмашина
                        // TODO: рассинхронизируется с тем, что мы показываем.
                        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.1f
                           && animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.3f)
                            state = AttackAnimationState.Force;
                        break;
                    case AttackAnimationState.Force:
                        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.5f)
                        {
                            state = AttackAnimationState.End;
                            if (direciton.magnitude < 3f && attackType != 0)
                            {
                                _playerRigidbody.AddExplosionForce(
                                    AttackImpulse[attackType - 1], transform.position,
                                    6.0f, 4.0f, ForceMode.VelocityChange);
                                _playerHealth.ApplyDamage(AttackDamage[attackType - 1]);
                            }
                        }

                        break;
                    case AttackAnimationState.End:
                        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1f - 0.05f)
                        {
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