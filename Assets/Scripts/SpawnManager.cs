using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[System.Serializable]

public class Formation
{
    public bool Type;

    public GameObject CommonEnemy;

    public GameObject EliteEnemy;

    public string CallMethod;

    public Vector3 SpawnPosition;


    public Vector3 MovementDirection;

    public Formation(bool Type, int NumberToSRespawn, GameObject CommonEnemy,GameObject EliteEnemy,string CallMethod, Vector3 SpawnPosition, Vector3 MovementDirection)
    {
        this.Type = Type;
        this.CommonEnemy = CommonEnemy;
        this.EliteEnemy = EliteEnemy;
        this.CallMethod = CallMethod;
        this.SpawnPosition = SpawnPosition;
        this.MovementDirection = MovementDirection;
    }

}

public class SpawnManager : MonoBehaviour
{
    #region Enemy

    [SerializeField]
    GameObject EnemyContainer;

    [SerializeField]
    float EnemySpawnTimer = 5;

    int _enemiesInPlay;

    [SerializeField]
    int _maxNumberToSpawn;

    [SerializeField]
    int _remainder;

    int _currentNumberToSpawn;

    int _eliteCoutner;

    int _numberToRespawn;

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

    #region Obstacle
    [SerializeField]
    GameObject _WarningIndicator;


    [SerializeField]
    GameObject[] _obstaclesToSpawn;

    [SerializeField]
    Vector3 _spawnPosition;

    [SerializeField]
    float _obstacleSpawnDelay;

    float _obstacleTime;

    #endregion

    #region Game Timer
    [SerializeField]
    float StartDelayTimer = 6;
    float StartTime;
    #endregion

    #region SpawnManagerUI
    [SerializeField]
    Text[] SpawnUI;

    [SerializeField]
    Text _gameTimeText;
    #endregion

    #region GameTimer
    float seconds;
    float minutes;
    float hours;
   

    float Seconds
    {
        get
        {
            return seconds;
        }
        set
        {
            seconds = value;
        }
    }
    float Minutes
    {
        get
        {
            minutes = seconds / 5;
            return seconds;
        }
        set
        {
            seconds = value;
            
        }
    }
    float Hours
    {
        get
        {
            return minutes;
        }
        set
        {
            hours= minutes / 60;
        }
    }

    #endregion

    #region Spawn positions

    //initial spawing
    public List<Formation> _formationList = new List<Formation>();

    [SerializeField]
    int _fomartionSelection;


    //respawning
    Vector3 _currentSpawnPosition;


    #endregion

    #region Wave
    int _currentWave;

    #endregion


    // Start is called before the first frame update
    void Start()
    {
            
            _obstacleTime = Time.time + _obstacleSpawnDelay;
            PowerUpTime = Time.time + PowerUpTimer;
            _currentSpawnPosition = _formationList[_fomartionSelection].SpawnPosition;
            StartCoroutine(StartGameCounter());
            
    }

    private void Update()
    {
        if (PlayerValues.PlayerIsDead == false)
        {
           // SpawnPowerUp();
            SpawnObstacles();
        }
    }

    void SpawnPowerUp()
    {
        if (PlayerValues.PlayerIsDead == false)
        {
            if (Time.time > PowerUpTime)
            {
                if (PowerUpGameObject != null)
                {

                    Instantiate(PowerUpGameObject, new Vector3(transform.position.x + Random.Range(-30, 30), 25, 0), Quaternion.identity);

                    PowerUpTime = Time.time + PowerUpTimer;

                }
            }
        }
    }

    void SpawnObstacles()
    {

        //set bool to only call one at a time
        //or start instantiating them instead an call a bunch / X amount

        //this flips on early because its being called constantly by the next object.

        if(Time.time > _obstacleTime * .9f && _WarningIndicator.activeInHierarchy == false)
        {
            _spawnPosition = new Vector3(Random.Range(-60, 55), _spawnPosition.y, _spawnPosition.z);

            _WarningIndicator.transform.position = new Vector2(_spawnPosition.x, 35);

            _WarningIndicator.SetActive(true);
        }

        if(Time.time > _obstacleTime)
        {
            GameObject _newObstacle = Instantiate(_obstaclesToSpawn[0], _spawnPosition , Quaternion.identity);

            ScrollingBackgrounds _obstacleVaryingSpeed = _newObstacle.GetComponent<ScrollingBackgrounds>();

            _obstacleVaryingSpeed.MovementSpeed = Random.Range(50, 70);

            _spawnPosition = new Vector3(Random.Range(-22, 22), _spawnPosition.y, _spawnPosition.z);

            _WarningIndicator.SetActive(false);

            _obstacleTime = Time.time + _obstacleSpawnDelay;
        }
    }

    void SpawnPlayer()
    {
        if (PlayerToSpawn != null)
        {
            SpawnedPlayer = Instantiate(PlayerToSpawn, new Vector3(0, -20, 0), Quaternion.identity);
        }
        else
        {
            Debug.LogError("PlayerToSpawn = null");
        }
    }

