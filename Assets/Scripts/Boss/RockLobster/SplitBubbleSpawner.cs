using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitBubbleSpawner : MonoBehaviour
{

    //scale
    [SerializeField]
    GameObject _bubbleToSpawn;

    int _amountToSpawn = 2;


    float _damage;

    float _size;


    Vector3 _inhertedScaleSize;

    //movement
    Vector3 _movementDirection;

    [SerializeField]
    bool _passed;
    bool _merger;

    BubbleProjectile bubbleprojectilescript;


    // Start is called before the first frame update
    void Start()
    {
        if(_bubbleToSpawn != null)
        {
            bubbleprojectilescript = _bubbleToSpawn.GetComponent<BubbleProjectile>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        Spawn();
    }

    public void PassedVaules (float CurrentSize,float Damage, bool Merge)
    {
        //pass and adjust to half
        _size = CurrentSize * 0.5f;

        _inhertedScaleSize = new Vector3(_size,_size,_size);
        _damage = Damage * 0.5f;

        _merger = Merge;
        _passed = true;
    }

    void Spawn()
    {
        if (_passed == true)
        {
            if(_amountToSpawn == 2)
            {
                _movementDirection = new Vector3(10, -10, 0);
                bubbleprojectilescript.InjectValues(_size,_damage, _movementDirection, false);
                Instantiate(_bubbleToSpawn, transform.position, Quaternion.identity);
                _amountToSpawn = 1;
            }
            if (_amountToSpawn == 1)
            {
                _movementDirection = new Vector3(-10, -10, 0);
                bubbleprojectilescript.InjectValues(_size, _damage, _movementDirection, false);
                Instantiate(_bubbleToSpawn, transform.position, Quaternion.identity);
                _amountToSpawn = 0;
            }

            if(_amountToSpawn == 0)
            {
                Destroy(gameObject);
            }

        }
    }
}
