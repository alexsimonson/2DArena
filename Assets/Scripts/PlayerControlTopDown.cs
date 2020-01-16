using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControlTopDown : MonoBehaviour
{
    private Rigidbody2D rb;
    // private PolygonCollider2D pc;

    private static float defaultWalkSpeed = 7f;
    private float walkSpeed = defaultWalkSpeed;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // pc = GetComponent<PolygonCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        MoveDirection();
    }

    void MoveDirection()
    {
        Vector2 targetVelocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        rb.velocity = targetVelocity * walkSpeed;
    }
}
