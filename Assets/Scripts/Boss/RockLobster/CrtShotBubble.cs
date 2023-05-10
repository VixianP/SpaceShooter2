using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrtShotBubble : MonoBehaviour
{

    [SerializeField]
    int _damage;

    [SerializeField]
    float _speed = 3;

    [SerializeField]
    float _boundaries = -80;

    #region CoolDowns

    [SerializeField]
    float _cooldownDelay = 1;
    float _cooldownTimer;


    #endregion

    #region Projectiles

    [SerializeField]
    GameObject _leftBubble;

    [SerializeField]
    GameObject _rightBubblle;

    #endregion

    //offset

    // Update is called once per frame
    void Update()
    {
        Movement();

        Trail();
        OutofBounds();
    }

    void Movement()
    {
        transform.Translate(0, -_speed * Time.deltaTime, 0);
    }

    void Trail()
    {
        if(Time.time > _cooldownTimer)
        {
            Instantiate(_leftBubble, transform.position, Quaternion.identity);
            Instantiate(_rightBubblle, transform.position, Quaternion.identity);
            _cooldownTimer = Time.time + _cooldownDelay;
        }
    }

    void OutofBounds()
    {
        if(transform.position.y < _boundaries)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if(coll.tag == "Player")
        {
            coll.gameObject.SendMessage("CollisionDmg", _damage);
        }
    }
}
