using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpOrbSpawner : MonoBehaviour
{


    [SerializeField]
    private int _amountOfExpToDrop;

    [SerializeField]
    GameObject _orbTospawn;

    float _spawnTimer;

    [SerializeField]
    float _SpawnTimeDelay = 1.2f;

    private void Start()
    {
        _spawnTimer = Time.time + _SpawnTimeDelay;
    }

    // Update is called once per frame
    void Update()
    {
        if(_orbTospawn != null)
        {
            if(_amountOfExpToDrop > 0)
            {
                Instantiate(_orbTospawn, transform.position, Quaternion.identity);
                _spawnTimer = Time.time + _SpawnTimeDelay;
                _amountOfExpToDrop--;
            } else
            {
                Destroy(gameObject);
            }
         
        }
    }
}
