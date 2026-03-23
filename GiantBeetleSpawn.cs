using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiantBeetleSpawn : Enemy
{
    [Header("GiantBeetle Things")]
    public GameObject littleBeteelsPrefab;

    public AudioClip projectileLaunchedAudio;
    public override void Start()
    {
        base.Start();

        audioSource = GetComponent<AudioSource>();

    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        if(health <= 0)
        {
            player.audioSource.PlayOneShot(projectileLaunchedAudio);
            for(int i = 0; i< 4; i++)
            {
                GameObject littleBeteele = Instantiate(littleBeteelsPrefab);
                littleBeteele.transform.position = new Vector2(transform.transform.position.x + i / 2, transform.transform.position.y + i / 2); 

               
            }
        }
    }

}
