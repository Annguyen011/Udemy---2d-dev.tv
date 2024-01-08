using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public static Action OnShoot;
    public Transform BulletSpawnPoint => _bulletSpawnPoint;

    [SerializeField] private Transform _bulletSpawnPoint;
    [SerializeField] private Bullet _bulletPrefab;
    [SerializeField] private float gunFireCD = .5f;

    private Vector2 mousePos;
    private float gunFireCount;

    private void OnEnable()
    {
        OnShoot += ShootProjectile;
        OnShoot += ResetLastFireTime;
    }

    private void OnDisable()
    {
        OnShoot -= ShootProjectile;
        OnShoot -= ResetLastFireTime;
    }
    private void Update()
    {
        Shoot();
        RotateGun();
        gunFireCount += Time.deltaTime;
    }

    private void Shoot()
    {
        if (Input.GetMouseButton(0) && gunFireCount >= gunFireCD) {
            OnShoot?.Invoke();
        }
    }

    private void ResetLastFireTime()
    {
        gunFireCount = 0;
    }

    private void ShootProjectile()
    {
        Bullet newBullet = Instantiate(_bulletPrefab, _bulletSpawnPoint.position, Quaternion.identity);
        newBullet.Init(_bulletSpawnPoint.position, mousePos);
    }

    private void RotateGun()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //Vector2 direction = mousePos - (Vector2)PlayerController.Instance.transform.position;
        Vector2 direction = PlayerController.Instance.transform.InverseTransformPoint(mousePos);

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.localRotation = Quaternion.Euler(0, 0, angle);
    }
}
