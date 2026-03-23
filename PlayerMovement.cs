using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityStandardAssets.CrossPlatformInput;

[RequireComponent(typeof(SetCurrentToSoundEffectsVolume))]
public class PlayerMovement : MonoBehaviour
{
    public GameObject dmgTextCanvasPrefab;
    public FixedJoystick fixedJoystick;
    public GameObject blackHolePrefab;
    [HideInInspector]
    public bool isFreezed;
    [Header("Effects")]
    public ParticleSystem bloodParticle;

    [Header("Movement")]

    public float speed;
    [HideInInspector]
    public Rigidbody2D rb2D;

    public float damage;

    [Header("UI")]

    public Slider healthSlider;
    public Slider enemyHealthSlider;
    public TextMeshProUGUI enemyHealthText;
    public TextMeshProUGUI relodText;
    public TextMeshProUGUI killsAmountText;
    public TextMeshProUGUI killsAmountTextInLoseMenu;
    public TextMeshProUGUI timeSurrvivedTextInLoseMenu;
    public TextMeshProUGUI timeSurrvivedTextIn;
    public TextMeshProUGUI powerUpRelodText;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI coinsAmountText;

    public TextMeshProUGUI readyText;

    public float enemyHealthSliderDelay;
    public GameObject pauseButton;
    public TextMeshProUGUI teleportText;

    public GameObject readyButtonPowerUp;
    [HideInInspector]
    public float enemyHealthTimer;

    [Header("Health")]
    public float health;

    private SpriteRenderer spriteRenderer;

    [Header("Charachters")]
    public characher tank = new characher();
    public characher healer = new characher();
    public characher reloder = new characher();


    public characher currentCharachter = new characher();

    [Header("PowerUps")]
    public PowerUp currentPowerUp = new PowerUp();

    public PowerUp healing = new PowerUp();
    public PowerUp hardening = new PowerUp();
    public PowerUp teleportation = new PowerUp();
    public PowerUp shield = new PowerUp();
    public PowerUp shootProjectileARound = new PowerUp();
    public PowerUp blackHoleShot = new PowerUp();

    [Header("Weapon")]
    public Weapon hands = new Weapon();
    public Weapon fireBall = new Weapon();
    public Weapon sword = new Weapon();
    public Weapon rifle = new Weapon();

    public Weapon currentWeapon = new Weapon();

    private float delay;

    [Header("Menues")]
    public GameObject pauseMenu;
    public GameObject loseMenu;

    [Header("Audios")]
    public AudioClip gruntVoice;
    public AudioClip deathVoiceForEnemies;

    [HideInInspector]
    public bool isPaused;

    [HideInInspector]
    public AudioSource audioSource;

    [HideInInspector]
    public int amountOfKills;

    [HideInInspector]
    public float timeSurrvived;

    [HideInInspector]
    public bool isDead = false;

    private ButtonManagerGamePlay buttonManagerGamePlay;

    [HideInInspector]
    public GameObject currentEnemy;

    [HideInInspector]
    public int coins;

    private bool telpMode = false;
    private ParticleSystem currentEffect;

    Vector2 move;

    private int strongerEach20Kills;
    private bool didUseExtraLifeChance;

    public ITEM_CLASS fasterItem;
    public ITEM_CLASS slowerItem;
    public ITEM_CLASS ExtraCoinsItem;
    public ITEM_CLASS relodesPowerFasterItem;
    public ITEM_CLASS extraLifeItem;
    public ITEM_CLASS strongerItem;


    public RawImage infoPowerup;
    public RawImage infoItem;

    private ITEM_CLASS currentItem;
    // Start is called before the first frame update
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        buttonManagerGamePlay = GameObject.FindObjectOfType<ButtonManagerGamePlay>();

        audioSource = GetComponent<AudioSource>();
        Time.timeScale = 1;

        LoadChar();
        LoadWeapon();
        LoadPower();

        print(PlayerPrefs.GetString("Item"));

        //currentPowerUp = shootProjectileARound;
        if (PlayerPrefs.GetString("Item") == "RelodesFaster")
        {
            currentPowerUp.relodTime -= 5;
        }

        if (currentPowerUp != null)
        {
            currentPowerUp.timer = currentPowerUp.relodTime;
        }
   

        speed = currentCharachter.startSpeed;

        health = currentCharachter.startHealth;

        healthSlider.maxValue = currentCharachter.startHealth;


        spriteRenderer.sprite = currentCharachter.forwardSprite;

        damage = currentWeapon.startdamage;

