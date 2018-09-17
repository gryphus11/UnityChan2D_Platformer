using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator), typeof(SpriteRenderer))]
public class CInputMovement : MonoBehaviour
{
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode holdToWalk = KeyCode.LeftShift;

    public LayerMask groundLayer = 0;
    public Vector2 groundCheckOffset = Vector2.zero;
    public float radius = 1.0f;

    public float speed = 2.0f;
    public float keyHoldSecond = 0.5f;

    [Header("Sensitive Jump 사용 여부")]
    public bool useSensitiveJump = true;

    [Header("일반 점프용")]
    public float jumpForce = 3.0f;

    [Header("Sensitive Jump 시작 Force")]
    public float startJumpForce = 2.0f;
    [Header("Sensitive Jump 가중 Force")]
    public float additiveJumpForce = 7.0f;

    public int maxJumpCount = 2;

    private Rigidbody2D _rigidbody2D = null;
    private Animator _animator = null;
    private SpriteRenderer _spriteRenderer = null;
    private BoxCollider2D _boxCollider = null;

    private bool _isJumpKeyHeld = false;
    [SerializeField]
    private bool _isGround = false;
    private bool _isJump = false;

    private float _keyHoldElipse = 0.0f;

    private int _currentJumpCount = 0;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Start()
    {
        _animator.SetBool("Ground", _isGround);
    }

    private void Update()
    {
        InputMove();
        if (useSensitiveJump)
        {
            InputSensitivityJump();
        }
        else
        {
            InputJump();
        }
    }

    private void InputMove()
    {
        float h = Input.GetAxis("Horizontal");
        float absHorizon = Mathf.Abs(h);
        bool isAttack = _animator.GetBool("IsAttack");

        if (absHorizon > 0)
        {
            _spriteRenderer.flipX = (h > 0.0f) ? false : true;
            _animator.SetBool("Move", true);
        }
        else
        {
            _animator.SetBool("Move", false);
        }

        if (Input.GetKey(holdToWalk))
        {
            h *= 0.5f;
            absHorizon = 0.0f;
        }

        _rigidbody2D.velocity = new Vector2(h * speed, _rigidbody2D.velocity.y);
        _animator.SetFloat("Horizontal", absHorizon);
        _animator.SetFloat("Vertical", _rigidbody2D.velocity.y);
    }

    #region 일반 점프
    private void InputJump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (_currentJumpCount < maxJumpCount)
            {
                ++_currentJumpCount;
                Jump();
            }
        }
    }

    private void Jump()
    {
        _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, 0.0f);
        _rigidbody2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        _animator.SetTrigger("Jump");
        _isJump = true;
    }
    #endregion

    #region 버튼을 누르는 시간에 따른 민감도 점프
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

        // 현재 최대 점프수를 넘지 않고 점프키를 누르고 있는 동안
        if (_isJumpKeyHeld && _currentJumpCount <= maxJumpCount)
        {
            if (_keyHoldElipse < keyHoldSecond)
            {
                _keyHoldElipse += Time.fixedDeltaTime;
                _rigidbody2D.AddForce(Vector3.up * additiveJumpForce * _rigidbody2D.mass, ForceMode2D.Force);
            }
        }
    }
    #endregion

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Ground")
        {
            _isGround = true;
            _animator.SetBool("Ground", _isGround);
            _currentJumpCount = 0;
            _isJump = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.tag == "Ground")
        {
            _isGround = false;
            _animator.SetBool("Ground", _isGround);
        }
    }
}
