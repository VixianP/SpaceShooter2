using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperK : MonoBehaviour
{

    #region Attachement
    [HideInInspector]
    public bool _isFrontAttached = true;

    [HideInInspector]
    public bool _isBackAttached;

    [HideInInspector]
    public bool _isSideAttached;

    [HideInInspector]
    public bool _isattached;

    [HideInInspector]
    public bool _front,_back,_side;

    bool _movementLock = false;

    public bool _PoweredUp;

    #endregion

    #region Movement And Positioning
    [SerializeField]
    int MovementSpeed;

    Vector3 _playerPos;
    
    Vector3 _positionToMoveTo;

    [SerializeField]
    Vector3 _positionOffset;

    [SerializeField]
    float _sendOutDistance;

    Vector3 _distanceToCover;


    [SerializeField]
    bool _isMoving;

    [SerializeField] [Range(0,3)]
    int _clickCount;

    bool _isReturning;

    bool _returnLock;

    public GameObject PlayerGameObject;

    Player _playerScript;

    #endregion

    #region FireModes
    [SerializeField]
    GameObject[] fireMode;  // 0 = solo firing, 1 back attach, 2 side attach

    [SerializeField]
    GameObject[] _poweredUpFireModes;

    //what superk will be shooting
    GameObject _inChamber;

    [SerializeField]
    float _fireDelay;

    float _fireTime;

    bool _autoFire;
    #endregion

    #region Health and Regeration
    [SerializeField]
    int SKHealth;
    float RegenTimer; //for health or death
    #endregion

    #region Collsion

    [SerializeField]
    Collider2D _leftCollider;

    #endregion

    void Start()
    {
        _isFrontAttached = true;
        PlayerGameObject = PlayerValues.playerGameobject;
        if(PlayerGameObject != null)
        {
            _playerScript = PlayerGameObject.GetComponent<Player>();
        }
        _inChamber = fireMode[0];
    }

    void Update()
    {

        //this does not get modified my other commands. its a reference of where the players position is in real time.
        _playerPos = PlayerGameObject.transform.position;

        //this gets modified by other commands
        _positionToMoveTo = new Vector3(_playerPos.x + _positionOffset.x, _playerPos.y + _positionOffset.y, 0);
        

        Attached();
        SuperKInputs();
        SendOut();
        Return();
        FireInput();

        //temporary
        if(PlayerValues.PlayerIsDead == true)
        {
            Destroy(gameObject);
        }

    }

    void SendOut()
    {
        if (_isMoving == true)
        {
                _isattached = false;
                _isFrontAttached = false;
                _isBackAttached = false;
                _isSideAttached = false;

                if(_PoweredUp == false)
            {
                _inChamber = fireMode[0];
            }

                _movementLock = true;
            if (transform.position.y < _distanceToCover.y)
            {
                transform.Translate(0, MovementSpeed * Time.deltaTime, 0);

            } else if(transform.position.y >= _distanceToCover.y)
            {
                _isMoving = false;
                _movementLock = false;
                _clickCount = 2;
            }
        }
    }

    void Return()
    {
        if (_isReturning == true && _returnLock == false)
        {

            
            // if directly above the player
            if(transform.position.y > PlayerGameObject.transform.position.y + 4)
            {
                _positionOffset.x = 0;
                _positionOffset.y = 7;
                _returnLock = true;
                _front = true;
                _back = false;

                if (_PoweredUp == false)
                {
                    _inChamber = fireMode[0];
                }
                
            }
            
            //if below the player
            if (transform.position.y < PlayerGameObject.transform.position.y - 4)
            {
                _positionOffset.x = 0;
                _positionOffset.y = -7;
                _front = false;
                _back = true;
                //movement speed increase


                _inChamber = fireMode[1];

                _returnLock = true;
            }
            
            //if on the sides of the player
            if (transform.position.x > PlayerGameObject.transform.position.x && transform.position.y < _playerPos.y + 2 && transform.position.y > _playerPos.y - 2)
            {
                 print("right");

                _positionOffset.y = 0;
                _positionOffset.x = 7;

                _side = true;
                _front = false;
                _back = false;

                _inChamber = fireMode[2];

                _returnLock = true;
            }

            if (transform.position.x < PlayerGameObject.transform.position.x && transform.position.y < _playerPos.y + 2 && transform.position.y > _playerPos.y - 2)
            {
                print("Left");

                _positionOffset.y = 0;
                _positionOffset.x = -7;

                _side = true;
                _front = false;
                _back = false;

                _inChamber = fireMode[2];

                _returnLock = true;
            }

        }

        //attaching.....
        if(_returnLock == true)
        {
            transform.position = Vector3.MoveTowards(transform.position, _positionToMoveTo, 0.7f);
            if(Vector3.Distance(transform.position,_positionToMoveTo) == 0)
            {
                _isattached = true;

                //front attachment
                if(_front == true)
                {
                    _isFrontAttached = true;
                } else if (_front == false)
                {
                    _isFrontAttached = false;
                }

                //back attachement
                if(_back == true)
                {
                    _isBackAttached = true;
                }
                else
                {
                    _back = false;
                    _isBackAttached = false;
                }

                
                if(_side == true)
                {
                    _isSideAttached = true;
                }
                else
                {
                    _isSideAttached = false;
                    _side = false;
                }

                

                _isReturning = false;
                _returnLock = false;
            }
        }
    }

    void SuperKInputs()
    {
        //combo input for auto fire
        if(Input.GetMouseButton(0) && Input.GetMouseButtonDown(1))
        {
            _autoFire = true;
        }
       
            //propel
            if (Input.GetMouseButtonDown(1) && _isMoving == false && _isattached == true && _isReturning == false)
            {
                _isMoving = !_isMoving;
            _distanceToCover = new Vector3(transform.position.x, transform.position.y + _sendOutDistance, transform.position.z);
            }  

            //stop
        if (Input.GetMouseButtonDown(1) && _movementLock == true && transform.position.y < _distanceToCover.y )
        {
            _isMoving = false;
            _movementLock = false;
        }
        if(Input.GetMouseButtonDown(1) && _clickCount == 0 && _isReturning == true)
        {
            //interrupt return
            print("Stoped");
            
        }

        //click counter
        if (Input.GetMouseButtonDown(1))
        {
            _clickCount += 1;
        }

        //attach
        if (Input.GetMouseButtonDown(1) && _movementLock == false && _clickCount == 3)
        {
            _clickCount = 0;
            _isReturning = true;
            _autoFire = false;
            
        }

        //add coasting. if player holds right click for 1 second. super k will move left and right with the player while sent out

    }

    void Attached()
    {
        if(_isattached == true)
        {
            //transform.position = new Vector3(PlayerGameObject.transform.position.x, PlayerGameObject.transform.position.y + _attachedPositionOffset, 0);
            transform.position = new Vector3(_positionToMoveTo.x, _positionToMoveTo.y,0);
        }
    }


    void FireInput()
    {
        if (Time.time > _fireTime)
        {
            if(_autoFire == true)
            {
                Instantiate(_inChamber, transform.position, Quaternion.identity);
                _fireTime = Time.time + _fireDelay;
            }

            if (Input.GetMouseButton(0) && _isFrontAttached == true)
            {
                Instantiate(_inChamber, transform.position, Quaternion.identity);
                _fireTime = Time.time + _fireDelay;
            }
            if (Input.GetMouseButton(0) && _isBackAttached == true)
            {
                Instantiate(_inChamber, _playerPos, Quaternion.identity);
                _fireTime = Time.time + _fireDelay;
            }

            if (Input.GetMouseButtonDown(0) && _isattached == false)
            {
                Instantiate(_inChamber, transform.position, Quaternion.identity);
                _fireTime = Time.time + _fireDelay;
            }

            if (Input.GetMouseButtonDown(0) && _isSideAttached == true)
            {
                Instantiate(_inChamber, transform.position, Quaternion.identity);
                _fireTime = Time.time + _fireDelay;
            }

        }
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "EnemyBullet")
        {
            Destroy(collision.gameObject);
        }

        //if collide with laser, take damage

    }
}
