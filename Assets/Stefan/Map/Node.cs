using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum NodeType
{
    Converge,
    Stay,
    Diverge
}

public enum NodeCatagory
{
    Enemy,
    Chest,
    Campfire,
    Boss
}

public class Node : MonoBehaviour
{
    public List<Node> nextNodes = new List<Node>();
    public List<Node> PreviousNodes  { get; set; } = new();
    public bool Connected { get; set; }
    public NodeType Type { get; set; }
    public NodeCatagory catagory;

    public Image img;

    public bool isInteractible = false;
    public Button button;


    public SO_Enemy enemy;


    private void Awake()
    {
        img = GetComponent<Image>();
        SetInteractible(isInteractible);
    }

    public void Setup()
    {
        img.sprite = MapHandler.Instance.GetIcon(catagory);
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
        SoundManager.Instance.PlaySound(SoundName.MapHover);
        MapHandler.Instance.updatePlayerPosition(this);
        MapHandler.Instance.SetMapActive(false);

        switch (catagory)
        {
            case NodeCatagory.Enemy:
                GameManager.Instance.SetEnemy(enemy);
                GameManager.Instance.StartGame();
                break;
            case NodeCatagory.Chest:
                GameManager.Instance.SetupChest();
                break;
            case NodeCatagory.Campfire:
                GameManager.Instance.SetupCampfire();
                break;
            default:
                Debug.LogWarning("Tried loading an node catagory that does not exist.");
                break;
        }

    }


}
