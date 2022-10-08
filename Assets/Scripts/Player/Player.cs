using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TMPro;

//always lock on 0

// 0 is the highest threat

//list will sort based on proxmity

//competitive system of range
//if enemy reaches a certain range threshold, it will decrement itself in the list and increment the object

public class Player : MonoBehaviour
{
    static GameObject PlayerGameObject; //will be used for later

    #region Player Stats
    [SerializeField]
    public float _PlayerDmg = 1;


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


    bool CanBoost;

    bool IsBoosting;

    [HideInInspector]
    public bool IsDodging;

    [SerializeField]
    Slider _dashSlider;
    #endregion

    #region Projectile

    //fire delay
    [SerializeField]
    float FiringSpeed;

    float FiringTimer = -1f;




    //regular shot and charged shot
    [SerializeField]
    GameObject[] BaseProjectile;

    [SerializeField]
    GameObject Laser;

    [SerializeField]
    GameObject ChargedLaser;

    [Range(0,1)]
    float ChargeTimer;

    [SerializeField]
    Slider _chargedShotBar;

    [SerializeField]
    Slider _chargedShotCoolDownTimerSlider;




    //timer for temporary power ups
    float WeaponTimer;

    [SerializeField]
    Text _ammoText;

    float _maxAmmo = 13;

    float _currentAmmoCount;


    [SerializeField]
    Slider _reloadBar;

    bool _isReloading;

    float _timeBetweenReloads;

    [SerializeField]
    Slider _reloadTimerBar;

    [SerializeField]
    GameObject _instandReloadBarDisplay;




    //Chance to Activate Ability on hit
    [SerializeField]
    bool isHoming;

    [SerializeField]
    float _homingRocketCoolDown;
    float _homingRocketCoolDownTime;
    [SerializeField]
    int _roll;




    //Targeting For Homing Shot
    public GameObject _lockOn;
    [SerializeField]
    GameObject _homingShot;


    bool _instantReloadAttempt;

    #endregion

    #region Abilities
    [SerializeField]

    GameObject PlayerShield;

    [SerializeField]
    int MaxShieldHealth;

    int _shieldHealth;

    float ShieldDuration;

    Animator _shieldAnim;

    [SerializeField]
    Slider shieldSlider;

    #endregion

    #region Experience and Level up

    [SerializeField]
    Slider _experienceBar;

    [SerializeField]
    TextMeshProUGUI _experienceText;

    [SerializeField]
    TextMeshProUGUI _playerCurrentLevelText;

    [SerializeField]
    int _experienceScaler;

    [SerializeField]
    private int ExperienceToLevel;

    private int experience;
    int CurrentLevel = 1;

    public int LevelUp
    {

        get
        {
            return CurrentLevel;
        }
        set
        {
            _levelUpText.gameObject.GetComponent<LevelUpNotification>().StartNotification();
            _statIncreaseText.GetComponent<LevelUpNotification>().StartNotification();
            //stat increases ie health,damage, and power up carrry capacity

            _PlayerDmg += 1;

            //heals the player to full health
            MaxHealth += 5;
            PlayerCurrentHealth = MaxHealth;

            PlayerHP.text = "HP " + PlayerCurrentHealth + "(" + _shieldHealth + ")" + "/" + MaxHealth;
            PlayerHpBar.value = PlayerCurrentHealth;

            CurrentLevel = value;

            ExperienceToLevel += _experienceScaler ;

            _experienceBar.maxValue = ExperienceToLevel;

        }
    }

