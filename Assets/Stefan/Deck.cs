using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    [SerializeField] Draw[] _cardToDrawPrefabs;
    [SerializeField] Hand _hand;
    Draw _topCard;
    public static Deck Instance {  get; private set; }
    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        else
            Instance = this;

        GetCard();
    }
    private void OnDestroy()
    {
        Instance = null;
    }

    public Draw GetCard()
    {
        var randomCard = _cardToDrawPrefabs[Random.Range(0, _cardToDrawPrefabs.Length)];
        var a = _topCard;
        _topCard = Instantiate(randomCard, transform);
        _topCard.transform.position = transform.position;

        return a;
    }

    public bool HasBackup()
    {
        return transform.childCount > 1;
    }
}
