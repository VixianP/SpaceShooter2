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

    [SerializeField]
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

    bool _movementTrack; // track player movement

    [SerializeField]
    float _distance;

    #endregion

    #region States
    [SerializeField]
    Animator _rockLobAnim;

    [SerializeField]
    int _chooseAttack;
    bool _attackChosen;

    [SerializeField]
    bool _isAttacking = false;

    [SerializeField]
    bool _isAnchored = false;
    [SerializeField]
    bool _isAligned = false;

    bool _bubbleSpawned;

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

    [SerializeField]
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
        _movementTrack = true;
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
                        _rockLobAnim.SetBool("CrtPrep", false);
                        _rockLobAnim.SetBool("CrtShot", false);
                        _isAttacking = false;
                        _bubbleSpawned = false;
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

            if (Time.time > _bubbleExecuteDelayTimer  && _bubbleSpawned == false)
            {
                _rockLobAnim.SetBool("Attack_Bubble", true);
                
                
                _instantiatedBubble = Instantiate(_bubbleGameObject, _bubbleAnchorPos, Quaternion.identity);
                _BubbleGameObjectScript = _instantiatedBubble.GetComponent<RockLobsterBubbleAttack>();
                

                _BubbleGameObjectScript.LobsterGameobject = gameObject;
                _BubbleGameObjectScript.LobsterPos = transform.position;

                _bubbleSpawned = true;
                _isAttacking = true;

                //bubble deflate
            }

            
            if(_isAttacking == true)
            {
                //current bubble size
                _bubbleSize = _BubbleGameObjectScript._cBubblesize;

                if (_bubbleSize > 4 && _instantiatedBubble != null)
                {
                    _deflating = true;
                }

                //update and link bubble size to animation
                if (_instantiatedBubble != null)
                {
                    _rockLobAnim.SetFloat("BubbleSize", _bubbleSize);

                    //knockout
                    if (_bubbleSize >= _BubbleGameObjectScript._knockout)
                    {
                        _movementTrack = false;
                        StartCoroutine(KnockoutTimer());
                        Destroy(_instantiatedBubble);
                        _instantiatedBubble = null;
                        _isAttacking = false;

                    } 
                    
                    if (_bubbleSize <= 2 && _deflating == true) //crtwindup
                    {


                        _bubbleExecuteDelayTimer = Time.time + 1.1f;
                        _rockLobAnim.SetBool("CrtPrep", true);
                        Destroy(_instantiatedBubble);
                        _instantiatedBubble = null;
                        _isAttacking = false;

                    }

                }

            }

            //crtshot execute
            if (_rockLobAnim.GetBool("CrtPrep") == true)
            {
                if (Time.time > _bubbleExecuteDelayTimer)
                {
                    _rockLobAnim.SetBool("CrtShot", true);
                    Instantiate(_crtShot, _firingPos, Quaternion.identity);

                    _deflating = false;

                    _rockLobAnim.SetBool("Bubble_Attack", false);
                    _rockLobAnim.SetBool("Attack_Bubble", false);

                    _attackTimer = Time.time + _attackDelay;
                    _bubbleTimer = Time.time + _bubbleAttackDelay;


                    _chooseAttack = 0;
                }
            }



        }
    }

    void Movement()
    {
        if (_movementTrack == true)
        {
            if (_isAnchored == false)
            {
                transform.position = Vector3.MoveTowards(transform.position, _positionToAnchor, 0.6f);
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
                        transform.position = Vector3.MoveTowards(transform.position, _pos, 0.8f);
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

    IEnumerator KnockoutTimer()
    {
        Time.timeScale = 0.1f;
        _rockLobAnim.Play("Knockout");
        yield return new WaitForSecondsRealtime(2);
        Time.timeScale = 1f;
        StartCoroutine(StunTimer());
    }

    IEnumerator StunTimer()
    {
        yield return new WaitForSeconds(1);

        _rockLobAnim.SetBool("Stun", true);

        yield return new WaitForSeconds(10);

        _rockLobAnim.SetBool("Stun", false);
        _movementTrack = true;

        _attackTimer = Time.time + _attackDelay;
        _bubbleTimer = Time.time + _bubbleAttackDelay;

        _rockLobAnim.SetBool("CrtShot", false);
        _rockLobAnim.SetBool("CrtPrep", false);
        _rockLobAnim.SetBool("Preparing", false);
        _rockLobAnim.SetBool("Bubble_Attack", false);
        _rockLobAnim.SetBool("Attack_Bubble", false);
        _isAttacking = false;
        _deflating = false;
        _chooseAttack = 0;
    }

    void Death()
    {
        _spawnManagerScript.GameEnd();
        Destroy(gameObject);
    }
}
