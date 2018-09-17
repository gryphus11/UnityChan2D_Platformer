using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CInputAttack : MonoBehaviour
{
    public KeyCode attackKey = KeyCode.Z;

    private int _attackType = 0;

    //5, 6, 7 오리지널 샘플레이트 12
    private Animator _animator = null;

    private bool _isAttack = false;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(attackKey) && !_animator.GetBool("IsAttack"))
        {
            _isAttack = true;
            _animator.SetBool("IsAttack", _isAttack);
            _animator.SetInteger("AttackType", _attackType);
            _animator.SetTrigger("Attack");
            ++_attackType;
            if (_attackType > 2)
            {
                _attackType = 0;
            }
        }
    }

    public void OnAttackStart()
    {

    }

    public void OnAttackEnd()
    {
        _isAttack = false;
        _animator.SetBool("IsAttack", _isAttack);
    }

    private void OnDisable()
    {
        OnAttackEnd();
    }
}
