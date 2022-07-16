using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingBackgrounds : MonoBehaviour
{
    [SerializeField]
    int Limit = -70;

    [SerializeField]
    int _spawnPoint = 70;

    [SerializeField]
    float _initalSpeed;

    public float MovementSpeed =5;

    float _baseSpeed;

    [SerializeField]
    bool _isOverHead;

    bool _canMove = true;

    public bool ToDestroy;

    private void Start()
    {
        _baseSpeed = MovementSpeed;

        //MovementSpeed = _initalSpeed;

        //start speed down coroutine
    }

    // Update is called once per frame
    void Update()
    {
        OverHeadReposition();
        if (_canMove == true)
        {
            transform.Translate(0, -MovementSpeed * Time.deltaTime, 0);

            Boundary();

        }
    }
    //create spawn behind, down the road

    void OverHeadReposition()
    {
        if (_isOverHead)
        {
            if (transform.position.x == 0)
            {
                transform.position = new Vector2(Random.Range(Mathf.Max(-60, -40), Mathf.Max(60, 40)), _spawnPoint);
            }

            transform.Translate(0, -MovementSpeed * Time.deltaTime, 0);

            if (transform.position.y < Limit)
            {
                transform.position = new Vector2(Random.Range(Mathf.Max(-60,-40), Mathf.Max(60, 40)), _spawnPoint);
                return;
            }
        }
    }

    void Boundary()
    {
        if (ToDestroy == false)
        {
            if (transform.position.y < Limit)
            {
                transform.position = new Vector2(transform.position.x, _spawnPoint);
            }
        } else
        {
            if (transform.position.y < Limit)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            collision.SendMessage("TakeDamage", 100);
        }
        /*
        if(collision.tag == "SuperKamio")
        {
            Destroy(collision.gameObject);
        }
        */
    }



    IEnumerator SpeedOut()
    {
        //decrease speed to base speed
        yield return new WaitForSeconds(0.1f);

    }
}
