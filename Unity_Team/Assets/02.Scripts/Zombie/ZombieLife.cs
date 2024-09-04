using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ZombieLife : MonoBehaviour
{
    // ���� ü��
    public int HP = 100;

    // �ڽ��� Ʈ������?
    private Transform myTr;

    // ���� ȿ�� �����ǰ�?
    public GameObject zombieBloodEffect;

    // ���� �߰�?
    public PhotonView pv = null;

    
    // ������Ʈ�ѷ� ���� ���۷���
    public ZombieController zombiecon;


    private void Awake()
    {
        myTr = GetComponent<Transform>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject);
        // �浹�� ���� ������Ʈ�� Item ������Ʈ�� ������ �ִ��� Ȯ��
        if (collision.gameObject.GetComponent<Item>())
        {
            GameObject hit = collision.gameObject;
            // �浹�� ���� ������Ʈ�� itemType�� Weapon���� Ȯ��
            if (hit.GetComponent<Item>().itemType == ItemType.Weapon)
            {
                Debug.Log("�÷��̾� ���ݿ� ����!");

                //HP - hit.GetComponent<FeildItem>().ItemPrefab.GetComponent<Item>().ATK;

                pv.RPC("Damage", PhotonTargets.AllBuffered, hit.GetComponent<FeildItem>().ItemPrefab.GetComponent<Item>().ATK * (1 + hit.transform.root.gameObject.GetComponent<Player>().uIController.charging));
            }

            //zombiecon.HitEnemy();
        }
    }

    [PunRPC]
    void Damage(int dam)
    {
        //���� �Ѿ��� �Ŀ��� ������ Enemy�� life�� ����
        HP -= dam;

        // ������� 0�̸� ������
        if (HP <= 0)
        {
            Debug.Log("���� ����");
            
            // ���� �߰�            
            //���� ��Ʈ�ѷ� ��ũ��Ʈ�� ���� ��� ó�� �Լ�?
            zombiecon.Die();
        }
    }

   

}
