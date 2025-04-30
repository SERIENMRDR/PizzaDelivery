using System;
using UnityEngine;


public class Movement : MonoBehaviour
{
    public Rigidbody body;
    public ConstantForce force;
    public float horizontalInput;
    public float forwardInput;
    public float speed = 8f;
    public bool isOnGround = false;
    public bool isOnObstacle = false;
    public float jumpForce = 5f;
    public Vector2 mouserot;
    [Range(1,100)]
    public float maxspeed = 5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        body.maxLinearVelocity = 50f;
    }
    
    
    // Update is called once per frame
    void Update()
    {
<<<<<<< Updated upstream
        
        if (body.linearVelocity.magnitude > maxspeed && isOnObstacle)
        {
=======

        if (isOnObstacle)
        {
            body.useGravity = false;
            force.force = new Vector3(0, -3, 0);
        }
        else
        {
            body.useGravity = true;
            force.force = new Vector3(0, 0, 0);
        }
        
        if (body.linearVelocity.magnitude > maxspeed && isOnObstacle)
        {
>>>>>>> Stashed changes
            body.linearVelocity = body.linearVelocity.normalized * maxspeed;
        }
        //MouseMovement
        if(Input.GetKey(KeyCode.Mouse0))
        {
            mouserot.y += Input.GetAxis("Mouse X");
            transform.rotation = Quaternion.Euler(0, mouserot.y * 5.0f, 0);
        }
        
        //Movement
        horizontalInput = Input.GetAxis("Horizontal");
        forwardInput = Input.GetAxis("Vertical");
        Vector3 v = new Vector3(horizontalInput, 0, forwardInput);
        v = transform.rotation * v;
        body.MovePosition(v * speed * Time.deltaTime + transform.position);

        //Jump
        if(Input.GetKeyDown(KeyCode.Space) && isOnGround || Input.GetKeyDown(KeyCode.Space) && isOnObstacle)
        {
            body.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

            body.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
            isOnGround = false;

        }
        //Dash
        if(Input.GetKey(KeyCode.LeftControl))
        {
            body.AddForce(maxspeed  * transform.forward * forwardInput, ForceMode.Impulse);
            body.AddForce(maxspeed  * transform.right * horizontalInput, ForceMode.Impulse);

        }

        //Obstacle
        if(isOnObstacle && Input.GetKey(KeyCode.F) && !isOnGround)
        {
            transform.Translate(Vector3.back * 1.5f);
            isOnObstacle = false;
            isOnGround = true;
        }
        if(Input.GetKey(KeyCode.R))
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






}
