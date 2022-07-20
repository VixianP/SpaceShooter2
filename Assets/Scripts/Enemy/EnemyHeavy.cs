using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHeavy : MonoBehaviour
{

    [SerializeField]
    bool _isElite = true;

    [SerializeField]
    float _movementSpeed;

    GameObject _playerGameObject;

    [SerializeField]
    Vector3 _offset;


    #region Health and Shields

    [SerializeField]
    int _enemyHealth = 50;

    [SerializeField]
    int _shieldHealth = 100;

    [SerializeField]
    GameObject _shieldGameObject;

    #endregion

    #region Fire

    [SerializeField]
    float _fireDelay;

    float _fireTime;

    [SerializeField]
    GameObject _bullet;

    #endregion

    #region Charging
    [SerializeField]
    float _distanceToCharge;

    float _chargeDistance;

    Vector3 _positionToMoveTo;


    bool _isCharging;

    #endregion

    #region Scripts

    Laser _laserScript;

    EnemyProjectileScript _eProjectile;

    SpawnManager _spawnManagerScript;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        _spawnManagerScript = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();

        _playerGameObject = PlayerValues.playerGameobject;

        _fireTime = Time.deltaTime + _fireDelay;
    }

    // Update is called once per frame
    void Update()
    {
        Fire();
        ChargeAttack();
        Movement();
    }

    void Fire()
    {
        if(Time.time > _fireTime)
        {
             Instantiate(_bullet, transform.position, Quaternion.identity);
             Instantiate(_bullet, transform.position, Quaternion.Euler(0,0,20));
            Instantiate(_bullet, transform.position, Quaternion.Euler(0, 0, -20));

            /*_eProjectile = _instantiatedBullet.GetComponent<EnemyProjectileScript>();
            _eProjectile.ObjectToFollow = gameObject;
            */
            _fireTime = Time.time + _fireDelay;
        }
    }
 
    void Movement()
    {
        if (_isCharging == false)
        {
            _positionToMoveTo = new Vector3(_playerGameObject.transform.position.x, _offset.y, 0);

            _offset.y -= _movementSpeed * Time.deltaTime;

            transform.position = Vector3.Lerp(transform.position, _positionToMoveTo, 0.3f);
        } else if(_isCharging == true)
        {
            transform.Translate(0, -_movementSpeed * Time.deltaTime, 0);
        }
        if(transform.position.y < -95)
        {
            _spawnManagerScript.ReSpawner(gameObject, _isElite);
        }
    }

    void ChargeAttack()
    {
        _chargeDistance = _playerGameObject.transform.position.y + _distanceToCharge;
        if(transform.position.y < _chargeDistance)
        {
            _isCharging = true;
        }
    }

    public void EnemyTakeDamage(int dmg)
    {
        if(_shieldHealth <= 0)
        {
            _enemyHealth -= dmg;
        }
        if(_enemyHealth <= 0)
        {
            _spawnManagerScript.EnemyDeath(gameObject, _isElite);
        }
    }
    
    void ShieldDamage(int dmg)
    {
        _shieldHealth -= dmg;
        if(_shieldHealth <= 0)
        {
            _shieldGameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        
        if(coll.gameObject.tag == "ChargedShot")
        {

            _laserScript = coll.gameObject.GetComponent<Laser>();
            ShieldDamage(_laserScript._laserDamageAmount);

        }
 
        if(coll.tag == "Laser")
        {

            _laserScript = coll.gameObject.GetComponent<Laser>();
            EnemyTakeDamage(_laserScript._laserDamageAmount);
        }

        if(coll.tag == "Player")
        {
            coll.SendMessage("CollisionDmg", 100);
        }

    }

}