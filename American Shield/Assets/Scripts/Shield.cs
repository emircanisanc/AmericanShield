using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Animator))]
public class Shield : WeaponBase
{
    [SerializeField] float shieldThrowDistance = 10f;
    [SerializeField] float shieldMoveDuration = 2.5f;
    [SerializeField] int damage = 100;
    [SerializeField] float dragSpeed = 0.5f;
    [SerializeField] LayerMask enemyLayer;
    [SerializeField] Vector3 throwAngle;
    [SerializeField] float minX = -0.2f, minY = -0.2f, maxY = 0.8f;

    public bool IsBlocking { get { return isHoldingShield && Time.time - holdingStartTime < 0.5f; } }

    Animator animator;
    Sequence sequence;
    Camera mainCamera;
    Vector3 shieldStartLocalPos;
    Vector3 shieldStartAngle;
    Vector2 touchEndPos;
    bool isHoldingShield = false;
    bool isShieldOnHand = true;
    bool isShieldTurningBack = false;
    float holdingStartTime;

    protected override void Awake()
    {
        base.Awake();
        mainCamera = Camera.main;
        animator = GetComponent<Animator>();
        shieldStartLocalPos = Vector3.zero;
    }

    private void HandleHoldingShield()
    {
        if (Input.touchCount > 0)
        {
            MoveShield();
        }
        else
        {
            ThrowShield();
        }
    }

    private void ThrowShield()
    {
        shieldStartAngle = transform.eulerAngles;
        RaycastHit raycastHit;
        Ray ray = mainCamera.ScreenPointToRay(touchEndPos);
        if (Physics.Raycast(ray, out raycastHit, shieldThrowDistance, enemyLayer))
        {
            MoveShieldTo(raycastHit.point);
        }
        else
        {
            MoveShieldTo(ray.origin + ray.direction * shieldThrowDistance);
        }
        isHoldingShield = false;
        isShieldOnHand = false;
        animator.SetTrigger("Throw");
    }

    private void MoveShieldTo(Vector3 pos)
    {
        Vector4 targetAngle = throwAngle + new Vector3(Random.Range(-15, 15f),
         Random.Range(-15, 15f), Random.Range(-15, 15f));
        float moveDuration = Mathf.Lerp(shieldMoveDuration / 3,
         shieldMoveDuration,
          (transform.position - pos).magnitude / shieldThrowDistance);
        sequence = DOTween.Sequence();
        sequence.Append(transform.DORotate(targetAngle, moveDuration, RotateMode.FastBeyond360));
        sequence.Join(transform.DOMove(pos, moveDuration).SetEase(Ease.OutFlash));
        sequence.OnComplete(() => TurnBack());
    }

    private void HandleNotHoldingShield()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                StartHolding();
            }
        }
    }

    private void StartHolding()
    {
        animator.SetTrigger("StartHolding");
        isHoldingShield = true;
        holdingStartTime = Time.time;
    }

    private void HandleFloatingShield()
    {
        if (Input.touchCount > 0 && !isShieldTurningBack)
        {
            sequence.Kill();
            TurnBack();
        }
    }

    private void TurnBack()
    {
        //animator.SetTrigger("Idle");
        isShieldTurningBack = true;
        sequence = DOTween.Sequence();
        sequence.Append(transform.DOLocalMove(shieldStartLocalPos, shieldMoveDuration / 2));
        sequence.Join(transform.DORotate(shieldStartAngle, shieldMoveDuration / 2));
        sequence.OnComplete(() => OnShieldTurnedBack());
    }

    private void OnShieldTurnedBack()
    {
        animator.SetTrigger("Idle");
        isShieldOnHand = true;
        isShieldTurningBack = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!isShieldTurningBack && !isShieldOnHand)
        {
            sequence.Kill();
            TurnBack();
        }
        if (other.TryGetComponent<IDamageable>(out var damageable))
        {
            if (!isShieldOnHand)
                damageable.ApplyDamage(damage);
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent<IDamager>(out var damager))
        {
            if (damager.IsAttacking())
            {
                if (IsBlocking)
                {
                    damager.TurnAttackBack();
                    animator.SetTrigger("BlockAttack");
                }
                else if (isHoldingShield)
                {
                    damager.BlockAttack();
                }

            }
        }
    }

    private void MoveShield()
    {
        Touch touch = Input.GetTouch(0);
        touchEndPos = touch.position;
        Vector3 touchInput = touch.deltaPosition;

        // Calculate the movement direction
        Vector3 movement = new Vector3(touchInput.x, touchInput.y, 0).normalized;

        // Move the shield relative to its current position
        Vector3 newPosition = transform.localPosition + movement * dragSpeed * Time.deltaTime;
        newPosition.y = Mathf.Clamp(newPosition.y, minY, maxY);
        newPosition.x = Mathf.Clamp(newPosition.x, minX, Mathf.Abs(minX));
        transform.localPosition = new Vector3(newPosition.x, newPosition.y, transform.localPosition.z);
    }

    private void ReleaseShield()
    {
        isHoldingShield = false;
        animator.SetTrigger("Idle");
    }

    public override void SkillOne()
    {
        if (isShieldOnHand)
        {
            if (isHoldingShield)
            {
                HandleHoldingShield();
            }
            else
            {
                if (Input.touchCount > 0)
                {
                    Touch touch = Input.GetTouch(0);
                    if (touch.phase == TouchPhase.Began)
                    {
                        isHoldingShield = true;
                    }
                }
            }
        }
        else
        {
            HandleFloatingShield();
        }
    }
    
    public override void SkillTwo()
    {
        if (!isShieldOnHand)
            return;
        
        if (isHoldingShield)
        {
            if (Input.touchCount > 0)
            {
                MoveShield();
            }
            else
            {
                ReleaseShield();
            }
        }
        else
        {
            HandleNotHoldingShield();
        }
    }

    public override void SkillThree()
    {
        
    }

    public override void SkillFour()
    {
        
    }
    
    public override void SkillFive()
    {
        
    }

}
