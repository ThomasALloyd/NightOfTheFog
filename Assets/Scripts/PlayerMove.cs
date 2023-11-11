using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float sens = 500;
    public float speed = 10;

    float mouseY;
    float rotateX;
    float mouseX;
    float rotateY;

    public Transform orientation;

    float hori;
    float vert;

    Vector3 move;

    Rigidbody rb;
    
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * sens;
        mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * sens;

        rotateY += mouseX;
        rotateX -= mouseY;
        rotateX = Mathf.Clamp(rotateX, -90f, 45f);

        transform.rotation = Quaternion.Euler(rotateX, rotateY, 0);
        orientation.rotation = Quaternion.Euler(0, rotateY, 0);

        hori = Input.GetAxis("Horizontal");
        vert = Input.GetAxis("Vertical");
    }

    void FixedUpdate()
    {
        move = orientation.forward*vert + orientation.right*hori;
        move = move.normalized * speed;
        rb.velocity = move;
    }
}
