using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpOrbScript : MonoBehaviour
{

    Collider2D _newTarget;
    LayerMask _mask;

    [SerializeField]
    Vector2 _aggroRange;

    float _scantimer;
    float _scanDelay = 0.5f;

    public int ExpValue = 5;

    private int movementDirection = 30;

    float timer;
    [SerializeField]
    float maxTime;

    Vector3 _moveDirection;
    // Start is called before the first frame update
    void Start()
    {
        _scantimer = Time.time + _scanDelay;
        timer = Time.time + maxTime;
        _moveDirection = new Vector3(Random.Range(-movementDirection, movementDirection + 15), 
                                     Random.Range(-movementDirection, movementDirection + 15), 
                                     0);

        _mask = LayerMask.GetMask("Player");
        Destroy(gameObject, 5);
    }

    // Update is called once per frame
    void Update()
    {
        //inital spawn
        if(timer > Time.time)
        {
            transform.Translate(_moveDirection * Time.deltaTime);
        } else
        {
            transform.Translate(0,-5 * Time.deltaTime, 0);
        }


        if(Time.time > _scantimer && _newTarget == null)
        {

            _newTarget = Physics2D.OverlapCapsule(transform.position, _aggroRange, CapsuleDirection2D.Horizontal, 0, _mask);
            _scantimer = Time.time + _scanDelay;
        }

        if(_newTarget != null)
        {
            transform.position = Vector3.MoveTowards(transform.position,PlayerValues.playerGameobject.transform.position, movementDirection * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag == "Obstacle")
        {
            Destroy(gameObject);
        }
    }
}
