using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public bool isWalking = false;
    private GameManager gm;

    public float speedModifier;
    [SerializeField]
    private float _normalSpeedMod = 1f;

    public List<AudioClip> playerFootstepAudioClips;
    public AudioClip currentClip;
    public AudioSource audioSource;

    public AudioClip ShootFireballClip;
    public AudioClip impSummonClip;

    public float ticksUntilFootstep = 3f;
    public float ticksSinceLastFootstep = 0f;

    //public List<AudioClip> walkAudioClips;

    //    public AudioClip currentClip;
    //    public AudioSource audioSource;

    public float pitchMin = 0.8f;
    public float pitchMax = 1.2f;
    public float volumeMin = 0.25f;
    public float volumeMax = 0.75f;

    public float volumeFootMin = 0.15f;
    public float volumeFootMax = 0.35f;

    public GameObject ImpContainer;

    [SerializeField]
    private Transform followPoint;
    [SerializeField]
    private Transform playerModel;

    public GameObject impPrefab;
    public Transform impSummonPoint;
    public ParticleSystem summonParticles;

    public Transform shootpoint;
    public GameObject projectile;
    public float bulletForce = 7f;


    private bool trySummon;
    private bool tryShoot;

    void Start()
    {
        tryShoot = false;
        trySummon = false;
        gm = GameManager.Instance;
        GameManager.Instance.onTick += gameTick;
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {


        if ((Input.GetButtonDown("Fire2")) )
        {

            trySummon = true;
        }
        else
        {
            
        }

        if (Input.GetButtonUp("Fire2"))
        {
            trySummon = false;
        }

        if ((Input.GetButtonDown("Fire1")))
        {

            tryShoot = true;
        }
        else
        {

        }

        if (Input.GetButtonUp("Fire1"))
        {
            tryShoot = false;
        }

    }

    private void FixedUpdate()
    {
        if (trySummon)
        {
            if (TryToSummon())
            {
                GameObject go = summonImp();
            }
        }
        if (tryShoot)
        {
            if (TryToShoot())
            {
                GameObject go = shootFire();
            }
        }
    }

    private GameObject summonImp()
    {
        GameObject obj = Instantiate(impPrefab, impSummonPoint.position, impSummonPoint.rotation, ImpContainer.transform);
        obj.transform.position = impSummonPoint.position;
        Instantiate(summonParticles, obj.transform);
        gm.spendFirepower(gm.getImpCost()); 
        trySummon = false;

        currentClip = impSummonClip;
        audioSource.clip = currentClip;
        audioSource.pitch = Random.Range(pitchMin, pitchMax);
        audioSource.volume = Random.Range(volumeMin, ((volumeMax - volumeMin) / 4) + volumeMin);
        audioSource.PlayOneShot(currentClip);

        return obj;
    }

    private bool TryToSummon()
    {
        float firepower = gm.getFirepower();
        float firepowerToSummon = gm.getImpCost();
        return (firepower >= firepowerToSummon);
    }

    private GameObject shootFire()
    {
        GameObject obj = Instantiate(projectile, shootpoint.transform.position, shootpoint.rotation);
        obj.transform.position = shootpoint.position;
        Rigidbody bulletRB = obj.GetComponent<Rigidbody>();
        bulletRB.AddForce(shootpoint.forward * bulletForce, ForceMode.Impulse);
        gm.spendFirepower(gm.getShootCost());
        tryShoot = false;

        currentClip = ShootFireballClip;
        audioSource.clip = currentClip;
        audioSource.pitch = Random.Range(pitchMin, pitchMax);
        audioSource.volume = Random.Range(volumeMin, ((volumeMax - volumeMin) / 4) + volumeMin);
        audioSource.PlayOneShot(currentClip);

        return obj;
    }

    private bool TryToShoot()
    {
        float firepower = gm.getFirepower();
        float firepowerToShoot = gm.getShootCost();
        return (firepower >= firepowerToShoot);
    }

    public void gameTick()
    {
        if (isWalking)
        {
            // footstep
            makeNoise();
        }
    }

    public void makeNoise()
    {

            currentClip = playerFootstepAudioClips[Random.Range(0, playerFootstepAudioClips.Count)];
            audioSource.clip = currentClip;
            audioSource.pitch = Random.Range(pitchMin, pitchMax);
            audioSource.volume = Random.Range(volumeFootMin, ((volumeFootMax - volumeFootMin) / 4) + volumeFootMin);
            audioSource.PlayOneShot(currentClip);

    }



    private bool checkForWin()
    {

        return false;
    }

    public void OnDisable()
    {
        GameManager.Instance.onTick -= gameTick;
    }
}