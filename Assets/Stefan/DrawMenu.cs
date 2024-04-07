using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawMenu : MonoBehaviour
{
    [SerializeField] int _cardsPerRow;
    [SerializeField] GameObject[] _rows;
    [SerializeField] DrawPile _drawPile;

    List<GameObject> _instances = new();

    void OnEnable()
    {
        int i = 0;
        int row = 0;
        foreach (var card in _drawPile.Cards)
        {
            var inst = _drawPile.GetCard(card);
            inst.GetComponent<CardFlipper>().FlipToFront();
            inst.transform.SetParent(_rows[row].transform);
            if(++i == _cardsPerRow)
            {
                i = 0;
                row++;
            }
            _instances.Add(inst);
        }
    }

    void OnDisable()
    {
        foreach (var inst in _instances)
        {
            Destroy(inst);
        }
    }
}
