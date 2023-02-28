using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class RockLobster : MonoBehaviour
{
    #region Stats
    [SerializeField]
    string _bossname;

    [SerializeField]
    float _maxHealth = 1000;

    float _currentHealth;

    [SerializeField]
    float _movementSpeed = 0.4f;
    #endregion

    #region Positioning
    //positioning
    [SerializeField]
    float _amplitude = 2;

    //position
    [SerializeField]
    Vector2 _positionToAnchor;

    Vector2 _pos;
    #endregion

    #region States
    [SerializeField]
    Animator _rockLobAnim;

    int _chooseAttack;

    [SerializeField]
    bool _isAnchored = false;
    [SerializeField]
    bool _isAligned = false;
    #endregion

    #region Attack Timers


    //time bettween attacks
    float _attackDelay = 5;
    float _attackTimer;


    //time to choose and execute an attack
    float _executionDelay = 3;
    float _executionTimer;



    //regular attack

    //bubble attack

    //charge


    #endregion

    #region External Scripts
    GameObject _playerGameObject;
    SpawnManager _spawnManagerScript;

    Player _playerScript;

    #endregion

    void Start()
    {
        if (PlayerValues.playerGameobject != null)
        {
            _playerGameObject = PlayerValues.playerGameobject;
            _playerScript = _playerGameObject.GetComponent<Player>();

            _playerScript.InitializeBoss(_bossname, _maxHealth);
            _currentHealth = _maxHealth;
        }

        if(_spawnManagerScript == null)
        {
            _spawnManagerScript = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        }

        _attackTimer = Time.time + _attackDelay;

        _rockLobAnim = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        Attack();
    }

    void Attack()
    {
        if(Time.time > _attackTimer)
        {
            _rockLobAnim.SetBool("Preparing", true);


            //run through all attacks and time
            if (_rockLobAnim.GetBool("Preparing") == true)
            {

                _chooseAttack = Random.Range(1, 3);

                if(_chooseAttack == 3)
                {
                    //bubble attack == true
                    //call bubble attack function
                }


                //_attackTimer = Time.time + _attackDelay;
            }
        }
    }

    void BubbleAttack()
    {
        //preparing = false
        //_attackTimer = Time.time + _attackDelay;
        //set up for bubble attack

        //executing bubble prepare animation. make boss invulnerable

        //if timer is a go
        //bubble attack set trigger
        //bubble attack for X amount of time.
        //instantiate bubble attack every 0.5f


        //dont forget to create the bubble projectile and link up its prefab to instantiate
    }

    void Movement()
    {
        if (_isAnchored == false)
        {
            transform.position = Vector3.MoveTowards(transform.position, _positionToAnchor, 0.5f);
            if (Vector3.Distance(transform.position, _positionToAnchor) == 0)
            {
                _isAnchored = true;
            }
        }

        if (_isAnchored == true)
        {


            if (_isAligned == false)
            {
                _pos.y = 30;
                _pos.x = _playerGameObject.transform.position.x;
                if (transform.position.x < _playerGameObject.transform.position.x || transform.position.x > _playerGameObject.transform.position.x)
                {
                    transform.position = Vector3.MoveTowards(transform.position, _pos, 0.3f);
                    if (Vector3.Distance(transform.position, _pos) == 0)
                    {
                        _pos.x = _playerGameObject.transform.position.x + Mathf.Cos(Time.time) * -_amplitude;
                        _isAligned = true;
                    }

                }
            }


            if (_isAligned == true)
            {
                _pos.x = _playerGameObject.transform.position.x + Mathf.Cos(Time.time) * -_amplitude;
                transform.position = Vector3.MoveTowards(transform.position, _pos, _movementSpeed);
            }
        }
    }

    void EnemyTakeDamage(float _laserDamageAmount)
    {
        if (_isAnchored == true)
        {
            _currentHealth -= _laserDamageAmount;
            _playerScript.UpdateBossUI(_currentHealth);
            if(_currentHealth < 1)
            {
                Death();
            }
        }
    }

    void Death()
    {
        _spawnManagerScript.GameEnd();
        Destroy(gameObject);
    }
}
