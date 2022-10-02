using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectileScript : MonoBehaviour
{
    [SerializeField]
    float _bulletSpeed = 5;
    
    [SerializeField]
    public int damage = 10;

    [SerializeField]
    float _lifeSpan;

    [SerializeField]
    float _flowerDelay;

    float timer_;



    public Vector2 moveDir_;

    Vector3 _playerPositon;

    public GameObject ObjectToFollow;


    //switches
    [SerializeField]
    bool _isTracking, _isHoming, _isStorming, _isFan;

    [SerializeField]
    bool _hasExpiration;

    private void Start()
    {
        timer_ = Time.time + _flowerDelay;
        _playerPositon = PlayerValues.playerGameobject.transform.position;

       
    }

    void Update()
    {
        //randomize firing direction
        TrackPlayer();
        Homeplayer();
        Storm();
        Fan();
        if (_hasExpiration == true)
        {
            Destroy(gameObject, _lifeSpan);
        }
    }

    void Storm()
    {
        if(_isStorming == true)
        {
            if (Time.time < timer_)
            {
                moveDir_ = Vector2.Lerp(transform.position, PlayerValues.playerGameobject.transform.position, 0.3f * Time.deltaTime);
                transform.position = moveDir_;
            }
            else
            {
                transform.Translate(_playerPositon  * Time.deltaTime);
                Destroy(gameObject, _lifeSpan);

                //other effect, stop and explode
            }
        }
    }
    void TrackPlayer()
    {
        if (_isTracking == true)
        {
            transform.Translate(_playerPositon * _bulletSpeed * Time.deltaTime);

            //transform.position = _playerPositon * _bulletSpeed * Time.deltaTime;
        }
    }

    void Homeplayer()
    {
        if (_isHoming == true)
        {
            if (Time.time < timer_)
            {
                moveDir_ = Vector2.Lerp(transform.position, PlayerValues.playerGameobject.transform.position, 1f * Time.deltaTime);
                transform.position = moveDir_;
            }
            else
            {
                transform.Translate(0,_bulletSpeed * Time.deltaTime,0);
                Destroy(gameObject, _lifeSpan);

                //other effect, stop and explode
            }
        }
    }
    void Fan()
    {
        if (_isFan)
        {
            transform.Translate(moveDir_ *_bulletSpeed * Time.deltaTime);
        }
    }
}
