using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleporterBeetle : Enemy
{
    [Header("Teleporter Things")]

    private TeleporterBeetle[] teleporterBeetles;
    private Enemy[] enemyBeetles;
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        audioSource = GetComponent<AudioSource>();

    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        teleporterBeetles = GameObject.FindObjectsOfType<TeleporterBeetle>();
        enemyBeetles = GameObject.FindObjectsOfType<Enemy>();

    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            audioSource.PlayOneShot(player.teleportation.usePowerUpAudio);
            ParticleSystem teleportEffect = Instantiate(player.teleportation.powerUpEffect);
            teleportEffect.gameObject.transform.position = transform.position;
           
                if(enemyBeetles.Length > 0)
                {
                    int randomNum = Random.Range(0, enemyBeetles.Length);

                    
                    player.transform.position = enemyBeetles[randomNum].transform.position;
                }
            
            ParticleSystem teleportEffectPlayer = Instantiate(player.teleportation.powerUpEffect);
            teleportEffectPlayer.gameObject.transform.position = player.transform.position;
        }
    }
  
}
