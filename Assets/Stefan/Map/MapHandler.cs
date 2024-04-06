using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class MapHandler : MonoBehaviour
{
    [SerializeField] Node _nodePrefab;
    [SerializeField] float _convergeNodeChance;
    [SerializeField] float _stayNodeChance;
    [SerializeField] float _divergeNodeChance;
    [SerializeField] float _lineThickness;
    [SerializeField] int _mapLength;
    [SerializeField] int _mapWidth;

    [SerializeField] float _spacing;
    
    [SerializeField] List<Node> _checkedNodes = new();
    List<Node> _newNodes = new();
    List<Node> _convergingNodes = new();

    void Start()
    {
        GenerateNodes();

        foreach (var node in _checkedNodes)
        {
            ConnectNode(node);
        }
    }

    void GenerateNodes()
    {
        for (int j = 1; j < _mapLength; j++)
        {
            var offset = Vector3.zero;

            for (int i = 0; i < _checkedNodes.Count; i++)
            {
                float totalChance = _stayNodeChance + _convergeNodeChance + _divergeNodeChance;
                float rand = Random.Range(0f, totalChance);
                float cumulativeChance = _stayNodeChance;

                var node = _checkedNodes[i];
                if (rand < cumulativeChance)
                {
                    CreateNode(node, offset);
                    node.Type = NodeType.Stay;

                }
                else if (rand < (cumulativeChance += _convergeNodeChance))
                {
                    //if there aren't stay or diverge nodes, 
                    if (_checkedNodes.Any(n => n.Type != NodeType.Converge))
                    {
                        _convergingNodes.Add(node);
                        node.img.color = Color.blue;
                        node.Type = NodeType.Converge;

                    }
                    else
                    {
                        CreateNode(node, offset);
                        node.Type = NodeType.Stay;
                    }

                }
                else if (rand < (cumulativeChance += _divergeNodeChance))
                {
                    //if curr number of nodes is bigger than max width - 1, don't do a _diverge, but a stay
                    if (_checkedNodes.Count <= _mapWidth - 1)
                    {
                        offset += _spacing * Vector3.right;
                        CreateNode(node, offset);
                        offset += _spacing * Vector3.right;
                    }

                    CreateNode(node, offset);
                    node.img.color = Color.red;
                    node.Type = NodeType.Diverge;
                }

            }
            //no new nodes because of converging nodes

            foreach (var item in _convergingNodes)
            {
                _newNodes.GetRandomItem().PreviousNodes.Add(item);
            }

            var copy = _checkedNodes;
            _checkedNodes = _newNodes;
            _newNodes = copy;
            _newNodes.Clear();
            _convergingNodes.Clear();
        }

        CreateBoss();
    }

    void CreateBoss()
    {
        //boss
        var bossNode = Instantiate(_nodePrefab, transform);
        bossNode.transform.position = _checkedNodes[0].transform.position;
        bossNode.transform.position += _spacing * Vector3.up;

        foreach (var n in _checkedNodes)
        {
            bossNode.PreviousNodes.Add(n);
        }
        _checkedNodes.Add(bossNode);
    }

    void CreateNode(Node node, Vector3 offset)
    {
        var newNode = Instantiate(_nodePrefab, transform);
        newNode.PreviousNodes.Add(node);
        _newNodes.Add(newNode);
        newNode.transform.position = node.transform.position;
        newNode.transform.position += _spacing * Vector3.up + offset;
    }

    void ConnectNode(Node node)
    {
        if (node == null || node.PreviousNodes.Count == 0) return;

        foreach (var prevNode in node.PreviousNodes)
        {
            MakeLine(node.transform.position, prevNode.transform.position);
            if (!prevNode.Connected)
            {
                ConnectNode(prevNode);
                prevNode.Connected = true;
            }
        }
        
    }

    public void MakeLine(Vector2 start, Vector2 end)
    {
        var go = new GameObject("Line").AddComponent<Image>();
        go.color = Color.red;
        go.transform.SetParent(transform);
        var rectTransf = go.GetComponent<RectTransform>();

        rectTransf.position = Vector2.Lerp(start, end, .5f);

        rectTransf.up = (start - end).normalized;
        rectTransf.sizeDelta = new Vector2(_lineThickness, Vector2.Distance(start, end));
    }
}
