using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private float moveSpeed;


    private float moveX;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        Move();
    }

    public void SetCurDir(float curDir)
    {
        moveX = curDir;
    }

    private void Move()
    {
        rb.velocity = new Vector2 (moveX * moveSpeed, rb.velocity.y);
    }
}
