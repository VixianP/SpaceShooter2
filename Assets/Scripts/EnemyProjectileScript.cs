using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectileScript : MonoBehaviour
{

    
    public int damage = 10;

    float timer_;

    Vector2 moveDir_;
    private void Start()
    {
        timer_ = Time.time + 2;
        //vector to get players last known position
    }

    void Update()
    {
        if (Time.time < timer_)
        {
            moveDir_ = Vector2.Lerp(transform.position, PlayerValues.playerGameobject.transform.position, 0.5f * Time.deltaTime);
            transform.position = moveDir_;
        } else
        {
            transform.Translate(PlayerValues.playerGameobject.transform.position * 2 * Time.deltaTime);
            Destroy(gameObject, 1.5f);

            //other effect, stop and explode
        }
    }
}
