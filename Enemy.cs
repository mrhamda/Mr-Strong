using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using TMPro;
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(SetCurrentToSoundEffectsVolume))]

public class Enemy : MonoBehaviour
{
    [Header("Damage/Health")]

    public float damage;
    public float health;
    public float startHealth;

    [Header("A*")]
    private AIDestinationSetter aIDestinationSetter;
    private AIPath aIPath;

    [Header("Movement")]
    public float speed;

    [HideInInspector]
    public PlayerMovement player;

    [Header("Effects")]
    public ParticleSystem bloodParticle;

    

    [HideInInspector]
    public AudioSource audioSource;

    // Start is called before the first frame update
    public virtual void Start()
    {
        aIDestinationSetter = GetComponent<AIDestinationSetter>();

        player = GameObject.FindObjectOfType<PlayerMovement>();

        aIPath = GetComponent<AIPath>();

        health = startHealth;

        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
   
    public virtual void Update()
    {
        Death();
        if(player !=null)
        {
            FollowPlayer();
        }
    }

    public void Death()
    {
        if(health <= 0)
        {
            if(player != null)
            {
                player.audioSource.PlayOneShot(player.deathVoiceForEnemies);
                Destroy(gameObject);
                player.amountOfKills++;

                int x = Random.Range(0, 2);

                if(x == 1)
                {
                    Spawner spawner = GameObject.FindObjectOfType<Spawner>();
                    GameObject heart = Instantiate(spawner.heart);
                    heart.transform.position = transform.position;
                }
            }
        }
    }
    public void FollowPlayer()
    {
        aIPath.maxSpeed = speed;
        aIDestinationSetter.target = player.transform;
        
    }

    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") || collision.CompareTag("Ice"))
        {
            player.health -= damage;
            ParticleSystem bloodPar = Instantiate(bloodParticle);
            bloodPar.transform.position = collision.gameObject.transform.position;

            player.audioSource.PlayOneShot(player.gruntVoice);

            GameObject _dmgTextCanvas = Instantiate(player.dmgTextCanvasPrefab);
            _dmgTextCanvas.transform.position = player.transform.position;
            _dmgTextCanvas.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "-" + damage;

        }
        else if(collision.CompareTag("BlackHole"))
        {
            Destroy(gameObject);
        }
    }
}
