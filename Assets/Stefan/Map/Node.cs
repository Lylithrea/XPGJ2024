using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum NodeType
{
    Converge,
    Stay,
    Diverge
}
public class Node : MonoBehaviour
{
    public List<Node> nextNodes = new List<Node>();
    public List<Node> PreviousNodes  { get; set; } = new();
    public bool Connected { get; set; }
    public NodeType Type { get; set; }

    public Image img;

    public bool isInteractible = false;
    public Button button;


    public SO_Enemy enemy;


    private void Awake()
    {
        img = GetComponent<Image>();
        SetInteractible(isInteractible);
    }


    public void SetInteractible(bool active)
    {
        isInteractible = active;
        button.interactable = active;
    }

    public void SetNextInteractible()
    {
        foreach (var node in nextNodes)
        {
            node.SetInteractible(true);
        }
    }

    public void OnClick()
    {
        MapHandler.Instance.updatePlayerPosition(this);
        GameManager.Instance.SetEnemy(enemy);
        MapHandler.Instance.SetMapActive(false);
    }


}