        strongerEach20Kills = amountOfKills + 20;

        if (currentCharachter == reloder)
        {
            currentWeapon.relodTime -=  currentWeapon.relodTime * 0.25f;
        }

        if(PlayerPrefs.GetString("Device") == "Phone")
        {
            pauseButton.gameObject.SetActive(true);
            fixedJoystick.gameObject.SetActive(true);
            readyButtonPowerUp.gameObject.SetActive(false);


        }
        else if(PlayerPrefs.GetString("Device") == "Computer")
        {
            pauseButton.gameObject.SetActive(false);
            fixedJoystick.gameObject.SetActive(false);
            readyButtonPowerUp.gameObject.SetActive(false);
        }

       

    }

    // Update is called once per frame
    void Update()
    {
        LoadItem();
        if(isFreezed == true)
        {
           StartCoroutine(Freeze());
        }
        if(PlayerPrefs.GetString("Device") == "Phone")
        {
            TeleporationForMobile();

        }

        if(isFreezed == false)
        {
            Move();

            Animation();

            if (isPaused == false)
            {
                DamageEnemies();

                if (currentPowerUp != null)
                {
                    UsePowerUp();
                }
            }
        }

       

        Death();

        HealthSlider();
        

        PauseMenu();

        TimeScale();

        EnemyHealthSlider();

        RelodText();

        KillsAmountText();

        Lose();

        TimeSurrvived();

     
        HealthText();

        if(shield.isOn == true)
        {
            KickEnemiesOutsideShield();
        }
        if(currentPowerUp == blackHoleShot && PlayerPrefs.GetString("Device") == "Phone" && currentPowerUp.isOn == true)
        {
            ShootBlackHoleShootMobile();
        }
        Coins();

        Healer();
        if(transform.position.x > 14.7f)
        {
            transform.position = new Vector2(13.45f, transform.position.y);
        }else if(transform.position.x < - 14.7)
        {
            transform.position = new Vector2(-13.45f, transform.position.y);
        }

        if(transform.position.y > 8)
        {
            transform.position = new Vector2(transform.position.x, 4.6f);

        }else if(transform.position.y < -8)
        {
            transform.position = new Vector2(transform.position.x, -4.6f);
        }
    }

    public IEnumerator InfoImages(float timer,RawImage img, Texture _texture)
    {
        img.transform.parent.gameObject.SetActive(true);
        img.texture = _texture;

        yield return new WaitForSeconds(timer);

        img.transform.parent.gameObject.SetActive(false);
        img.texture = null;

    }


    public void UseEffect(GameObject effect,AudioClip audioClip)
    {

        GameObject _effect = Instantiate(effect);
        effect.transform.position = transform.position;

        effect.transform.localScale = new Vector3(1, 1, 1);

        audioSource.PlayOneShot(audioClip);
    }
    public void ShootBlackHoleShootMobile()
    {
        if(currentPowerUp.isOn == true && currentPowerUp == blackHoleShot)
        {
            teleportText.gameObject.SetActive(true);
            teleportText.text = "Click where you want to shoot";
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 screenPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);



                Vector2 direction = screenPos - new Vector2(transform.position.x, transform.position.y);


                GameObject projectile = Instantiate(currentPowerUp.projectilePrefab);

                projectile.transform.position = transform.position;

                
                projectile.transform.GetComponent<Rigidbody2D>().AddForce(direction * 2.5f, ForceMode2D.Impulse);

                projectile.transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);


                currentPowerUp.timer = currentPowerUp.relodTime;
                audioSource.PlayOneShot(currentPowerUp.usePowerUpAudio);

                ParticleSystem effect = Instantiate(currentPowerUp.powerUpEffect);
                effect.transform.position = transform.position;

                effect.transform.localScale = new Vector3(1, 1, 1);

                currentEffect = effect;

                teleportText.gameObject.SetActive(false);

                currentPowerUp.isOn = false;

            }

        }
    }

    [System.Serializable]
    public class ITEM_CLASS
    {
        public GameObject effect;
        public AudioClip soundEffect;
        public Texture imgTexture;
    }
    public void TeleporationForMobile()
    {
        if (telpMode == true && currentPowerUp == teleportation)
        {
            teleportText.gameObject.SetActive(true);

            if (Input.GetMouseButtonDown(0))
            {
                currentPowerUp.timer = currentPowerUp.relodTime;
                audioSource.PlayOneShot(currentPowerUp.usePowerUpAudio);

                ParticleSystem effect = Instantiate(currentPowerUp.powerUpEffect);
                effect.transform.position = transform.position;

                effect.transform.localScale = new Vector3(1, 1, 1);

                currentEffect = effect;

              
                Vector2 screenPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                screenPos.x = Mathf.Clamp(screenPos.x, -13, 13);
                screenPos.y = Mathf.Clamp(screenPos.y, -7, 5);

                transform.position = screenPos;

                ParticleSystem _effect = Instantiate(currentPowerUp.powerUpEffect);
                _effect.transform.position = transform.position;

                _effect.transform.localScale = new Vector3(1, 1, 1);

                telpMode = false;
                teleportText.gameObject.SetActive(false);
            }
        }
    }
    public void LoadChar()
    {
        if(PlayerPrefs.GetString("Char") == "Healer")
        {
            currentCharachter = healer;
        }else if(PlayerPrefs.GetString("Char") == "Reloder")
        {
            currentCharachter = reloder;

        }
        else if (PlayerPrefs.GetString("Char") == "Tank")
        {
            currentCharachter = tank;

        }
    }

    public void LoadWeapon()
    {
        if (PlayerPrefs.GetString("Weapon") == "Rifle")
        {
            currentWeapon = rifle;
        }
        else if (PlayerPrefs.GetString("Weapon") == "FireBall")
        {
            currentWeapon = fireBall;

        }
        else if (PlayerPrefs.GetString("Weapon") == "Sword")
        {
            currentWeapon = sword;

        }
        else if (PlayerPrefs.GetString("Weapon") == "Hands")
        {
            currentWeapon = hands;

        }
    }
    public void LoadItem()
    {
        if(PlayerPrefs.GetString("Item") == "Stronger")
        {
            currentItem = strongerItem;
            if(amountOfKills == strongerEach20Kills)
            {
                StartCoroutine(UseStrengthPowerUp());
                strongerEach20Kills = amountOfKills + 20;

                audioSource.PlayOneShot(currentPowerUp.usePowerUpAudio);

                ParticleSystem effect = Instantiate(hardening.powerUpEffect);
                effect.transform.position = transform.position;

                effect.transform.localScale = new Vector3(1, 1, 1);

                effect.transform.SetParent(transform);
                StartCoroutine(InfoImages(2, infoItem, currentItem.imgTexture));


            }

        }
        else if(PlayerPrefs.GetString("Item") == "Faster")
        {
            Spawner spawner = GameObject.FindObjectOfType<Spawner>();
            currentItem = fasterItem;

            spawner.amountOfDividePlayer = 250;
        }
        else if (PlayerPrefs.GetString("Item") == "Slower")
        {
            Spawner spawner = GameObject.FindObjectOfType<Spawner>();
            currentItem = slowerItem;

            spawner.amountOfDivideEnemies = 410;
        }
        else if (PlayerPrefs.GetString("Item") == "ExtraCoins")
        {
            currentItem = ExtraCoinsItem;

        }
        else if (PlayerPrefs.GetString("Item") == "ExtraLife")
        {
            currentItem = extraLifeItem;

        }
        else if (PlayerPrefs.GetString("Item") == "RelodesFaster")
        {
            currentItem = relodesPowerFasterItem;

        }

        if(PlayerPrefs.GetString("Item") != "ExtraLife" && PlayerPrefs.GetString("Item") != "Stronger")
        {
            StartCoroutine(InfoImages(1000000000, infoItem, currentItem.imgTexture));

        }

        //Extra Coins function in Coins Func
        //Extra Life In Dead Func
        // Reold faster in startFunction!


    }
    public void LoadPower()
    {
        if (PlayerPrefs.GetString("Power") == "Healing")
        {
            currentPowerUp = healing;
        }
        else if (PlayerPrefs.GetString("Power") == "Shielding")
        {
            currentPowerUp = shield;

        }
        else if (PlayerPrefs.GetString("Power") == "Hardening")
        {
            currentPowerUp = hardening;

        }
        else if (PlayerPrefs.GetString("Power") == "Teleportation")
        {
            currentPowerUp = teleportation;

        }else if(PlayerPrefs.GetString("Power") == "")
        {
            readyText.gameObject.SetActive(false);
            powerUpRelodText.gameObject.SetActive(false);
            currentPowerUp = null;
        }

        else if (PlayerPrefs.GetString("Power") == "ShootAround")
        {
            currentPowerUp = shootProjectileARound;

        }

        else if (PlayerPrefs.GetString("Power") == "BlackHoleShoot")
        {
            currentPowerUp = blackHoleShot;

        }


    }
    public void Healer()
    {
        if(currentCharachter == healer && isPaused == false)
        {
            if(health < currentCharachter.startHealth)
            {
                health += 0.0015f;
            }
        }
    }

    public void Coins()
    {
        if(isDead == true)
        {
            if (PlayerPrefs.GetString("Item") == "ExtraCoins")
            {
                coins = Mathf.RoundToInt(timeSurrvived * 0.35f + amountOfKills * 10);
            }
            else
            {
                coins = Mathf.RoundToInt(timeSurrvived * 0.35f + amountOfKills * 7);
            }

            coinsAmountText.text = coins.ToString();

            PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") + coins);

            if(timeSurrvived > PlayerPrefs.GetFloat("Highscore"))
            {
                PlayerPrefs.SetFloat("Highscore", timeSurrvived);
            }

            print(PlayerPrefs.GetInt("Coins"));
        }
    }
    [System.Serializable]
    public class Weapon
    {

        public float startdamage;
        
        public string name;
        public GameObject slashEffect;
        public float relodTime;
        public bool isMelee;
        public GameObject projectile;
        public AudioClip clashAudio;
        public bool isBoth;
    }

    public void HealthText()
    {
        healthText.text = Mathf.RoundToInt(health) + " / " + currentCharachter.startHealth;
    }

    [System.Serializable]
    public class characher
    {

        public string nameOfCharachter;


        public float startSpeed;

        public float startHealth;

        public Sprite forwardSprite;
        public Sprite backwardSprite;
        public Sprite rightSprite;
        public Sprite leftSprite;

    }

    [System.Serializable]

    public class PowerUp
    {

        public string nameOfPowerUp;

        public float timer;

        public ParticleSystem powerUpEffect;

        public float relodTime;

        public GameObject projectilePrefab;

        public AudioClip usePowerUpAudio;

        public bool isOn;


        public Texture imgTexture;
    }


    public void UsePowerUp()
    {
        if(currentPowerUp.timer > 0)
        {
            powerUpRelodText.text = Mathf.RoundToInt(currentPowerUp.timer).ToString();
            powerUpRelodText.gameObject.SetActive(true);

            if (PlayerPrefs.GetString("Device") == "Phone")
            {
                readyButtonPowerUp.SetActive(false);
            }
        }
        else if((currentPowerUp.timer <= 0))
        {
            if(PlayerPrefs.GetString("Device") == "Computer")
            {
                powerUpRelodText.text = "Ready!";
            }else if(PlayerPrefs.GetString("Device") == "Phone")
            {
                powerUpRelodText.gameObject.SetActive(false);
                readyButtonPowerUp.SetActive(true);
            }
        }

        if (currentPowerUp.timer > 0)
        {
            currentPowerUp.timer -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.V) && currentPowerUp.timer <= 0)
        {
                
                currentPowerUp.timer = currentPowerUp.relodTime;
                audioSource.PlayOneShot(currentPowerUp.usePowerUpAudio);

                ParticleSystem effect = Instantiate(currentPowerUp.powerUpEffect);
                effect.transform.position = transform.position;

                effect.transform.localScale = new Vector3(1, 1, 1);

                currentEffect = effect;

            
            if(currentPowerUp != null && currentPowerUp != hardening && currentPowerUp != shield)
            {
                StartCoroutine(InfoImages(2,infoPowerup,currentPowerUp.imgTexture));
            }else if(currentPowerUp == hardening || currentPowerUp == shield)
            {
                StartCoroutine(InfoImages(10,infoPowerup,currentPowerUp.imgTexture));

            }

            if(currentPowerUp == blackHoleShot)
            {
                infoPowerup.transform.localScale = new Vector3(0.46f, 0.46f, 0.46f);
            }


            if (currentPowerUp.nameOfPowerUp == "Healing")
            {
                float differnce = currentCharachter.startHealth - health;

                GameObject _dmgTextCanvas = Instantiate(dmgTextCanvasPrefab);
                _dmgTextCanvas.transform.position = transform.position;


                if (currentCharachter.startHealth <= 100)
                {
                    health += differnce;

                    _dmgTextCanvas.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "+" + Mathf.RoundToInt(differnce);

                }

                if (currentCharachter.startHealth >100 && differnce >= 100)
                {
                    health += 100;
                    _dmgTextCanvas.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "+100";

                }
                else if(currentCharachter.startHealth > 100 && differnce < 100)
                {
                    health += differnce;
                    _dmgTextCanvas.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "+" + Mathf.RoundToInt(differnce);
                }
            }
            else if(currentPowerUp.nameOfPowerUp == "Hardening")
            {
                print('a');
                effect.transform.SetParent(transform);
                StartCoroutine(UseStrengthPowerUp());
            }
            else if (currentPowerUp.nameOfPowerUp == "Teleportation")
            {
                Vector2 screenPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                screenPos.x = Mathf.Clamp(screenPos.x, -13, 13);
                screenPos.y = Mathf.Clamp(screenPos.y, -7, 5);

                transform.position = screenPos;

                ParticleSystem _effect = Instantiate(currentPowerUp.powerUpEffect);
                _effect.transform.position = transform.position;

                _effect.transform.localScale = new Vector3(1, 1, 1);
            }
            else if (currentPowerUp.nameOfPowerUp == "Shield")
            {
                StartCoroutine(UseShieldPowerUp());
                effect.transform.SetParent(transform);

            }
            else if (currentPowerUp.nameOfPowerUp == "ShootAround")
            {

                GameObject projectile = Instantiate(currentPowerUp.projectilePrefab);
                projectile.transform.position = transform.position;
            }
            else if (currentPowerUp.nameOfPowerUp == "BlackHoleShoot" && PlayerPrefs.GetString("Device") == "Computer")
            {

                Vector2 screenPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);



                Vector2 direction = screenPos - new Vector2(transform.position.x, transform.position.y);


                GameObject projectile = Instantiate(currentPowerUp.projectilePrefab);

                projectile.transform.position = transform.position;

                projectile.transform.GetComponent<Rigidbody2D>().AddForce(direction * 2.5f, ForceMode2D.Impulse);

                projectile.transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);
            }
        }

        
       
    }

    public void UsePowerUpForMobile()
    {
        if(currentPowerUp != teleportation && currentPowerUp != blackHoleShot)
            {
            currentPowerUp.timer = currentPowerUp.relodTime;
            audioSource.PlayOneShot(currentPowerUp.usePowerUpAudio);

            ParticleSystem effect = Instantiate(currentPowerUp.powerUpEffect);
            effect.transform.position = transform.position;

            effect.transform.localScale = new Vector3(1, 1, 1);

            currentEffect = effect;
        }


        if (currentPowerUp.nameOfPowerUp == "Healing")
        {
            float differnce = currentCharachter.startHealth - health;

            if (currentCharachter.startHealth <= 100)
            {
                health += differnce;
            }

            if (currentCharachter.startHealth > 100 && differnce >= 100)
            {
                health += 100;

            }
            else if (currentCharachter.startHealth > 100 && differnce < 100)
            {
                health += differnce;
            }
        }
        else if (currentPowerUp.nameOfPowerUp == "Hardening")
        {
            currentEffect.transform.SetParent(transform);
            StartCoroutine(UseStrengthPowerUp());
        }
        else if (currentPowerUp.nameOfPowerUp == "Teleportation")
        {
            telpMode = true;
            
        }
        else if (currentPowerUp.nameOfPowerUp == "Shield")
        {
            StartCoroutine(UseShieldPowerUp());
            currentEffect.transform.SetParent(transform);

        }

        else if (currentPowerUp.nameOfPowerUp == "BlackHoleShoot")
        {
            currentPowerUp.isOn = true;
        }
    }
    public void KickEnemiesOutsideShield()
    {
        Collider2D[] collision2Ds = Physics2D.OverlapCircleAll(transform.position, 1.5f);

        for (int i = 0; i < collision2Ds.Length; i++)
        {
            if(!collision2Ds[i].CompareTag("Collider") && !collision2Ds[i].CompareTag("Projectile"))
            {
                if (collision2Ds[i].transform.position.y > transform.position.y)
                {
                    collision2Ds[i].transform.position = Vector2.Lerp(collision2Ds[i].transform.position, new Vector2(collision2Ds[i].transform.position.x, collision2Ds[i].transform.position.y + 1.5f), 0.5f);
                }
                else if (collision2Ds[i].transform.position.y < transform.position.y)
                {
                    collision2Ds[i].transform.position = Vector2.Lerp(collision2Ds[i].transform.position, new Vector2(collision2Ds[i].transform.position.x, collision2Ds[i].transform.position.y - 1.5f), 0.5f);

                }

                if (collision2Ds[i].transform.position.x > transform.position.x)
                {
                    collision2Ds[i].transform.position = Vector2.Lerp(collision2Ds[i].transform.position, new Vector2(collision2Ds[i].transform.position.x + 1.5f, collision2Ds[i].transform.position.y), 0.5f);

                }
                else if (collision2Ds[i].transform.position.x < transform.position.x)
                {
                    collision2Ds[i].transform.position = Vector2.Lerp(collision2Ds[i].transform.position, new Vector2(collision2Ds[i].transform.position.x - 1.5f, collision2Ds[i].transform.position.y), 0.5f);

                }
            }
            

        }
    }
    public IEnumerator UseStrengthPowerUp()
    {
        damage = currentWeapon.startdamage + 40;
        spriteRenderer.color = new Color(195,255,0);
        yield return new WaitForSeconds(10f);
        damage = currentWeapon.startdamage;
        spriteRenderer.color = new Color(255, 255, 255);

    }

    public IEnumerator UseShieldPowerUp()
    {
        shield.isOn = true;
        spriteRenderer.color = new Color(0, 255, 0);

        yield return new WaitForSeconds(10f);
        shield.isOn = false;
        spriteRenderer.color = new Color(255, 255, 255);


    }
    public void TimeSurrvived()
    {
        if(isDead == false)
        {
            timeSurrvived += Time.deltaTime;
            timeSurrvivedTextIn.text = Mathf.RoundToInt(timeSurrvived).ToString();
            timeSurrvivedTextInLoseMenu.text = Mathf.RoundToInt(timeSurrvived).ToString();

        }
    }
    public void Lose()
    {
        if(isDead == true)
        {
            loseMenu.SetActive(true);
            killsAmountTextInLoseMenu.text = amountOfKills.ToString();
        }
    }
    public void KillsAmountText()
    {
        killsAmountText.text = amountOfKills.ToString();
    }

    public void EnemyHealthSlider()
    {
        if(currentEnemy == null)
        {
            enemyHealthText.text = "0";
        }
        if (currentEnemy != null)
        {

            
            enemyHealthText.text = currentEnemy.GetComponent<Enemy>().health.ToString();
            

            enemyHealthSlider.maxValue = currentEnemy.GetComponent<Enemy>().startHealth;

            enemyHealthSlider.value = currentEnemy.GetComponent<Enemy>().health;
          

        }

        if (enemyHealthTimer > 0)
        {
            enemyHealthTimer -= Time.deltaTime;
            enemyHealthSlider.gameObject.SetActive(true);
        }else
        {
            enemyHealthSlider.gameObject.SetActive(false);
        }
    }

    public void RelodText()
    {
        if(delay <= 0)
        {
            relodText.gameObject.SetActive(false);
        }else
        {
            relodText.gameObject.SetActive(true);
        }
        relodText.text = Mathf.RoundToInt(delay).ToString();
    }
    public void HealthSlider()
    {
        healthSlider.value = health;
    }
    public void PauseMenu()
    {
        if(Input.GetKeyDown(KeyCode.P) && isPaused == false && pauseMenu.activeSelf == false && isDead == false)
        {
            isPaused = true;
            pauseMenu.SetActive(true);
        }else if(Input.GetKeyDown(KeyCode.P) && isPaused == true && pauseMenu.activeSelf == true )
        {
            buttonManagerGamePlay.Resume();
        }
    }

    public void TimeScale()
    {
        if (isPaused == true)
        {
            Time.timeScale = 0;
        }else if(isPaused == false)
        {
            Time.timeScale = 1;
        }else if(isDead == true)
        {
            Time.timeScale = 0;
        }
    }
     public void Move()
     {
       if(PlayerPrefs.GetString("Device") == "Computer")
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");

            Vector2 move = Vector2.right * (horizontal * speed) + Vector2.up * (vertical * speed);

            rb2D.velocity = move;
        }else if (PlayerPrefs.GetString("Device") == "Phone")
        {
            move.x = fixedJoystick.Horizontal;
            move.y = fixedJoystick.Vertical;

            Vector2 _move = Vector2.right * (move.x * speed) + Vector2.up * (move.y * speed);

            rb2D.velocity = _move;

        }




    }

    public void Animation()
    {
        
            

        if (PlayerPrefs.GetString("Device") == "Computer")
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            if (horizontal >= 1)
            {
                spriteRenderer.sprite = currentCharachter.rightSprite;
            }
            else if (horizontal <= -1)
            {
                spriteRenderer.sprite = currentCharachter.leftSprite;

            }

            if (vertical >= 1)
            {
                spriteRenderer.sprite = currentCharachter.forwardSprite;

            }
            else if (vertical <= -1)
            {
                spriteRenderer.sprite = currentCharachter.backwardSprite;
            }
        }
        else if (PlayerPrefs.GetString("Device") == "Phone")
        {
            float horizontal = CrossPlatformInputManager.GetAxisRaw("Horizontal");
            float vertical = CrossPlatformInputManager.GetAxisRaw("Vertical");
            if (move.x >= 0.1)
            {
                spriteRenderer.sprite = currentCharachter.rightSprite;
            }
             if (move.x <= -0.1)
            {
                spriteRenderer.sprite = currentCharachter.leftSprite;

            }

            if (move.y >= 0.1)
            {
                spriteRenderer.sprite = currentCharachter.forwardSprite;

            }
             if (move.y <= -0.1)
            {
                spriteRenderer.sprite = currentCharachter.backwardSprite;
            }
        }

    }

    public void Death()
    {
        if(health <= 0)
        {
            if(PlayerPrefs.GetString("Item") == "ExtraLife" && didUseExtraLifeChance == false)
            {
                health = currentCharachter.startHealth;
                didUseExtraLifeChance = true;
                StartCoroutine(InfoImages(2, infoItem, currentItem.imgTexture));

                UseEffect(extraLifeItem.effect, extraLifeItem.soundEffect);
            }
            if(PlayerPrefs.GetString("Item") != "ExtraLife"  && health <= 0)
            {
                Destroy(gameObject);
                isDead = true;
            }

            if (PlayerPrefs.GetString("Item") == "ExtraLife" && health <= 0 && didUseExtraLifeChance == true)
            {
                Destroy(gameObject);
                isDead = true;
            }
        }
    }
    public void DamageEnemies()
    {
        Collider2D[] collision2Ds = Physics2D.OverlapCircleAll(transform.position, 1.5f);

        if (currentWeapon.isMelee == true && currentWeapon.isBoth == false)
        {
            if (delay > 0)
            {
                delay -= Time.deltaTime;
            }
            for (int i = 0; i < collision2Ds.Length; i++)
            {
                if (collision2Ds[i].CompareTag("Enemy") && delay <= 0)
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        delay = currentWeapon.relodTime;

                        enemyHealthTimer = enemyHealthSliderDelay;

                        audioSource.PlayOneShot(currentWeapon.clashAudio);
                        collision2Ds[i].GetComponent<Enemy>().health -= damage;
                        GameObject slashEffect = Instantiate(currentWeapon.slashEffect);
                        slashEffect.transform.position = collision2Ds[i].transform.position;

                        currentEnemy = collision2Ds[i].gameObject;

                        GameObject _dmgTextCanvas = Instantiate(dmgTextCanvasPrefab);
                        _dmgTextCanvas.transform.position = collision2Ds[i].gameObject.transform.position;

                        _dmgTextCanvas.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "-" + damage.ToString();

                        if (collision2Ds[i].transform.position.y > transform.position.y)
                        {
                            collision2Ds[i].transform.position = Vector2.Lerp(collision2Ds[i].transform.position, new Vector2(collision2Ds[i].transform.position.x, collision2Ds[i].transform.position.y + 1.5f), 0.5f);
                        }
                        else if (collision2Ds[i].transform.position.y < transform.position.y)
                        {
                            collision2Ds[i].transform.position = Vector2.Lerp(collision2Ds[i].transform.position, new Vector2(collision2Ds[i].transform.position.x, collision2Ds[i].transform.position.y - 1.5f), 0.5f);

                        }

                        if (collision2Ds[i].transform.position.x > transform.position.x)
                        {
                            collision2Ds[i].transform.position = Vector2.Lerp(collision2Ds[i].transform.position, new Vector2(collision2Ds[i].transform.position.x + 1.5f, collision2Ds[i].transform.position.y), 0.5f);

                        }
                        else if (collision2Ds[i].transform.position.x < transform.position.x)
                        {
                            collision2Ds[i].transform.position = Vector2.Lerp(collision2Ds[i].transform.position, new Vector2(collision2Ds[i].transform.position.x - 1.5f, collision2Ds[i].transform.position.y), 0.5f);

                        }
                    }
                }
            }
        }
        else if (currentWeapon.isMelee == false && currentWeapon.isBoth == false)
        {
            if (delay > 0)
            {
                delay -= Time.deltaTime;
            }

            if (Input.GetMouseButtonDown(0) && delay <= 0)
            {
                delay = currentWeapon.relodTime;


                audioSource.PlayOneShot(currentWeapon.clashAudio);

                Vector2 screenPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);



                Vector2 direction = screenPos - new Vector2(transform.position.x, transform.position.y);

               

                GameObject projectile = Instantiate(currentWeapon.projectile);

                projectile.transform.position = transform.position;

                projectile.transform.GetComponent<Rigidbody2D>().AddForce(direction * 2.5f, ForceMode2D.Impulse);

                projectile.transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);


            }
        }
        if (currentWeapon.isBoth == true)
        {
            if (delay > 0)
            {
                delay -= Time.deltaTime;
            }
            for (int i = 0; i < collision2Ds.Length; i++)
            {
                if (collision2Ds[i].CompareTag("Enemy") && delay <= 0)
                {
                    if (Input.GetMouseButtonDown(0) && delay <= 0)
                    {
                        delay = currentWeapon.relodTime;

                        enemyHealthTimer = enemyHealthSliderDelay;
                        audioSource.PlayOneShot(hands.clashAudio);
                        collision2Ds[i].GetComponent<Enemy>().health -= damage;
                        GameObject slashEffect = Instantiate(hands.slashEffect);
                        slashEffect.transform.position = collision2Ds[i].transform.position;

                        currentEnemy = collision2Ds[i].gameObject;

                        GameObject _dmgTextCanvas = Instantiate(dmgTextCanvasPrefab);
                        _dmgTextCanvas.transform.position = collision2Ds[i].gameObject.transform.position;

                        _dmgTextCanvas.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "-" + damage.ToString();

                        if (collision2Ds[i].transform.position.y > transform.position.y)
                        {
                            collision2Ds[i].transform.position = Vector2.Lerp(collision2Ds[i].transform.position, new Vector2(collision2Ds[i].transform.position.x, collision2Ds[i].transform.position.y + 1.5f), 0.5f);
                        }
                        else if (collision2Ds[i].transform.position.y < transform.position.y)
                        {
                            collision2Ds[i].transform.position = Vector2.Lerp(collision2Ds[i].transform.position, new Vector2(collision2Ds[i].transform.position.x, collision2Ds[i].transform.position.y - 1.5f), 0.5f);

                        }

                        if (collision2Ds[i].transform.position.x > transform.position.x)
                        {
                            collision2Ds[i].transform.position = Vector2.Lerp(collision2Ds[i].transform.position, new Vector2(collision2Ds[i].transform.position.x + 1.5f, collision2Ds[i].transform.position.y), 0.5f);

                        }
                        else if (collision2Ds[i].transform.position.x < transform.position.x)
                        {
                            collision2Ds[i].transform.position = Vector2.Lerp(collision2Ds[i].transform.position, new Vector2(collision2Ds[i].transform.position.x - 1.5f, collision2Ds[i].transform.position.y), 0.5f);

                        }
                    }
                }else if (Input.GetMouseButtonDown(1) && delay <= 0)
                {
                    if(currentCharachter == reloder)
                    {
                        delay = currentWeapon.relodTime + 5 * 0.52f;

                    }
                    else
                    {
                        delay = currentWeapon.relodTime + 5;
                    }


                    audioSource.PlayOneShot(currentWeapon.clashAudio);

                    Vector2 screenPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                    

                    Vector2 direction = screenPos - new Vector2(transform.position.x, transform.position.y);

                    


                    GameObject projectile = Instantiate(currentWeapon.projectile);

                    projectile.transform.position = transform.position;

                    projectile.transform.GetComponent<Rigidbody2D>().AddForce(direction * 2.5f, ForceMode2D.Impulse);

                    projectile.transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);
                }
            }

        }

        

    }
    public IEnumerator Freeze()
    {
        rb2D.simulated = false;
        yield return new WaitForSeconds(4f);
        rb2D.simulated = true;
        isFreezed = false;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Heart"))
        {
            GameObject _dmgTextCanvas = Instantiate(dmgTextCanvasPrefab);
            _dmgTextCanvas.transform.position = transform.position;
          
            float differnce = currentCharachter.startHealth - health;

            
            if (differnce >= 25)
            {
                health += 25;
               
                _dmgTextCanvas.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "+25" ;
                
            }
            else if(differnce < 25)
            {
                float healLeft = currentCharachter.startHealth - health;
                health += healLeft;
                _dmgTextCanvas.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "+" + Mathf.RoundToInt(healLeft);

            }
            if (health == currentCharachter.startHealth)
            {
                _dmgTextCanvas.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "+0";
            }
        }
       

    }
   

}
