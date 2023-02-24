using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperKSideAttachement : MonoBehaviour
{
    GameObject _superKGameObject;
    SuperK _superKScript;

    [SerializeField]
    bool _isLeft;

    [SerializeField]
    bool _isRight;

    private void Start()
    {
        if(_superKGameObject != null)
        {
            _superKScript = _superKGameObject.GetComponent<SuperK>();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            if(_isLeft == true)
            {

            }
            if(_isRight == true)
            {

            }
        }
    }
}
