using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShutGun : MonoBehaviour
{
    [SerializeField]
    GameObject Bullet;
    [SerializeField]
    Transform firePos;
    [SerializeField]
    FeildItem feildItem;

    public Animator ani;
    public PhotonView pv;

    public float bulletSpeed = 20f; // 총알의 속도
    public float spreadAngle = 30f; // 원뿔 각도

    public int chamber = 0;

    private void Awake()
    {
        ani = GetComponent<Animator>();
        pv = GetComponent<PhotonView>();
    }

    void Start()
    {
        
    }

    
    void Update()
    {
        if (feildItem.use)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                Shutgun_Anime(1);
            }
            if (Input.GetMouseButtonDown(1))
            {
                Shutgun_Anime(2);
            }
        } 
    }
    public void Shutgun_Anime(int i)
    {
        if(i == 1 && transform.root.gameObject.GetComponent<Player>().inventory.CheckBullet())
        {
            ani.SetTrigger("Reload");
        }
        else if(i == 2)
        {
            ani.SetBool("Aiming", !ani.GetBool("Aiming"));
        }
    }
    public void Fire_Shugun()
    {
        if (chamber > 0)
        {
            if (!ani.GetBool("Fire"))
            {
                StartCoroutine(Fire());
                SoundManager.instance.PlayerSound("Shotgun_shot", transform.position);
                chamber--;
            }
        } 
    }

    IEnumerator Fire()
    {
        ani.SetBool("Fire", true);
        
        for(int i = 0; i < 8; i++)
        {
            GameObject bullet = Instantiate(Bullet, firePos.position, firePos.rotation);

            // 랜덤한 각도로 원뿔 모양으로 발사
            float randomYaw = Random.Range(-spreadAngle / 2, spreadAngle / 2);
            float randomPitch = Random.Range(-spreadAngle / 2, spreadAngle / 2);
            Quaternion spreadRotation = Quaternion.Euler(0, randomYaw, randomPitch);

            // 총알의 방향과 속도 설정
            Vector3 bulletDirection = spreadRotation * firePos.right;
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            rb.velocity = bulletDirection * bulletSpeed;

            bullet.GetComponent<Bullet>().playerID = gameObject.transform.root.GetComponent<Player>().pv.viewID;
        }

        yield return new WaitForSeconds(0.7f);
        ani.SetBool("Fire", false);
    }
}
