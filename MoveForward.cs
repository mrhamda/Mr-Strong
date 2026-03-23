using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveForward : MonoBehaviour
{
    private Rigidbody2D rb2D;
    public float speed;
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        rb2D.AddForce(transform.right * speed, ForceMode2D.Impulse);

    }

    // Update is called once per frame
    void Update()
    {
    }
}
