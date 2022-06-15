using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnManager : MonoBehaviour
{
    #region Enemy
    [SerializeField]
    GameObject EnemyToSpawn;
    [SerializeField]
    GameObject EnemyContainer;
    [SerializeField]
    float EnemySpawnTimer = 5;
    [SerializeField]
    int AmountOfEnemiesToSpawn = 5;
    [SerializeField]
    int EnemyCount;
    int EnemiesInPlay;
    public int CurrentCount
    {
        get
        {
            return EnemiesInPlay;
        }
        set
        {
            EnemiesInPlay = value;
            print("Enemy Count is " + EnemyCount);
            if(EnemiesInPlay <= 0)
            {
                StartCoroutine(SpawnCoroutine());
            }
        }
    }
    #endregion
    #region Player
    [SerializeField]
    GameObject PlayerToSpawn;
    GameObject SpawnedPlayer;
    #endregion
    #region PowerUp
    [SerializeField]
    GameObject PowerUpGameObject;
    [SerializeField]
    float PowerUpTimer = 10;
    float PowerUpTime;
    #endregion
    #region Game Timer
    [SerializeField]
    float StartDelayTimer = 6;
    float StartTime;
    #endregion
    #region SpawnManagerUI
    [SerializeField]
    Text[] SpawnUI;

    #endregion

    #region DevNotes
    //the longer the player plays, the harder to game gets


    #endregion
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StartGameCounter());
    }

 

    void SpawnPlayer()
    {
        if (PlayerToSpawn != null)
        {
            SpawnedPlayer = Instantiate(PlayerToSpawn, new Vector3(0, -12, 0), Quaternion.identity);
        }
        else
        {
            Debug.LogError("PlayerToSpawn = null");
        }
    }
    void SpawnEnemy()
    {
        if (EnemyToSpawn == null)
        {
            Debug.LogError("EnemyToSpawn = null");
            return;
        }
        StartCoroutine(SpawnCoroutine());
    }
    void WaveSpawner()
    {

    }
    public void EnemyDeath()
    {
        //make this a property
        if(EnemiesInPlay <=0 && EnemyCount <= 0)
        {
            //next wave
        }
    }
 
    IEnumerator SpawnCoroutine()
    {
        while (EnemyCount > 0 && SpawnedPlayer != null && PlayerValues.PlayerIsDead != true)
        {
            yield return new WaitForSeconds(EnemySpawnTimer);
            GameObject SpawnedEnemy = Instantiate(EnemyToSpawn, new Vector3(Random.Range(-16, 16), 25, 0), Quaternion.identity);
            if (EnemyContainer != null)
            {
                SpawnedEnemy.transform.parent = EnemyContainer.transform;
            }
            if(Time.time > PowerUpTime)
            {
                if(PowerUpGameObject != null)
                {
                    
                    Instantiate(PowerUpGameObject, new Vector3(transform.position.x + Random.Range(-30, 30), 25, 0), Quaternion.identity);
                    PowerUpTime = Time.time + PowerUpTimer;
                }
            }
            EnemyCount--;
            EnemiesInPlay++;

        }
    }
    public IEnumerator StartGameCounter()
    {
        SpawnUI[0].gameObject.SetActive(true);
        SpawnPlayer();
        yield return new WaitForSeconds(2);
        SpawnUI[0].gameObject.SetActive(false);
        SpawnUI[1].gameObject.SetActive(true);
        PowerUpTime = Time.time + PowerUpTimer;
        EnemyCount = AmountOfEnemiesToSpawn;
        SpawnEnemy();
        yield return new WaitForSeconds(2);
        SpawnUI[1].gameObject.SetActive(false);
    }
}
