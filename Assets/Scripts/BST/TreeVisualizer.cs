using System.Collections.Generic;
using UnityEngine;

public class TreeVisualizer : MonoBehaviour
{
    public enum Type
    {
        Pow,
        Level,
        // InOrder
    }

    [SerializeField] private GameObject _nodePrefab;
    [SerializeField] private Type _type = Type.Pow;
    [SerializeField][Range(1f, 5f)] public float _horizontalSpacing = 2.0f;
    [SerializeField][Range(1f, 5f)] public float _verticalSpacing = 2.0f;

    private BinarySerachTree<int, int> _bst = new();
    private readonly Dictionary<object, Vector3> _nodePositions = new();
    private Type _currType;

    private void Start()
    {
        while (_bst.Count < 50)
        {
            var random = Random.Range(0, 100);
            if (_bst.ContainsKey(random)) continue;
            _bst.Add(random, 0);
        }

        Visualize();
    }

    private void Update()
    {
        if (_currType == _type) return;

        Visualize();
    }

    private void Visualize()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        } // 안하면 계속 누적 ㅋ

        _nodePositions.Clear();

        var root = _bst.Root;
        if (root == null) return;

        switch (_type)
        {
            case Type.Pow:
                AssignPositionsPow(root, Vector3.zero, root.Height);
                break;
            case Type.Level:
                AssignPositionsLevelOrder(root);
                break;
        }

        _currType = _type;

        InstantiateSubtree(root);
    }

    private void AssignPositionsPow<TKey, TValue>(
        TreeNode<TKey, TValue> node,
        Vector3 position,
        int height)
    {
        if (node == null) return;

        _nodePositions[node] = position;

        float offset = _horizontalSpacing * 0.5f * Mathf.Pow(2, height - 1);

        Vector3 childBase = position + Vector3.down * _verticalSpacing;

        AssignPositionsPow(node.Left, childBase + Vector3.left * offset, height - 1);
        AssignPositionsPow(node.Right, childBase + Vector3.right * offset, height - 1);
    }

    private void AssignPositionsLevelOrder<TKey, TValue>(TreeNode<TKey, TValue> root)
    {
        var levels = new List<List<TreeNode<TKey, TValue>>>();
        var queue = new Queue<(TreeNode<TKey, TValue> node, int depth)>();

        queue.Enqueue((root, 0));

        while (queue.Count > 0)
        {
            var (node, depth) = queue.Dequeue();

            while (levels.Count <= depth)
            {
                levels.Add(new List<TreeNode<TKey, TValue>>());
            }

            levels[depth].Add(node);

            if (node.Left != null)
                queue.Enqueue((node.Left, depth + 1));
            if (node.Right != null)
                queue.Enqueue((node.Right, depth + 1));
        }

        for (int depth = 0; depth < levels.Count; depth++)
        {
            float y = -depth * _verticalSpacing;
            var row = levels[depth];

            for (int i = 0; i < row.Count; i++)
            {
                _nodePositions[row[i]] = new Vector3(i * _horizontalSpacing, y, 0f);
            }
        }
    }

    private void InstantiateSubtree<TKey, TValue>(TreeNode<TKey, TValue> node)
    {
        if (node == null) return;

        Vector3 pos = _nodePositions[node];
        var go = Instantiate(
            _nodePrefab,
            pos,
            Quaternion.identity,
            parent: transform);

        var visualizer = go.GetComponent<NodeVisualizer>();
        visualizer.SetKey(node.Key);
        if (node.Left != null) visualizer.SetLeftEdge(_nodePositions[node.Left]);
        if (node.Right != null) visualizer.SetRightEdge(_nodePositions[node.Right]);

        InstantiateSubtree(node.Left);
        InstantiateSubtree(node.Right);
    }
}
