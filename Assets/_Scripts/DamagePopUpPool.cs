using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class DamagePopUpPool : GameObjectPool
{
    public ObjectPool<PopUpTextScript> Pool;
    private PopUpTextScript _popUpText;
    private PopUpTextScript _damagePopUp;

    // Pop Up Settings
    private Vector3 _popUpLoc;
    private bool _isCrit;
    private string _text;
    private Color32 _normalAtkColor;
    private Color32 _critAtkColor;

    private void Awake()
    {
        _popUpText = Resources.Load<PopUpTextScript>("DamagePopUp");
    }

    void Start()
    {
        Pool = new ObjectPool<PopUpTextScript>(() =>
        {
            PopUpTextScript damagePopUp = Instantiate(_popUpText, transform);
            damagePopUp.SetPoolSender(_releaseToPool);
            return damagePopUp;
        }, damagePopUp =>
        {
            damagePopUp.ResetTimer();
            _damagePopUp = damagePopUp;
            SetDamagePopUpSettings(_damagePopUp, _popUpLoc, _isCrit, _text, _normalAtkColor, _critAtkColor);
            damagePopUp.gameObject.SetActive(true);
        }, damagePopUp =>
        {
            damagePopUp.ResetPopUpSettings();
            damagePopUp.gameObject.SetActive(false);
        }, damagePopUp =>
        {
            Destroy(damagePopUp.gameObject);
        }, true, 500, 1000);
    }

    private void SetDamagePopUpSettings(PopUpTextScript popup, Vector3 popUpLocation, bool isCrit, string text, Color32 normalAtkColor, Color32 critAtkColor)
    {
        popup.SetLocationPopUpLocation(popUpLocation);
        popup.SetPopUpText(text, isCrit, normalAtkColor, critAtkColor);
    }

    public void SpawnDamageText(Vector3 popUpLocation, bool isCrit, string text, Color32 normalAtkColor, Color32 critAtkColor)
    {
        _popUpLoc = popUpLocation;
        _isCrit = isCrit;
        _text = text;
        _normalAtkColor = normalAtkColor;
        _critAtkColor = critAtkColor;
        Pool.Get();
    }

    private void _releaseToPool(PopUpTextScript popUpText)
    {
        Pool.Release(popUpText);
    }
}
