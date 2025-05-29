using System;
using System.Transactions;
using UnityEngine;


public class Movement : MonoBehaviour
{

    [Header("References")]
    public GameObject player;
    public Rigidbody body;
    
    [Header("Movement")]
    public float currentSpeed;
    public float horizontalInput;
    public float forwardInput;
    [Range(1, 100)] public float speed = 8;
    [Range(1, 100)]public float normSpeed = 8;

    
    [Header("Checks")]
    public bool isOnGround = false;
    public bool isOnObstacle = false;
    
    [Header("Sprint")]
    public bool isSprinting = false;
    [Range(1, 100)] public float sprintspeed = 12f;
    
    [Header("Dash")]
    public bool readyToDash = true;
    public float dashCooldown = 1f;
    [Range(1, 100)] public float dashspeed = 4f;
    
    [Header("Jump")]
    public float jumpForce = 5f;
    
    [Header("Rotation")]
    public Vector2 mouserot;

    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        body.maxLinearVelocity = 50f;
    }


    // Update is called once per frame
    void Update()
    {
        currentSpeed = body.linearVelocity.magnitude;
        //MouseMovement
        if (Input.GetKey(KeyCode.Mouse0))
        {
            mouserot.y += Input.GetAxis("Mouse X");
            transform.rotation = Quaternion.Euler(0, mouserot.y * 5.0f, 0);
        }

        //Movement
        horizontalInput = Input.GetAxis("Horizontal");
        forwardInput = Input.GetAxis("Vertical");
        Vector3 v = new Vector3(horizontalInput, 0, forwardInput);
        v = transform.rotation * v;
        body.Move(v * (speed * Time.deltaTime) + transform.position,transform.rotation);

        //Jump
        if (Input.GetKeyDown(KeyCode.Space) && isOnGround || Input.GetKeyDown(KeyCode.Space) && isOnObstacle)
        {
            body.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

            body.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
            isOnGround = false;
        }

        //Dash
        if (Input.GetKeyDown(KeyCode.LeftControl) && readyToDash)
        {
            readyToDash = false;
            body.AddForce(dashspeed * transform.forward * forwardInput, ForceMode.Impulse);
            body.AddForce(dashspeed * transform.right * horizontalInput, ForceMode.Impulse);
            Invoke(nameof(ResetDash), dashCooldown);
        }

        //Sprint
        if (Input.GetKey(KeyCode.LeftShift))
        {
            isSprinting = true;
            speed = sprintspeed;
        }
        else
        {
            isSprinting = false;
            speed = normSpeed;
        }

        if (isSprinting)
        {

        }

        //Obstacle
        if (isOnObstacle && Input.GetKey(KeyCode.F) && !isOnGround)
        {
            transform.Translate(Vector3.back * 1.5f);
            isOnObstacle = false;
            isOnGround = true;
        }

        if (Input.GetKey(KeyCode.R))
        {
            //Reset
            Debug.Log("Reset");
            Reset();
        }
    }


    private void OnTriggerEnter(Collider other)
        {
            Debug.Log("On Trigger");
            if (other.tag == "Wall")
            {
                isOnObstacle = true;
                Debug.Log("On Wall");
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.tag == "Wall")
            {
                isOnObstacle = false;
                Debug.Log("On Exit");
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Hindernis"))
            {
                isOnGround = true;
            }
        }

        /// <summary>
        /// Resets the character to the current Position
        /// </summary>
        private void Reset()
        {
            transform.rotation = Quaternion.Euler(0f, -90f, 0f);
            gameObject.transform.position = new Vector3(1f, 1f, 0f);
            body.constraints = RigidbodyConstraints.FreezeAll;
        }

        private void ResetDash()
        {
            readyToDash = true;
        }

    
}
