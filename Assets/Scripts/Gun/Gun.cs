using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Gun : MonoBehaviour
{
    public static Action OnShoot;
    public Transform BulletSpawnPoint => _bulletSpawnPoint;

    [SerializeField] private Transform _bulletSpawnPoint;
    [SerializeField] private Bullet _bulletPrefab;
    [SerializeField] private float gunFireCD = .5f;

    private Vector2 mousePos;
    private float gunFireCount;
    private Animator animator;

    private ObjectPool<Bullet> bulletPool;
    private CinemachineImpulseSource impulseSource;

    private void Awake()
    {
        impulseSource = GetComponent<CinemachineImpulseSource>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        CreatePool();
    }

    private void OnEnable()
    {
        OnShoot += ShootProjectile;
        OnShoot += ResetLastFireTime;
        OnShoot += FireAnimation;
        OnShoot += GunScreenShake;
    }

    private void OnDisable()
    {
        OnShoot -= ShootProjectile;
        OnShoot -= ResetLastFireTime;
        OnShoot -= FireAnimation;
        OnShoot -= GunScreenShake;
    }

    public void ReleaseBulletFromPool(Bullet bullet)
    {
        bulletPool.Release(bullet);
    }
    private void Update()
    {
        Shoot();
        RotateGun();
        gunFireCount += Time.deltaTime;
    }

    private void Shoot()
    {
        if (Input.GetMouseButton(0) && gunFireCount >= gunFireCD)
        {
            OnShoot?.Invoke();
        }
    }

    private void ResetLastFireTime()
    {
        gunFireCount = 0;
    }

    private void ShootProjectile()
    {
        Bullet newBullet = bulletPool.Get();
        
        newBullet.transform.parent = transform;

        newBullet.Init(this, _bulletSpawnPoint.position, mousePos);
    }

    private void FireAnimation()
    {
        animator.Play("Fire", 0, 0f);
    }

    private void RotateGun()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //Vector2 direction = mousePos - (Vector2)PlayerController.Instance.transform.position;
        Vector2 direction = PlayerController.Instance.transform.InverseTransformPoint(mousePos);

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.localRotation = Quaternion.Euler(0, 0, angle);
    }

    private void CreatePool()
    {
        bulletPool = new ObjectPool<Bullet>(() =>
        {
            return Instantiate(_bulletPrefab);
        },
        bullet =>
        {
            bullet.gameObject.SetActive(true);
        },
        bullet =>
        {
            bullet.gameObject.SetActive(false);
        },
        bullet =>
        {
            Destroy(bullet);
        });
    }

    private Bullet CreateBullet()
    {
        return Instantiate(_bulletPrefab);
    }

    private void ActiveBullet(Bullet bullet)
    {
        bullet.gameObject.SetActive(true);
    }

    private void GunScreenShake()
    {
        impulseSource.GenerateImpulse();
        impulseSource.m_DefaultVelocity.x = (transform.position.x < mousePos.x) ? -.5f : .5f;
    }

}
