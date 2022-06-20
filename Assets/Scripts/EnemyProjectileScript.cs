using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectileScript : MonoBehaviour
{
    [SerializeField]
    float _bulletSpeed = 5;
    
    public int damage = 10;

    float timer_;



    Vector2 moveDir_;

    Vector2 _playerPositon;
    private void Start()
    {
        timer_ = Time.time + 2;
        _playerPositon = PlayerValues.playerGameobject.transform.position;
    }

    void Update()
    {
        transform.Translate(Vector2.down * _bulletSpeed * Time.deltaTime);
    }
    void Fire()
    {
        if (transform.position.y < 4)
        {
            transform.Translate(_playerPositon * _bulletSpeed * Time.deltaTime);
        } else
        {
            
        }
    }
    void FollowePlayer()
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
