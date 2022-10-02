using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingShotScript : MonoBehaviour
{
    [SerializeField]
    int LaserSpeed;

    public int _laserDamageAmount = 5;

    #region Targeting
    private GameObject _player;

    private GameObject _target;

    Collider2D[] _enemiesInRange;

    private int _selection;
    
    [SerializeField]
    Vector2 _detectionRange;

    bool _checked;
    #endregion

    #region Movement
    Vector3 _dir;
    Vector3 _rotDir;
    #endregion

    void Update()
    {
        LockOn();
        Movement();
        Destroy(gameObject, 2.5f);
    }

    void LockOn()
    {
        if (_target == null)
        {
            if (_checked == false)
            {
                _enemiesInRange = Physics2D.OverlapCapsuleAll(transform.position, _detectionRange, CapsuleDirection2D.Horizontal, 0);
                //for loop to check if enemy is less than X range of player. if it is, then choose whatever comes first. if its dead, rechoose
            }
            if(_enemiesInRange[_selection] != null)
            {

                _target = _enemiesInRange[_selection].gameObject;

                _selection = 0;

                _checked = true;

            }
            else
            {
                _selection++;
            }
            //if loop, increment is not in range or alive
        }
    }

    void Movement()
    {
        if (_checked == true)
        {
            _player = PlayerValues.playerGameobject;
            _dir = _player.transform.position - transform.position;
            _rotDir = Vector3.RotateTowards(transform.forward, _dir, LaserSpeed * Time.deltaTime, 0.0f);

            transform.rotation = Quaternion.LookRotation(_rotDir);

            //move towards _target
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
