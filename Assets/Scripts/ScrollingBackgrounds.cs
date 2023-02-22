using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingBackgrounds : MonoBehaviour
{

    float _currentSpeed;


    [SerializeField]
    float _baseSpeed = -10;

    [SerializeField]
    float _rushSpeed;


    #region Spawing

    bool _isSpawning = true;

    [SerializeField]
    Vector2 _startPos;

    [SerializeField]
    Vector2 _endPos;

    [SerializeField]
    int Limit = -200;

    #endregion
    #region Timer

    [SerializeField]
    private float SpawnDelay;
    private float DelayTimer;

    #endregion


    private void Start()
    {
        DelayTimer = Time.time + SpawnDelay;
        transform.position = _startPos;
    }


    void Update()
    {

        SpawnIn();
        transform.Translate(0, _currentSpeed* Time.deltaTime, 0);
        OutofBounds();

    }

    public void Respawn()
    {

        //speed out of bounds
        //wait
        //speed in

            transform.position = _startPos;
            if (transform.position.y > _endPos.y)
            {
                if (_currentSpeed > _rushSpeed)
                {
                    _currentSpeed -= 80;
                }
            }
            else
            {
                _currentSpeed = _baseSpeed;
            }
        
    }

    void SpawnIn()
    {
        if (_isSpawning == true && Time.time > DelayTimer)
        {
            if (transform.position.y > _endPos.y)
            {
                if(_currentSpeed > _rushSpeed)
                {
                    _currentSpeed -= 80;
                }
            } else
            {
                _isSpawning = false;
                _currentSpeed = _baseSpeed;
            }
        } 
        
            
        
    }

    void OutofBounds()
    {
        if (transform.position.y < Limit)
        {
            transform.position = _startPos;
        }
    }


}
