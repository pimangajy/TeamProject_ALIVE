using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dummy : MonoBehaviour
{
    ZombieController zom;
    [SerializeField]
    GameObject hitImpact;
    Vector3 hit_Pos;

    PhotonView pv;

    [SerializeField]
    float distence;
    Vector3 impact_Pos;

    private void Awake()
    {
        zom = GetComponent<ZombieController>();
        pv = GetComponent<PhotonView>();
    }

    private void OnTriggerEnter(Collider other)
    {
        //if (other.gameObject.GetComponent<FeildItem>())
        //{
        //    GameObject hit = other.gameObject;

        //    if (hit.GetComponent<FeildItem>().ItemPrefab.GetComponent<Item>().itemType == ItemType.Weapon)
        //    {
        //        zom.HP -= hit.GetComponent<FeildItem>().ItemPrefab.GetComponent<Item>().ATK * (1 + hit.transform.root.gameObject.GetComponent<Player>().uIController.charging);
        //    }
        //}
        if (other.gameObject.GetComponent<FeildItem>())
        {
            GameObject hit = other.gameObject;
            FeildItem fieldItem = hit.GetComponent<FeildItem>();
            Player player = hit.transform.root.gameObject.GetComponent<Player>();

            hit_Pos = other.ClosestPoint(transform.position);
            Vector3 zom_Pos = (transform.position - hit_Pos).normalized;
            impact_Pos = hit_Pos + zom_Pos * distence;

            pv.RPC("Hit_impact", PhotonTargets.All);
            
            //Instantiate(hitImpact, hit_Pos, Quaternion.identity);

            if (fieldItem.ItemPrefab.GetComponent<Item>().itemType == ItemType.Weapon)
            {
                float damage = fieldItem.ItemPrefab.GetComponent<Item>().ATK * (1 + player.uIController.charging);
                zom.TakeDamage(damage, PhotonView.Get(player).viewID); // 데미지와 플레이어의 ViewID 전달
            }
        }
    }

    [PunRPC]
    public void Hit_impact()
    {
        Instantiate(hitImpact, impact_Pos, Quaternion.identity);
    }
}
