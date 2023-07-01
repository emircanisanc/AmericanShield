using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyMovement : MonoBehaviour, IDamageable, IDamager
{
    [SerializeField] private int enemyHp = 100;
    [SerializeField] private float movementSpeed = 2f;
    [SerializeField] private float followDistnace = 2f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float attackDuration;
    [SerializeField] private float hitAnimTime = 0.5f;

    public Action OnEnemyDie;

    private Animator animator;
    private Rigidbody rigidBody;
    private GameObject player;
    private bool isLife = true;
    private bool distanceWalk;
    private bool distanceAttack;
    private float nextDamageTime;
    private bool isAttacking;
    private bool isAttackBlocked;
    private bool isHurt;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player");
        //  StartCoroutine(AnimationsStarts());
    }


    private void Update()
    {
        if (isLife)
        {
            if (!isHurt)
            {
                FindDistance();
                if (distanceAttack)
                {
                    if (Time.time >= nextDamageTime)
                    {
                        if (!isAttackBlocked)
                        {
                            isAttacking = true;
                            // APPLY DAMAGE TO ENEMY
                        }
                        Invoke("StopAttack", attackDuration / 2);
                        nextDamageTime = Time.time + attackDuration;
                    }
                }
            }
        }
    }

    private void StopAttack()
    {
        isAttacking = false;
        isAttackBlocked = false;
    }

    private void Walk()
    {
        distanceAttack = false;
        distanceWalk = true;
        Vector3 targetDirection = player.transform.position - transform.position;
        targetDirection.y = 0f;
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        animator.SetBool("isAttack", false);
        animator.SetBool("isWalk", true);
        Vector3 movement = transform.forward * movementSpeed * Time.deltaTime * 0.75f;
        rigidBody.MovePosition(transform.position + movement);
    }
    private void Attack()
    {
        animator.SetBool("isAttack", true);
        animator.SetBool("isWalk", false);
        if (!distanceAttack)
        {
            distanceAttack = true;
            distanceWalk = false;
            nextDamageTime = Time.time + attackDuration;
        }
    }
    public void GetHit()
    {
        isHurt = true;
        animator.SetBool("isGetHit", true);
        animator.SetBool("isAttack", false);
        animator.SetBool("isWalk", false);
        StartCoroutine(GetHitAnim());
    }

    private void DeadStates()
    {
        animator.SetBool("isWalk", false);
        animator.SetBool("isAttack", false);
        distanceWalk = false;
        distanceAttack = false;
        GetComponent<Collider>().enabled = false;
        rigidBody.isKinematic = true;
        Invoke("CallDieEvent", 2f);
        Destroy(this.gameObject, 3f);
    }

    private void CallDieEvent()
    {
        OnEnemyDie?.Invoke();
    }
    private void FindDistance()
    {
        float distance = Vector3.Distance(transform.position, player.transform.position);
        if (distance < followDistnace)
        {
            Attack();
        }
        else
        {
            Walk();
        }
    }
    public void ApplyDamage(int damage)
    {
        enemyHp -= damage;
        if (enemyHp <= 0 && isLife)
        {
            isLife = false;
            DeadStates();
            animator.SetTrigger("isDie");
        }
        else
        {
            GetHit();
        }
    }
    IEnumerator GetHitAnim()
    {
        yield return new WaitForSeconds(hitAnimTime);
        animator.SetBool("isGetHit", false);
        isHurt = false;
    }
    IEnumerator AnimationsStarts()
    {
        yield return new WaitForSeconds(2f);
        distanceWalk = true;
        animator.SetBool("isWalk", true);
    }

    public bool IsAttacking()
    {
        return isAttacking;
    }

    public void TurnAttackBack()
    {
        isAttacking = false;
        isAttackBlocked = true;
        ApplyDamage(enemyHp);
    }

    public void BlockAttack()
    {
        isAttackBlocked = true;
    }
}
