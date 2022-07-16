using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperK : MonoBehaviour
{
    public bool _isattached;

    bool _movementLock = false;

    #region Movement And Positioning
    [SerializeField]
    int MovementSpeed;

    Vector3 _positionToMoveTo;

    [SerializeField]
    float _sendOutDistance;

    Vector3 _distanceToCover;

    [SerializeField]
    int _attachedPositionOffset = 7;


    [SerializeField]
    bool _isMoving;

    [SerializeField] [Range(0,3)]
    int _clickCount;

    bool _isReturning;

    bool _returnLock;

    public GameObject PlayerGameObject;

    Vector3 PlayerPosition;
    #endregion

    #region FireModes
    [SerializeField]
    GameObject[] fireMode;  // 0 = solo firing, 1 = combined, 2 Combined ChargeShot

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



    void Update()
    {
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
            if(transform.position.y > PlayerGameObject.transform.position.y)
            {
                _positionToMoveTo = PlayerGameObject.transform.position;
                _positionToMoveTo.y += _attachedPositionOffset;
                _returnLock = true;
            }
        }
        if(_returnLock == true)
        {
            transform.position = Vector3.MoveTowards(transform.position, _positionToMoveTo, 0.2f);
            if(Vector3.Distance(transform.position,_positionToMoveTo) == 0)
            {
                _isattached = true;
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
            transform.position = new Vector3(PlayerGameObject.transform.position.x, PlayerGameObject.transform.position.y + _attachedPositionOffset, 0);
        }
    }

    void FireInput()
    {
        if (Time.time > _fireTime)
        {
            if(_autoFire == true)
            {
                Instantiate(fireMode[1], transform.position, Quaternion.identity);
                _fireTime = Time.time + _fireDelay;
            }

            if (Input.GetMouseButton(0) && _isattached == true)
            {
                Instantiate(fireMode[1], transform.position, Quaternion.identity);
                _fireTime = Time.time + _fireDelay;
            }
            if (Input.GetMouseButtonDown(0) && _isattached == false)
            {
                Instantiate(fireMode[0], transform.position, Quaternion.identity);
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
