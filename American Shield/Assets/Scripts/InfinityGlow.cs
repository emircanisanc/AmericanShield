using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(LineRenderer))]
public class InfinityGlow : WeaponBase
{
    [SerializeField] float glowLaserDistance = 15f;
    [SerializeField] float laserWidth = 2.5f;
    [SerializeField] int damage = 100;
    [SerializeField] float dragSpeed = 0.5f;
    [SerializeField] LayerMask enemyLayer;
    [SerializeField] float minX, minY, maxY;
    [SerializeField] Meteor water;
    [SerializeField] Transform waterSpawnPoint;
    [SerializeField] Transform laserEndParticle;


    LineRenderer LaserLine;
    Transform LaserOrigin;
    Animator animator;
    Camera mainCamera;
    Vector3 glowStartAngle;
    Vector2 touchEndPos;
    float holdingStartTime;
    float nextDamageTime;
    bool isHolding;
    public AudioClip hitClip;
    public AudioClip blockClip;
    Vector3 laserEndPos;

    protected override void Awake()
    {
        base.Awake();
        GameObject meteorObj = Instantiate(water.gameObject);
        water = meteorObj.GetComponent<Meteor>();
        water.gameObject.SetActive(false);
        mainCamera = Camera.main;
        animator = GetComponent<Animator>();
        glowStartAngle = transform.eulerAngles;
        LaserOrigin = transform.GetChild(0).GetChild(0);
        LaserLine = GetComponent<LineRenderer>();
    }

    protected override void ChangeSkill(int skill)
    {
        base.ChangeSkill(skill);
        if (skill != 0)
        {
            LaserLine.enabled = false;
        }
    }

    private void MoveAround(Touch touch)
    {
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
    private float audioCooldown = 0.1f;
    private void LaserFire()
    {
        RaycastHit raycastHit;
        LaserLine.enabled = true;
        Vector3 dir = LaserOrigin.forward - transform.right * transform.localPosition.x + transform.up * transform.localPosition.y / 2;
        audioCooldown -= Time.deltaTime;
        LaserLine.startWidth = 0.050f * laserWidth + Mathf.Sin(Time.time) / 50;
        if (Time.time >= nextDamageTime)
        {
            if (audioCooldown <= 0f)
            {
                AudioManager.PlayClip(hitClip, transform.position);
                audioCooldown = 0.2f; // İstenilen bekleme süresi
            }

            if (Physics.Raycast(transform.position, dir, out raycastHit, glowLaserDistance, enemyLayer))
            {
                if (raycastHit.transform.TryGetComponent<IDamageable>(out var damageable))
                {
                    ApplyDamageToEnemy(damageable, raycastHit.point);
                    nextDamageTime = Time.time + 0.5f;
                    laserEndPos = raycastHit.point;
                }
            }
            else
            {
                laserEndPos = dir * glowLaserDistance + transform.position;
            }
            Ray ray = mainCamera.ScreenPointToRay(touchEndPos);
            if (Physics.Raycast(ray, out raycastHit, glowLaserDistance, enemyLayer))
            {
                if (raycastHit.transform.TryGetComponent<TrapButton>(out var trapButton))
                {
                    trapButton.ApplyDamage(200);
                }
            }
        }
        else
        {
            if (Physics.Raycast(transform.position, dir, out raycastHit, glowLaserDistance, enemyLayer))
                laserEndPos = raycastHit.point;
            else
                laserEndPos = dir * glowLaserDistance + transform.position;
        }

    }

    private void WaterShoot()
    {
        RaycastHit raycastHit;
        if (Time.time >= nextDamageTime)
        {
            Vector3 shootDirection = waterSpawnPoint.forward + transform.right * transform.localPosition.x + transform.up * transform.localPosition.y / 2; ;
            if (Physics.Raycast(transform.position, waterSpawnPoint.forward, out raycastHit, glowLaserDistance, enemyLayer))
            {
                water.transform.position = waterSpawnPoint.position;
                shootDirection = (raycastHit.point - transform.position).normalized;
                water.MoveDirection(shootDirection);
            }
            else
            {
                water.transform.position = waterSpawnPoint.position;
                water.MoveDirection(shootDirection);
            }
            // SPAWN WATER BULLET
            nextDamageTime = Time.time + 2f;
        }
        Ray ray = mainCamera.ScreenPointToRay(touchEndPos);
        if (Physics.Raycast(ray, out raycastHit, glowLaserDistance, enemyLayer))
        {
            if (raycastHit.transform.TryGetComponent<TrapButton>(out var trapButton))
            {
                trapButton.ApplyDamage(200);
            }
        }
    }

    void LateUpdate()
    {
        LaserLine.SetPosition(0, waterSpawnPoint.position);
        LaserLine.SetPosition(1, laserEndPos);
        laserEndParticle.position = laserEndPos;
    }

    public override void SkillOne()
    {
        if (isHolding)
        {
            if (PlayerController.IsTouchingScreen(out var touch))
            {
                MoveAround(touch);
                LaserFire();

            }
            else
            {
                LaserLine.enabled = false;
                laserEndParticle.gameObject.SetActive(false);
                isHolding = false;
            }
        }
        else
        {
            if (PlayerController.IsTouchingScreen(out var touch))
            {
                if (touch.phase == TouchPhase.Began)
                {
                    isHolding = true;
                    laserEndParticle.gameObject.SetActive(true);
                    holdingStartTime = Time.time;
                }
            }
        }
    }


    public override void SkillTwo()
    {
        if (isHolding)
        {
            if (PlayerController.IsTouchingScreen(out var touch))
            {
                MoveAround(touch);
            }
            else
            {
                WaterShoot();
                isHolding = false;
            }
        }
        else
        {
            if (PlayerController.IsTouchingScreen(out var touch))
            {
                if (touch.phase == TouchPhase.Began)
                {
                    isHolding = true;
                    holdingStartTime = Time.time;
                }
            }
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

    protected void ApplyDamageToEnemy(IDamageable damageable, Vector3 pos)
    {
        if (damageable.ApplyDamage(damage))
        {
            if (!hitParticleObj.activeSelf)
            {
                hitParticleObj.transform.position = pos;
                hitParticleObj.SetActive(true);
                Invoke("CloseParticle", 0.6f);
            }
        }
    }

    protected void CloseParticle()
    {
        hitParticleObj.SetActive(false);
    }

}
