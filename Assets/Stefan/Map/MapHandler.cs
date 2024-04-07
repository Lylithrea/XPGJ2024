using DG.Tweening;
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

    public float chestChance = 0.1f;
    public float campfireChance = 0.1f;

    public Node playerNode;

    [SerializeField] List<Node> _checkedNodes = new();
    List<Node> _newNodes = new();

    public List<Node> allNodes = new List<Node> ();

    public List<SO_Enemy> possibleEnemies = new List<SO_Enemy>();
    public List<SO_Enemy> bossEnemies = new List<SO_Enemy>();

    public static MapHandler Instance;
    public GameObject map;
    public Transform nodeParent;
    public Transform lineParent;


    public Sprite enemyIcon;
    public Sprite bossIcon;
    public Sprite chestIcon;
    public Sprite campfireIcon;

    public float mapAnimationDuration = 1f;

    public Color roadColor;

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
        //map.SetActive(isActive);
        if (isActive)
        {
            StartCoroutine(OpenMap());
        }
        else
        {
            StartCoroutine(CloseMap());
        }
    }

    IEnumerator CloseMap()
    {
        float duration = 0;
        while (duration < mapAnimationDuration)
        {
            float t = duration / mapAnimationDuration;

            map.transform.eulerAngles = new Vector3(90 * t, map.transform.eulerAngles.y, map.transform.eulerAngles.z);

            yield return null;
            duration += Time.deltaTime;
        }
        map.transform.eulerAngles = new Vector3(90, map.transform.eulerAngles.y, map.transform.eulerAngles.z);
    }

    IEnumerator OpenMap()
    {
        float duration = 0;
        while (duration < mapAnimationDuration)
        {
            float t = duration / mapAnimationDuration;

            map.transform.eulerAngles = new Vector3(90 * (1 - t), map.transform.eulerAngles.y, map.transform.eulerAngles.z);

            yield return null;
            duration += Time.deltaTime;
        }
        map.transform.eulerAngles = new Vector3(0, map.transform.eulerAngles.y, map.transform.eulerAngles.z);
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
                var node = _checkedNodes[i];

                //algorithm that guarantees at least one node spawn
                float totalChance = _stayNodeChance + _convergeNodeChance + _divergeNodeChance;
                float rand = Random.Range(0f, totalChance);
                float cumulativeChance = _stayNodeChance;

                if (rand < cumulativeChance)
                {
                    CreateStayNode(node, j);

                }
                else if (rand < (cumulativeChance += _convergeNodeChance))
                {
                    //if there aren't stay or diverge nodes, 
                    if (_checkedNodes.Any(n => n.Type != NodeType.Converge))
                        PrepareConvergeNode(node);
                    else
                        CreateStayNode(node, j);

                }
                else if (rand < (cumulativeChance += _divergeNodeChance))
                {
                    if (_checkedNodes.Count <= _mapWidth - 1)
                        CreateDivergeNode(node, j);
                    else
                        CreateStayNode(node, j);
                }

            }

            ////position nodes
            Vector3 firstNodePos = _checkedNodes[0].transform.localPosition;
            Vector3 secondNodePos = _checkedNodes[_checkedNodes.Count - 1].transform.localPosition;
            Vector3 dir = (secondNodePos - firstNodePos);
            Vector3 normal = new Vector3 (-dir.y, dir.x).normalized;
            Vector3 centerOfPreviousNodes = Vector2.Lerp(firstNodePos, secondNodePos, .5f);

            Vector3 positionInFront = centerOfPreviousNodes + _spacing  * normal;
            Vector3 centerOfNewNodes = Vector2.Lerp(_newNodes[0].transform.localPosition, _newNodes[_newNodes.Count - 1].transform.localPosition, .5f);
            Vector3 offset = positionInFront - centerOfNewNodes;

            foreach (var item in _newNodes)
            {
                item.transform.localPosition += offset;
            }
            ConnectConvergeNodes();

            var copy = _checkedNodes;
            _checkedNodes = _newNodes;
            _newNodes = copy;
            _newNodes.Clear();
        }

        CreateBoss();
    }

    void PrepareConvergeNode(Node parentNode)
    {
        parentNode.Type = NodeType.Converge;
    }

    void ConnectConvergeNodes()
    {
        //connect to closest next node
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
    }

    void CreateStayNode(Node parentNode, int height)
    {
        Node newNode = CreateNode(parentNode, height);
        parentNode.nextNodes.Add(newNode);
        parentNode.Type = NodeType.Stay;
    }

    void CreateDivergeNode(Node parentNode, int height)
    {
            Node newNode1 = CreateNode(parentNode, height);
            parentNode.nextNodes.Add(newNode1);
            parentNode.Type = NodeType.Diverge;

            Node newNode = CreateNode(parentNode, height);
            parentNode.nextNodes.Add(newNode);

    }

    void PositionInitialNodes()
    {
        for (int i = 0; i < _checkedNodes.Count; i++)
        {
            var node = _checkedNodes[i];
            node.transform.localPosition = _spacing * i * Vector3.right;
            node.SetInteractible(true);
            allNodes.Add(node);
            node.catagory = NodeCatagory.Enemy;
            node.enemy = possibleEnemies[Random.Range(0, possibleEnemies.Count)];
            node.transform.localRotation = Quaternion.Euler(0, 0, nodeParent.localRotation.eulerAngles.z * -1);
            node.Setup();
        }
    }
    void CreateBoss()
    {
        var bossNode = Instantiate(_nodePrefab, nodeParent);
        bossNode.transform.localPosition = Vector3.Lerp(_checkedNodes[0].transform.localPosition, _checkedNodes[_checkedNodes.Count - 1].transform.localPosition, 0.5f);
        bossNode.transform.localPosition += _spacing * Vector3.up;

        foreach (var n in _checkedNodes)
        {
            bossNode.PreviousNodes.Add(n);
        }
        _checkedNodes.Add(bossNode);
        bossNode.catagory = NodeCatagory.Boss;
        bossNode.enemy = bossEnemies[Random.Range(0, bossEnemies.Count)];
        bossNode.transform.localRotation = Quaternion.Euler(0, 0, nodeParent.localRotation.eulerAngles.z * -1);
        bossNode.Setup();
    }


    public Sprite GetIcon(NodeCatagory catagory)
    {
        switch (catagory)
        {
            case NodeCatagory.Enemy:
                return enemyIcon;            
            case NodeCatagory.Boss:
                return bossIcon;
            case NodeCatagory.Chest:
                return chestIcon;
            case NodeCatagory.Campfire:
                return campfireIcon;
            default:
                Debug.LogWarning("Trying to get icon for node catagory that does not exist.");
                return null;
        }
    }

    Node CreateNode(Node node, int yIndex)
    {
        var newNode = Instantiate(_nodePrefab, nodeParent);
        newNode.PreviousNodes.Add(node);
        _newNodes.Add(newNode);
        allNodes.Add(newNode);

        newNode.transform.SetLocalPositionAndRotation(
            _spacing * yIndex * Vector3.up + _spacing * (_newNodes.Count - 1) * Vector3.right + new Vector3(Random.Range(-offset, offset), Random.Range(-offset, offset), 0), 
            Quaternion.Euler(0, 0, nodeParent.localRotation.eulerAngles.z * -1)
        );

        float chestRand = Random.Range(0, 1f);
        float campfireRand = Random.Range(0, 1f);
        if (chestRand < chestChance)
        {
            newNode.catagory = NodeCatagory.Chest;
        }
        else if (campfireRand < campfireChance)
        {
            newNode.catagory = NodeCatagory.Campfire;
        }
        else
        {
            newNode.catagory = NodeCatagory.Enemy;
            newNode.enemy = possibleEnemies[Random.Range(0, possibleEnemies.Count)];
        }
        newNode.Setup();
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
        go.color = roadColor;
        go.transform.SetParent(lineParent);
        var rectTransf = go.GetComponent<RectTransform>();

        rectTransf.localPosition = Vector2.Lerp(start, end, .5f);

        rectTransf.localRotation = Quaternion.Euler(0, 0, Mathf.Atan2((end.y - start.y) , (end.x - start.x)) * Mathf.Rad2Deg - 90f);
        rectTransf.sizeDelta = new Vector2(_lineThickness, Vector2.Distance(start, end));
    }
}
