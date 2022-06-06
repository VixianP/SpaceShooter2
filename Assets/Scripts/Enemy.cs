using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    int EnemyMoveSpeed = 4;
    [SerializeField]
    private int CollsionDamage = 1;

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
            Destroy(gameObject);
        }
        if(other.tag == "Player")
        {
            other.GetComponent<Player>().TakeDamage(CollsionDamage);
        }
    }
}
