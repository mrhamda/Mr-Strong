using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Projectile : MonoBehaviour
{
    public float damage;

    PlayerMovement player;

    public bool isFriendly;

    public bool isPower;

    public string type;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindObjectOfType<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && isFriendly == false && isPower == false)
        {
            ParticleSystem bloodPar = Instantiate(player.bloodParticle);

            bloodPar.gameObject.transform.position = transform.position;

            player.audioSource.PlayOneShot(player.gruntVoice);

            collision.GetComponent<PlayerMovement>().health -= damage;

            GameObject _dmgTextCanvas = Instantiate(player.dmgTextCanvasPrefab);
            _dmgTextCanvas.transform.position = player.transform.position;

            _dmgTextCanvas.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "-" + damage.ToString() ;

            Destroy(gameObject);
        }else if(collision.CompareTag("Enemy") && player.currentWeapon.isMelee == false && isFriendly == true && isPower == false)
        {
            if(player.currentWeapon.isBoth == false)
            {
                collision.GetComponent<Enemy>().health -= player.damage;

                GameObject _dmgTextCanvas = Instantiate(player.dmgTextCanvasPrefab);
                _dmgTextCanvas.transform.position = transform.position;

                _dmgTextCanvas.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "-" + player.damage.ToString();
            }
            else if(player.currentWeapon.isBoth == true)
            {
                collision.GetComponent<Enemy>().health -= player.damage * 2.5f;

                GameObject _dmgTextCanvas = Instantiate(player.dmgTextCanvasPrefab);
                _dmgTextCanvas.transform.position = transform.position;

                _dmgTextCanvas.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "-" + player.damage * 2.5f;

            }
            GameObject expolison = Instantiate(player.currentWeapon.slashEffect);

            expolison.gameObject.transform.position = transform.position;

            player.enemyHealthTimer = player.enemyHealthSliderDelay;


            player.currentEnemy = collision.gameObject;

          

            Destroy(gameObject);


        }else if(collision.CompareTag("Collider") )
        {
            Destroy(gameObject);
        }else if(collision.CompareTag("Enemy") &&  isPower == true)
        {
            collision.GetComponent<Enemy>().health -= damage;

            GameObject _dmgTextCanvas = Instantiate(player.dmgTextCanvasPrefab);
            _dmgTextCanvas.transform.position = transform.position;

            _dmgTextCanvas.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "-" + damage;

            GameObject expolison = Instantiate(player.currentWeapon.slashEffect);

            expolison.gameObject.transform.position = transform.position;

            player.enemyHealthTimer = player.enemyHealthSliderDelay;


            player.currentEnemy = collision.gameObject;

            Destroy(gameObject);
        }

        if (!collision.CompareTag("Player") && isFriendly == true && isPower == false && type == "BlackHole")
        {
            GameObject blackHole = Instantiate(player.blackHolePrefab);
            blackHole.transform.position = transform.position;

            Destroy(gameObject);
        }


    }
}
