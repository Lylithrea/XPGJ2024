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

    [SerializeField] float offset;

    public Node playerNode;

    [SerializeField] List<Node> _checkedNodes = new();
    List<Node> _newNodes = new();
    List<Node> _convergingNodes = new();

    public List<Node> allNodes = new List<Node> ();

    public List<SO_Enemy> possibleEnemies = new List<SO_Enemy>();

    public static MapHandler Instance;
    public GameObject map;


    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }


    public void updatePlayerPosition(Node node)
    {
        foreach (Node currentNode in allNodes)
        {
            currentNode.SetInteractible(false);
        }
        playerNode = node;
        playerNode.SetNextInteractible();
    }


    public void SetMapActive(bool isActive)
    {
        map.SetActive(isActive);
    }


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

        PositionInitialNodes();

        for (int j = 1; j < _mapLength; j++)
        {

            for (int i = 0; i < _checkedNodes.Count; i++)
            {

                float totalChance = _stayNodeChance + _convergeNodeChance + _divergeNodeChance;
                float rand = Random.Range(0f, totalChance);
                float cumulativeChance = _stayNodeChance;
                //Node newNode = new Node();

                var node = _checkedNodes[i];
                if (rand < cumulativeChance)
                {
                    Node newNode = CreateNode(node, j);
                    node.nextNodes.Add(newNode);
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
                        Node newNode = CreateNode(node, j);
                        node.nextNodes.Add(newNode);
                        node.Type = NodeType.Stay;
                    }

                }
                else if (rand < (cumulativeChance += _divergeNodeChance))
                {
                    //if curr number of nodes is bigger than max width - 1, don't do a _diverge, but a stay
                    if (_checkedNodes.Count <= _mapWidth - 1)
                    {
                        Node newNode1 = CreateNode(node, j);
                        node.nextNodes.Add(newNode1);
                        node.Type = NodeType.Diverge;
                        node.img.color = Color.red;

                    }

                    Node newNode = CreateNode(node, j);
                    node.nextNodes.Add(newNode);
                    node.Type = NodeType.Stay;
                }

            }

            for (int i = 0; i < _checkedNodes.Count; i++)
            {
                var node = _checkedNodes[i];
                if (node.Type == NodeType.Converge)
                {
                    var closestNum = Utils.ClosestNumberInRange(0, _newNodes.Count - 1, i);
                    _newNodes[closestNum].PreviousNodes.Add(node);
                    node.nextNodes.Add(_newNodes[closestNum]);
                }

            }

            var copy = _checkedNodes;
            _checkedNodes = _newNodes;
            _newNodes = copy;
            _newNodes.Clear();
            _convergingNodes.Clear();
        }

        CreateBoss();
    }

    void PositionInitialNodes()
    {
        for (int i = 0; i < _checkedNodes.Count; i++)
        {
            var node = _checkedNodes[i];
            node.transform.localPosition = _spacing * i * Vector3.right;
            node.SetInteractible(true);
            allNodes.Add(node);
            node.enemy = possibleEnemies[Random.Range(0, possibleEnemies.Count)];
        }
    }
    void CreateBoss()
    {
        var bossNode = Instantiate(_nodePrefab, transform);
        bossNode.transform.localPosition = Vector3.Lerp(_checkedNodes[0].transform.localPosition, _checkedNodes[_checkedNodes.Count - 1].transform.localPosition, 0.5f);
        bossNode.transform.localPosition += _spacing * Vector3.up;

        foreach (var n in _checkedNodes)
        {
            bossNode.PreviousNodes.Add(n);
        }
        _checkedNodes.Add(bossNode);
    }

    Node CreateNode(Node node, int yIndex)
    {
        var newNode = Instantiate(_nodePrefab, transform);
        newNode.PreviousNodes.Add(node);
        _newNodes.Add(newNode);
        allNodes.Add(newNode);
        newNode.transform.localPosition = _spacing * yIndex * Vector3.up + _spacing * (_newNodes.Count - 1) * Vector3.right + new Vector3(Random.Range(-offset, offset), Random.Range(-offset, offset), 0);
        newNode.enemy = possibleEnemies[Random.Range(0, possibleEnemies.Count)];
        return newNode;
    }

    void ConnectNode(Node node)
    {
        if (node == null || node.PreviousNodes.Count == 0) return;

        foreach (var prevNode in node.PreviousNodes)
        {
            MakeLine(prevNode.transform.localPosition, node.transform.localPosition);
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

        rectTransf.localPosition = Vector2.Lerp(start, end, .5f);

        //rectTransf.up = (start - end).normalized;

        rectTransf.localRotation = Quaternion.Euler(0, 0, Mathf.Atan2((end.y - start.y) , (end.x - start.x)) * Mathf.Rad2Deg - 90f);
        rectTransf.sizeDelta = new Vector2(_lineThickness, Vector2.Distance(start, end));
    }
}
