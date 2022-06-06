using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region Player Stats
    [SerializeField]
    private int speed;
    [SerializeField]
    int Health;
    public int PlayerHealth
    {
        get
        {
            return Health;
        }
        set
        {
            Health = value;
            if (PlayerHealth < 1)
            {
                Destroy(gameObject);
            }
        }
    }

    [SerializeField]
    float FiringSpeed;
    float FiringTimer = -1f;
    #endregion
    #region Projectile
    GameObject BaseProjectile;
    [SerializeField]
    GameObject Laser;
    float WeaponTimer;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        BaseProjectile = Laser;
        PlayerHealth = Health;
    }

    // Update is called once per frame
    void Update()
    {
        PlayerInputs();
        Movement();
        TemporaryPowerUptimer();
    }
    void Movement()
    {
        float h = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        float v = Input.GetAxis("Vertical") * speed * Time.deltaTime;
        if(transform.position.x > 37)
        {
            transform.position = new Vector3(-37, transform.position.y, 0);
        } else if (transform.position.x < -37)
        {
            transform.position = new Vector3(37, transform.position.y, 0);
        }
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -18, 19),0);
        transform.Translate(h, v, 0);
    }
    void PlayerInputs()
    {
        if ( Time.time > FiringTimer)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Instantiate(Laser, new Vector3(transform.position.x, transform.position.y + 2, 0), Quaternion.identity);
                FiringTimer= Time.time + FiringSpeed;
            }
        } 
    }
    public void TakeDamage(int value)
    {
        PlayerHealth -= value;
        print(PlayerHealth);
    }
    void ProjectilePowerUp(GameObject Projectile, int Timer)
    {
        WeaponTimer = Timer + Time.time;
        Laser = Projectile;
    }
    void TemporaryPowerUptimer()
    {
        if(Time.time > WeaponTimer)
        {
            Laser = BaseProjectile;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "TempPowerUp")
        {
            PowerUpScript PowUp = collision.gameObject.GetComponent<PowerUpScript>();
            int Selector = Random.Range(0, PowUp.PowerUp.Count);
            Laser = PowUp.PowerUp[Selector].Projectile;
            WeaponTimer = PowUp.PowerUp[Selector].PowerUpDuration + Time.time;
        }
    }
}
