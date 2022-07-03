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
    Vector3 _distanceBetweenPlayer;

    private void Start()
    {
        timer_ = Time.time + 2;
        _playerPositon = PlayerValues.playerGameobject.transform.position;
    }

    void Update()
    {
        transform.Translate(moveDir_ * _bulletSpeed * Time.deltaTime);
        Destroy(gameObject, 3);
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
