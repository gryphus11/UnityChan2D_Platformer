using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CInputMovement : MonoBehaviour
{
    public KeyCode jumpKey = KeyCode.Space;

    public float speed = 6.4f;
    public float keyHoldSecond = 0.5f;

    public float jumpForce = 20.0f;
    public float startJumpForce = 10.0f;
    public float additiveJumpForce = 50.0f;

    public int maxJumpCount = 1;

    public bool isJump = false;
    public bool isGround = false;
    public bool isJumpKeyHeld = false;

    private Rigidbody2D _rigidbody2D;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;

    private bool _isJumpKeyHeld = false;
    private bool _isJump = false;
    private bool _isGround = false;

    private float _keyHoldElipse = 0.0f;

    private int _currentJumpCount = 0;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
    }

    private void Update()
    {
        InputMove();
        //InputJump();
        InputSensitivityJump();

        isJump = _isJump;
        isGround = _isGround;
        isJumpKeyHeld = _isJumpKeyHeld;
    }

    private void FixedUpdate()
    {
    }

    private void InputMove()
    {
        float h = Input.GetAxisRaw("Horizontal");

        if (Mathf.Abs(h) > 0)
            _spriteRenderer.flipX = (h == 1) ? false : true;

        _rigidbody2D.velocity = new Vector2(h * speed, _rigidbody2D.velocity.y);
        _animator.SetFloat("Horizontal", h);
        _animator.SetFloat("Vertical", _rigidbody2D.velocity.y);
    }

    private void InputJump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (_currentJumpCount < maxJumpCount)
            {
                ++_currentJumpCount;
                Jump();
                _isJump = true;
            }
        }
    }

    private void Jump()
    {
        _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, 0.0f);
        _rigidbody2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        _animator.SetTrigger("Jump");
    }

    private void InputSensitivityJump()
    {
        if (Input.GetKeyDown(jumpKey))
        {
            _isJumpKeyHeld = true;
            if (_currentJumpCount < maxJumpCount)
            {
                _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, 0.0f);
                _rigidbody2D.AddForce(Vector3.up * startJumpForce * _rigidbody2D.mass, ForceMode2D.Impulse);
                _animator.SetTrigger("Jump");
                _isJump = true;
            }
            ++_currentJumpCount;
        }
        else if (Input.GetKeyUp(jumpKey))
        {
            _isJumpKeyHeld = false;
            _keyHoldElipse = 0.0f;
        }

        if (Input.GetKey(jumpKey) && _currentJumpCount <= maxJumpCount)
        {
            if (_keyHoldElipse < keyHoldSecond)
            {
                _keyHoldElipse += Time.fixedDeltaTime;
                _rigidbody2D.AddForce(Vector3.up * additiveJumpForce * _rigidbody2D.mass, ForceMode2D.Force);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Ground")
        {
            _isGround = true;
            _animator.SetBool("IsGround", _isGround);
            _currentJumpCount = 0;
            _isJump = false;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.tag == "Ground")
        {
            _isGround = false;
            _animator.SetBool("IsGround", _isGround);
        }
    }
}
