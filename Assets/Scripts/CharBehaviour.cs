using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharBehaviour : MonoBehaviour
{

    private Animator _anim;
    private Transform _enemy;
    private Rigidbody2D _rigid;
    private SpriteRenderer _sprite;
    private GameObject _aura;
    private bool _isWalking;
    private bool _isCrouching;
    private bool _isJumping;
    private bool _isPunching;
    private bool _isUlting;
    private bool _ultimateEnabled;
    private float _horizontal;
    private float _vertical;
    private float _life;

    public float speed;
    public float jumpForce;
    public float maxLife;
    public float damage;
    public float hitDistance;
    public float knockBackForce;
    public bool isGamepad;

    public float Life
    {
        set
        {
            if (_life - value <= 0)
            {
                _life = 0;
                Destroy(this.gameObject);
            } 
            else _life -= value;
        }

        get { return _life; }
    }

    void Start()
    {
        Init();
        AddEvents();
    }

    void Update()
    {
        if (!isGamepad) CheckMovement();
        else CheckGamepadMovement();
    }

    void FixedUpdate()
    {
        Move();
    }

    /// <summary>Initialize all variables.</summary>
    private void Init()
    {
        _anim = this.GetComponent<Animator>();
        _rigid = this.GetComponent<Rigidbody2D>();
        _sprite = this.GetComponent<SpriteRenderer>();
        _aura = this.transform.Find("Aura").gameObject;
        _enemy = this.gameObject.name == "Char" ? GameObject.Find("Char2").transform : GameObject.Find("Char").transform;

        _life = maxLife;
    }

    /// <summary>Adds all the event listeners.</summary>
    private void AddEvents()
    {
        EventManager.AddEventListener(Events.ON_ATTACK_EXIT, OnAttackExit);
        EventManager.AddEventListener(Events.ON_ULTIMATE_EXIT, OnUltimateExit);
        EventManager.AddEventListener(Events.ON_HIT, OnHit);
    }

    /// <summary>Checks inputs from the keyboard.</summary>
    private void CheckMovement()
    {
        _horizontal = InputManager.instance.GetHorizontalMovement();

        if (_horizontal != 0 && !_isWalking && !_isCrouching && !_isJumping && !_isPunching && !_isUlting)
            StartWalk();
        else if (_horizontal == 0 && _isWalking)
            StopWalk();

        if (InputManager.instance.GetCrouchDown() && !_isCrouching && !_isJumping && !_isPunching && !_isUlting)
        {
            StartCrouch();
            StopWalk();
        }

        if (InputManager.instance.GetCrouchUp() && _isCrouching)
            StopCrauch();

        if (InputManager.instance.GetJump() && !_isJumping && !_isPunching && !_isUlting)
        {
            StartJump();
            StopWalk();
            StopCrauch();
        }

        if (InputManager.instance.GetPunch())
        {
            if (_ultimateEnabled)
            {
                StopCrauch();
                StopWalk();
                StartUltimate();
            }
            else if (_isCrouching)
                StartCrouchPunch();
            else if (_isJumping)
                StartPunch();
            else
            {
                StopCrauch();
                StopWalk();
                StartPunch();
            }
        }

        if (InputManager.instance.GetEnableUltimate())
        {
            _ultimateEnabled = !_ultimateEnabled;

            if (_ultimateEnabled)
                _aura.SetActive(true);
            else
                _aura.SetActive(false);
        }
    }

    /// <summary>Checks inputs from the gamepad.</summary>
    private void CheckGamepadMovement()
    {
        _horizontal = InputManager.instance.GetGamepadHorizontalMovement();

        if (_horizontal != 0 && !_isWalking && !_isCrouching && !_isJumping && !_isPunching && !_isUlting)
            StartWalk();
        else if (_horizontal == 0 && _isWalking)
            StopWalk();

        if (InputManager.instance.GetGamepadCrouch() && !_isCrouching && !_isJumping && !_isPunching && !_isUlting)
        {
            StartCrouch();
            StopWalk();
        }

        if (!InputManager.instance.GetGamepadCrouch() && _isCrouching)
            StopCrauch();

        if (InputManager.instance.GetGamepadJump() && !_isJumping && !_isPunching && !_isUlting)
        {
            StartJump();
            StopWalk();
            StopCrauch();
        }

        if (InputManager.instance.GetGamepadPunch())
        {
            if (_ultimateEnabled)
            {
                StopCrauch();
                StopWalk();
                StartUltimate();
            }
            else if (_isCrouching)
                StartCrouchPunch();
            else if (_isJumping)
                StartPunch();
            else
            {
                StopCrauch();
                StopWalk();
                StartPunch();
            }
        }

        if (InputManager.instance.GetGamepadEnableUltimate())
        {
            _ultimateEnabled = !_ultimateEnabled;

            if (_ultimateEnabled)
                _aura.SetActive(true);
            else
                _aura.SetActive(false);
        }
    }

    /// <summary>Moves the character by physics.</summary>
    private void Move()
    {
        if (_isWalking)
            _rigid.velocity = new Vector2(_horizontal * speed, _rigid.velocity.y) * Time.deltaTime;
    }

    /// <summary>Makes the character to start walk.</summary>
    private void StartWalk()
    {
        _isWalking = true;
        _anim.SetBool("walk", _isWalking);

        _sprite.flipX = _horizontal > 0 ? false : true;
    }

    /// <summary>Makes the character to stop walk.</summary>
    private void StopWalk()
    {
        _isWalking = false;
        _anim.SetBool("walk", _isWalking);
    }

    /// <summary>Makes the character to start crouch.</summary>
    private void StartCrouch()
    {
        _isCrouching = true;
        _anim.SetBool("crouch", _isCrouching);
        _rigid.velocity = Vector3.zero;
    }

    /// <summary>Makes the character to stop crouch.</summary>
    private void StopCrauch()
    {
        _isCrouching = false;
        _anim.SetBool("crouch", _isCrouching);
    }

    /// <summary>Makes the character to throw a punch while crouch.</summary>
    private void StartCrouchPunch()
    {
        _isPunching = true;
        _anim.SetBool("punch", _isPunching);
    }

    /// <summary>Makes the character jump.</summary>
    private void StartJump()
    {
        _isJumping = true;
        _anim.SetBool("jump", _isJumping);
        _rigid.AddForce(this.transform.up * jumpForce * Time.deltaTime, ForceMode2D.Impulse);
    }

    /// <summary>Makes the character to stop jump.</summary>
    private void StopJump()
    {
        _isJumping = false;
        _anim.SetBool("jump", _isJumping);
    }

    /// <summary>Makes the character throw a punch.</summary>
    private void StartPunch()
    {
        _isPunching = true;
        _anim.SetBool("punch", _isPunching);
        if (!_isJumping) _rigid.velocity = Vector3.zero;

        var dir = _enemy.position - this.transform.position;

        if (Vector2.SqrMagnitude(dir) <= hitDistance)
            EventManager.DispatchEvent(Events.ON_HIT, new object[] { this.gameObject.name, damage, knockBackForce });
    }

    /// <summary>Makes the character to execute it's ultimate attack.</summary>
    private void StartUltimate()
    {
        _ultimateEnabled = false;
        _aura.SetActive(false);
        _rigid.velocity = Vector3.zero;
        _isUlting = true;
        _anim.SetBool("ultimate", _isUlting);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Floor"))
        {
            StopJump();
            if (InputManager.instance.GetCrouch())
                StartCrouch();
        }
    }

    private void OnAttackExit(params object[] paramsContainer)
    {
        _isPunching = false;
    }

    private void OnUltimateExit(params object[] paramsContainer)
    {
        _isUlting = false;
    }

    private void OnHit(params object[] paramsContainer)
    {
        if(this.gameObject.name != (string)paramsContainer[0])
        {
            Life = (float)paramsContainer[1];
            EventManager.DispatchEvent(Events.ON_LIFE_UPDATE, new object[] { this.gameObject.name, Life/maxLife });
            var dir = -(_enemy.position - this.transform.position).normalized;
            _rigid.AddForce(dir * (float)paramsContainer[2] * Time.deltaTime, ForceMode2D.Impulse);
        }
    }
}
