using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetMine : MonoBehaviour
{

    GameObject _player;

    Vector2 _playerPos;

    [SerializeField]
    Vector2 _PlayerposOffset;

    [SerializeField]
    GameObject _mineGameObject;

    // Start is called before the first frame update
    void Start()
    {
        if(PlayerValues.PlayerIsDead == false)
        {
            _player = PlayerValues.playerGameobject;
            _playerPos = new Vector2(_player.transform.position.x + _PlayerposOffset.x, 
                                     _player.transform.position.y + _PlayerposOffset.y);

        }
    }

    // Update is called once per frame
    void Update()
    {
        SetTrap();
    }

    void SetTrap()
    {
        if(_player != null)
        {
            transform.position = Vector2.MoveTowards(transform.position,_playerPos, 3f);
            if(Vector2.Distance(transform.position, _playerPos) == 0)
            {
                Instantiate(_mineGameObject, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
        }
    }
}
