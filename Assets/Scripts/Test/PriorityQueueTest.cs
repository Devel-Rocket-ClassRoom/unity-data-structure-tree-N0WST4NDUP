using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class PriorityQueueTest : MonoBehaviour
{
    private void Start()
    {
        PriorityQueue<string, int> pq = new();
        for (int i = 0; i < 20; i++)
        {
            var num = Random.Range(0, 100);
            pq.Enqueue(num.ToString(), num);

            StringBuilder sb = new();
            foreach (var item in pq.Elements)
            {
                sb.Append($"{item.Element} ");
            }
            Debug.Log($"{i + 1}번째 삽입 ({num}), 원소들 순서: " + sb.ToString());
        }
    }

}