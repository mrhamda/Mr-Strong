using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHoleBeetle : Enemy
{
    [Header("BlackHoleBeetle Things")]
    public GameObject blackHolePrefab;
    private float distance;
    public override void Start()
    {
        base.Start();

        audioSource = GetComponent<AudioSource>();

    }

    // Update is called once per frame
    public override void Update()
    {
        if(player != null)
        {
            distance = (transform.position - player.transform.position).magnitude;

        }
        base.Update();

        if(health <= 0)
        {
            Die();
        }
       
    }



    public override void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") || collision.CompareTag("Ice"))
        {
            Die();
        }
    }
    public void Die()
    {
        GameObject blackhole = Instantiate(blackHolePrefab);
        blackhole.transform.position = transform.position;
        Destroy(gameObject);
    }
}
