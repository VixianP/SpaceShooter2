using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockLobsterBubbleAttack : MonoBehaviour
{


    #region Filling and Deflating


    [SerializeField]
    float _fillspeed;

    [SerializeField]
    float _deflatespeed;

    [SerializeField]
    float _mBubblesize = 6.9f;

    float _cBubblesize = 0;
    
    Vector3 _maxBubbleSize, _currentBubbleSize;

    bool _initfill,_done;


    #endregion

    #region projectile


    [SerializeField]
    float _firingDelay;

    float _fireTimer;

    [SerializeField]
    GameObject _projectilePrefab;


    #endregion


    // Start is called before the first frame update
    void Start()
    {
        _maxBubbleSize = new Vector3(_mBubblesize, _mBubblesize, _mBubblesize);
        _currentBubbleSize = new Vector3(_cBubblesize, _cBubblesize, _cBubblesize);
        _initfill = true;
    }

    // Update is called once per frame
    void Update()
    {

        Fill();

        Shoot();

        Expire();
        
    }

    void Fill()
    {
        //initial fill
        if (_cBubblesize < _mBubblesize && _initfill == true)
        {
            _cBubblesize += _fillspeed;
            _currentBubbleSize = new Vector3(_cBubblesize, _cBubblesize, _cBubblesize);
            transform.localScale = _currentBubbleSize;
        }
    }


    void Shoot()
    {
        //shoot
        if (_cBubblesize > 0 && _initfill == false)
        {
            _cBubblesize -= _deflatespeed;

        }
        else
        {
            _done = true;
        }
    }

    void Expire()
    {
        if(_done == true)
        {
            Destroy(gameObject);
        }
    }


}
