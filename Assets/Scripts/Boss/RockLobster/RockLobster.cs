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

    float _bubbleSize;
    float _mbubbleSize;

    bool _deflating;
    #endregion

    #region Positioning
    //positioning
    [SerializeField]
    float _amplitude = 2;

    //position
    [SerializeField]
    Vector2 _positionToAnchor;

    Vector2 _pos;
    Vector3 _playerPos;

    Vector2 _bubbleAnchorPos;

    Vector2 _firingPos; //for crt bubble and other potential projectiles

    [SerializeField]
    float _distance;

    #endregion

    #region States
    [SerializeField]
    Animator _rockLobAnim;

    [SerializeField]
    int _chooseAttack;
    bool _attackChosen;

    bool _isAttacking;

    [SerializeField]
    bool _isAnchored = false;
    [SerializeField]
    bool _isAligned = false;
    #endregion

    #region Attack Timers


    //General cool down bettween all attacks
    float _attackDelay = 5;
    float _attackTimer;


    //regular attack timer
    float _strikeTimer;

    float _strikeDelay = 6;

    //charge timer
    float _chargerTimer;

    float _chargeDelay = 20;
    

    //bubble attack timer
    float _bubbleTimer;

    float _bubbleAttackDelay = 4.5f;
    float _bubblePrepareTimer;
    float _bubbleExecuteDelayTimer;
    float _bubbleTimerDuration;

    //Crtshot
    float _crtShotDelay = 1;
    float _crtShotTimer;



    #endregion

    #region GameObjects

    [SerializeField]
    GameObject _bubbleGameObject; //the projectile its going to shoot ( that spawns out of the big bubble )

    [SerializeField]
    GameObject _crtShot;

    GameObject _instantiatedBubble; //the bubble that fills and deflates

    #endregion

    #region External Scripts
    GameObject _playerGameObject;
    SpawnManager _spawnManagerScript;

    Player _playerScript;

    RockLobsterBubbleAttack _BubbleGameObjectScript;
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

        WindClocks();

    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        Attack();
        BubbleAttack();
        FiringPositions();
    }

    void FiringPositions()
    {
        _bubbleAnchorPos = transform.position;
        _bubbleAnchorPos.y = transform.position.y - 15;

        _firingPos = transform.position;
        _firingPos.x = transform.position.x - 10;
    }

    void WindClocks()
    {
        _strikeTimer = Time.time + _strikeDelay;
        _chargerTimer = Time.time + _chargeDelay;
        _bubbleTimer = Time.time + _bubbleAttackDelay;
    }

    void Attack()
    {

        if(Time.time > _attackTimer && _chooseAttack == 0)
        {

            _rockLobAnim.SetBool("Preparing", true);

                if (_rockLobAnim.GetBool("Preparing") == true)
                {



                    //choosing  attack
                    if (_attackChosen == false)
                    {
                        
                        if(Time.time > _strikeTimer)
                        {
                            _chooseAttack = 1;
                        }

                        if (Time.time > _chargerTimer)
                        {
                            _chooseAttack = 2;
                        }

                    if (Time.time > _bubbleTimer)
                    {
                        _bubbleExecuteDelayTimer = Time.time + _bubbleAttackDelay;
                        _bubblePrepareTimer = Time.time + 2;
                        _chooseAttack = 3;
                    }
                }

                }

            }  
        

    }

    void BubbleAttack()
    {
        if (_chooseAttack == 3 && Time.time > _bubblePrepareTimer)
        {
            _rockLobAnim.SetBool("Preparing", false);
            _rockLobAnim.SetBool("Bubble_Attack", true);

            if (Time.time > _bubbleExecuteDelayTimer  && _isAttacking == false)
            {
                _rockLobAnim.SetBool("Attack_Bubble", true);
                
                
                _instantiatedBubble = Instantiate(_bubbleGameObject, _bubbleAnchorPos, Quaternion.identity);
                _BubbleGameObjectScript = _instantiatedBubble.GetComponent<RockLobsterBubbleAttack>();
                

                _BubbleGameObjectScript.LobsterGameobject = gameObject;
                _BubbleGameObjectScript.LobsterPos = transform.position;
                
                _isAttacking = true;
            }

            if(_isAttacking == true)
            {
                _bubbleSize = _BubbleGameObjectScript._cBubblesize;
                _mbubbleSize = _BubbleGameObjectScript._mBubblesize;

                if(_bubbleSize >= _mbubbleSize && _deflating == false)
                {
                    _deflating = true;
                }



                if (_bubbleSize <= 2 && _deflating == true)
                {

                    Destroy(_instantiatedBubble);

                    _bubbleExecuteDelayTimer = Time.time + 1.1f;

                    _rockLobAnim.SetBool("CrtPrep", true);
                    _deflating = false;

                }

                if(_rockLobAnim.GetBool("CrtPrep") == true)
                {
                    if(Time.time > _bubbleExecuteDelayTimer && _deflating == false)
                    {
                        _rockLobAnim.SetBool("CrtShot", true);
                        Instantiate(_crtShot, _firingPos, Quaternion.identity);
                        _bubbleExecuteDelayTimer = Time.time + 10;
                    }
                }

      


            }

            //use a bool, use size, or check if the state has reached the knock out animation to exit the attack.

            //refresh attack delay on exit

        }
    }

    void Movement()
    {
        if (_isAnchored == false)
        {
            transform.position = Vector3.MoveTowards(transform.position, _positionToAnchor, 0.06f);
            if (Vector3.Distance(transform.position, _positionToAnchor) == 0)
            {
                _isAnchored = true;
            }
        }

        if (_isAnchored == true)
        {


            if (_isAligned == false)
            {
                _pos.y = _positionToAnchor.y;
                _pos.x = _playerGameObject.transform.position.x;
                if (transform.position.x < _playerGameObject.transform.position.x || transform.position.x > _playerGameObject.transform.position.x)
                {
                    transform.position = Vector3.MoveTowards(transform.position, _pos, 0.03f);
                    if (Vector3.Distance(transform.position, _pos) == 0)
                    {
                        _pos.x = _playerGameObject.transform.position.x + 10 + Mathf.Cos(Time.time) * -_amplitude;
                        _isAligned = true;
                    }

                }
            }


            if (_isAligned == true && _isAttacking == false)
            {
                _pos.x = _playerGameObject.transform.position.x + 10 + Mathf.Cos(Time.time) * -_amplitude;
                transform.position = Vector3.MoveTowards(transform.position, _pos, _movementSpeed);
            }

            if (_isAligned == true && _isAttacking == true)
            {
                _pos.x = _playerGameObject.transform.position.x + 10;
                transform.position = Vector3.MoveTowards(transform.position, _pos, _movementSpeed);
            }


        }
    }

    void EnemyTakeDamage(float _laserDamageAmount)
    {
        if (_isAnchored == true)
        {
            if(Vector3.Distance(transform.position,_playerPos) < 50)
            {
                //take full damage if close
                _currentHealth -= _laserDamageAmount;
                _playerScript.UpdateBossUI(_currentHealth);
            } else if(Vector3.Distance(transform.position, _playerPos) > 50)
            {
                //take half damage if futher away
                _currentHealth -= _laserDamageAmount * 0.5f;
                _playerScript.UpdateBossUI(_currentHealth);
            }

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
