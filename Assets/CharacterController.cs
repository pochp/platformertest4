using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour {
    public GameObject GroundedCheck;
    public Rigidbody2D RB;
    
    public Vector3 Velocity;
    public float MaxHorizontalWalkSpeed = 0.2f;
    public float HorizontalWalkAcceleration = 0.02f;
    public float HorizontalMovementInertia = 0.02f;
    public float HorizontalAirMovementInfluence = 0.01f;

    public int MaxJumps = 2;
    public int JumpsRemaining = 0;
    public float JumpRisingSpeed = 0.4f;
    public float JumpRisingDuration = 0.5f;
    public float CurrentJumpDuration = -1f;
    private float JumpFallingSpeed = 0.01f;
    public float JumpCooldownRemaining = 0f;
    public float JumpCooldownBase = 0.2f;
    public float VerticalAirMovementInfluence = 0.005f;

    public int MaxAirdash = 2;
    public int AirdashesRemaining = 0;
    private float AirdashInitialVelocity = 0.4f;
    public float AirdashDuration = 0.01f;
    public float CurrentAirdashDuration;
    public bool Airdashing = false;

    public enum StatePosition { Grounded, Airborne }
    public StatePosition State_Position = StatePosition.Airborne;

    public bool IgnoreGravity = false;

    private bool CanInputStuff
    {
        get
        {
            return !Airdashing;
        }
    }

    public bool Grounded = false;
	// Use this for initialization
	void Start () {
        Velocity = new Vector3(0f, 0f, 0f);

    }
	
	// Update is called once per frame
	void Update () {
        HandleJumping();
        HandleAirdashing();
    }

    public void HandleAirdashing()
    {
        if(Airdashing)
        {
            CurrentAirdashDuration -= Time.deltaTime;
            if(CurrentAirdashDuration < 0)
            {
                Airdashing = false;
            }
        }
    }

    public void HandleJumping()
    {
        //if(!IgnoreGravity && State_Position == StatePosition.Airborne)
        //{
        //    Velocity.y = JumpFallingSpeed * -1f;
        //}
        if(CurrentJumpDuration > 0f)
        {
            CurrentJumpDuration -= Time.deltaTime;
            if(CurrentJumpDuration < 0)
            {
                //IgnoreGravity = false;
            }
        }

        //check if landed
        if (JumpCooldownRemaining > 0f)
        {
            JumpCooldownRemaining -= Time.deltaTime;
        }
        if(Input.GetButtonDown("Jump") && CanInputStuff)
        {
            if(JumpCooldownRemaining < 1 && JumpsRemaining > 0)
            {
                RB.velocity = new Vector2(RB.velocity.x, 0f);
                Vector2 jumpforce = new Vector3(0f, 400f);
                RB.AddForce(jumpforce, ForceMode2D.Force);
                //Velocity.y = JumpRisingSpeed;
                //CurrentJumpDuration = JumpRisingDuration;
                JumpCooldownRemaining = JumpCooldownBase;
                JumpsRemaining--;
            }
        }
    }

    public void OnLanded(Collider2D landedOn)
    {
        if (RB.velocity.y > 0)
            return;
        JumpsRemaining = MaxJumps;
        JumpCooldownRemaining = 0f;
        AirdashesRemaining = MaxAirdash;
        State_Position = StatePosition.Grounded;

        //set position to flat on the platform
        var y = landedOn.transform.position.y + landedOn.transform.localScale.y / 2f + transform.localScale.y / 2f;
        transform.position = new Vector3(transform.position.x, y, transform.position.z);
    }

    public void OnAirborne()
    {
        if (RB.velocity.y < 0)
            return;
        State_Position = StatePosition.Airborne;
    }

    void FixedUpdate()
    {
        UpdateVelocityFromInputs();
        this.transform.position += Velocity;
        Physics2D.Simulate(Time.fixedDeltaTime);
    }

    private void UpdateVelocityFromInputs()
    {
        if(CanInputStuff)
        {
            if (HandleAirdashInput())
                return;
            UpdateHorizontalVelocityFromInputs();
            UpdateVerticalVelocityFromInputs();
        }
    }

    private bool HandleAirdashInput()
    {
        if(AirdashesRemaining > 0 && Input.GetButtonDown("Fire3"))
        {
            Velocity = new Vector3(AirdashInitialVelocity, 0f);
            Airdashing = true;
            CurrentAirdashDuration = AirdashDuration;
            AirdashesRemaining--;
            return true;
        }
        return false;
    }

    private void UpdateVerticalVelocityFromInputs()
    {
        float vertical = Velocity.y;

    }

    private void UpdateHorizontalVelocityFromInputs()
    {
        float horizontalVelocity = Velocity.x;
        //left-right
        var horizontal = Input.GetAxisRaw("Horizontal");
        if (horizontal < -0.001f)
        {
            horizontalVelocity -= HorizontalWalkAcceleration;
            if (horizontalVelocity < MaxHorizontalWalkSpeed * -1f)
            {
                horizontalVelocity = MaxHorizontalWalkSpeed * -1f;
            }
        }
        else if (horizontal > 0.001f)
        {
            horizontalVelocity += HorizontalWalkAcceleration;
            if (horizontalVelocity > MaxHorizontalWalkSpeed)
            {
                horizontalVelocity = MaxHorizontalWalkSpeed;
            }
        }
        else
        {
            if (horizontalVelocity > 0f)
            {
                horizontalVelocity -= HorizontalMovementInertia;
                if (horizontalVelocity < 0f)
                    horizontalVelocity = 0f;
            }
            else if (horizontalVelocity < 0f)
            {
                horizontalVelocity += HorizontalMovementInertia;
                if (horizontalVelocity > 0f)
                    horizontalVelocity = 0f;
            }
        }
        Velocity.x = horizontalVelocity;
    }
}
