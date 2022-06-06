using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    int LaserSpeed;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(0, LaserSpeed * Time.deltaTime, 0);
        Destroy(gameObject, 1.5f);
    }
}
