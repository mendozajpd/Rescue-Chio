using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PopUpTextScript : MonoBehaviour
{
    protected TextMeshPro _tmp;
    protected Animator _anim;
    protected bool timerEnabled = true;
    protected float _timePassed;
    protected string _text;
    protected float _timeUntilDestruction;

    [Header("Settings")]
    [SerializeField] protected float fadeOutSpeed = 0.15f;
    [SerializeField] protected float offSetAmount = 2;

    [Header("Default Settings")]
    [SerializeField] protected float defaultTimeUntilDestruction = 1.5f;
    [SerializeField] protected float defaultFontSize = 5;

    private System.Action<PopUpTextScript> _sendToPool;


    private void OnEnable()
    {
        // Settrigger true
        _anim.SetTrigger("PopUp");
    }

    private void Awake()
    {
        _timeUntilDestruction = defaultTimeUntilDestruction;
        _tmp = GetComponent<TextMeshPro>();
        _anim = GetComponent<Animator>();
        _tmp.overrideColorTags = true;
    }
    // vector 3 up * offset
    void Start()
    {
    }

    void Update()
    {
        if (timerEnabled) _timePassed += Time.deltaTime;

        if (_timePassed >= _timeUntilDestruction)
        {
            timerEnabled = false;
            _anim.SetTrigger("ShrinkDown");
            StartCoroutine(sendToPoolWithDelay(0.15f));
        }
    }

    //interpolate from
    public void SetPopUpText(string text, Color32 textColor)
    {
        float fontSize = _tmp.fontSize;
        float offSetRNGx = Random.Range(-offSetAmount, offSetAmount);
        float offSetRNGy = Random.Range(-offSetAmount, offSetAmount);
        float fontSizeRNG = Random.Range(fontSize - 0.5f, fontSize + 0.5f);
        _tmp.text = text;
        _tmp.fontSize = fontSizeRNG;
        _tmp.color = textColor;
        _timeUntilDestruction += offSetRNGx * 0.2f;
        transform.position = new Vector3(transform.position.x + offSetRNGx * 0.1f, transform.position.y + offSetRNGy * 0.1f, 1);
        transform.rotation = Quaternion.Euler(0, 0, -offSetRNGx * 0.8f);
    }

    public void SetDamagePopUpText(string text, bool isCrit, Color32 normalAttack, Color32 critAttack)
    {
        float fontSize = _tmp.fontSize;
        float offSetRNGx = Random.Range(-offSetAmount, offSetAmount);
        float offSetRNGy = Random.Range(-offSetAmount, offSetAmount);
        float fontSizeRNG = Random.Range(fontSize - 0.5f, fontSize + 0.5f);
        _tmp.text = text;
        _tmp.fontSize = isCrit ? fontSizeRNG * 1.35f : fontSizeRNG;
        _tmp.color = isCrit ? critAttack : normalAttack;
        _timeUntilDestruction = isCrit ? (_timeUntilDestruction + Mathf.Abs(offSetRNGx * 0.1f)) * 1.5f : _timeUntilDestruction + offSetRNGx * 0.1f;
        _tmp.sortingOrder = isCrit ? 1 : 0;
        transform.position = new Vector3(transform.position.x + offSetRNGx * 0.1f, transform.position.y + offSetRNGy * 0.1f, 1);
        transform.rotation = Quaternion.Euler(0, 0, -offSetRNGx * 0.8f);
    }

    public void SetHealPopUpText(string text)
    {
        Color32 healColor = new Color32(85, 185, 104, 255);
        float fontSize = _tmp.fontSize - 0.5f;
        float offSetRNGx = Random.Range(-offSetAmount, offSetAmount);
        float offSetRNGy = Random.Range(-offSetAmount, offSetAmount);
        float fontSizeRNG = Random.Range(fontSize - 0.5f, fontSize + 0.5f);
        _tmp.text = text;
        _tmp.fontSize = fontSizeRNG;
        _tmp.color = healColor;
        _timeUntilDestruction = _timeUntilDestruction + offSetRNGx * 0.08f;
        _tmp.sortingOrder = 1;
        transform.position = new Vector3(transform.position.x + offSetRNGx * 0.1f, transform.position.y + offSetRNGy * 0.1f, 1);
        transform.rotation = Quaternion.Euler(0, 0, -offSetRNGx * 0.8f);
    }

    public void ResetPopUpSettings()
    {
        _tmp.fontSize = defaultFontSize;
        _timeUntilDestruction = defaultTimeUntilDestruction;
    }

    public void SetLocationPopUpLocation(Vector3 spawnLocation)
    {
        transform.position = spawnLocation;
    }

    public void SetPoolSender(System.Action<PopUpTextScript> poolSender)
    {
        _sendToPool = poolSender;
    }

    IEnumerator sendToPoolWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        _sendToPool(this);
    }

    public void ResetTimer()
    {
        _timePassed = 0;
        timerEnabled = true;
    }

}
