using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    int LaserSpeed;

    [SerializeField]
    float _projectileLifeSpan = 1.5f;

    public int _laserDamageAmount;

    //critical strike
    int _crtRoll;
    int _crtThreshold = 92;
    float _crtMultiplier = 1.5f;
    public float CrtDamage;




    Player _player;
    private void Awake()
    {
        if(PlayerValues.PlayerIsDead == false)
        {
            _player = PlayerValues.playerGameobject.GetComponent<Player>();
            _laserDamageAmount = Mathf.FloorToInt( _player._PlayerDmg + _laserDamageAmount);
        }
    }

    void Update()
    {
        transform.Translate(0, LaserSpeed * Time.deltaTime, 0);
        Destroy(gameObject, _projectileLifeSpan);
    }

    void Damage(GameObject Coll)
    {
        _player.RollDice();
        CrticialStike();
        _player.Targeting(Coll.gameObject);
        Coll.gameObject.SendMessage("EnemyTakeDamage", _laserDamageAmount);
        if (gameObject.tag != "ChargedShot")
        {
            Destroy(gameObject);
        }
    }

    void CrticialStike()
    {
        _crtRoll = Random.Range(0, 100);
        if (_crtRoll >= _crtThreshold)
        {
            CrtDamage = Mathf.FloorToInt(_player._PlayerDmg * _crtMultiplier);
            print("Critical Hit For :" + CrtDamage);
            print("Roll " + _crtRoll);
        }
    }


    private void OnTriggerEnter2D(Collider2D colli)
    {
        if(colli.tag == "Enemy")
        {
            Damage(colli.gameObject);
        }
    }
}
