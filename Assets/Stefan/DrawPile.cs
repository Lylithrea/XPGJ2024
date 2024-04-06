using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawPile : MonoBehaviour
{
    [SerializeField] GameObject _cardPrefab;
    [SerializeField] List<SO_Card> _cards= new List<SO_Card>();
    [SerializeField] Hand _hand;

    public static DrawPile Instance {  get; private set; }

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        else
            Instance = this;

        //GetCard();
    }

    private void OnDestroy()
    {
        Instance = null;
    }

    public GameObject GetCard()
    {
        //GameObject a = _topCard;
        GameObject a = Instantiate(_cardPrefab, transform);
        a.GetComponentInChildren<CardHandler>().SetupCard(_cards[Random.Range(0, _cards.Count)]);
        a.transform.position = transform.position;

        return a;
    }

    public bool HasBackup()
    {
        return transform.childCount > 1;
    }
}
