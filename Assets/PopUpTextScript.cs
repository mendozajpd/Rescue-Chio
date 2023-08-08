using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PopUpTextScript : MonoBehaviour
{
    private TextMeshPro _tmp;
    private Animator _anim;
    private bool timerEnabled = true;
    private float _timePassed;
    [SerializeField] private string _text;
    [SerializeField] private float _timeUntilDestruction;
    [SerializeField] private float fadeOutSpeed = 0.15f;

    [SerializeField] private float rotationOffSetAmount = 2;

    private void Awake()
    {
        _tmp = GetComponent<TextMeshPro>();
        _anim = GetComponent<Animator>();
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
            Destroy(gameObject, 0.15f);
        }
    }

    // interpolate from
    public void SetPopUpText(string text)
    {
        float fontSize = _tmp.fontSize;
        float offSetRNGx = Random.Range(-rotationOffSetAmount, rotationOffSetAmount);
        float offSetRNGy = Random.Range(-rotationOffSetAmount, rotationOffSetAmount);
        float fontSizeRNG = Random.Range(fontSize - 0.5f, fontSize + 0.5f);
        _text = text;
        _tmp.fontSize = fontSizeRNG;
        transform.position = new Vector3(transform.position.x + offSetRNGx * 0.1f, transform.position.y + offSetRNGy * 0.1f, 1);
        transform.rotation = Quaternion.Euler(0, 0, offSetRNGx);
    }

}
