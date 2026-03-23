using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotherGiantBeetle : Enemy
{
    [Header("Mother Things")]
    public GameObject smallBeteeles;
    public float delay;
    private float timer;

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

        if(timer >0)
        {
            timer -= Time.deltaTime;
        }else if(timer <= 0)
        {

            GameObject small_Be = Instantiate(smallBeteeles);
            small_Be.transform.position = new Vector2(transform.position.x + 0.1f, transform.position.y - 0.5f);

            audioSource.PlayOneShot(projectileLaunchedAudio);

            timer = delay;
        }
    }
}
