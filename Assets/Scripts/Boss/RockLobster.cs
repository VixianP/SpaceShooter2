using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockLobster : MonoBehaviour
{

    [SerializeField]
    float _amplitude = 2;


    //position
    [SerializeField]
    Vector2 _positionToAnchor;

    Vector2 _pos;


    //States
    bool _isAttacking;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    void Movement()
    {
        _pos.y = 22;
        _pos.x = Mathf.Cos(Time.time) * -_amplitude;
        transform.position = _pos;
    }
}
