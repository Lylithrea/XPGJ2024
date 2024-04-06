using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
[Serializable]
public struct TweenData
{
    public Ease Easing;
    public float Duration;

    public TweenData(Ease easing, float duration = 1)
    {
        Easing = easing;
        Duration = duration;
    }
}
[RequireComponent(typeof(CardFlipper))]
public class Draw : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField] TweenData _goBack;
    Vector2 _originalPos;

    CardFlipper _flipper;
    List<RaycastResult> _results = new();

    void Awake()
    {
        _flipper = GetComponent<CardFlipper>();
    }

    private void Start()
    {
        _originalPos = transform.position;

    }

    public void SetRestPosition(Vector2 pos)
    {
        _originalPos = pos;
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        transform.DOKill();
        _flipper.FlipToFront();
        var deck = Deck.Instance;
        if(!deck.HasBackup())
            deck.GetCard();

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
            DrawFail();

    }

    
    void DrawFail()
    {
        _flipper.FlipToBack();

        transform.DOMove(_originalPos, _goBack.Duration).SetEase(_goBack.Easing);
    }
}
