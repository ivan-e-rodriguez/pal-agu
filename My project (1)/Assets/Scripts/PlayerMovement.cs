using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    [Range(1f, 2000f)]
    private float movementForce = 3f;

    [SerializeField]
    [Range(1f, 2000f)]
    private float jumpForce = 40f;

    [SerializeField]
    [Range(1f, 200f)]
    private float maxSpeed = 5f;

    [SerializeField]
    [Range(1f, 200f)]
    private float delayNextJump = 1f;

    [SerializeField]
    private Animator playerAnimator;
    public bool CanJump { get => canJump; set => canJump = value; }
    public Rigidbody MyRigidbody { get => myRigidbody; set => myRigidbody = value; }
    public float MaxSpeed { get => maxSpeed; set => maxSpeed = value; }
    private bool canJump = true;
    private bool inDelayJump = false;
    private float cameraAxisX = 0f;
    private Vector3 playerDirection;
    private Rigidbody myRigidbody;

    void Start()
    {
        MyRigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        RotatePlayer();

        bool forward = Input.GetKeyDown(KeyCode.W);
        bool back = Input.GetKeyDown(KeyCode.S);
        bool left = Input.GetKeyDown(KeyCode.A);
        bool right = Input.GetKeyDown(KeyCode.D);

        if (forward) playerAnimator.SetTrigger("FORWARD");
        if (back) playerAnimator.SetTrigger("BACK");
        if (left) playerAnimator.SetTrigger("LEFT");
        if (right) playerAnimator.SetTrigger("RIGHT");

        if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
        {
            if (!IsAnimation("IDLE")) playerAnimator.SetTrigger("IDLE");
        }

        playerDirection = Vector3.zero;

        if (Input.GetKey(KeyCode.W)) playerDirection += Vector3.forward;
        if (Input.GetKey(KeyCode.S)) playerDirection += Vector3.back;
        if (Input.GetKey(KeyCode.D)) playerDirection += Vector3.right;
        if (Input.GetKey(KeyCode.A)) playerDirection += Vector3.left;


        if (Input.GetKeyDown(KeyCode.Space) && canJump)
        {
            canJump = false;

        }
    }

    private void FixedUpdate()
    {
        if (playerDirection != Vector3.zero && MyRigidbody.velocity.magnitude < MaxSpeed)
        {

            MyRigidbody.AddForce(transform.TransformDirection(playerDirection) * movementForce, ForceMode.Force);

        }

        if (!canJump && !inDelayJump)
        {

            MyRigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            inDelayJump = true;
            Invoke("DelayNextJump", delayNextJump);
        }
    }

    private void DelayNextJump()
    {
        inDelayJump = false;
        canJump = true;
    }

    private bool IsAnimation(string animName)
    {
        return playerAnimator.GetCurrentAnimatorStateInfo(0).IsName(animName);
    }

    public void RotatePlayer()
    {
        cameraAxisX += Input.GetAxis("Mouse X");
        Quaternion newRotation = Quaternion.Euler(0, cameraAxisX, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, 2.5f * Time.deltaTime);
    }
}

