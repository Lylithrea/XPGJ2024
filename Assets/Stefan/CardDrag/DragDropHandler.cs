using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDropHandler : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField] TweenData _scale;
    [SerializeField] TweenData _goBack;

    [SerializeField] float _scaleAdd;
    [SerializeField] float _distanceUntilVertical;
    Vector3 _origScale;
    Vector2 _origPos;
    float _origRotationZ;

    readonly List<RaycastResult> _results = new();

    public void SetRestPosition(Vector2 pos)
    {
        _origPos = pos;
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
        _origScale = transform.localScale;

        _origRotationZ = transform.eulerAngles.z;
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
                break;
            }
        }
        transform.DOScale(_origScale, _scale.Duration).SetEase(_scale.Easing);

        if (!interacted)
            transform.DOMove(_origPos, _goBack.Duration).SetEase(_goBack.Easing).OnUpdate(RotateDependingOnDistance);
    }

    void RotateDependingOnDistance()
    {
        float currDistance = Vector2.Distance(_origPos, transform.position);
        float t = currDistance / _distanceUntilVertical;
        t = Mathf.Clamp01(t);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, Lerp(_origRotationZ, 0, t));
    }
}
