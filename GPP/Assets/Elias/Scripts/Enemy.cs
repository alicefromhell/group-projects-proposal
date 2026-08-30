using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private int damage = 5;
    [SerializeField] private float attackCooldown = 1.5f;
    [SerializeField] private int maxHealth = 20;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private NavMeshAgent _agent;

    [Header("Audio")]
    [SerializeField] private AudioSource _enemyAttackSfx; 

    //If we ever want to do infinite rounds we can use the cost to determine what to spawn
    [SerializeField] private int cost = 1;

    private GameObject target;
    float cooldown;

    enum EnemyState
    {
        Chase,
        Attack
    }

    private EnemyState _enemyState = EnemyState.Chase;

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Core");
    }

    private void Update()
    {
        if(!target.activeSelf)
            return;

        cooldown += Time.deltaTime;

        switch (_enemyState)
        {
            case EnemyState.Chase:
                Chase();
                break;
            case EnemyState.Attack:
                Attack();
                break;
        }
    }

    public virtual void Chase()
    {
        _agent.SetDestination(target.transform.position);

        float dist = Vector3.Distance(transform.position, target.transform.position);

        if(dist < attackRange)
            _enemyState = EnemyState.Attack;
    }

    public void MoveTowards(Vector3 target, float speed)
    {
        Vector3 direction = (target - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;
        transform.LookAt(target);
    }

    public virtual void Attack()
    {
        if(cooldown > attackCooldown)
        {
            target.GetComponent<Entity>().DoDamage(damage);
            cooldown = 0f;

            //SND: Enemy Attack
            if (_enemyAttackSfx != null)
            {
                _enemyAttackSfx.Play(); 
            }
        }
    }
}
