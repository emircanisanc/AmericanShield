using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(LineRenderer))]
public class InfinityGlow : WeaponBase
{
    [SerializeField] float glowLaserDistance = 15f;
    [SerializeField] float laserDuration = 2.5f;
    [SerializeField] int damage = 100;
    [SerializeField] float dragSpeed = 0.5f;
    [SerializeField] LayerMask enemyLayer;
    [SerializeField] Vector3 throwAngle;

    public bool IsBlocking { get { return isHoldingShield && Time.time - holdingStartTime < 0.5f; } }
    LineRenderer LaserLine;
    Transform LaserOrigin;
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
        LaserOrigin = transform.GetChild(0).GetChild(0);
        LaserLine = GetComponent<LineRenderer>();
    }

    private void HandleHoldingShield()
    {
        if (Input.touchCount > 0)
        {
            MoveShield();

        }
        else
        {

            LaserLine.enabled = false;// MoveShield();
        }
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
        sequence.Append(transform.DOLocalMove(shieldStartLocalPos, glowLaserDistance / 2));
        sequence.Join(transform.DORotate(shieldStartAngle, glowLaserDistance / 2));
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
        newPosition.y = Mathf.Clamp(newPosition.y, -0.05f, 0.25f);
        newPosition.x = Mathf.Clamp(newPosition.x, -0.3f, 0.3f);
        transform.localPosition = new Vector3(newPosition.x, newPosition.y, transform.localPosition.z);

        LaserFire(); // LaserFire fonksiyonunu MoveShield içinde çağır

        // Diğer işlemler
    }
    private void LaserFire()
    {
        RaycastHit raycastHit;
        Ray ray = mainCamera.ScreenPointToRay(touchEndPos);
        LaserLine.enabled = true;
        LaserLine.SetPosition(0, LaserOrigin.position);
        LaserLine.SetPosition(1, LaserOrigin.forward + new Vector3(transform.position.x, transform.position.y, glowLaserDistance));
        if (Physics.Raycast(ray, out raycastHit, -glowLaserDistance, enemyLayer))
        {
            Debug.Log("GirrdiMaGİRDİ");
            if (raycastHit.transform.TryGetComponent<IDamageable>(out var damageable))
            {
                Debug.Log("Girdi");
                damageable.ApplyDamage(damage);
            }
        }
        else
        {
            // Gerekirse başka bir işlem yapılabilir
        }
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
