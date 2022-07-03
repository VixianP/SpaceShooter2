using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    #region EnemyStats
    [SerializeField]
    private int EnemyHealth;

    [SerializeField]
    private int CollsionDamage = 1;
    #endregion

    #region Enemy Projectile

    [SerializeField]
    GameObject enemyProjetileGameobject_;

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

        timeBetweenShots += Time.deltaTime;
        _playerPosition = PlayerValues.playerGameobject.transform.position;

    }
    // Update is called once per frame
    void Update()
    {
        EnemyFire();
        EnemyMovement();

    }
    void EnemyFire()
    {

        if (Time.time > timeBetweenShots)
        {


            if (Down == false)
            {
                //if comming from above and alighned
                if (_playerPosition.y > 0)
                {
                    if (transform.position.x > _playerPosition.x)
                    {
                        _firingDirection = new Vector3(0, 1, 0);
                        GameObject _instantiatedBullet = Instantiate(enemyProjetileGameobject_, transform.position, Quaternion.identity);
                        _instantiatedBullet.GetComponent<EnemyProjectileScript>().moveDir_ = _firingDirection;
                        timeBetweenShots = timeBetweenShots + 2.5f;
                    }
                }

                if (_playerPosition.y < 0)
                {
                    if (transform.position.x > _playerPosition.x)
                    {
                        _firingDirection = new Vector3(0, -1, 0);
                        GameObject _instantiatedBullet = Instantiate(enemyProjetileGameobject_, transform.position, Quaternion.identity);
                        _instantiatedBullet.GetComponent<EnemyProjectileScript>().moveDir_ = _firingDirection;
                        timeBetweenShots = timeBetweenShots + 2.5f;
                    }
                }
            }
            else
            {

                //if on the sides of the player and lined up
                if (transform.position.y < _playerPosition.y)
                {
                    if (transform.position.x > 0)
                    {

                        _firingDirection = new Vector3(-1, 0.1f, 0);

                    }
                    else
                    {

                        _firingDirection = new Vector3(1, 0.1f, 0);

                    }

                    GameObject _instantiatedBullet = Instantiate(enemyProjetileGameobject_, transform.position, Quaternion.identity);
                    _instantiatedBullet.GetComponent<EnemyProjectileScript>().moveDir_ = _firingDirection;
                    timeBetweenShots = timeBetweenShots + 2.5f;

                }
            }

            
        }
        
    }
    void EnemyMovement()
    {
        //down movement
        transform.Translate(MveDirection * Time.deltaTime);

       if(transform.position.x < -45 || transform.position.x > 40)
        {
            _spwnManager.ReSpawner(gameObject);
        }

        if(transform.position.y < -25 || transform.position.y > 25)
        {
            _spwnManager.ReSpawner(gameObject);
        }
       
    }
    public void Death()
    {
        Instantiate(Explosion, transform.position, Quaternion.identity);

        _spwnManager.EnemyDeath(gameObject);
        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Laser")
        {
            Destroy(other.gameObject);

            PlayerValues.Score += PointValue;

            Death();

        }
        if(other.tag == "ChargedShot")
        {
            PlayerValues.Score += PointValue;

            Death();
        }
        if(other.tag == "Player")
        {
            other.GetComponent<Player>().CollisionDmg(CollsionDamage * 50);

            Death();
        }
    }
}
