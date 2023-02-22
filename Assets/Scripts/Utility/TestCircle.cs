using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCircle : MonoBehaviour
{

    [SerializeField]
    List<Collider2D> _objectCaught = new List<Collider2D>();

    [SerializeField]
    Collider2D _lookForTarget;

    [SerializeField]
    Collider2D _newTarget;

    [SerializeField]
    Vector2 _size;

    [SerializeField]
    float _radius;

    LayerMask mask;

    private void Start()
    {
        mask = LayerMask.GetMask("Enemy");
    }

    // Start is called before the first frame update
    void Update()
    {
        if (_newTarget == null)
        {
            //physics collider in a list
            _lookForTarget = Physics2D.OverlapCapsule(transform.position, _size, CapsuleDirection2D.Horizontal,0,mask);
            if(_lookForTarget != null)
            {
                _newTarget = _lookForTarget;
            }
        }
    }



}
