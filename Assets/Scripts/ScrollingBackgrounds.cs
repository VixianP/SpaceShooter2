using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingBackgrounds : MonoBehaviour
{
    [SerializeField]
    int Limit = -70;

    [SerializeField]
    int _spawnPoint = 70;

    
    public int MovementSpeed =5;

    [SerializeField]
    bool _isOverHead;

    bool _canMove = true;

    public bool ToDestroy;

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

    IEnumerator Speedin()
    {

        yield return new WaitForSeconds(0.1f);

    }

    IEnumerator SpeedOut()
    {

        yield return new WaitForSeconds(0.1f);

    }
}
