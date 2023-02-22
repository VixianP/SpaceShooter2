using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingShotScript : MonoBehaviour
{
    [SerializeField]
    int LaserSpeed;

    public int _laserDamageAmount = 5;

    [SerializeField]
    float _lifetime = 8;

    #region Targeting

    //initial player target
    private Player _player;

    bool _lock;

    //retarget
    [SerializeField]
    Vector2 _size;

    LayerMask _mask;

    //retarget
    private Collider2D _newTarget;


    #endregion

    #region Movement
    Vector3 _dir;
    Vector3 _rotDir;
    #endregion



    private void Awake()
    {
        if(PlayerValues.PlayerIsDead == false)
        {
            _player = PlayerValues.playerGameobject.GetComponent<Player>();
            _mask = LayerMask.GetMask("Enemy");

        }
    }

    void Update()
    {
        Movement();
        Retarget();
        Destroy(gameObject, _lifetime);
    }

    void Retarget()
    {

        //cannot target to what player is shooting, find a different target
        if (_newTarget == null && _lock == true)
        {

            _newTarget = Physics2D.OverlapCapsule(transform.position, _size, CapsuleDirection2D.Horizontal, 0, _mask);

            if (_newTarget == null && _player._lockOn == null)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
                transform.Translate(Vector3.up * LaserSpeed * Time.deltaTime);
            }

        }

        if (_newTarget != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, _newTarget.transform.position, LaserSpeed * Time.deltaTime);

            _dir = _newTarget.transform.position - transform.position;

            Debug.DrawRay(transform.position, _dir, Color.green);

            float angle = Mathf.Atan2(_dir.y, _dir.x) * Mathf.Rad2Deg + 90;

            Quaternion _angleAxis = Quaternion.AngleAxis(angle, Vector3.forward);

            transform.rotation = Quaternion.Slerp(transform.rotation, _angleAxis, Time.deltaTime * 50);
        }

    }

    void Movement()
    {

        //if player hits a target, go to it
            if (_player._lockOn != null && _player != null && _lock ==false)
            {

                transform.position = Vector3.MoveTowards(transform.position, _player._lockOn.transform.position, LaserSpeed * Time.deltaTime);

                _dir = _player._lockOn.transform.position - transform.position;

                Debug.DrawRay(transform.position, _dir, Color.green);

                float angle = Mathf.Atan2(_dir.y, _dir.x) * Mathf.Rad2Deg + 90;

                Quaternion _angleAxis = Quaternion.AngleAxis(angle, Vector3.forward);

                transform.rotation = Quaternion.Slerp(transform.rotation, _angleAxis, Time.deltaTime * 20);


            }
            //target lost
            else
            {

            _lock = true;

            }       

    }

 

    private void OnTriggerEnter2D(Collider2D colli)
    {
        if (colli.tag == "Enemy")
        {
            colli.gameObject.SendMessage("EnemyTakeDamage", _laserDamageAmount);
            Destroy(gameObject);
        }
    }
}
