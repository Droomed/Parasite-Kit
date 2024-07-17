using System;
using UnityEngine;

public class PlayerController : MonoBehaviour, IPlayerController
{
    private Animator animator;
    private Rigidbody2D _rb;
    private BoxCollider2D _col;
    private FrameInput _frameInput;
    private Vector2 _frameVelocity;
    private bool _cachedQueryStartInColliders;

    public Vector2 FrameInput => _frameInput.Move;
    public event Action<bool, float> GroundedChanged;
    public event Action Jumped;

    private float _time;

    private bool _jumpToConsume;
    private bool _bufferedJumpUsable;
    private bool _endedJumpEarly;
    private bool _coyoteUsable;
    private float _timeJumpWasPressed;

    private bool HasBufferedJump => _bufferedJumpUsable && _time < _timeJumpWasPressed + .2f;

    private float _frameLeftGrounded = float.MinValue;
    private bool _grounded;

    void Start()
    {
        _rb = gameObject.GetComponent<Rigidbody2D>();
        _col = gameObject.GetComponent<BoxCollider2D>();
        animator = gameObject.GetComponent<Animator>();

        _cachedQueryStartInColliders = Physics2D.queriesStartInColliders;
    }

    void Update()
    {
        _time += Time.deltaTime;
        GatherInput();
    }

    private void FixedUpdate()
    {
        HandleCollision();
        HandleJump();
        HandleDirection();
        HandleGravity();
        ApplyMovement();
        var scale = gameObject.transform.localScale;
        if (_frameInput.Move.x < -0.01)
        {
            gameObject.transform.localScale = new Vector3(-1, scale.y, scale.y);
        }
        else if (_frameInput.Move.x > 0.01)
        {
            gameObject.transform.localScale = new Vector3(1, scale.y, scale.y);
        }
        
        animator.SetBool("run", _frameInput.Move.x != 0);
    }
    
    private void GatherInput()
    {
        _frameInput = new FrameInput
        {
            JumpDown = Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.C),
            JumpHeld = Input.GetButton("Jump") || Input.GetKey(KeyCode.C),
            Move = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"))
        };

        // if (_stats.SnapInput)
        // {
        //     _frameInput.Move.x = Mathf.Abs(_frameInput.Move.x) < _stats.HorizontalDeadZoneThreshold ? 0 : Mathf.Sign(_frameInput.Move.x);
        //     _frameInput.Move.y = Mathf.Abs(_frameInput.Move.y) < _stats.VerticalDeadZoneThreshold ? 0 : Mathf.Sign(_frameInput.Move.y);
        // }

        if (_frameInput.JumpDown)
        {
            _jumpToConsume = true;
            _timeJumpWasPressed = _time;
        }
    }


    private void HandleCollision()
    {
        Physics2D.queriesStartInColliders = false;

        // Ground and Ceiling
        bool groundHit =
            Physics2D.BoxCast(_col.bounds.center, _col.size, 0, Vector2.down, 0.05f);
        bool ceilingHit =
            Physics2D.BoxCast(_col.bounds.center, _col.size, 0, Vector2.up, 0.05f);

        // Hit a Ceiling
        if (ceilingHit) _frameVelocity.y = Mathf.Min(0, _frameVelocity.y);

        // Landed on the Ground
        if (!_grounded && groundHit)
        {
            _grounded = true;
            _coyoteUsable = true;
            _bufferedJumpUsable = true;
            _endedJumpEarly = false;
            GroundedChanged?.Invoke(true, Mathf.Abs(_frameVelocity.y));
        }
        // Left the Ground
        else if (_grounded && !groundHit)
        {
            _grounded = false;
            _frameLeftGrounded = _time;
            GroundedChanged?.Invoke(false, 0);
        }

        Physics2D.queriesStartInColliders = _cachedQueryStartInColliders;
    }
    
    private void HandleJump()
    {
        if (!_endedJumpEarly && !_grounded && !_frameInput.JumpHeld && _rb.velocity.y > 0) _endedJumpEarly = true;

        if (!_jumpToConsume && !HasBufferedJump) return;

        if (_grounded) ExecuteJump();

        _jumpToConsume = false;
    }

    private void ExecuteJump()
    {
        _endedJumpEarly = false;
        _timeJumpWasPressed = 0;
        _bufferedJumpUsable = false;
        _coyoteUsable = false;
        _frameVelocity.y = /*_stats.JumpPower*/36;
        Jumped?.Invoke();
    }

    private void HandleDirection()
    {
        if (_frameInput.Move.x == 0)
        {
            var deceleration = _grounded ? /*_stats.GroundDeceleration*/60 : /*_stats.AirDeceleration*/30;
            _frameVelocity.x = Mathf.MoveTowards(_frameVelocity.x, 0, deceleration * Time.fixedDeltaTime);
        }
        else
        {
            _frameVelocity.x = Mathf.MoveTowards(_frameVelocity.x, _frameInput.Move.x * /*_stats.MaxSpeed*/14, /*_stats.Acceleration*/120 * Time.fixedDeltaTime);
        }
    }

    private void HandleGravity()
    {
        if (_grounded && _frameVelocity.y <= 0f)
        {
            _frameVelocity.y = -1.5f; //_stats.GroundingForce;
        }
        else
        {
            var inAirGravity = 110; //_stats.FallAcceleration;
            if (_endedJumpEarly && _frameVelocity.y > 0) inAirGravity *= /*_stats.JumpEndEarlyGravityModifier*/3;
            _frameVelocity.y = Mathf.MoveTowards(_frameVelocity.y, - /*_stats.MaxFallSpeed*/40,
                inAirGravity * Time.fixedDeltaTime);
        }
    }
    
    private void ApplyMovement() => _rb.velocity = _frameVelocity;
}

public struct FrameInput
{
    public bool JumpDown;
    public bool JumpHeld;
    public Vector2 Move;
}

public interface IPlayerController
{
    public event Action<bool, float> GroundedChanged;

    public event Action Jumped;
    public Vector2 FrameInput { get; }
}