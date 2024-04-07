using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDropHandler : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField] TweenData _scale;
    [SerializeField] TweenData _goBack;
    public bool canUseCard = true;

    [SerializeField] float _scaleAdd;
    [SerializeField] float _distanceUntilVertical;
    Vector3 _origScale;
    Vector2 _origPos;
    Vector3 _origRotation;

    readonly List<RaycastResult> _results = new();

    public void SetRestPosition(Transform pos)
    {
        _origPos = Vector2.zero;
        Debug.Log("Rotation: " + pos.eulerAngles);
        _origRotation = pos.eulerAngles;
        
    }

    public void Copy(DragDropHandler proto)
    {
        _goBack = proto._goBack;
        _scale = proto._scale;
        _scaleAdd = proto._scaleAdd;
        _distanceUntilVertical = proto._distanceUntilVertical;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!canUseCard) return;
        _origScale = transform.localScale;
        SoundManager.Instance.PlaySound(SoundName.CardHover);

        //_origRotationZ = transform.eulerAngles.z;
        transform.DOScale(transform.localScale.x + _scaleAdd, _scale.Duration).SetEase(_scale.Easing);
    }

    public void OnDrag(PointerEventData eventData)
    {
        RotateDependingOnDistance();
        transform.position = eventData.position;
    }


    float Lerp(float a, float b, float t)
    {
        return a + (b - a) * t;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _results.Clear();
        EventSystem.current.RaycastAll(eventData, _results);
        bool interacted = false;
        foreach (var r in _results)
        {
            if (r.gameObject.TryGetComponent<ICardInteractable>(out var interactable) && interactable.CanInteract(gameObject))
            {
                interactable.Interact(gameObject);
                interacted = true;
                DeckHandler.Instance.SacrificeCard(this.gameObject);
                break;
            }
        }
        transform.DOScale(_origScale, _scale.Duration).SetEase(_scale.Easing);
        Debug.Log("original pos: " + _origPos);
        if (!interacted)
            transform.DOLocalMove(_origPos, _goBack.Duration).SetEase(_goBack.Easing).OnUpdate(RotateDependingOnDistance);
    }

    void RotateDependingOnDistance()
    {
        float currDistance = Vector2.Distance(_origPos, transform.localPosition);
        float t = currDistance / _distanceUntilVertical;
        t = Mathf.Clamp01(t);

        // Calculate the target rotation
        //Quaternion targetRotation = Quaternion.Euler(0, 0, Mathf.Lerp(0, _origRotation.z * -1, t));
        Quaternion targetRotation = Quaternion.Slerp(Quaternion.identity, Quaternion.Euler(0, 0, -_origRotation.z), t);
        // Apply the rotation
        transform.localRotation = targetRotation;
    }


    public System.Collections.IEnumerator DestroyCardAfterSeconds(int seconds = 1)
    {
        while(seconds > 0)
        {
            seconds--;
            yield return new WaitForSecondsRealtime(1f);
        }
        Destroy(this.gameObject);
        yield return null;
    }

}
