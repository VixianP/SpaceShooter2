using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[System.Serializable]

public class Formation
{
    public bool Type;

    public int MaxNumberToSpawn;

    public int CurrentNumberToSpawn;

    public int NumberToRespawn;

    public int InPlay;

    public GameObject CommonEnemy;

    public GameObject EliteEnemy;

    public string CallMethod;

    public Vector3 SpawnPosition;

    public Vector3 RepawnDirection;

    public Vector3 MovementDirection;

    public Formation(bool Type, int MaxNumberToSpawn, int CurrentNumberToSpawn, int NumberToSRespawn, int InPlay, GameObject CommonEnemy,GameObject EliteEnemy,string CallMethod, Vector3 SpawnPosition, Vector3 RepawnDirection, Vector3 MovementDirection)
    {
        this.Type = Type;
        this.MaxNumberToSpawn = MaxNumberToSpawn;
        this.CurrentNumberToSpawn = CurrentNumberToSpawn;
        this.NumberToRespawn = NumberToSRespawn;
        this.InPlay = InPlay;
        this.CommonEnemy = CommonEnemy;
        this.EliteEnemy = EliteEnemy;
        this.CallMethod = CallMethod;
        this.SpawnPosition = SpawnPosition;
        this.RepawnDirection = RepawnDirection;
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
            SpawnPowerUp();
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
        if(Time.time > _obstacleTime * .9f && _WarningIndicator.activeInHierarchy == false)
        {
            _spawnPosition = new Vector3(Random.Range(-_spawnPosition.x, _spawnPosition.x), _spawnPosition.y, _spawnPosition.z);
            _WarningIndicator.transform.position = new Vector2(_spawnPosition.x, 19);
            _WarningIndicator.SetActive(true);
        }
        if(Time.time > _obstacleTime)
        {
            GameObject _newObstacle = Instantiate(_obstaclesToSpawn[0], _spawnPosition , Quaternion.identity);
            ScrollingBackgrounds _obstacleVaryingSpeed = _newObstacle.GetComponent<ScrollingBackgrounds>();
            _obstacleVaryingSpeed.MovementSpeed = Random.Range(30, 70);
            _WarningIndicator.SetActive(false);
            _obstacleTime = Time.time + _obstacleSpawnDelay;
        }
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
        if (_formationList[_fomartionSelection].CommonEnemy != null)
        {
            _currentSpawnPosition = _formationList[_fomartionSelection].SpawnPosition;

            _formationList[_fomartionSelection].CurrentNumberToSpawn = _formationList[_fomartionSelection].MaxNumberToSpawn;
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
        _formationList[_fomartionSelection].CurrentNumberToSpawn = _formationList[_fomartionSelection].MaxNumberToSpawn;
        _currentSpawnPosition = _formationList[_fomartionSelection].SpawnPosition;
        yield return new WaitForSeconds(3);
        StartCoroutine(SpawnCoroutine(_fomartionSelection));
    }
         
    public void ReSpawner(GameObject EnemyGameObject)
    {
        Enemy _eScript = EnemyGameObject.GetComponent<Enemy>();

        _formationList[_eScript.FormationID].InPlay--;

        _formationList[_eScript.FormationID].NumberToRespawn++;

        Destroy(EnemyGameObject);
        

            if (_formationList[_eScript.FormationID].InPlay == 0)
            {

            int _selectedFormationInLIst = _eScript.FormationID;

            //up down
            if (_formationList[_selectedFormationInLIst].Type == true) 
            {
                if (Mathf.Sign(_currentSpawnPosition.x) == 1)
                {
                    _formationList[_selectedFormationInLIst].CurrentNumberToSpawn = _formationList[_selectedFormationInLIst].NumberToRespawn;

                    _currentSpawnPosition.x -= _formationList[_selectedFormationInLIst].SpawnPosition.x * _formationList[_selectedFormationInLIst].RepawnDirection.x;

                    StartCoroutine(SpawnCoroutine(_selectedFormationInLIst));
                }
                else
                {
                    _formationList[_selectedFormationInLIst].CurrentNumberToSpawn = _formationList[_selectedFormationInLIst].NumberToRespawn;

                    _currentSpawnPosition.x += _formationList[_selectedFormationInLIst].SpawnPosition.x * _formationList[_selectedFormationInLIst].RepawnDirection.x;

                    StartCoroutine(SpawnCoroutine(_selectedFormationInLIst));
                }
            }

            //left right
            if (_formationList[EnemyGameObject.GetComponent<Enemy>().FormationID].Type == false)
            {
                if (Mathf.Sign(_currentSpawnPosition.y) == 1)
                {
                    _formationList[EnemyGameObject.GetComponent<Enemy>().FormationID].CurrentNumberToSpawn = _formationList[EnemyGameObject.GetComponent<Enemy>().FormationID].NumberToRespawn;

                    _currentSpawnPosition.y = -16;
                    //_eScript.MveDirection.x =

                    StartCoroutine(SpawnCoroutine(_selectedFormationInLIst));
                }
                else
                {
                    _formationList[EnemyGameObject.GetComponent<Enemy>().FormationID].CurrentNumberToSpawn = _formationList[EnemyGameObject.GetComponent<Enemy>().FormationID].NumberToRespawn;

                    _currentSpawnPosition.y = 12;

                    StartCoroutine(SpawnCoroutine(_selectedFormationInLIst));
                }
            }
        }


    }

    public void EnemyDeath(GameObject enemyGameobject)
    {
        Enemy _escirpt = enemyGameobject.GetComponent<Enemy>();
        _formationList[_escirpt.FormationID].InPlay--;
        if(_formationList[_escirpt.FormationID].InPlay== 0 && _formationList[_escirpt.FormationID].CurrentNumberToSpawn == 0 && _formationList[_escirpt.FormationID].NumberToRespawn == 0)
        {
            WaveSpawner();
        }
        Destroy(enemyGameobject);
    }
 
    IEnumerator SpawnCoroutine(int Selection)
    {
        print("spawing");
        while (_formationList[Selection].CurrentNumberToSpawn > 0 && SpawnedPlayer != null && PlayerValues.PlayerIsDead != true)
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

            _formationList[Selection].CurrentNumberToSpawn--;

            _formationList[Selection].InPlay++;

            if (_formationList[Selection].NumberToRespawn > 0)
            {
                _formationList[Selection].NumberToRespawn--;
            }
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

        //SpawnEnemy();

        yield return new WaitForSeconds(2);

        SpawnUI[1].gameObject.SetActive(false);
    }

}
