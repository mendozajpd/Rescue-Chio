using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AggroZone : MonoBehaviour
{
    public System.Action aggroTrigger;
    public Enemy target;
    
    [Header("AggroZone Settings")]
    [SerializeField] private bool triggerStay;
    [SerializeField] private bool triggerEnterOnce;
    private bool triggered = false;

    private void Start()
    {

    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        // For single targets
        if (!triggerStay && triggerEnterOnce && !triggered)
        {
            Enemy enemy = collider.gameObject.GetComponent<Enemy>();

            if (enemy != null)
            {
                target = enemy;
                aggroTrigger.Invoke();
                triggered = true;
                Debug.Log("triggered once");
            }
        }        
        
        // This changes the target enemy every time an enemy enters trigger
        if (!triggerStay && !triggerEnterOnce)
        {
            Enemy enemy = collider.gameObject.GetComponent<Enemy>();

            if (enemy != null)
            {
                target = enemy;
                aggroTrigger.Invoke();
                Debug.Log("triggered once multiple times");
            }
        }

       
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        if (triggerStay)
        {
            Enemy enemy = collider.gameObject.GetComponent<Enemy>();

            if (enemy != null)
            {
                aggroTrigger.Invoke();
            }
        }
    }

}
