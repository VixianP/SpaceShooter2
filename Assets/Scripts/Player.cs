using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    static GameObject PlayerGameObject; //will be used for later
    #region Player Stats
    [SerializeField]
    private int speed;
    [SerializeField]
    private int Damage;
    [SerializeField]
    private int MaxHealth;
    private int PlayerCurrentHealth;
    public int PlayerHealth
    {
        get
        {
            return PlayerCurrentHealth;
        }
        set
        {
            PlayerCurrentHealth = value;
            if (PlayerHealth < 1)
            {
                PlayerValues.PlayerIsDead = true;
                print(PlayerValues.PlayerIsDead + " Player is dead equals ");
                PlayerCollider.enabled = false;
                PlayerSpriteRenderer.enabled = false;
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
    #region Abilities
    [SerializeField]
    GameObject PlayerShield;
    [SerializeField]
    int ShieldHealth = 3;

    int shieldhealth
    {
        get
        {
            return ShieldHealth;
        }
        set
        {
            if(ShieldHealth < 1)
            {
                PlayerShield.SetActive(false);
            }
            ShieldHealth = value;
        }
    }
    float ShieldDuration;
    #endregion
    #region Experience and Level up
    [SerializeField]
    private int experience;
    private int ExperienceToLevel;
    int ExperienceScaler;
    int CurrentLevel = 1;

    public int LevelUp
    {

        get
        {
            return CurrentLevel;
        }
        set
        {
            //stat increases ie health,damage, and power up carrry capacity
            //heals the player to full health
            MaxHealth += 5;
            PlayerCurrentHealth = MaxHealth;
            CurrentLevel = value;
        }
    }

    //level up
    public  int Experience
    {
        get
        {
            return experience;
        }
        set
        {
            if (value == ExperienceToLevel)
            {
                ExperienceToLevel = ExperienceScaler + value;
                CurrentLevel++;
            }
            experience = value;
        }

    }
    #endregion
    #region Colliders,Components,Other Scripts
    Collider2D PlayerCollider;
    SpriteRenderer PlayerSpriteRenderer;
    #endregion

    //level property where it alters other stats
    private void Awake()
    {
        ResetPlayer();
    }

    void Start()
    {
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerValues.PlayerIsDead == false)
        {
            PlayerInputs();
            Movement();
            TemporaryPowerUptimer();
        }
    }
    private void Initialize()
    {
        BaseProjectile = Laser;
        PlayerCurrentHealth = MaxHealth;
        PlayerCollider = gameObject.GetComponent<Collider2D>();
        PlayerSpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
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
        if (PlayerShield.activeInHierarchy == false)
        {
            PlayerHealth -= value;
            print(PlayerHealth);
        } else
        {
            shieldhealth--;
        }
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
    void ActivateShield()
    {
        PlayerShield.SetActive(true);
        print("shield");
    }
    //ieum for accelerate
    //ienum for decellerate
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "PowerUp")
        {
            CallPowerUp(collision.gameObject);
        }
    }
    void CallPowerUp(GameObject PowerUpGameObject)
    {
        PowerUpScript PowUp = PowerUpGameObject.gameObject.GetComponent<PowerUpScript>();

        if (PowUp.PowerUp[PowUp.PowerUpSelector].Category == "Weapon" && PowUp.PowerUp[PowUp.PowerUpSelector].Type == "Temp")
        {
            ProjectilePowerUp(PowUp.PowerUp[PowUp.PowerUpSelector].Projectile, PowUp.PowerUp[PowUp.PowerUpSelector].PowerUpDuration);
        }
        if (PowUp.PowerUp[PowUp.PowerUpSelector].Category == "Weapon" && PowUp.PowerUp[PowUp.PowerUpSelector].Type == "Perm")
        {
            BaseProjectile = PowUp.PowerUp[PowUp.PowerUpSelector].Projectile;
        }
        if (PowUp.PowerUp[PowUp.PowerUpSelector].Category == "Utility" && PowUp.PowerUp[PowUp.PowerUpSelector].Type == "Temp")
        {
            Invoke(PowUp.PowerUp[PowUp.PowerUpSelector].CallMethod,0.1f);
        }
        Destroy(PowerUpGameObject);
        
    }
    void ResetPlayer()
    {
        PlayerValues.PlayerIsDead = false;
        PlayerValues.Score = 0;
    }
}