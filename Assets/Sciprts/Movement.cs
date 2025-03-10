using System;
using UnityEngine;


public class Movement : MonoBehaviour
{
    public Rigidbody body;
    public float horizontalInput;
    public float forwardInput;
    public float speed = 8f;
    public bool isOnGround = false;
    public bool isOnObstacle = false;
    public float jumpForce = 5f;
    public Vector2 mouserot;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    
    
    // Update is called once per frame
    void Update()
    {
        //MouseMovement
        if(Input.GetKey(KeyCode.Mouse0))
        {
            mouserot.y += Input.GetAxis("Mouse X");
            transform.rotation = Quaternion.Euler(0, mouserot.y * 5.0f, 0);
        }
        
        //Movement
        horizontalInput = Input.GetAxis("Horizontal");
        forwardInput = Input.GetAxis("Vertical");

        transform.Translate(Vector3.forward * Time.deltaTime * speed * forwardInput);
        transform.Translate(Vector3.right * Time.deltaTime * speed * horizontalInput);

        //Jump
        if(Input.GetKeyDown(KeyCode.Space) && isOnGround || Input.GetKeyDown(KeyCode.Space) && isOnObstacle)
        {
            body.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

            body.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
            isOnGround = false;

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

    private void Reset()
    {
        transform.rotation = Quaternion.Euler(0f, -90f, 0f);
        gameObject.transform.position = new Vector3(1f, 1f, 0f);
        body.constraints = RigidbodyConstraints.FreezeAll;
        
    }






}
