using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Astar : MonoBehaviour, IPointerClickHandler
{
    public int rowSize = 3;  // 예제 기본값
    public int collumSize = 3;  // 예제 기본값

    public Collum[] collum;

    public GameObject player;

    public void OnPointerClick(PointerEventData eventData)
    {
        
    }

    void Start()
    {
        // 열 배열 초기화
        collum = new Collum[collumSize];

        for (int i = 0; i < collumSize; i++)
        {
            collum[i] = new Collum(rowSize);  // 각 열의 행 배열 초기화
        }
    }
    [System.Serializable]
    public class Collum
    {
        public Row[] row;

        // 생성자를 사용하여 배열 초기화
        public Collum(int rowSize)
        {
            row = new Row[rowSize];
            for (int i = 0; i < rowSize; i++)
            {
                row[i] = new Row();  // 각 행 객체 초기화
            }
        }
    }

    [System.Serializable]
    public class Row
    {
        // Row 클래스의 내용
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
