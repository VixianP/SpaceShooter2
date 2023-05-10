using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombProjectileScript : MonoBehaviour
{

    [SerializeField]
    float _lifetime;

    [SerializeField]
    Vector2 _travelDirection;

    [SerializeField]
    int _projectileDamage;


    private void Start()
    {
        Destroy(gameObject, _lifetime);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(_travelDirection  * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if(coll.tag == "Player")
        {
            coll.gameObject.GetComponent<Player>().TakeDamage(_projectileDamage);
            Destroy(gameObject);
        }
 
    }
}
