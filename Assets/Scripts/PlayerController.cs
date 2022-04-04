using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public float speed = 1;
    public TextMeshProUGUI countText;
    public GameObject winTextObject;

    private Rigidbody rb;
    private int count;
    private float movementX;
    private float movementY;

    public bool onGround;
    public bool canDoubleJump;

    public float groundDist;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        count = 0;

        SetCountText();
        winTextObject.SetActive(false);

        groundDist = GetComponent<Collider>().bounds.extents.y;
    }

    private void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();

        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    void SetCountText()
    {
        countText.text = "Score: " + count.ToString();
        if(count >= 15)
        {
            winTextObject.SetActive(true);
            rb.AddForce(new Vector3(0, 1000.0f, 0));
        }
    }

    /*    void Update()
        {
            if (Input.GetKeyDown("space"))
            {
                if (onGround)
                {
                    rb.AddForce(new Vector3(0, 10, 0));
                    canDoubleJump = true;
                } else
                {
                    if (canDoubleJump)
                    {
                        canDoubleJump = false;
                        rb.AddForce(new Vector3(0, 10, 0));
                    }
                }
            }
        }*/


    private void Update()
    {
        var keyboard = Keyboard.current;
        Vector3 jump = new Vector3(movementX, 50.0f, movementY);
        if (keyboard.spaceKey.wasPressedThisFrame)
        {
            if (onGround)
            {
                rb.AddForce(jump * speed);
                canDoubleJump = true;
                onGround = false;
            }
            else
            {
                if (canDoubleJump)
                {
                    canDoubleJump = false;
                    rb.AddForce(jump * speed);
                }
            }
        }
        onGround = Physics.Raycast(transform.position, Vector3.down, groundDist + 0.1f);
    }


    private void FixedUpdate()
    {
        Vector3 movement = new Vector3(movementX, 0.0f, movementY);

        rb.AddForce(movement * speed);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PickUp"))
        {
            other.gameObject.SetActive(false);
            count++;

            SetCountText();
        }
    }
}