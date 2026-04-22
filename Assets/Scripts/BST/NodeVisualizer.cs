using System;
using TMPro;
using UnityEngine;

public class NodeVisualizer : MonoBehaviour
{
    public TextMeshPro KeyText;
    public LineRenderer LeftEdge;
    public LineRenderer RightEdge;

    public void SetKey(object key) => KeyText.text = key?.ToString() ?? "";

    public void SetLeftEdge(Vector3 childWorldPos)
    {
        LeftEdge.gameObject.SetActive(true);
        LeftEdge.SetPosition(0, transform.position);
        LeftEdge.SetPosition(1, childWorldPos);
    }
    public void ClearLeftEdge() => LeftEdge.gameObject.SetActive(false);

    public void SetRightEdge(Vector3 childWorldPos)
    {
        RightEdge.gameObject.SetActive(true);
        RightEdge.SetPosition(0, transform.position);
        RightEdge.SetPosition(1, childWorldPos);
    }
    public void ClearRightEdge() => RightEdge.gameObject.SetActive(false);
}