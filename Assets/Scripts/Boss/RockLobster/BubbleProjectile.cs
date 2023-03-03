using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleProjectile : MonoBehaviour
{
    #region Stats
    [SerializeField]
    float _movementspeed;

    [SerializeField]
    float _damage;

    [SerializeField]
    Vector3 _dir;

    #endregion


    #region Scaling

    bool _grow = true;

    [SerializeField]
    float _growSpeed;

    float _currentsize;

    [SerializeField]
    float _maxSize = 8;

    Vector3 _sizeOfBubble;


    #endregion


    #region Splitting

    [SerializeField]
    GameObject _splitBubbleSpawner;

    SplitBubbleSpawner splitbubblespawnerscript;

    #endregion

    Collider2D _collider;


    [SerializeField]
    float _boundaries;


    private void Start()
    {
        splitbubblespawnerscript = _splitBubbleSpawner.GetComponent<SplitBubbleSpawner>();
        _collider = gameObject.GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(_dir * _movementspeed * Time.deltaTime);
        Scale();
        OutofBounds();
    }

    //used to continously split until its at 1
    public void InjectValues(float CurrentSize, float Damage, Vector3 Direction, bool Grow)
    {
        _currentsize = CurrentSize * 0.5f;
        _damage = Damage * 0.5f;
        _dir = Direction;

        //must be passed false always to stop growing
        _grow = Grow;
    }

    void Scale()
    {
        if (_grow == true)
        {
            if (_currentsize < _maxSize)
            {
                _currentsize += _growSpeed * Time.deltaTime;
                _sizeOfBubble = new Vector3(_currentsize, _currentsize, _currentsize);

                transform.localScale = _sizeOfBubble;
            }
        }
        
    }

    void OutofBounds()
    {
        
        if(transform.position.y < _boundaries)
        {
            Destroy(gameObject);
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if(collision.tag == "Player")
        {
            if (_currentsize > 2)
            {
                _collider.enabled = false;
                collision.gameObject.SendMessage("CollisionDmg", _damage);
                splitbubblespawnerscript.PassedVaules(_currentsize, _damage, true);
                Instantiate(_splitBubbleSpawner, transform.position, Quaternion.identity);
                Destroy(gameObject);
            } else
            {
                collision.gameObject.SendMessage("CollisionDmg", _damage);
                Destroy(gameObject);
            }
        }

        if(collision.tag == "Laser")
        {
            Destroy(collision.gameObject);
            _collider.enabled = false;
            splitbubblespawnerscript.PassedVaules(_currentsize, _damage, true);
            Instantiate(_splitBubbleSpawner, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        //if laser hits it, split into two that grows into half size
        //if that is hit then grown into 2 more that is smallest size
        

        //if hits super k, attach on
    }
}
