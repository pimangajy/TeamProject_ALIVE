using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Astar : MonoBehaviour, IPointerClickHandler
{
    public int rowSize = 3;  // ���� �⺻��
    public int collumSize = 3;  // ���� �⺻��

    public Collum[] collum;

    public GameObject player;

    public void OnPointerClick(PointerEventData eventData)
    {
        
    }

    void Start()
    {
        // �� �迭 �ʱ�ȭ
        collum = new Collum[collumSize];

        for (int i = 0; i < collumSize; i++)
        {
            collum[i] = new Collum(rowSize);  // �� ���� �� �迭 �ʱ�ȭ
        }
    }
    [System.Serializable]
    public class Collum
    {
        public Row[] row;

        // �����ڸ� ����Ͽ� �迭 �ʱ�ȭ
        public Collum(int rowSize)
        {
            row = new Row[rowSize];
            for (int i = 0; i < rowSize; i++)
            {
                row[i] = new Row();  // �� �� ��ü �ʱ�ȭ
            }
        }
    }

    [System.Serializable]
    public class Row
    {
        // Row Ŭ������ ����
    }

    private void Update()
    {
        for(int i = 0; i < collumSize; i++)
        {
            for(int j = 0; j< rowSize; j++)
            {
                //Astar.Collum.
            }
        }
    }
}
