using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 10f;
    [SerializeField] private int _damageAmount = 1;

    private Vector2 _fireDirection;

    private Gun gun;
    private Rigidbody2D _rigidBody;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {

    }
    private void FixedUpdate()
    {
        _rigidBody.velocity = _fireDirection * _moveSpeed;
        if(Vector2.Distance(gun.transform.position, transform.position) > 70f)
        {
            print("Pool active");
            gun.ReleaseBulletFromPool(this);
        }
    }

    public void Init(Gun gun, Vector2 bulletSpawnPos, Vector2 mousePos)
    {
        this.gun = gun;
        transform.position = bulletSpawnPos;

        _fireDirection = (mousePos - bulletSpawnPos).normalized;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Health health = other.gameObject.GetComponent<Health>();
            health?.TakeDamage(_damageAmount);
            gun.ReleaseBulletFromPool(this);
            
        }
        //Destroy(this.gameObject);
    }


}