    void SpawnEnemy()
    {
        

        if (_formationList[_fomartionSelection].CommonEnemy != null)
        {
            _currentSpawnPosition = _formationList[_fomartionSelection].SpawnPosition;
            
            
                _currentNumberToSpawn = Random.Range(1, _maxNumberToSpawn);

            if(_currentNumberToSpawn > 5)
            {
                _currentNumberToSpawn = 5;
            }

                _remainder = _maxNumberToSpawn - _currentNumberToSpawn;
            
            

            if (_formationList[_fomartionSelection].EliteEnemy != null)
            {
                if (_currentNumberToSpawn == 5)
                {
                    _eliteCoutner += 1;
                    _currentNumberToSpawn -= 1;
                   
                }
            }
        }

        StartCoroutine(SpawnCoroutine(_fomartionSelection));
    }

    void WaveSpawner()
    {
        print("Wave is Done");
        _fomartionSelection = Random.Range(0, _formationList.Count);
        StartCoroutine(WaveSpawnDelay());
    }

    IEnumerator WaveSpawnDelay()
    {
        _currentNumberToSpawn = Random.Range(1, _maxNumberToSpawn);
        _fomartionSelection = Random.Range(0, _formationList.Count);
        _currentSpawnPosition = _formationList[_fomartionSelection].SpawnPosition;
        //wave transition start

        //stage counter = max waves 1% / 50% / 75% / 100% segemnts. round to int
        yield return new WaitForSeconds(3);
        //wave transition end
        StartCoroutine(SpawnCoroutine(_fomartionSelection));
    }
         
    public void ReSpawner(GameObject EnemyGameObject)
    {
        _enemiesInPlay--;
        _remainder++;
        Destroy(EnemyGameObject);
    }

    public void EnemyDeath(GameObject enemyGameobject)
    {
        
        _enemiesInPlay--;

        //if enemies in play = 0 and remainer > 0

        if(_enemiesInPlay == 0 && _currentNumberToSpawn == 0 && _remainder == 0)
        {
            WaveSpawner();
        }
        Destroy(enemyGameobject);
    }

    #region formation spawn behaviors
    void Line()
    {


        //randomize position


        //left right movmeent
        if (Mathf.Sign(_currentSpawnPosition.x) == 0)
        {

            //change the spawnpoint to positive
            //change the move direciton to negative

        }
        else
        {

            //change the spawnpoint to negative
            //change the move direciton to posiive

        }
    }

    void Stagger()
    {

    }
    #endregion

    IEnumerator SpawnCoroutine(int Selection)
    {
        yield return new WaitForSeconds(3);


        //formation call methods
        Invoke(_formationList[_fomartionSelection].CallMethod, 0);

      
        //regular enemy spawn
        while (_currentNumberToSpawn > 0 && SpawnedPlayer != null && PlayerValues.PlayerIsDead != true)
        {
            yield return new WaitForSeconds(EnemySpawnTimer);

            GameObject SpawnedEnemy = Instantiate(_formationList[Selection].CommonEnemy, _currentSpawnPosition, Quaternion.identity);

            //injection
            Enemy _spawnedEnemyScript = SpawnedEnemy.GetComponent<Enemy>();

            _spawnedEnemyScript.MveDirection = _formationList[Selection].MovementDirection;

            _spawnedEnemyScript._spwnManager = this.GetComponent<SpawnManager>();

            _spawnedEnemyScript.FormationID = Selection;

            _spawnedEnemyScript.Down = _formationList[Selection].Type;

            if (EnemyContainer != null)
            {

                SpawnedEnemy.transform.parent = EnemyContainer.transform;

            }

            _currentNumberToSpawn--;

            _enemiesInPlay++;
        }

        //spawn elite
        if(_currentNumberToSpawn == 0 && _eliteCoutner > 0)
        {
           

            if(_formationList[_fomartionSelection].CallMethod == "Line")
            {
                GameObject _instantiatedEliteEnemy = Instantiate(_formationList[_fomartionSelection].EliteEnemy, _currentSpawnPosition, Quaternion.identity);

                //injection
                Enemy _spawnedEnemyScript =_instantiatedEliteEnemy.GetComponent<Enemy>();

                _spawnedEnemyScript.MveDirection = _formationList[Selection].MovementDirection;

                _spawnedEnemyScript._spwnManager = this.GetComponent<SpawnManager>();

                _spawnedEnemyScript.FormationID = Selection;

                _spawnedEnemyScript.Down = _formationList[Selection].Type;

                _enemiesInPlay++;
            }

            
        }


        //continue to spawn until _currentnumbertospawn = 0
        if (_remainder > 0)
        {
            print("spawn again");

            _fomartionSelection = Random.Range(0, _formationList.Count);

            _currentNumberToSpawn = Random.Range(1, _remainder);
            if(_currentNumberToSpawn > 5)

            {
                _currentNumberToSpawn = 5;

                if (_formationList[_fomartionSelection].EliteEnemy != null)
                {
                    if (_currentNumberToSpawn == 5)
                    {
                        _eliteCoutner += 1;
                        _currentNumberToSpawn -= 1;

                    }
                }

            }
            _remainder -= _currentNumberToSpawn;

            _currentSpawnPosition = _formationList[_fomartionSelection].SpawnPosition;
            StartCoroutine(SpawnCoroutine(_fomartionSelection));
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

        SpawnEnemy();

        yield return new WaitForSeconds(2);

        SpawnUI[1].gameObject.SetActive(false);
    }

}