    public  int Experience
    {
        get
        {
            return experience;
        }
        set
        {
         
            experience = value;


            if (value >= ExperienceToLevel)
            {

               

                //level up
                if (value == ExperienceToLevel)
                {
                    LevelUp += 1;

                    experience = 0;

                    _experienceBar.value = 0;

                    _playerCurrentLevelText.text = "LvL " + CurrentLevel.ToString();

                } 


                //over level
                while (experience> ExperienceToLevel)
                {
                        LevelUp += 1;

                        experience = value - ExperienceToLevel;

                    if (value < 0)
                    {
                        value = 0;
                    }

                    _experienceText.text = experience + "/" + ExperienceToLevel;
                    _experienceBar.value = experience;
                }



            } 
            
            if (value < ExperienceToLevel)
            {

                _experienceText.text = experience + "/" + ExperienceToLevel;
                _experienceBar.value = experience;
            }

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

    Text _powerUpDescription;

    Image _powerUpImage;


    //Level Up Notificiations


    [SerializeField]
    TextMeshProUGUI _levelUpText;

    [SerializeField]
    TextMeshProUGUI _statIncreaseText;


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

    #region Visuals and Aniation

    //the damage visuals
    [SerializeField]
    GameObject[] PlayerDamage;

    Animator _cameraAnim;
    #endregion

    #region The Super K

    [SerializeField]
    GameObject _superKGameObjectToSpawn;

    GameObject _theSuperKObject;

    SuperK SK;

    //instantiate super k
    //when death, destroy the super k
    #endregion

 

    private void Awake()
    {
        ResetPlayer();
    }

    void Start()
    {
        Initialize();
        SpawnSuperK();
    }


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
        Time.timeScale = 1;

        PlayerValues.playerGameobject = gameObject;

        _shieldAnim = PlayerShield.GetComponent<Animator>();

        BaseSpeed = speed;

        _currentAmmoCount = _maxAmmo;

        DashDistance = speed * MaxDashDistance;

        PlayerCurrentHealth = MaxHealth;
        PlayerCollider = gameObject.GetComponent<Collider2D>();
        PlayerSpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        PlayerAudio = GetComponent<AudioSource>();
        PlayerHP.text = "HP " + PlayerCurrentHealth + "(" + _shieldHealth + ")" + "/" + MaxHealth;
        PlayerHpBar.maxValue = MaxHealth;
        PlayerHpBar.value = MaxHealth;

        _cameraAnim = Camera.main.GetComponent<Animator>();

        _experienceText.text = experience + "/" + ExperienceToLevel;
        _experienceBar.value = experience;
        _experienceBar.maxValue = ExperienceToLevel;
    }

    void Movement()
    {
        float h = Input.GetAxis("Horizontal") * speed * Time.deltaTime;

        float v = Input.GetAxis("Vertical") * speed * Time.deltaTime;

        /*
        if(transform.position.x > 95)
        {

            transform.position = new Vector3(-95, transform.position.y, 0);

        } else if (transform.position.x < -95)
        {

            transform.position = new Vector3(95, transform.position.y, 0);

        }

        */

        transform.position = new Vector3(Mathf.Clamp(transform.position.x,-90,90), Mathf.Clamp(transform.position.y, -50,50),0);

        transform.Translate(h, v, 0);
    }

    void PlayerInputs()
    {

        if(Input.GetKeyDown(KeyCode.R) && _isReloading == false && _currentAmmoCount != _maxAmmo && Time.time > _timeBetweenReloads)
        {
                _timeBetweenReloads = Time.time + 5;

                _isReloading = true;

            _reloadTimerBar.value = _timeBetweenReloads;

            StartCoroutine(ReloadDelayTimer());
            StartCoroutine(ReloadingCouroutine());
        }


        if (SK._isattached == false)
        {
            if (_chargedShotCoolDownTimerSlider.value == 0 && Time.timeScale == 1 && _currentAmmoCount > 0)
            {
                if (Input.GetMouseButton(0) && _currentAmmoCount % 3 == 0)
                {
                    _isReloading = false;

                    _reloadBar.value = 0;

                    ChargeTimer += 1 * Time.deltaTime;

                    PlayerAudio.clip = SoundClips[0];

                    _chargedShotBar.value = ChargeTimer;

                    //play charging effect

                    return;

                }
                else if (Input.GetMouseButtonUp(0))
                {

                    if (ChargeTimer > .99f)
                    {
                        PlayerAudio.Play();

                        Instantiate(ChargedLaser, new Vector3(transform.position.x, transform.position.y + 2, 0), Quaternion.identity);

                        FiringTimer = Time.time + FiringSpeed + .30f;

                        ChargeTimer = 0;

                        _currentAmmoCount -= 3;

                        _ammoText.text = _currentAmmoCount.ToString() + "/" + _maxAmmo;

                        _chargedShotCoolDownTimerSlider.value = _chargedShotCoolDownTimerSlider.maxValue;

                        _chargedShotBar.value = 0;

                        StartCoroutine(ChargeShotCoolDown());

                        if (_currentAmmoCount < 1)
                        {
                            _ammoText.text = "Press R";
                        }

                    }
                    else
                    {
                        PlayerAudio.Play();

                        Instantiate(Laser, new Vector3(transform.position.x, transform.position.y + 2, 0), Quaternion.identity);

                        FiringTimer = Time.time + FiringSpeed;

                        ChargeTimer = 0;

                        _chargedShotBar.value = 0;

                        _currentAmmoCount -= 1;

                        _ammoText.text = _currentAmmoCount.ToString() + "/" + _maxAmmo;

                        if (_currentAmmoCount < 1)
                        {
                            _ammoText.text = "Press R"; ;
                        }

                    }
                }
            }
        }


        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (_dashSlider.value == 0)
            {
                StartCoroutine(Boost(DashDistance * .8f));
                
                _dashSlider.value = _dashSlider.maxValue;

                StartCoroutine(DashCoolDownCoroutine());

            }
        }
    }

