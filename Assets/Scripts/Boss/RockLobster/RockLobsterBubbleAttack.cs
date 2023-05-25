using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockLobsterBubbleAttack : MonoBehaviour
{

    public GameObject LobsterGameobject;
    public Vector2 LobsterPos;

    #region Filling and Deflating


    [SerializeField]
    float _fillspeed;

    [SerializeField]
    float _deflatespeed;

    
    public float _mBubblesize = 6.9f;

    
    public float _knockout = 10;

    public float _cBubblesize = 0;
    
    Vector3 _maxBubbleSize, _currentBubbleSize;

    bool _initfill,_done;

    [SerializeField]
    float _fillDelay = 2;
    float _fillTimer;

    CircleCollider2D _collider;
    #endregion



    [SerializeField]
    GameObject _leftprojectilePrefab, _midprojectilePrefab, _rightprojectilePrefab;

    [SerializeField]
    float _spawnDelay = 0.7f;
    float _timeTospawn;


    // Start is called before the first frame update
    void Start()
    {
        _maxBubbleSize = new Vector3(_mBubblesize, _mBubblesize, _mBubblesize);
        _currentBubbleSize = new Vector3(_cBubblesize, _cBubblesize, _cBubblesize);
        _initfill = true;

        _fillTimer = Time.time + _fillDelay;

        _collider = gameObject.GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {

        if (LobsterGameobject != null)
        {
            LobsterPos = new Vector2(LobsterGameobject.transform.position.x - 12, LobsterGameobject.transform.position.y - 20);
            transform.position = LobsterPos;
        }

        _currentBubbleSize = new Vector3(_cBubblesize, _cBubblesize, _cBubblesize);
        transform.localScale = _currentBubbleSize;
        _collider.radius = _cBubblesize;



        Fill();

        Shoot();

        
    }

    void Fill()
    {
        //initial fill
        if (Time.time > _fillTimer)
        {
            if (_cBubblesize < 5 && _initfill == true)
            {
                _cBubblesize += _fillspeed;
            }
            else
            {
                _initfill = false;
            }
        }
    }


    void Shoot()
    {
        //shoot
        if (_cBubblesize > 0 && _initfill == false)
        {
            if (Time.time > _timeTospawn)
            {
                Instantiate(_leftprojectilePrefab, transform.position, Quaternion.identity);
                Instantiate(_midprojectilePrefab, transform.position, Quaternion.identity);
                Instantiate(_rightprojectilePrefab, transform.position, Quaternion.identity);
                _timeTospawn = Time.time + _spawnDelay;
            }

            _cBubblesize -= _deflatespeed * Time.deltaTime;


        }

    }



    private void OnTriggerEnter2D(Collider2D coll)
    {
        if(coll.tag == "Laser")
        {
            if (_initfill == false)
            {
                _cBubblesize += 0.1f;
            }
        }
    }


}
