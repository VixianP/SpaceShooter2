using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    static GameObject PlayerGameObject; //will be used for later
    #region Player Stats
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
                PlayerDamage[0].SetActive(false);
                PlayerDamage[1].SetActive(false);
                PlayerDamage[2].SetActive(false);
                PlayerCollider.enabled = false;
                PlayerSpriteRenderer.enabled = false;
                return;
            }
            if (PlayerCurrentHealth <= MaxHealth * .7 && PlayerCurrentHealth > MaxHealth * .5)
            {
                PlayerDamage[0].SetActive(true);
                PlayerDamage[1].SetActive(false);
                PlayerDamage[2].SetActive(false);
                return;
            }
            if (PlayerCurrentHealth <= MaxHealth * .5 && PlayerCurrentHealth > MaxHealth * .3)
            {

                PlayerDamage[1].SetActive(true);
                PlayerDamage[2].SetActive(false);
                return;
            } else if(PlayerCurrentHealth <= MaxHealth * .10)
            {
                PlayerDamage[2].SetActive(true);
                return;
            }
        }
    }

    [SerializeField]
    float FiringSpeed;
    float FiringTimer = -1f;
    #endregion
    #region Speed and Dodging
    [SerializeField]
    private float speed;
    private float BaseSpeed;
    [SerializeField]
    float MaxDashDistance;
    float DashDistance;
    [SerializeField]
    float MaxSpeed;

    float DashCooldown;

    bool CanBoost;
    bool IsBoosting;
    [HideInInspector]
    public bool IsDodging;
    #endregion
    #region Projectile
    [SerializeField]
    GameObject[] BaseProjectile;
    [SerializeField]
    GameObject Laser;
    [SerializeField]
    GameObject ChargedLaser;
    float ChargeTimer;
    float WeaponTimer;
    #endregion
    #region Abilities
    [SerializeField]
    GameObject PlayerShield;
    [SerializeField]
    int MaxShieldHealth;
    int ShieldHealth;

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
    #region PlayerUI
    [SerializeField]
    Text PlayerHP;
    [SerializeField]
    Slider PlayerHpBar;
    #endregion
    #region Audio
    AudioSource PlayerAudio;
    [SerializeField]
    AudioClip[] SoundClips;
    /// <summary> Sound Clip Index
    /// 0 = Laser
    /// 1 = Player Damaged
    /// 3 = Power Up 
    /// 4 = Player Dash
    /// 5 = Player Death
    /// </summary>
    #endregion
    #region Instantiated objects and position
    [SerializeField]
    GameObject[] PlayerDamage;
    #endregion
    #region The Super K
    [SerializeField]
    GameObject SuperKGameObject;
    SuperK SK;

    //instantiate super k
    //when death, destroy the super k
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
        PlayerValues.playerGameobject = gameObject;
        SK = SuperKGameObject.GetComponent<SuperK>();
        SK.PlayerGameObject = gameObject;
        BaseSpeed = speed;
        DashDistance = speed * MaxDashDistance;
        PlayerCurrentHealth = MaxHealth;
        PlayerCollider = gameObject.GetComponent<Collider2D>();
        PlayerSpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        PlayerAudio = GetComponent<AudioSource>();
        PlayerHP.text = "HP " + PlayerCurrentHealth + "(" + shieldhealth + ")" + "/" + MaxHealth;
        PlayerHpBar.maxValue = MaxHealth;
        PlayerHpBar.value = MaxHealth;
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
        if ( Time.time > FiringTimer && Time.timeScale == 1)
        {
            if (Input.GetMouseButton(0))
            {
                PlayerAudio.clip = SoundClips[0];
                ChargeTimer += 1 * Time.deltaTime;
                //play charging effect
                //if charged fully, fire full charge
                return;
            } else if (Input.GetMouseButtonUp(0))
            {
                if (ChargeTimer > .90f)
                {
                    PlayerAudio.Play();
                    Instantiate(ChargedLaser, new Vector3(transform.position.x, transform.position.y + 2, 0), Quaternion.identity);
                    FiringTimer = Time.time + FiringSpeed + .30f;
                    ChargeTimer = 0;
                } else
                {
                    PlayerAudio.Play();
                    Instantiate(Laser, new Vector3(transform.position.x, transform.position.y + 2, 0), Quaternion.identity);
                    FiringTimer = Time.time + FiringSpeed;
                    ChargeTimer = 0;
                }
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            //superk
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (Time.time > DashCooldown)
            {
                StartCoroutine(Boost(DashDistance * .8f));
                DashCooldown = Time.time + 1.3f;
            }
        }
    }
    public void TakeDamage(int value)
    {
        if (IsDodging == false)
        {
            PlayerAudio.clip = SoundClips[1];
            if (PlayerShield.activeInHierarchy == false)
            {
                PlayerAudio.Play();
                PlayerHealth -= value;
                PlayerHpBar.value = PlayerCurrentHealth;
                PlayerHP.text = "HP " + PlayerCurrentHealth + "(" + shieldhealth + ")" + "/" + MaxHealth;
            }
            else
            {
                shieldhealth--;
            }
        }
    }
   public void CollisionDmg(int CollDmg)
    {
        if (PlayerShield.activeInHierarchy == false)
        {
            PlayerAudio.clip = SoundClips[1];
            PlayerAudio.Play();
            PlayerHealth -= CollDmg;
            PlayerHpBar.value = PlayerCurrentHealth;
            PlayerHP.text = "HP " + PlayerCurrentHealth + "(" + shieldhealth + ")" + "/" + MaxHealth;
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
            Laser = BaseProjectile[0];
        }
    }
    void ActivateShield()
    {
        MaxShieldHealth = +3;
        shieldhealth = MaxShieldHealth;
        PlayerShield.SetActive(true);
    }
    void SpeedBoost()
    {
        if (speed < MaxSpeed)
        {
            speed++;
        } else
        {
            speed = MaxSpeed;
            ActivateShield();
        }
    }
    //ieum for accelerate
    IEnumerator Boost(float RateToAmplifySpeed)
    {
        if (speed < DashDistance)
        {
            IsDodging = true;
            IsBoosting = true;
            while (IsBoosting == true)
            {
                yield return new WaitForSeconds(0.1f);
                speed += RateToAmplifySpeed;
                if (speed > DashDistance)
                {
                    speed = DashDistance;
                    CanBoost = false;
                    StartCoroutine(Deccerlate(-DashDistance * 0.5f));
                    IsBoosting = false;
                }
            }
        }
    }
    //ienum for decellerate
    IEnumerator Deccerlate(float boostnum)
    {
        if (speed > BaseSpeed)
        {
            while (CanBoost == false)
            {
                yield return new WaitForSeconds(0.1f);
                speed += boostnum;
                if (speed < BaseSpeed)
                {
                    speed = BaseSpeed;
                    IsDodging = false;
                    CanBoost = true;
                }
            }
        }
    }
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
            BaseProjectile[0] = PowUp.PowerUp[PowUp.PowerUpSelector].Projectile;
        }
        if (PowUp.PowerUp[PowUp.PowerUpSelector].Category == "Utility")
        {
            Invoke(PowUp.PowerUp[PowUp.PowerUpSelector].CallMethod,0.1f);
        }
        Destroy(PowerUpGameObject);
        
    }
    void ResetPlayer()
    {
        PlayerValues.PlayerIsDead = false;

        PlayerValues.Score = 0;

        PlayerValues.playerGameobject = null;
    }
}