    void SpawnSuperK()
    {
        _theSuperKObject = Instantiate(_superKGameObjectToSpawn, new Vector3(transform.position.x, transform.position.y + 6, transform.position.z),Quaternion.identity);
        SK = _theSuperKObject.GetComponent<SuperK>();
        SK.PlayerGameObject = gameObject;
        SK._isattached = true;

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

                _cameraAnim.SetTrigger("Shake");

                PlayerHpBar.value = PlayerCurrentHealth;

                PlayerHP.text = "HP " + PlayerCurrentHealth + "(" + _shieldHealth + ")" + "/" + MaxHealth;

            }

            else
            {
                
                if(value > _shieldHealth)
                {
                    _shieldHealth -= value;

                    PlayerHealth += _shieldHealth;

                    _shieldHealth = 0;

                    PlayerAudio.Play();

                    PlayerHP.text = "HP " + PlayerCurrentHealth + "(" + _shieldHealth + ")" + "/" + MaxHealth;

                    PlayerShield.SetActive(false);
                }


                _shieldHealth-= value;
                shieldSlider.value = _shieldHealth;

                ShieldEffects();


                if (_shieldHealth < 1)
                {
                    PlayerShield.SetActive(false);
                    _shieldHealth = 0;
                    PlayerHP.text = "HP " + PlayerCurrentHealth + "(" + _shieldHealth + ")" + "/" + MaxHealth;
                }

                if (_shieldHealth != 0)
                {
                    PlayerHP.text = "HP " + PlayerCurrentHealth + "<color=aqua>(</color>" + _shieldHealth + "<color=aqua>)</color>" + "/" + MaxHealth;
                }
                else
                {
                    PlayerHP.text = "HP " + PlayerCurrentHealth + "/" + MaxHealth;
                }

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
            PlayerHP.text = "HP " + PlayerCurrentHealth + "(" + _shieldHealth + ")" + "/" + MaxHealth;
        } else
        {

            if (CollDmg > _shieldHealth)
            {
                _shieldHealth -= CollDmg;

                PlayerHealth += _shieldHealth;

                _shieldHealth = 0;

                PlayerAudio.Play();

                MaxShieldHealth = 30;

                PlayerHpBar.value = PlayerCurrentHealth;
                PlayerHP.text = "HP " + PlayerCurrentHealth + "(" + _shieldHealth + ")" + "/" + MaxHealth;

                PlayerShield.SetActive(false);
            }

            _shieldHealth-=CollDmg;
            shieldSlider.value = _shieldHealth;
            ShieldEffects();

            if (_shieldHealth < 1)
            {
                PlayerShield.SetActive(false);
                MaxShieldHealth = 30;
                _shieldHealth = 0;
                PlayerHP.text = "HP " + PlayerCurrentHealth + "(" + _shieldHealth + ")" + "/" + MaxHealth;
            }
            if (_shieldHealth != 0)
            {
                PlayerHP.text = "HP " + PlayerCurrentHealth + "<color=aqua>(</color>" + _shieldHealth + "<color=aqua>)</color>" + "/" + MaxHealth;
            } else
            {
                PlayerHP.text = "HP " + PlayerCurrentHealth +  "/" + MaxHealth;
            }
            
        }
    }

    void ShieldEffects()
    {
        if (_shieldHealth < MaxShieldHealth * .7)
        {
            _shieldAnim.SetFloat("shieldAnimFloat", .7f);
        }
        if (_shieldHealth < MaxShieldHealth * .5)
        {
            _shieldAnim.SetFloat("shieldAnimFloat", .5f);
        }
        if (_shieldHealth < MaxShieldHealth * .3f)
        {
            _shieldAnim.SetFloat("shieldAnimFloat", .2f);
        }
    }

    //assigns the power up
    void ProjectilePowerUp(GameObject Projectile, int Timer)
    {
        WeaponTimer = Timer + Time.time;
        Laser = Projectile;
    }

    public void IncreaseAmmo()
    {
        _maxAmmo += 13;
        _currentAmmoCount = _maxAmmo;
        _ammoText.text = _currentAmmoCount.ToString() + "/" + _maxAmmo;
    }

    //keeps track of the Power up duration
    void TemporaryPowerUptimer()
    {
        if(Time.time > WeaponTimer)
        {
            Laser = BaseProjectile[0];
        }
    }

    void ActivateShield()
    {
        MaxShieldHealth += 30;
        _shieldHealth = MaxShieldHealth;
        shieldSlider.maxValue = MaxShieldHealth;
        shieldSlider.value = MaxShieldHealth;
        PlayerShield.SetActive(true);
        PlayerHP.text = "HP " + PlayerCurrentHealth + "<color=aqua>(</color>" + _shieldHealth + "<color=aqua>)</color>" + "/" + MaxHealth;
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

    void Heal()
    {
        PlayerCurrentHealth = MaxHealth;
        if (_shieldHealth > 0)
        {
            PlayerHP.text = "HP " + PlayerCurrentHealth + "<color=aqua>(</color>" + _shieldHealth + "<color=aqua>)</color>" + "/" + MaxHealth;
            PlayerHpBar.value = PlayerCurrentHealth;
        }
        else
        {
            PlayerHP.text = "HP " + PlayerCurrentHealth + "(" + _shieldHealth + ")" + "/" + MaxHealth;
            PlayerHpBar.value = PlayerCurrentHealth;
        }
    }

    void HealthBoost()
    {

    }

    IEnumerator ChargeShotCoolDown()
    {
        while (_chargedShotCoolDownTimerSlider.value > 0)
        {
            yield return new WaitForSeconds(0.1f);
            _chargedShotCoolDownTimerSlider.value -= 0.033f;
            if (_chargedShotCoolDownTimerSlider.value < 0)
            {
                _chargedShotCoolDownTimerSlider.value = 0;
            }
        }
    }
    IEnumerator ReloadDelayTimer()
    {
        while (Time.time < _timeBetweenReloads)
        {
            yield return new WaitForSeconds(0.1f);

            _reloadTimerBar.value -= 0.1f;
        }
    }

    IEnumerator ReloadingCouroutine()
    {
        while (_isReloading == true)
        {
            _reloadBar.value += 0.1f;

            _currentAmmoCount += Mathf.RoundToInt(_maxAmmo * .1f);

            _ammoText.text = _currentAmmoCount.ToString() + "/" + _maxAmmo;

            yield return new WaitForSeconds(0.1f);

            if(_currentAmmoCount >= _maxAmmo)
            {

                _currentAmmoCount = _maxAmmo;

                _reloadBar.value = 0;

                _ammoText.text = _currentAmmoCount.ToString() + "/" + _maxAmmo;

                _isReloading = false;

            }
            if(_reloadBar.value >= _reloadBar.maxValue)
            {

                _currentAmmoCount = _maxAmmo;

                _reloadBar.value = 0;

                _ammoText.text = _currentAmmoCount.ToString() + "/" + _maxAmmo;

                _isReloading = false;

            }
        }
    }

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

                }
            }
        }
    }

    IEnumerator DashCoolDownCoroutine()
    {
        while(_dashSlider.value > 0) 
        { 
        yield return new WaitForSeconds(0.2f);
        _dashSlider.value -= _dashSlider.maxValue * .10f;
        }
        if(_dashSlider.value == 0)
        {
            CanBoost = true;
        }

    }

    #region Abilities

    public void Targeting(GameObject EnemyToTarget)
    {
        _lockOn = EnemyToTarget;
    }

    public void RollDice()
    {
        _roll = Random.Range(0, 100);
        TestShot(_roll);

    }

    void TestShot(int roll)
    {
        if (isHoming == true)
        {
            if (roll > 50)
            {
                //spawn homing bullet aka Create it and link it to LockOm
                Instantiate(_homingShot, new Vector3(transform.position.x, transform.position.y + 2, 0), Quaternion.identity);


            }
        }
    }


    #endregion

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "EnemyBullet" && IsDodging == false)
        {
            
            EnemyProjectileScript Bullet = collision.gameObject.GetComponent<EnemyProjectileScript>();
            if(Bullet != null)
            {
                TakeDamage(Bullet.damage);
                Destroy(collision.gameObject);
            }
        }
        if (collision.tag == "PowerUp")
        {
            CallPowerUp(collision.gameObject);
        }
    }

    //checks what kind of power up it is
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
