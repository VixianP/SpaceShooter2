using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectileScript : MonoBehaviour
{
    [SerializeField]
    float _bulletSpeed = 5;
    
    public int damage = 10;

    float timer_;



    public Vector2 moveDir_;

    Vector3 _playerPositon;

    public GameObject ObjectToFollow;


    //switches
    [SerializeField]
    bool _isTracking, _isHoming, _isStorming;

    [SerializeField]
    bool _hasExpiration;

    private void Start()
    {
        timer_ = Time.time + 2;
        _playerPositon = PlayerValues.playerGameobject.transform.position;

       
    }

    void Update()
    {
        //randomize firing direction
        TrackPlayer();
        Homeplayer();

        if (_hasExpiration == true)
        {
            Destroy(gameObject, 6.5f);
        }
    }

    void Storm()
    {
        if(_isStorming == true)
        {
            if(ObjectToFollow != null)
            {
                transform.RotateAround(transform.position, ObjectToFollow.transform.position, 20);
                transform.Translate(Vector3.right * _bulletSpeed * Time.deltaTime);
            }
        }
    }
    void TrackPlayer()
    {
        if (_isTracking == true)
        {
            transform.Translate(_playerPositon * _bulletSpeed * Time.deltaTime);
        }
    }

    void Homeplayer()
    {
        if (_isHoming == true)
        {
            if (Time.time < timer_)
            {
                moveDir_ = Vector2.Lerp(transform.position, PlayerValues.playerGameobject.transform.position, 0.2f * Time.deltaTime);
                transform.position = moveDir_;
            }
            else
            {
                transform.Translate(PlayerValues.playerGameobject.transform.position * 2 * Time.deltaTime);
                Destroy(gameObject, 1.5f);

                //other effect, stop and explode
            }
        }
    }
}
