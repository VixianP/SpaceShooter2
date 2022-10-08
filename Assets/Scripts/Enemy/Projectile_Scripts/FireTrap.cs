using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTrap : MonoBehaviour
{
    int _roll;

    [SerializeField]
    GameObject _trapBulletGameobject;

    // Start is called before the first frame update
    void Start()
    {
        _roll = Random.Range(0, 100);

        if(_roll > 70)
        {
            Instantiate(_trapBulletGameobject, transform.position, Quaternion.identity);
        }
    }

    

   
}
