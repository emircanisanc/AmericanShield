using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Shield : MonoBehaviour
{
    [SerializeField] float shieldThrowDistance = 10f;
    [SerializeField] float shieldMoveDuration = 2.5f;
    [SerializeField] LayerMask enemyLayer;
    [SerializeField] Vector3 throwAngle;

    Sequence sequence;
    Camera mainCamera;
    Vector3 shieldStartLocalPos;
    Vector3 shieldStartAngle;
    Vector2 touchEndPos;
    bool isHoldingShield = false;
    bool isShieldOnHand = true;
    bool isShieldTurningBack = false;
    float holdingStartTime;

    void Awake()
    {
        mainCamera = Camera.main;    
    }

    void Update()
    {
        if (isShieldOnHand)
        {
            if (isHoldingShield)
            {
                HandleHoldingShield();
            }
            else
            {
                HandleNotHoldingShield();
            }
        }
        else
        {
            HandleFloatingShield();
        }
    }

    private void HandleHoldingShield()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            touchEndPos = touch.position;
        }
        else
        {
            ThrowShield();
        }
    }

    private void ThrowShield()
    {
        shieldStartLocalPos = transform.localPosition;
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
    }

    private void MoveShieldTo(Vector3 pos)
    {
        Vector4 targetAngle = throwAngle + new Vector3(Random.Range(-15, 15f),
         Random.Range(-15, 15f), Random.Range(-15, 15f));
        sequence = DOTween.Sequence();
        sequence.Append(transform.DORotate(targetAngle, 1f));
        sequence.Join(transform.DOMove(pos, shieldMoveDuration));
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
        isShieldTurningBack = true;
        sequence = DOTween.Sequence();
        sequence.Append(transform.DOLocalMove(shieldStartLocalPos, shieldMoveDuration / 2));
        sequence.Join(transform.DORotate(shieldStartAngle, shieldMoveDuration / 2));
        sequence.OnComplete(() => OnShieldTurnedBack());
    }

    private void OnShieldTurnedBack()
    {
        isShieldOnHand = true;
        isShieldTurningBack = false;
    }

    void OnCollisionEnter(Collision other)
    {
        if (!isShieldTurningBack)
        {
            sequence.Kill();
            TurnBack();
        }
        if (other.collider.TryGetComponent<IDamageable>(out var damageable))
        {
            damageable.ApplyDamage(5);
        }
    }

}
