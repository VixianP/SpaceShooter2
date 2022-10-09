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

    //retarget
    private GameObject _newTarget;

    bool _locked;

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
        }
    }

    void Update()
    {
        Retarget();
        Movement();
        Destroy(gameObject, _lifetime);
    }

    void Retarget()
    {
        //if no target
        //retarget
        
    }


    void Movement()
    {

            if (_player._lockOn != null && _player != null && _locked == false)
            {

                transform.position = Vector3.MoveTowards(transform.position, _player._lockOn.transform.position, LaserSpeed * Time.deltaTime);

                _dir = _player._lockOn.transform.position - transform.position;

                Debug.DrawRay(transform.position, _dir, Color.green);

                float angle = Mathf.Atan2(_dir.y, _dir.x) * Mathf.Rad2Deg + 90;

                Quaternion _angleAxis = Quaternion.AngleAxis(angle, Vector3.forward);

                transform.rotation = Quaternion.Slerp(transform.rotation, _angleAxis, Time.deltaTime * 50);


            }
            else
            {

            //cannot target to what player is shooting, find a different target
            if (_newTarget == null)
            {
                _locked = true;
                transform.rotation = Quaternion.Euler(0, 0, 0);
                transform.Translate(Vector3.up * LaserSpeed * Time.deltaTime);
            } 

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
