using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    #region EnemyStats
    [SerializeField]
    private int _enemyHealth;

    [SerializeField]
    int _enemyShieldHealth;

    [SerializeField]
    private int CollsionDamage = 1;

    #endregion

    #region Enemy Projectile

    [SerializeField]
    GameObject enemyProjetileGameobject_;

    [SerializeField]
    float _fireDelay = 5;


    float timeBetweenShots = 0;

    #endregion

    #region Score,Value,Drops

    [SerializeField]
    private int PointValue;

    #endregion

    #region OnDeathStuff

    [SerializeField]
    private GameObject Explosion;

    #endregion

    #region Elite Variables

    [SerializeField]
    bool _isElite;

    [SerializeField]
    bool _hasShield;

    [SerializeField]
    GameObject _enemyShield;

    [SerializeField]
    GameObject _powerUpDrops;

    #endregion

    #region PoistionChecks

    public Vector3 MveDirection;

    Vector3 _playerPosition;
    Vector3 _firingDirection;

    Vector3 _respawnPosition;

    public bool Down;

    #endregion

    public int FormationID;

    public SpawnManager _spwnManager;

    private void Start()
    {

        //timeBetweenShots += Time.deltaTime + 2;
        _playerPosition = PlayerValues.playerGameobject.transform.position;

    }

    void Update()
    {
        EnemyFire();
        EnemyMovement();

    }

    void EnemyFire()
    {

        if (Time.time > timeBetweenShots)
        {

            Instantiate(enemyProjetileGameobject_, transform.position, Quaternion.identity);
            Instantiate(enemyProjetileGameobject_, transform.position, Quaternion.Euler(0,0,-10));
            Instantiate(enemyProjetileGameobject_, transform.position, Quaternion.Euler(0, 0, 10));
            timeBetweenShots = Time.time + _fireDelay;
            
        }
        
    }

    void EnemyMovement()
    {
        //down movement
        transform.Translate(MveDirection * Time.deltaTime);

        OutOfBounds();

    }

    void OutOfBounds()
    {
        if (transform.position.x < -95 || transform.position.x > 95)
        {
            _spwnManager.ReSpawner(gameObject,_isElite);
        }

        if (transform.position.y < -55 || transform.position.y > 55)
        {
            _spwnManager.ReSpawner(gameObject,_isElite);
        }
    }

    void EnemyTakeDamage(int _damageAmount)
    {
        if (_hasShield == false)
        {
            _enemyHealth -= _damageAmount;
            if (_enemyHealth <= 0)
            {
                Death();
            }
        } else
        {
            EnemyShieldDamage(_damageAmount);
        }
    }

    void EnemyShieldDamage(int _damageAmount)
    {

        if(_damageAmount > _enemyShieldHealth)
        {
            _enemyShieldHealth -= _damageAmount;
            _enemyHealth += _enemyShieldHealth;
            _enemyShieldHealth = 0;

            if(_enemyHealth <= 0)
            {
                Death();
            }

        }
        if(_enemyShieldHealth <= 0)
        {
            _enemyShield.SetActive(false);
            _hasShield = false;
        }

    }

    void Flower()
    {
        Instantiate(enemyProjetileGameobject_, transform.position, Quaternion.Euler(0, 0, -180));
        Instantiate(enemyProjetileGameobject_, transform.position, Quaternion.Euler(0, 0, 180));
        Instantiate(enemyProjetileGameobject_, transform.position, Quaternion.Euler(0, 0, -90));
        Instantiate(enemyProjetileGameobject_, transform.position, Quaternion.Euler(0, 0, 90));
        Instantiate(enemyProjetileGameobject_, transform.position, Quaternion.Euler(0, 0, -45));
        Instantiate(enemyProjetileGameobject_, transform.position, Quaternion.Euler(0, 0, 45));
        Instantiate(enemyProjetileGameobject_, transform.position, Quaternion.Euler(0, 0, -20));
        Instantiate(enemyProjetileGameobject_, transform.position, Quaternion.Euler(0, 0, 20));
    }

    public void Death()
    {


        Flower();


        Instantiate(Explosion, transform.position, Quaternion.identity);



        if(_isElite == true)
        {
            Instantiate(_powerUpDrops, transform.position, Quaternion.identity);
        }

        _spwnManager.EnemyDeath(gameObject,_isElite);

        PlayerValues.Score += PointValue;

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if(other.tag == "Laser")
        {
            Destroy(other.gameObject);

        }

        if(other.tag == "Player")
        {
            other.GetComponent<Player>().CollisionDmg(CollsionDamage * 50);

            Death();
        }
        if(_isElite == true && other.tag == "PowerUp")
        {
            Flower();
            Destroy(other.gameObject);
        }
    }
}
