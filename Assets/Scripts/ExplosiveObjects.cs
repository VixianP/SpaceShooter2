using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveObjects : MonoBehaviour
{
    [SerializeField]
    GameObject _explosionFX;

    [SerializeField]
    Vector2 _explosionSize;


    [SerializeField]
    Collider2D[] _objectsInRange;

    private void OnTriggerEnter2D(Collider2D coll)
    {
        _objectsInRange = Physics2D.OverlapCapsuleAll(transform.position,_explosionSize,CapsuleDirection2D.Horizontal,0);
        for (int i = 0; i < _objectsInRange.Length; i++)
        {
           coll.SendMessage("Death");
            coll.SendMessage("TakeDamage", 100);
        }
        Instantiate(_explosionFX, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

}
