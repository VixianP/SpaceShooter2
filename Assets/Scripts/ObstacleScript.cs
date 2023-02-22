using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleScript : MonoBehaviour
{

    [SerializeField]
    GameObject _destoyedPowerUp;

    float Limit;

    [SerializeField]
    float _spawnPoint;

    [SerializeField]
    float MovementSpeed = 30;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        transform.Translate(0, -MovementSpeed * Time.deltaTime, 0);

    }

    void Boundary()
    {

            if (transform.position.y < Limit)
            {
                transform.position = new Vector2(transform.position.x, _spawnPoint);
            }
        
        else
        {
            if (transform.position.y < Limit)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            //Instantiate(_destoyedPowerUp, collision.transform.position, Quaternion.identity);
            collision.SendMessage("TakeDamage", 1000);
        }

        if (collision.tag == "PowerUp")
        {
            //Instantiate(_destoyedPowerUp, collision.transform.position, Quaternion.identity);
            Destroy(collision.gameObject);
        }

        if (collision.tag == "EnemyBullet")
        {
            Destroy(collision.gameObject);
        }

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            //Instantiate(_destoyedPowerUp, collision.transform.position, Quaternion.identity);
            collision.SendMessage("TakeDamage", 1000);
        }

        if (collision.tag == "PowerUp")
        {
            //Instantiate(_destoyedPowerUp, collision.transform.position, Quaternion.identity);
            Destroy(collision.gameObject);
        }
    }
}
