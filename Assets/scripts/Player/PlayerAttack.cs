using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private TimeManagement timeManagement;
    [SerializeField] private float damage;
    [SerializeField] private float gravityScaleStorage;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    #region Basic Attacks [legacy code]
    //select what hitbox to use and instantiate any VFX
    public void GroundAttack1()
    {

    }

    public void GroundAttack2()
    {

    }

    public void GroundAttack3()
    {

    }

    public void AirAttack1()
    {

    }

    public void AirAttack2()
    {

    }

    public void AirAttack3()
    {

    }
    

    IEnumerator Attack()
    {

        return null;
    }

    private void MagicAttack()
    {
        //activate the Barrage
    }

    IEnumerator Barrage()
    {
        //spawn in x amount of magic missles
        return null;
    }

    private void UltimateAttack()
    {
        //activate LightningStep
    }

    IEnumerator LightningStep()
    {
        //do whatever this attack is. take away controls from player
        return null;
    }
    #endregion
    private void OnTriggerEnter2D(Collider2D trigger)
    {
        Debug.Log("attack hit something.");
        Debug.Log(trigger.gameObject.name);
        Debug.Log(trigger.gameObject.tag);
        if (trigger.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("attack hit '" + trigger.gameObject.name + "' for " + damage + " damage.");
            trigger.gameObject.GetComponentInParent<DummyHit>().TakeDamage(damage);
            timeManagement.Stop(1);
        }
    }
}
