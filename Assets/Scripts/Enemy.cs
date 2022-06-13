using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private int EnemyHealth;
    [SerializeField]
    int EnemyMoveSpeed = 4;
    [SerializeField]
    private int CollsionDamage = 1;

    #region Score,Value,Drops
    [SerializeField]
    private int PointValue;
    #endregion

    // Update is called once per frame
    void Update()
    {
        EnemyMovement();
    }
    void EnemyMovement()
    {
        transform.Translate(0, -EnemyMoveSpeed * Time.deltaTime, 0);
        if(transform.position.y < -12)
        {
            transform.position = new Vector3(Random.Range(18, -18), 12, 0);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Laser")
        {
            Destroy(other.gameObject);
            PlayerValues.Score += PointValue;
            //gives the player experiance
            //adds currency
            Destroy(gameObject);
        }
        if(other.tag == "Player")
        {
            other.GetComponent<Player>().TakeDamage(CollsionDamage * 50);
            //gives the player experiance
            //adds currency
            Destroy(gameObject);
        }
    }
}
