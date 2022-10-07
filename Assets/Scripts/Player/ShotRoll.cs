using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotRoll : MonoBehaviour
{
    Player _player;
    private void Start()
    {
        _player = PlayerValues.playerGameobject.GetComponent<Player>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if(collision.gameObject.tag == "Enemy")
        {
            _player.RollDice();
            Destroy(gameObject);
        }

    }
}
