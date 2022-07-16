using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHeavy : MonoBehaviour
{
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
    GameObject _bulletStormGameObj;

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
            GameObject _instantiatedBullet = Instantiate(_bulletStormGameObj, transform.position, Quaternion.identity);
            _eProjectile = _instantiatedBullet.GetComponent<EnemyProjectileScript>();
            _eProjectile.ObjectToFollow = gameObject;
            _fireTime = Time.time + _fireDelay;
        }
    }
 
    void Movement()
    {
        if (_isCharging == false)
        {
            _positionToMoveTo = new Vector3(_playerGameObject.transform.position.x, _offset.y, 0);

            _offset.y -= _movementSpeed * Time.deltaTime;

            transform.position = Vector3.Lerp(transform.position, _positionToMoveTo, 5f);
        } else if(_isCharging == true)
        {
            transform.Translate(0, -_movementSpeed * 20 * Time.deltaTime, 0);
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
            Destroy(gameObject);
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

    void SetSpawnManager(GameObject _spawnManagerGameObjsect)
    {

        _spawnManagerScript = _spawnManagerGameObjsect.GetComponent<SpawnManager>();

    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        
        if(coll.gameObject.tag == "ChargedShot")
        {

            _laserScript = coll.gameObject.GetComponent<Laser>();
            ShieldDamage(_laserScript._laserDamageAmount);

        }
       
        if(coll.tag == "Player")
        {
            coll.SendMessage("CollisionDmg", 100);
        }

    }

}
