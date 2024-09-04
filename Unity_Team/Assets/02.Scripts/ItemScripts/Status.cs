using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Status : MonoBehaviour
{

    [Header("Scripts")]
    [SerializeField]
    Player player;
    [SerializeField]
    Inventory inventory;
    [SerializeField]
    UIController uIController;
    [Space(30)]


    public float Hp;
    public float Stamina = 100;
    public float Hunger;
    public float thirst;

    public Slider Hpval;
    public Slider Staminaval;
    public Slider hungerval;
    public Slider thirstval;

    PhotonView pv;

    private void Awake()
    {
        pv = GetComponent<PhotonView>();
    }
    public void GetObj()
    {
        Hpval = GameObject.Find("Player/Canvas/Staters_Slot/HP").GetComponent<Slider>();
        Staminaval = GameObject.Find("Player/Canvas/Staters_Slot/ST").GetComponent<Slider>();
        hungerval = GameObject.Find("Player/Canvas/Panel/PanelEquipment/Staters/Hunger").GetComponent<Slider>();
        thirstval = GameObject.Find("Player/Canvas/Panel/PanelEquipment/Staters/Water").GetComponent<Slider>();
    }

    void Update()
    {
        if (pv.isMine)
        {
            Hpval.value = Hp;
            Staminaval.value = Stamina;
            hungerval.value = Hunger;
            thirstval.value = thirst;

            if(Hunger == 0)
            {
                Hp -= Time.deltaTime;
            }
            if(thirst > 0)
            {
                Stamina += Time.deltaTime * 2;
            }

            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == 3)
            {
                Hunger -= Time.deltaTime;
                thirst -= (Time.deltaTime * 1.5f); 
            }

            Hp = Mathf.Clamp(Hp, 0, 100);
            Stamina = Mathf.Clamp(Stamina, 0, 100);
            Hunger = Mathf.Clamp(Hunger, 0, 100);
            thirst = Mathf.Clamp(thirst, 0, 100);
        }

        if(Hp <= 0)
        {
            player.die = true;
        }
    }

    public void Hit()
    {
        int Damage = Random.Range(15, 21);
        player.StartFadeCoroutine();
        Hp -= Damage;
    }

    public void StaminaUse()
    {
        Stamina -= 7 * Time.deltaTime;
    }

    public void ItemUseRecorvery(float val)
    {
        if (pv.isMine)
        {
            Hp += val; 
        }
    }

    public void ItemUseFullness(float val)
    {
        if (pv.isMine)
        {
            Hunger += val; 
        }
    }

    public void ItemUseHydration(float val)
    {
        if (pv.isMine)
        {
            thirst += val; 
        }
    }
}
