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
    [SerializeField] protected string _text;
    [SerializeField] protected float _timeUntilDestruction;
    [SerializeField] protected float fadeOutSpeed = 0.15f;

    [SerializeField] protected float rotationOffSetAmount = 2;

    private System.Action<PopUpTextScript> _sendToPool;

    private void OnEnable()
    {
        // Settrigger true
        _anim.SetTrigger("PopUp");
    }

    private void Awake()
    {
        _tmp = GetComponent<TextMeshPro>();
        _anim = GetComponent<Animator>();
        _tmp.overrideColorTags = true;
    }
    // vector 3 up * offset
    void Start()
    {
        _tmp.text = _text;
    }

    void Update()
    {
        if (timerEnabled) _timePassed += Time.deltaTime;

        if (_timePassed >= _timeUntilDestruction)
        {
            timerEnabled = false;
            _anim.SetTrigger("ShrinkDown");
            //Destroy(gameObject, 0.15f);
            // Send to Pool
            StartCoroutine(sendToPoolWithDelay(0.15f));
        }
    }

    // interpolate from
    public void SetPopUpText(string text, Color32 textColor)
    {
        float fontSize = _tmp.fontSize;
        float offSetRNGx = Random.Range(-rotationOffSetAmount, rotationOffSetAmount);
        float offSetRNGy = Random.Range(-rotationOffSetAmount, rotationOffSetAmount);
        float fontSizeRNG = Random.Range(fontSize - 0.5f, fontSize + 0.5f);
        _text = text;
        _tmp.fontSize = fontSizeRNG;
        _tmp.color = textColor;
        _timeUntilDestruction += offSetRNGx * 0.2f;
        transform.position = new Vector3(transform.position.x + offSetRNGx * 0.1f, transform.position.y + offSetRNGy * 0.1f, 1);
        transform.rotation = Quaternion.Euler(0, 0, -offSetRNGx * 0.8f);
    }

    public void SetPopUpText(string text, bool isCrit, Color32 normalAttack, Color32 critAttack)
    {
        float fontSize = _tmp.fontSize;
        float offSetRNGx = Random.Range(-rotationOffSetAmount, rotationOffSetAmount);
        float offSetRNGy = Random.Range(-rotationOffSetAmount, rotationOffSetAmount);
        float fontSizeRNG = Random.Range(fontSize - 0.5f, fontSize + 0.5f);
        _text = text;
        _tmp.fontSize = isCrit ? fontSizeRNG * 1.5f : fontSizeRNG;
        _tmp.color = isCrit ? critAttack : normalAttack;
        _timeUntilDestruction = isCrit ? (_timeUntilDestruction + offSetRNGx * 0.2f)  * 2 : _timeUntilDestruction + offSetRNGx * 0.1f;
        _tmp.sortingOrder = isCrit ? 1 : 0;
        transform.position = new Vector3(transform.position.x + offSetRNGx * 0.1f, transform.position.y + offSetRNGy * 0.1f, 1);
        transform.rotation = Quaternion.Euler(0, 0, -offSetRNGx * 0.8f);
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
