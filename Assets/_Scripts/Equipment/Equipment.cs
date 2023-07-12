using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : MonoBehaviour
{
    public GameObject player;

    private void Awake()
    {
        player = GetComponentInParent<PlayerController>().gameObject;
    }
    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
