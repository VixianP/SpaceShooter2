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

    public string Description;

    public Vector3 SpawnPosition;


    public Vector3 MovementDirection;

    public Formation(bool Type, int NumberToSRespawn, GameObject CommonEnemy,GameObject EliteEnemy,string CallMethod,string Description, Vector3 SpawnPosition, Vector3 MovementDirection)
    {
        this.Type = Type;
        this.CommonEnemy = CommonEnemy;
        this.EliteEnemy = EliteEnemy;
        this.CallMethod = CallMethod;
        this.Description = Description;
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
    int _enemiesInPlay;

    [SerializeField]
    int _maxNumberToSpawn; 

    [SerializeField]
    int _spawnPool; //how many that can be currently spawned in this wave

    [SerializeField]
    int _currentNumberToSpawn; // how many that can be spawned per set


    //elites
    [SerializeField]
    int _maxElites;

    int _elitesToSpawn;

    int _eliteCoutner;

    int _numberToRespawn;


    //boss
    bool _isBoss;


    //waves
    [SerializeField]
    int _maxWaves;

    [SerializeField]
    int _currentWave;

    [SerializeField]
    int _enemyWaveIncrement;

    int _beginLevel;
    int _midLevel;


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


    //Game Start Delay
    [SerializeField]
    float StartDelayTimer = 6;
    float StartTime;

    //elite CD
    [SerializeField]
    float _eliteSpawnCoolDown = 20;

    float _eliteSpawntimer;

    //Special Spawn timers
    float _SpecialSpawnCoolDown = 20;

    float _Speicaltime;

    //interval bettween regular enemy spawn
    [SerializeField]
    float EnemySpawnTimer = 5;
    #endregion

    #region SpawnManagerUI
    [SerializeField]
    Text[] SpawnUI;

    [SerializeField]
    Text _gameTimeText;
    #endregion

    #region GameClock

    //ror mechanic
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
            hours = minutes / 60;
        }
    }

    #endregion

    #region Spawn positions

    //the list of enemies to spawn
    public List<Formation> _formationList = new List<Formation>();

    //selects from list of enemies to spawn. It will always begin at 0.
    [SerializeField]
    int _fomartionSelection;

    //respawning
    Vector3 _currentSpawnPosition;

    #endregion


    void Start()
    {

        _obstacleTime = Time.time + _obstacleSpawnDelay;
        PowerUpTime = Time.time + PowerUpTimer;
        _eliteSpawntimer = Time.time + _eliteSpawnCoolDown;
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

        if (Time.time > _obstacleTime * .9f && _WarningIndicator.activeInHierarchy == false)
        {
            _spawnPosition = new Vector3(Random.Range(-60, 55), _spawnPosition.y, _spawnPosition.z);

            _WarningIndicator.transform.position = new Vector2(_spawnPosition.x, 35);

            _WarningIndicator.SetActive(true);
        }

        if (Time.time > _obstacleTime)
        {
            GameObject _newObstacle = Instantiate(_obstaclesToSpawn[0], _spawnPosition, Quaternion.identity);

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

    //initial start of the game spawn
    void SpawnEnemy()
    {
        RerollSet();

        StartCoroutine(SpawnCoroutine(_fomartionSelection));
    }

    void SpawnElite(int Selection)
    {

        if (_formationList[_fomartionSelection].CallMethod == "Line")
        {
            GameObject _instantiatedEliteEnemy = Instantiate(_formationList[_fomartionSelection].EliteEnemy, _currentSpawnPosition, Quaternion.identity);

            //injection
            Enemy _spawnedEnemyScript = _instantiatedEliteEnemy.GetComponent<Enemy>();

            _spawnedEnemyScript.MveDirection = _formationList[Selection].MovementDirection;

            _spawnedEnemyScript._spwnManager = this.GetComponent<SpawnManager>();

            _spawnedEnemyScript.FormationID = Selection;

            _spawnedEnemyScript.Down = _formationList[Selection].Type;

            _enemiesInPlay++;
            _eliteCoutner++;
            _elitesToSpawn--;
            _eliteSpawntimer = Time.time + _eliteSpawnCoolDown;
        }

        if (_formationList[_fomartionSelection].CallMethod == "Stagger")
        {

            GameObject _instantiatedEliteEnemy = Instantiate(_formationList[_fomartionSelection].EliteEnemy, _currentSpawnPosition, Quaternion.identity);

            _enemiesInPlay++;
            _eliteCoutner++;
            _elitesToSpawn--;
            _eliteSpawntimer = Time.time + _eliteSpawnCoolDown;

        }

        _eliteSpawntimer = Time.time + _eliteSpawnCoolDown;

    }

    void SpecialSpawn()
    {


        if (Time.time > _Speicaltime && _isBoss == false)
        {
            if (_formationList[_fomartionSelection].CallMethod == "Special")
            {
                print("spawn special");
                _currentNumberToSpawn = 0;
                _currentSpawnPosition = _formationList[_fomartionSelection].SpawnPosition;

                GameObject _instantiatedEliteEnemy = Instantiate(_formationList[_fomartionSelection].EliteEnemy, _currentSpawnPosition, Quaternion.identity);

                _enemiesInPlay++;

                _Speicaltime = Time.time + _SpecialSpawnCoolDown;

            }
        } else if (Time.time < _Speicaltime && _isBoss == false)
        {
            RerollSet();
        }

        if(_isBoss == true)
        {

        }

    }

    //starts a new wave
    void WaveSpawner()
    {
        if (_currentWave < _maxWaves)
        {
            print("Wave is Done");
            _currentWave++;

            _maxNumberToSpawn += _enemyWaveIncrement;
            _spawnPool = 0;
            _currentNumberToSpawn = 0;

            RerollSet();

            StartCoroutine(SpawnCoroutine(_fomartionSelection));
        }

        if(_currentWave > _maxWaves)
        {
            _currentWave = _maxWaves;
        }

        if(_currentWave == _maxWaves)
        {
            _spawnPool = 0;
            _currentNumberToSpawn = 0;
            print("End Reached");
        }

        //wave transition start

        //stage counter = max waves 1% / 50% / 75% / 100% segemnts. round to int
        //wave transition end


        //if currentwave > maxwave
        //call boss

    }

    //randomizes and choose from spawning list (formation list)
    void RerollSet() 
    {

        //if its a fresh wave
        if ( _spawnPool == 0)
        {
            //set
            _spawnPool = _maxNumberToSpawn;

            //selects from list of enemies to spawn
            _fomartionSelection = Random.Range(0, _formationList.Count);

        

            if (_formationList[_fomartionSelection].CallMethod == "Special"  && Time.time > _Speicaltime)
            {
                print("called");
                SpecialSpawn();
                return;
            }
            else
            {
                //selects from list of enemies to spawn
                _fomartionSelection = Random.Range(0, _formationList.Count - Random.Range(1,_formationList.Count));
                _currentSpawnPosition = _formationList[_fomartionSelection].SpawnPosition;

                if (_formationList[_fomartionSelection].CommonEnemy != null)
                {

                    _currentNumberToSpawn = Random.Range(1, _maxNumberToSpawn);

                    //max amount of enemies to spawn at once is 5.
                    if (_currentNumberToSpawn > 5)
                    {
                        _currentNumberToSpawn = 5;
                    }


                    //if it rolls 5, itll spawn an elite too
                    if (_formationList[_fomartionSelection].EliteEnemy != null && Time.time > _eliteSpawntimer)
                    {
                        if (_currentNumberToSpawn == 5)
                        {
                            if (_eliteCoutner < _maxElites)
                            {
                                _elitesToSpawn++;
                                _currentNumberToSpawn -= 1;
                            }
                        }
                    }
                }
            }


            //keeps track of the amount of enemies that can be spawned for this wave.
            _spawnPool -= _currentNumberToSpawn;

            StartCoroutine(SpawnCoroutine(_fomartionSelection));

        }
        else if (_spawnPool > 0 ) //if its not a fresh wave
        {
            //selects from list of enemies to spawn

            if (_formationList[_fomartionSelection].CallMethod == "Special" && Time.time > _Speicaltime)
            {
                print("called");
                SpecialSpawn();
                return;
            }
            else
            {
                _fomartionSelection = Random.Range(0, _formationList.Count - 1);
                _currentNumberToSpawn = Random.Range(1, _maxNumberToSpawn);
                _currentSpawnPosition = _formationList[_fomartionSelection].SpawnPosition;

                if (_formationList[_fomartionSelection].CommonEnemy != null)
                {

                    //max amount of enemies to spawn at once is 5.
                    if (_currentNumberToSpawn > 5)
                    {
                        _currentNumberToSpawn = 5;
                    }


                    //if it rolls 5, itll spawn an elite too
                    if (_formationList[_fomartionSelection].EliteEnemy != null && Time.time > _eliteSpawntimer)
                    {
                        if (_currentNumberToSpawn == 5)
                        {
                            if (_eliteCoutner < _maxElites)
                            {
                                _elitesToSpawn++;
                                _currentNumberToSpawn -= 1;
                            }
                        }
                    }
                }

            }

            //keeps track of the amount of enemies that can be spawned for this wave.
            _spawnPool -= _currentNumberToSpawn;

        }

        StartCoroutine(SpawnCoroutine(_fomartionSelection));
    }

    //called when enemies go out of bounds
    public void ReSpawner(GameObject EnemyGameObject,bool Elite)
    {
        _enemiesInPlay--;
        _spawnPool++;
        Destroy(EnemyGameObject);

        if (_enemiesInPlay == 0 && _currentNumberToSpawn == 0 && _spawnPool > 0)
        {
            RerollSet();
        }

    }

    public void EnemyDeath(GameObject enemyGameobject, bool Elite)
    {

        _enemiesInPlay--;

        if (Elite == true)
        {
            _eliteCoutner--;
            _elitesToSpawn++;

            if (_enemiesInPlay == 0  && _spawnPool > 0)
            {
                print("special dead, rerolling");
                RerollSet();
            }

        }

        Destroy(enemyGameobject);

        if (_currentNumberToSpawn < 0)
        {
            _currentNumberToSpawn = 0;
            print(_currentNumberToSpawn + " Correction");
        }

        if (_spawnPool < 0)
        {
            _spawnPool = 0;
        }

        if(_enemiesInPlay < 0)
        {
            _enemiesInPlay = 0;
        }

        if (_enemiesInPlay == 0 && _currentNumberToSpawn == 0 && _spawnPool > 0)
        {
         

            RerollSet();
        }

        if (_enemiesInPlay == 0 && _currentNumberToSpawn == 0 && _spawnPool <= 0)
        {

            WaveSpawner();
        }

        //first check if _currentnumbertospawn is zero!

        // *special elite situation*
        //if enemies in play is zero, but _currentNumberToSpawn is greater than zero, or remainder not equal to zero
        //set new formation selection
        //spawn again

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
        if (SpawnedPlayer != null && PlayerValues.PlayerIsDead != true)
        {
            //pause bettween sets
            yield return new WaitForSeconds(3);

            if (_formationList[_fomartionSelection].CommonEnemy != null)
            {

                //regular enemy spawn
                while (_currentNumberToSpawn > 0)
                {
                    _enemiesInPlay++;
                    //space in bettween spawns
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
                    
                    if(_currentNumberToSpawn > 0)
                    {
                        _currentNumberToSpawn--;
                        print(_currentNumberToSpawn);
                    }
                    
                }
            }

            //spawn elite
            if (_elitesToSpawn > 0 && Time.time > _eliteSpawntimer)
            {
                SpawnElite(Selection);
            }


            //special / boss spawn

        }
    }
    

    //where the game begins
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



    #region Stage Transitions

    #endregion



}
