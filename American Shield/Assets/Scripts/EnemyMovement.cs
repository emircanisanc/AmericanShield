using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour, IDamageable
{
    [SerializeField] private int enemyHp = 100;
    [SerializeField] private float movementSpeed = 2f;
    [SerializeField] private float rotationSpeed = 10f;

    private Animator animator;
    private Rigidbody rigidBody;
    private GameObject player;
    private bool isLife = true;
    private bool distanceWalk;
    private bool distanceAttack;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(AnimationsStarts());
    }


    private void Update()
    {
        if (isLife)
        {
            FindDistance();
        }
        else
        {
            DeadStates();
        }
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
        Vector3 movement = transform.forward * movementSpeed * Time.deltaTime;
        rigidBody.MovePosition(transform.position + movement);
    }
    private void Attack()
    {
        animator.SetBool("isAttack", true);
        animator.SetBool("isWalk", false);
        distanceAttack = true;
        distanceWalk = false;
    }
    public void GetHit()
    {
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
        Destroy(this.gameObject, 3f);
    }
    private void FindDistance()
    {
        float distance = Vector3.Distance(transform.position, player.transform.position);
        if (distance < 2.5f)
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
        if (enemyHp <= 0)
        {
            isLife = false;
            animator.SetTrigger("isDie");
        }
    }
    IEnumerator GetHitAnim()
    {
        yield return new WaitForSeconds(1.5f);
        animator.SetBool("isGetHit", false);
    }
    IEnumerator AnimationsStarts()
    {
        yield return new WaitForSeconds(2f);
        distanceWalk = true;
        animator.SetBool("isWalk", true);
    }
}
