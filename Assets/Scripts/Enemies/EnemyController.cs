using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

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

    void Start()
    {
        player = GameObject.FindGameObjectsWithTag("Player")[0].transform;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        gameObject.SetActive(ActiveOnAwake);
        animator.SetBool(runName, true);
        _playerHealth = player.gameObject.GetComponent<PlayerHealth>();
        _playerRigidbody = player.gameObject.GetComponent<Rigidbody>();
    }
    
    void Update()
    {
        if (!animator.GetBool(deadName))
        {
            direciton = player.position - transform.position;
            if (!animator.GetBool(runName))
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

    private bool isAnimated;

    void FixedUpdate()
    {
        if (!animator.GetBool(deadName))
        {
            if (direciton.magnitude > 3f && animator.GetInteger(attackName) == 0)
            {
                animator.SetBool(runName, true);
            }
            else
            {
                animator.SetBool(runName, false);
                animator.SetInteger(attackName, attackType);
                
                if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.1f)
                    setForce = true;
                if (setForce && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.5f)
                {
                    setForce = false;
                    _playerRigidbody
                        .AddExplosionForce(1f, transform.position, 6.0f, 4.0f, ForceMode.Impulse);
                    if (direciton.magnitude < 3f && attackType != 0)
                    {
                        Debug.Log(attackType);
                        _playerHealth.ApplyDamage(AttackDamage[attackType - 1]);
                    }
                }
                if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f)
                {
                    animator.SetBool(runName, false);
                    animator.SetInteger(attackName, 0);
                    attackType = Random.Range(1, AttackRange + 1);
                }
            }
        }
    }
}