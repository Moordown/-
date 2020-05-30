using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator))]
public class BossController : MonoBehaviour
{
    public int AttackRange = 3;
    public FightJudger Judger;

    private NavMeshAgent agent;
    private Transform player;
    private Animator animator;
    private int attackType;
    private bool setForce = false;
    private Vector3 direciton;

    public static string deadName = "isDead";
    public static string attackName = "isAttacking";
    public static string runName = "isRunning";
    private Rigidbody _playerRigidbody;
    private PlayerHealth _playerHealth;
    private CapsuleCollider _capsuleCollider;

    void Start()
    {
        player = GameObject.FindGameObjectsWithTag("Player")[0].transform;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        gameObject.SetActive(false);
        _capsuleCollider = gameObject.GetComponent<CapsuleCollider>();
        _playerHealth = player.gameObject.GetComponent<PlayerHealth>();
        _playerRigidbody = player.gameObject.GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (!animator.GetBool(deadName))
        {
            direciton = player.position - transform.position;
            if (direciton.magnitude > 2f)
                transform.LookAt(new Vector3(0, player.position.y, player.position.z));
            if (!animator.GetBool(runName) && attackType == 0)
                attackType = 1 + (attackType + 1) % AttackRange;
        }
        else
        {
            agent.isStopped = true;
            _capsuleCollider.enabled = false;
            Judger.OnDeath(gameObject);
        }
    }

    void FixedUpdate()
    {
        if (!animator.GetBool(deadName))
        {
            if (direciton.magnitude > 3f)
            {
                agent.isStopped = false;
                agent.SetDestination(player.position);
                
                animator.SetBool(runName, true);
                attackType = 0;
                animator.SetInteger(attackName, 0);
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
                    if (direciton.magnitude < 3f)
                        _playerHealth.ApplyDamage(attackType);
                }
                else
                {
                    agent.isStopped = true;
                    
                    animator.SetBool(runName, false);
                    animator.SetInteger(attackName, 0);
                }
            }
        }
    }
}