using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiantBeetle :  Enemy
{
    [Header("GiantBeetle Things")]
    public GameObject projectilePrefab;
    public float delay;
    private float timer;
    public float projectileSpeed;

    public AudioClip projectileLaunchedAudio;
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        audioSource = GetComponent<AudioSource>();

        timer = delay;
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        ShootingAtPlayer();
    }

    public void ShootingAtPlayer()
    {
        if(player != null)
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
            }
            else if (timer <= 0)
            {
                GameObject _projectile = Instantiate(projectilePrefab);
                _projectile.transform.position = transform.position;

                Vector3 direction = player.transform.position - transform.position;

                audioSource.PlayOneShot(projectileLaunchedAudio);

                _projectile.GetComponent<Rigidbody2D>().AddForce(direction * projectileSpeed, ForceMode2D.Impulse);
                timer = delay;
            }
        }
       

    }
    
}
