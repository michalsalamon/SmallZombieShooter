using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Rigidbody))]
[RequireComponent (typeof(CapsuleCollider))]
[RequireComponent (typeof(PhysicMaterial))]

public class PlayerController : MonoBehaviour
{
    private Rigidbody RB;
    private CapsuleCollider coll;
    private PlayerCameraController playerCamera;

    private float playerHeight;
    private float playerRadius;
    private bool crouch = false;
    private bool isActorCrouching = false;
    private float crouchSizeReduction = 2;
    private bool isMovePlayer = false;
    private float dynamicFrictionHolder;

    //check timing
    private float timeToCheck = 0.25f;
    private float nextTimeCheck;

    [SerializeField] private LayerMask playerExclusionForChecking;

    private void Awake()
    {
        RB = GetComponent<Rigidbody>();
        coll = GetComponent<CapsuleCollider>();
        playerCamera = FindObjectOfType<PlayerCameraController>();

        playerHeight = coll.height;
        playerRadius = coll.radius;
        dynamicFrictionHolder = coll.material.dynamicFriction;

        nextTimeCheck = Time.time;
    }

    private void FixedUpdate()
    {
        //checks
        if (isActorCrouching && !crouch)
        {
            CrouchControl();
        }

        if (isMovePlayer)
        {
            coll.material.dynamicFriction = 0;
            isMovePlayer = false;
        }
        else if (coll.material.dynamicFriction == 0)
        {
            coll.material.dynamicFriction = dynamicFrictionHolder;
        }
    }

    public void MoveActor(Vector3 direction, float acclereation, float maxSpeed, float AirControl)
    {
        isMovePlayer = true;
        bool isActorInAir = !ActorGroundCheck();
        if (isActorCrouching && !isActorInAir)
        {
            maxSpeed *= 0.5f;
        }

        float accelerationModifier = 1;
        if (isActorInAir)
        {
            accelerationModifier = AirControl;
        }

        if (RB.velocity.magnitude < maxSpeed || isActorInAir)
        {
            RB.AddRelativeForce(accelerationModifier * acclereation * Time.deltaTime * direction);
        }
    }

    public void LookAround(Vector2 lookDirections, float lookSpeed, int verticalAxisSwitch)
    {
        lookDirections.y *= verticalAxisSwitch;
        lookDirections *= lookSpeed;
        Vector3 tempRot = transform.localEulerAngles;
        tempRot.y += lookDirections.x * Time.deltaTime;

        if (tempRot.y > 360)
        {
            tempRot.y -= 360;
        }
        else if (tempRot.y < 0)
        {
            tempRot.y += 360;
        }

        transform.localEulerAngles = tempRot;

        playerCamera.LookVertical(lookDirections.y);
    }

    public void ActorJump(float JumpForce)
    {
        if (ActorGroundCheck())
        {
            RB.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
        }
    }

    public void ActorCrouch()
    {
        crouch = true;
        CrouchControl();
    }

    public void ActorStandUp()
    {
        crouch = false;
        CrouchControl();
    }

    public bool ActorGroundCheck()
    {
        return Physics.OverlapSphere(coll.transform.position - Vector3.up * coll.height / 2 + Vector3.up * (coll.radius - 0.02f), coll.radius - 0.01f, playerExclusionForChecking).Length != 0;
    }

    public bool RoofOverCrouchingActor()
    {
        return Physics.OverlapCapsule(new Vector3(coll.transform.position.x, coll.transform.position.y - coll.height / 2 + coll.radius, coll.transform.position.z), new Vector3(coll.transform.position.x, coll.transform.position.y + coll.height / 2 * crouchSizeReduction - coll.radius, coll.transform.position.z), coll.radius - 0.01f, playerExclusionForChecking).Length != 0;

        //return Physics.OverlapBox(coll.transform.position + Vector3.up * (coll.bounds.size.y), new Vector3(coll.bounds.extents.x - 0.1f, coll.bounds.size.y, coll.bounds.extents.z - 0.1f), coll.transform.rotation, playerExclusionForChecking).Length != 0;
    }

    private void CrouchControl()
    {
        if (!isActorCrouching && crouch)
        {
            isActorCrouching = true;
            coll.height /= crouchSizeReduction;
        }

        if (isActorCrouching && !crouch && !RoofOverCrouchingActor())
        {
            isActorCrouching = false;
            coll.height *= crouchSizeReduction;
        }
    }
}
