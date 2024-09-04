using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ZombieLife : MonoBehaviour
{
    // 좀비 체력
    public int HP = 100;

    // 자신의 트랜스폼?
    private Transform myTr;

    // 혈흔 효과 넣을건가?
    public GameObject zombieBloodEffect;

    // 포톤 추가?
    public PhotonView pv = null;

    
    // 좀비컨트롤러 연결 레퍼런스
    public ZombieController zombiecon;


    private void Awake()
    {
        myTr = GetComponent<Transform>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject);
        // 충돌한 게임 오브젝트가 Item 컴포넌트를 가지고 있는지 확인
        if (collision.gameObject.GetComponent<Item>())
        {
            GameObject hit = collision.gameObject;
            // 충돌한 게임 오브젝트의 itemType이 Weapon인지 확인
            if (hit.GetComponent<Item>().itemType == ItemType.Weapon)
            {
                Debug.Log("플레이어 공격에 맞음!");

                //HP - hit.GetComponent<FeildItem>().ItemPrefab.GetComponent<Item>().ATK;

                pv.RPC("Damage", PhotonTargets.AllBuffered, hit.GetComponent<FeildItem>().ItemPrefab.GetComponent<Item>().ATK * (1 + hit.transform.root.gameObject.GetComponent<Player>().uIController.charging));
            }

            //zombiecon.HitEnemy();
        }
    }

    [PunRPC]
    void Damage(int dam)
    {
        //맞은 총알의 파워를 가져와 Enemy의 life를 감소
        HP -= dam;

        // 생명력이 0이면 죽이자
        if (HP <= 0)
        {
            Debug.Log("좀비 죽음");
            
            // 포톤 추가            
            //좀비 컨트롤러 스크립트의 좀비 사망 처리 함수?
            zombiecon.Die();
        }
    }

   

}
