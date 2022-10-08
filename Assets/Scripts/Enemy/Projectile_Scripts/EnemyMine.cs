using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMine : MonoBehaviour
{
    [SerializeField]
    float _explosionDelay;

    float _timeToExplode;

    [SerializeField]
    GameObject _explosionProjectile;

    private void Start()
    {
        _timeToExplode = Time.time + _explosionDelay;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= _timeToExplode)
        {
            Instantiate(_explosionProjectile, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
   
}
