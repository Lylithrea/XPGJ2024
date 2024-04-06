using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class HandDraw : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField] TweenData _scale;
    [SerializeField] TweenData _goBack;

    [SerializeField] float _scaleAdd;

    Vector2 _origPos;
    List<RaycastResult> _results = new();

    public void SetRestPosition(Vector2 pos)
    {
        _origPos = pos;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        transform.DOScale(transform.localScale.x + _scaleAdd, _scale.Duration).SetEase(_scale.Easing);
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
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

        if(!interacted)
            transform.DOMove(_origPos, _goBack.Duration).SetEase(_goBack.Easing);
    }


}
