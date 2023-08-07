using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AggroZone : MonoBehaviour
{
    public System.Action aggroTrigger;
    public EnemyManager target;
    
    [Header("AggroZone Settings")]
    [SerializeField] private bool triggerStay;

    private void Start()
    {

    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        // For single targets
        if (!triggerStay)
        {
            EnemyManager enemy = collider.gameObject.GetComponent<EnemyManager>();

            if (enemy != null)
            {
                target = enemy;
                aggroTrigger?.Invoke();
            }
        }        

       
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        if (triggerStay)
        {
            EnemyManager enemy = collider.gameObject.GetComponent<EnemyManager>();

            if (enemy != null)
            {
                aggroTrigger.Invoke();
            }
        }
    }

}
