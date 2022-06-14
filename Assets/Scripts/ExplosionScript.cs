using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionScript : MonoBehaviour
{

    Animator ExplodeAnim;
    [SerializeField]
    Vector3 Size;
    [SerializeField]
    int MovementSpeed = 10;
    private void Start()
    {
        transform.localScale = Size;
        ExplodeAnim = gameObject.GetComponent<Animator>();
    }

    private void Update()
    {
        transform.Translate(0, MovementSpeed * Time.deltaTime, 0);
        if(ExplodeAnim.GetCurrentAnimatorStateInfo(0).IsName("Dead"))
        {
            Destroy(gameObject);
        }
    }

}
    
