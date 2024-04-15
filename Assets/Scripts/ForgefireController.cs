using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForgefireController : MonoBehaviour, IDamageable
{
    private GameManager gm;
    public GameObject forgefire;
    public float fireforgeDrainAmount = -3;
    private float minScale = 1;
    [SerializeField]
    private float maxScale = 5f;

    public float health { get; set; }
    private float maxHealth = 1000000f;


    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.Instance;
        GameManager.Instance.onForgefireChange += onForgefireChange;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Kill()
    {


    }


    public void Damage(float damageTaken)
    {
        Debug.Log("Fireforge taking damage: " + damageTaken.ToString());
        if (damageTaken > 0) health = Mathf.Clamp(health - damageTaken, 0, maxHealth);
        gm.changeFireforge(damageTaken);


    }

    public void Heal(float damageHealed)
    {
        //throw new System.NotImplementedException();
    }
    void onForgefireChange(float curForgefire)
    {
        int forgefireValue = Mathf.RoundToInt(curForgefire);
        float forgefirePercent = Mathf.Clamp(forgefireValue / gm.getFireInForgeToWin(), 0, 1);
        float scaleToSet = ((maxScale - minScale) * forgefirePercent) + minScale;
        Debug.Log("Should be setting " + scaleToSet.ToString() + " to the bonfire scale. The Forgefire percent is: " + forgefirePercent.ToString());
        forgefire.transform.localScale = new Vector3(scaleToSet, scaleToSet, scaleToSet);

    }

    void OnDisable()
    {
        GameManager.Instance.onForgefireChange -= onForgefireChange;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<Enemy>().Damage(10);
            gm.drainFireforge(fireforgeDrainAmount);
        }

    }
}
