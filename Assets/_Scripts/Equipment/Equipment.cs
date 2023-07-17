using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Equipment : MonoBehaviour
{
    public GameObject player;
    private PlayerInputActions playerControls;

    private InputAction switchWeapon;

    // Weapon
    [SerializeField] private MeleeWeapon knife;
    [SerializeField] private RangedWeapon pistol;
    private int currentWeapon = 1;

    private void OnEnable()
    {
        switchWeapon = playerControls.Player.ChangeWeapon;

        switchWeapon.Enable();
        switchWeapon.performed += changeGear;
    }

    private void OnDisable()
    {
        switchWeapon.performed -= changeGear;
        switchWeapon.Disable();
    }

    private void Awake()
    {
        player = GetComponentInParent<PlayerController>().gameObject;
        playerControls = new PlayerInputActions();
    }
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void changeGear(InputAction.CallbackContext context)
    {
        currentWeapon = currentWeapon == 1 ? 2 : 1;

        switch (currentWeapon)
        {
            // Melee
            case 1:
                var rangedWeapon = GetComponentInChildren<RangedWeapon>();
                if (rangedWeapon != null) Destroy(rangedWeapon.gameObject);

                Instantiate(knife, gameObject.transform);
                break;
            case 2:
                var meleeWeapon = GetComponentInChildren<MeleeWeapon>();
                if (meleeWeapon != null) Destroy(meleeWeapon.gameObject);

                Instantiate(pistol, gameObject.transform);
                break;
        }
    }
}
