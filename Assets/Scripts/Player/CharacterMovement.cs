using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class CharacterMovement : MonoBehaviour
{
    public float speed = 5;
    public float jumpPower = 4;
    public Collider[] ignoreCollisionObjects;
    Rigidbody rb;
    CapsuleCollider col;

    // Use this for initialization
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        rb = GetComponent<Rigidbody>();
        col = GetComponent<CapsuleCollider>();
        foreach (var ignoreCollisionObject in ignoreCollisionObjects)
            Physics.IgnoreCollision(col, ignoreCollisionObject);
    }

    void Update()
    {
        //Get the input value from the controllers
        float Horizontal = Input.GetAxis("Horizontal") * speed;
        float Vertical = Input.GetAxis("Vertical") * speed;
        Horizontal *= Time.deltaTime;
        Vertical *= Time.deltaTime;
        //Translate our character via our inputs.
        transform.Translate(Horizontal, 0, Vertical);

        if (isGrounded() && Input.GetButtonDown("Jump"))
        {
            //Add upward force to the rigid body when we press jump.
            rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
        }

        if (Input.GetKeyDown("escape"))
            Cursor.lockState = CursorLockMode.None;
    }

    private bool isGrounded()
    {
        //Test that we are grounded by drawing an invisible line (raycast)
        //If this hits a solid object e.g. floor then we are grounded.
        return Physics.Raycast(transform.position, Vector3.down, col.bounds.extents.y + 0.1f);
    }
}