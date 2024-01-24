using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    [SerializeField] private Transform feetTranform;
    [SerializeField] private Vector2 groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _jumpStrength = 7f;

    private PlayerInput _playerInput;
    private FrameInput _frameInput;
    private bool _isGrounded = false;
    private Movement _movement;

    private Rigidbody2D _rigidBody;

    public void Awake()
    {
        if (Instance == null) { Instance = this; }

        _rigidBody = GetComponent<Rigidbody2D>();
        _playerInput = GetComponent<PlayerInput>();
        _frameInput = GetComponent<FrameInput>();
    }

    private void Update()
    {
        GatherInput();
        Jump();
        HandleSpriteFlip();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private bool CheckGround()
    {
        Collider2D isGround = Physics2D.OverlapBox(feetTranform.position, groundCheck, 0f, groundLayer);
        return isGround;

    }
    public bool IsFacingRight()
    {
        return transform.eulerAngles.y == 0;
    }

    private void GatherInput()
    {
        //float moveX = Input.GetAxis("Horizontal");
        //_movement = new Vector2(moveX * _moveSpeed, _rigidBody.velocity.y);

        _frameInput = _playerInput.FrameInput;
    }

    private void Move()
    {
        _movement.SetCurDir(_frameInput.move.x);
    }

    private void Jump()
    {
        if (_frameInput.Jump && CheckGround())
        {
            _rigidBody.AddForce(Vector2.up * _jumpStrength, ForceMode2D.Impulse);
        }
    }

    private void HandleSpriteFlip()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (mousePosition.x < transform.position.x)
        {
            transform.eulerAngles = new Vector3(0f, -180f, 0f);
        }
        else
        {
            transform.eulerAngles = new Vector3(0f, 0f, 0f);
        }
    }
}
