using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkEffect : MonoBehaviour
{

    [Header("Colors")]
    [SerializeField]private Color _DefaultColor;
    [SerializeField]private Color _BlinkColor;

    [Header("Time")]
    [SerializeField]private float _TimeIn;
    [SerializeField]private float _TimeWait;
    [SerializeField]private float _TimeOut;

    [Header("Renderer")]
    [SerializeField]private UnityEngine.UI.Image _Renderer;


    private Coroutine _BlinkCoroutine;
    
    public void Blink()
    {
        if (_BlinkCoroutine != null)
        {
            StopCoroutine(_BlinkCoroutine);
        }
        _BlinkCoroutine = StartCoroutine(BlinkUpdate());
    }

    private IEnumerator BlinkUpdate()
    {

        var time = 0.0f;
        var startColor = _Renderer.color;
        while (time < _TimeIn)
        {
            time += Time.deltaTime;
            var lerp = Mathf.SmoothStep(0.0f, 1.0f, time / _TimeIn);
            _Renderer.color = Color.Lerp(startColor, _BlinkColor, lerp);
            yield return null;
        }

        yield return new WaitForSeconds(_TimeWait);

        time = 0.0f;
        while (time < _TimeOut)
        {
            time += Time.deltaTime;
            var lerp = Mathf.SmoothStep(0.0f, 1.0f, time / _TimeOut);
            _Renderer.color = Color.Lerp(_BlinkColor, _DefaultColor, lerp);
            yield return null;
        }

        _Renderer.color = _DefaultColor;
    }
}
