using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObservePlayer : MonoBehaviour
{

    public List<MouseLook> playerCamera;
    public List<Player> playerList;
    int camerainx = 0;       // 카메라 인덱스
    public Text playerName;

    private void Awake()
    {
        playerCamera = new List<MouseLook>();      
        playerList = new List<Player>();
    }

    private void OnEnable()         // 플레이어가 죽으면 UI가 켜짐
    {
        foreach (GameObject _camera in GameObject.FindGameObjectsWithTag("Player"))             // 플레이어 찾기
        {
            Player player = _camera.GetComponent<Player>();
            MouseLook _playerCamera = _camera.GetComponentInChildren<MouseLook>();               // 카메라에 있는 컴포넌트
            if (_playerCamera != null && player != null)
            {
                if (_playerCamera.lis.enabled == false && _playerCamera.specificCamera.enabled == false)    // 오디오 리스너와 카메라가 꺼져있으면 다른플레이어
                {
                    playerList.Add(player);  // 플레이어 닉네임 추가
                    playerCamera.Add(_playerCamera);                                                        
                }
            }                                                     
        }

        camerainx = Random.Range(0, playerCamera.Count);

        playerCamera[camerainx].lis.enabled = true;
        playerCamera[camerainx].specificCamera.enabled = true;
        playerName.text = playerList[camerainx].pv.owner.NickName;
    }

    public void ClickRight()    // 오른쪽 버튼 클릭
    {
        playerCamera[camerainx].lis.enabled = false;               // 현재 플레이어의 오디오리스너와 카메라 끄기
        playerCamera[camerainx].specificCamera.enabled = false;
        

        camerainx++;   // 카메라 인덱스 증가
        if (camerainx == playerCamera.Count)   // 증가한 값이 리스트에 개수이면 리스트 처음인덱스로
        {
            camerainx = 0;
        }
            
        if (playerList[camerainx].die == false)                     // 플레이어가 살아있을때 실행
        {
            playerCamera[camerainx].lis.enabled = true;             // 다음 플레이어의 카메라와 리스너 켜기
            playerCamera[camerainx].specificCamera.enabled = true;
            playerName.text = playerList[camerainx].pv.owner.NickName;
        }
      
    }

    public void ClickLeft()  // 왼쪽 버튼 클릭
    {
        playerCamera[camerainx].lis.enabled = false;                // 현재 플레이어의 오디오리스너와 카메라 끄기
        playerCamera[camerainx].specificCamera.enabled = false;
        

        camerainx--;            // 카메라 인덱스 감소

        if (camerainx < 0)   // 감소한 값이 0보다 작으면 리스트 끝 인덱스로
        {
            camerainx = playerCamera.Count - 1;
        }

        if (playerList[camerainx].die == false)                     // 플레이어가 살아있을때 실행
        {
            playerCamera[camerainx].lis.enabled = true;             // 다음 플레이어의 카메라와 리스너 켜기
            playerCamera[camerainx].specificCamera.enabled = true;
            playerName.text = playerList[camerainx].pv.owner.NickName;
        }
      
    }

    private void Update()
    {
        // 현재 관전 중인 플레이어가 죽었는지 확인
        if (playerList[camerainx].die)
        {
            ClickRight();
        }
    }
}
