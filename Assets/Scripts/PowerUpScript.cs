using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PowerUps
{
    
    public GameObject Projectile;
    public GameObject SuperKProjectile;
    public GameObject SuperKComboProjectile;
    public GameObject SuperKFrontProjectile;
    public GameObject SuperKPairProjectile;

    public int PowerUpDuration;
    public int Damage;
    public string Category;
    public string Type;
    public string CallMethod;
    public Sprite PowerUpImage;
    //category
    //type temp or perm
    //sprite to show on sprite renderer

    public PowerUps(GameObject Projectile, GameObject SuperKProjectile, GameObject SuperKComboProjectile,GameObject SuperKFrontProjectile, GameObject SuperKPairProjectile, int PowerUpDuration, int Damage,string Category,string Type,string CallMethod, Sprite PowerUpImage)
    {
        this.Projectile = Projectile;
        this.SuperKProjectile = SuperKProjectile;
        this.SuperKComboProjectile = SuperKComboProjectile;
        this.SuperKFrontProjectile = SuperKFrontProjectile;
        this.SuperKPairProjectile = SuperKPairProjectile;

        this.PowerUpDuration = PowerUpDuration;
        this.Damage = Damage;
        this.Category = Category;
        this.Type = Type;
        this.CallMethod = CallMethod;
        this.PowerUpImage = PowerUpImage;

    }
}
public class PowerUpScript : MonoBehaviour
{
    
    public List<PowerUps> PowerUp = new List<PowerUps>();
    
    public int PowerUpSelector = 2;

    [SerializeField]
    int PowerUpMoveDownSpeed = 3;

    [SerializeField]
    GameObject _player;

    Vector3 _moveDir;

    bool _isCollecting;

    [SerializeField]
    bool _testMode;

    [SerializeField]
    float _distance;
    private void Start()
    {
        _moveDir = new Vector3(0, -PowerUpMoveDownSpeed, 0);

        if (_testMode == false)
        {
            PowerUpSelector = Random.Range(0, PowerUp.Count);
        }

        if(gameObject.GetComponent<SpriteRenderer>().sprite == null && PowerUp[PowerUpSelector].PowerUpImage != null)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = PowerUp[PowerUpSelector].PowerUpImage;
        }

        if(_player == null)
        {
            _player = GameObject.FindWithTag("Player");
        }
    }
    private void Update()
    {
        _distance = Vector3.Distance(transform.position, _player.transform.position);
        MoveDown();
        Collect();
    }
    void MoveDown()
    {
        if (_isCollecting == false)
        {
            transform.Translate(_moveDir * Time.deltaTime);
        } else
        {
            transform.position = _moveDir;
        }
        Collect();
    }
    void Collect()
    {
        if (Input.GetKey(KeyCode.C))
        {
            if(_player != null && Vector3.Distance(transform.position,_player.transform.position) < 30)
            {
                _isCollecting = true;
                _moveDir = Vector3.Lerp(transform.position, _player.transform.position, 0.005f);
            }
        }
        else
        {
            _isCollecting = false;
            _moveDir = new Vector3(0, -PowerUpMoveDownSpeed, 0);
        }
    }
}
