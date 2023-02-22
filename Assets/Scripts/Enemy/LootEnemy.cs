using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootEnemy : MonoBehaviour
{

    [SerializeField]
    float _movementSpeed;

    float _baseSpeed;

    [SerializeField]
    GameObject _expOrbSpawner;

    [SerializeField]
    GameObject _powerUpToDrop;

    [SerializeField]
    int _dodgeDistance;

    bool _isDodging;
    bool _isboosting;

    bool _outOfBounds;

    SpawnManager _spawnManagerScript;


    void Start()
    {
        _spawnManagerScript = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        _baseSpeed = _movementSpeed;

    }

    // Update is called once per frame
    void Update()
    {
        MovementFunction();
        Boundaries();

    }

    void MovementFunction()
    {
        transform.Translate(_movementSpeed * Time.deltaTime, 0, 0);
    }

    void Boundaries()
    {
        if(transform.position.x > 90)
        {
            _outOfBounds = true;
            onDeath();
        }
    }

    void Dodge()
    {
        StartCoroutine(Boost(_dodgeDistance * 0.5f));
    }

    IEnumerator Boost(float RateToAmplifySpeed)
    {
        if (_movementSpeed < _dodgeDistance)
        {
            _isDodging = true;
            _isboosting = true;

            while (_isboosting == true)
            {
                yield return new WaitForSeconds(0.1f);
                _movementSpeed += RateToAmplifySpeed;
                if (_movementSpeed > _dodgeDistance)
                {
                    //_movementSpeed = _dodgeDistance;
                    StartCoroutine(Deccerlate(-_dodgeDistance * 0.8f));
                    _isboosting = false;
                }
            }
        }
    }

    IEnumerator Deccerlate(float boostnum)
    {
        if (_movementSpeed > _baseSpeed)
        {

            while (_movementSpeed > _baseSpeed)
            {

                yield return new WaitForSeconds(0.1f);

                _movementSpeed += boostnum;

                if (_movementSpeed < _baseSpeed)
                {

                    _movementSpeed = _baseSpeed;

                    _isDodging = false;

                }
            }
        }
    }

    void EnemyTakeDamage(int dmg)
    {
        Dodge();
    }

    void onDeath()
    {
        //if killed, award exp
        if (_outOfBounds == false)
        {
            if (_expOrbSpawner != null)
            {
                Instantiate(_expOrbSpawner, transform.position, Quaternion.identity);
            }
            _spawnManagerScript.EnemyDeath(gameObject, true);
        } else
        {
            //if not and went out of bounds, award nothing
            _spawnManagerScript.EnemyDeath(gameObject, true);
        }

    }

    private void OnTriggerEnter2D(Collider2D coll)
    {

        if(coll.tag == "ChargedShot")
        {

            Instantiate(_powerUpToDrop, transform.position, Quaternion.identity);
            onDeath();
        }
    }
}
