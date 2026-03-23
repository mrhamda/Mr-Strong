using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PowerUp : MonoBehaviour
{
    public float delay;

    private Animator anim;

    public AudioClip tookPowerUpAudio;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();

        StartCoroutine(_Destoy());

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator _Destoy()
    {
        yield return new WaitForSeconds(delay);
        anim.SetBool("Fade", true);
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);

    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerMovement>().audioSource.PlayOneShot(tookPowerUpAudio);
            

            Destroy(gameObject);
        }
    }
}
