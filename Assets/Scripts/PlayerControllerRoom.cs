using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerRoom : MonoBehaviour
{

    public float speed;
    private CharacterController rb;
    private Vector3 movement = Vector3.zero;
    public float gravity = 20.0f;



    void Start()
    {
        rb = GetComponent<CharacterController>();
    }

    void Update()
    {


            movement = new Vector3(0.0f, 0.0f, Input.GetAxis("Vertical"));
            movement = transform.TransformDirection(movement);
            movement *= speed;
            rb.Move(movement * Time.deltaTime);
            movement.y -= gravity * Time.deltaTime;
            transform.Rotate(Vector3.up * Input.GetAxis("Horizontal") * Time.deltaTime * speed * 50);
            rb.Move(movement * Time.deltaTime);


    }

}
