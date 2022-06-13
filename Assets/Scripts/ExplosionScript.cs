using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionScript : MonoBehaviour
{
    [SerializeField]
    private Vector3 Scale;

    Animator ExplosionAnim;

    private void Start()
    {
        ExplosionAnim = gameObject.GetComponent<Animator>();
        gameObject.transform.localScale = Scale;
    }
    private void Update()
    {
      //checks if animation state is exit
    }
}
