using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public bool isWalking = false;


    public float speedModifier;
    [SerializeField]
    private float _normalSpeedMod = 1f;


    //public List<AudioClip> walkAudioClips;

//    public AudioClip currentClip;
//    public AudioSource audioSource;

    public float pitchMin = 0.8f;
    public float pitchMax = 1.2f;
    public float volumeMin = 0.25f;
    public float volumeMax = 0.75f;

    [SerializeField]
    private Transform followPoint;
    [SerializeField]
    private Transform playerModel;

    public GameObject impPrefab;
    public Transform impSummonPoint;
    public ParticleSystem summonParticles;


    private bool trySummon;


    void Start()
    {
        GameManager.Instance.onTick += gameTick;
 //       audioSource = GetComponent<AudioSource>();
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

    }

    private void FixedUpdate()
    {
        if (trySummon)
        {
            Debug.Log("Try to summon an imp");
            GameObject go = summonImp();
        }
    }

    private GameObject summonImp()
    {
        GameObject obj = Instantiate(impPrefab, impSummonPoint.position, impSummonPoint.rotation);
        obj.transform.position = impSummonPoint.position;
        Instantiate(summonParticles, obj.transform);
        trySummon = false;
        return obj;
    }


    public void gameTick()
    {
        if (isWalking)
        {
           // footstep
        }
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