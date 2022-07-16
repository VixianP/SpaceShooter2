using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    int LaserSpeed;

   
    public int _laserDamageAmount = 5;


    void Update()
    {
        transform.Translate(0, LaserSpeed * Time.deltaTime, 0);
        Destroy(gameObject, 1.5f);
    }

    private void OnTriggerEnter2D(Collider2D colli)
    {
        if(colli.tag == "Enemy")
        {
            colli.gameObject.SendMessage("EnemyTakeDamage", _laserDamageAmount);
        }
    }
}
