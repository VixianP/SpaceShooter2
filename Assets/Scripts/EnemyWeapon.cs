using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    [SerializeField]
    float timeBetweenShots_ =2;
    [SerializeField]
    GameObject enemyProjectile_;

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(PlayerValues.playerGameobject.transform.position);
        if (Time.time > timeBetweenShots_)
        {
            Instantiate(enemyProjectile_, transform.position, Quaternion.identity);
            timeBetweenShots_ += Time.time + 2;
        }

    }
}
