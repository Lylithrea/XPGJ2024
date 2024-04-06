using System.Collections;
using UnityEngine;

public class CardFlipper : MonoBehaviour
{
    [SerializeField] float _duration = 0.5f;
    [SerializeField] GameObject _back;
    Coroutine _backCoroutine;
    Coroutine _frontCoroutine;

    public void FlipToFront()
    {
        if (_frontCoroutine == null)
            _frontCoroutine = StartCoroutine(FlipCardToFront());
        
        if(_backCoroutine != null)
        {
            StopCoroutine(_backCoroutine);
            _backCoroutine = null;
        }
    }

    public void FlipToBack()
    {
        if (_frontCoroutine != null)
        {
            StopCoroutine(_frontCoroutine);
            _frontCoroutine= null;
        }

        if (_backCoroutine == null)
            _backCoroutine = StartCoroutine(FlipCardToBack());
    }

    IEnumerator FlipCardToFront()
    {
        float duration = 0;
        while (duration < _duration)
        {
            float t = duration / _duration;

            transform.eulerAngles = new Vector3(transform.eulerAngles.x, 180 * (1 - t), transform.eulerAngles.z);
            if (t > 0.5f) _back.SetActive(false);

            yield return null;
            duration += Time.deltaTime;
        }
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, 0, transform.eulerAngles.z);
        _frontCoroutine = null;
    }

    IEnumerator FlipCardToBack()
    {
        float duration = 0;
        while (duration < _duration)
        {
            float t = duration / _duration;

            transform.eulerAngles = new Vector3(transform.eulerAngles.x, 180 * t, transform.eulerAngles.z);
            if (t > 0.5f) _back.SetActive(true);

            yield return null;
            duration += Time.deltaTime;
        }
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, 0, transform.eulerAngles.z);
        _backCoroutine = null;

    }
}
