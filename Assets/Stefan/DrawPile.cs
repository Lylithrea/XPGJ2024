using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawPile : MonoBehaviour
{
    [SerializeField] GameObject _cardPrefab;
    GameObject _topCard;
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

        GetCard();
    }

    private void OnDestroy()
    {
        Instance = null;
    }

    public GameObject GetCard()
    {
        GameObject a = _topCard;
        _topCard = Instantiate(_cardPrefab, transform);
        _topCard.transform.position = transform.position;

        return a;
    }

    public bool HasBackup()
    {
        return transform.childCount > 1;
    }
}
