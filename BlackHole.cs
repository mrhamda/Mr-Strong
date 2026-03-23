using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHole : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(!collision.CompareTag("Collider"))
        {
            if(collision.GetComponent<PlayerMovement>() || collision.CompareTag("Ice"))
            {
                PlayerMovement player = GameObject.FindObjectOfType<PlayerMovement>();
                player.health -= 1000;
            }else if(!collision.GetComponent<PlayerMovement>())
            {
                Destroy(collision.gameObject);

            }
        }
    }
}
