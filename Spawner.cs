using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(SetCurrentToSoundEffectsVolume))]

public class Spawner : MonoBehaviour
{
    [Header("Enemies")]
    public __Enemy iceEnemy;
    public __Enemy blackHoleEnemy;
    public __Enemy smallEnemy;
    public __Enemy verySmallEnemy;
    public __Enemy teleportEnemy;
    public __Enemy giantEnemy;
    public __Enemy giantDiesSpawnsEnemy;
    public __Enemy motherEnemy;

    public Enemy[] enemies;

    private __Enemy[] enemiesType;

    [Header("Powerups")]
    public GameObject heart;

    [Header("Audio")]

    private AudioSource audioSource;
    public AudioClip respawnAudioEffect;

    private PlayerMovement player;

    [HideInInspector]
    public float fullSpeedOfPlayer;
    public float amountOfDividePlayer;
    public float amountOfDivideEnemies;

    // Start is called before the first frame update
    void Start()
    {
        enemiesType = new __Enemy[] { blackHoleEnemy, smallEnemy, verySmallEnemy, teleportEnemy, giantEnemy, giantDiesSpawnsEnemy, motherEnemy,iceEnemy };

        for (int i = 0; i < enemiesType.Length; i++)
        {
          
            enemiesType[i].delay = enemiesType[i].spawnSpeedMinMax.x;
        }

        player = GameObject.FindObjectOfType<PlayerMovement>();

        fullSpeedOfPlayer = player.currentCharachter.startSpeed + 6;
        amountOfDividePlayer = 350;
        amountOfDivideEnemies = 380;
    }

    // Update is called once per frame
    void Update()
    {
        enemies = GameObject.FindObjectsOfType<Enemy>();

        audioSource = GetComponent<AudioSource>();

        SpawnerSystem();

        player.speed = Mathf.Lerp(player.currentCharachter.startSpeed, fullSpeedOfPlayer, Time.timeSinceLevelLoad / amountOfDividePlayer);

    }
    public void SpawnerSystem()
    {

        for (int i = 0; i < enemiesType.Length; i++)
        {

            enemiesType[i].spawnSpeed = Mathf.Lerp(enemiesType[i].spawnSpeedMinMax.x, enemiesType[i].spawnSpeedMinMax.y, Time.timeSinceLevelLoad / 300);
            enemiesType[i].speed = Mathf.Lerp(enemiesType[i].speedMinMax.x, enemiesType[i].speedMinMax.y, Time.timeSinceLevelLoad / amountOfDivideEnemies);

            if (enemiesType[i].delay > 0)
            {
                enemiesType[i].delay -= Time.deltaTime;
            }

            if (enemiesType[i].delay <= 0 && enemies.Length < 30)
            {
                audioSource.PlayOneShot(respawnAudioEffect);
                enemiesType[i].delay = enemiesType[i].spawnSpeed;

                GameObject enemy = Instantiate(enemiesType[i].enemyPrefab);

                float positionIndex = Random.Range(0, 5);

                enemy.gameObject.GetComponent<Enemy>().speed = enemiesType[i].speed;

                if (positionIndex == 1)
                {
                    enemy.transform.position = new Vector2(15, 0);
                }
                else if (positionIndex == 2)
                {
                    enemy.transform.position = new Vector2(-15, 0);
                }
                else if (positionIndex == 3)
                {
                    enemy.transform.position = new Vector2(0, 10);
                }
                else if (positionIndex == 4)
                {
                    enemy.transform.position = new Vector2(0, -10);
                }
                else if (positionIndex == 0)
                {
                    enemy.transform.position = new Vector2(0, 10);
                }
                else if (positionIndex == 5)
                {
                    enemy.transform.position = new Vector2(-15, 0);
                }
            }
        }

        

    }

    [System.Serializable]
    public class __Enemy
    {
        public GameObject enemyPrefab;

        public Vector2 speedMinMax;
        public Vector2 spawnSpeedMinMax;

        public float speed;
        public float spawnSpeed;

        public float delay;

        public float maxOfSameType;
    }
}
