using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class TextPopupPool : GameObjectPool
{
    public ObjectPool<PopUpTextScript> DamagePopupTextPool;
    public ObjectPool<PopUpTextScript> HealPopupTextPool;
    private PopUpTextScript _popUpText;
    private PopUpTextScript _textPopUp;

    // Pop Up Settings
    private Vector3 _popUpLoc;
    private bool _isCrit;
    private string _text;
    private Color32 _normalAtkColor;
    private Color32 _critAtkColor;

    private void Awake()
    {
        _popUpText = Resources.Load<PopUpTextScript>("Units/TextPopUp");
    }

    void Start()
    {
        DamagePopupTextPool = new ObjectPool<PopUpTextScript>(() =>
        {
            PopUpTextScript damagePopupText = Instantiate(_popUpText, transform);
            damagePopupText.SetPoolSender(_releaseDamagePopupTextToPool);
            return damagePopupText;
        }, damagePopupText =>
        {
            damagePopupText.ResetTimer();
            _textPopUp = damagePopupText;
            SetDamagePopUpSettings(_textPopUp, _popUpLoc, _isCrit, _text, _normalAtkColor, _critAtkColor);
            damagePopupText.gameObject.SetActive(true);
        }, damagePopupText =>
        {
            damagePopupText.ResetPopUpSettings();
            damagePopupText.gameObject.SetActive(false);
        }, damagePopupText =>
        {
            Destroy(damagePopupText.gameObject);
        }, true, 500, 1000);        
        
        HealPopupTextPool = new ObjectPool<PopUpTextScript>(() =>
        {
            PopUpTextScript healPopupText = Instantiate(_popUpText, transform);
            healPopupText.SetPoolSender(_releaseHealPopupTextToPool);
            return healPopupText;
        }, healPopupText =>
        {
            healPopupText.ResetTimer();
            _textPopUp = healPopupText;
            SetHealPopUpSettings(_textPopUp, _popUpLoc, _text);
            healPopupText.gameObject.SetActive(true);
        }, healPopupText =>
        {
            healPopupText.ResetPopUpSettings();
            healPopupText.gameObject.SetActive(false);
        }, healPopupText =>
        {
            Destroy(healPopupText.gameObject);
        }, true, 500, 1000);
    }

    #region Damage Popup Text
    private void SetDamagePopUpSettings(PopUpTextScript popup, Vector3 popUpLocation, bool isCrit, string text, Color32 normalAtkColor, Color32 critAtkColor)
    {
        popup.SetLocationPopUpLocation(popUpLocation);
        popup.SetDamagePopUpText(text, isCrit, normalAtkColor, critAtkColor);
    }

    public void SpawnDamageText(Vector3 popUpLocation, bool isCrit, string text, Color32 normalAtkColor, Color32 critAtkColor)
    {
        _popUpLoc = popUpLocation;
        _isCrit = isCrit;
        _text = text;
        _normalAtkColor = normalAtkColor;
        _critAtkColor = critAtkColor;
        DamagePopupTextPool.Get();
    }

    private void _releaseDamagePopupTextToPool(PopUpTextScript popUpText)
    {
        DamagePopupTextPool.Release(popUpText);
    }
    #endregion

    #region Heal Popup Text
    private void SetHealPopUpSettings(PopUpTextScript popup, Vector3 popUpLocation, string text)
    {
        popup.SetLocationPopUpLocation(popUpLocation);
        popup.SetHealPopUpText(text);
    }

    public void SpawnHealText(Vector3 popUpLocation, string text)
    {
        _popUpLoc = popUpLocation;
        _text = text;
        HealPopupTextPool.Get();
    }

    private void _releaseHealPopupTextToPool(PopUpTextScript popUpText)
    {
        HealPopupTextPool.Release(popUpText);
    }

    #endregion

}
