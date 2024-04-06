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
    public List<Node> PreviousNodes  { get; set; } = new();
    public bool Connected { get; set; }
    public NodeType Type { get; set; }

    public Image img;
    private void Awake()
    {
        img = GetComponent<Image>();
    }

}
