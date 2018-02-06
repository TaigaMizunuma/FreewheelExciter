using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharaAnimManager : MonoBehaviour
{
    Animator _animator;
    GameObject NowEquipment;
    public GameObject HandPos;//武器を出現させる手の場所
   
    // Use this for initialization
    void Start()
    {
        _animator = GetComponent(typeof(Animator)) as Animator;

        if (gameObject.tag == "Enemy")
        {
            var childs = GetComponentsInChildren<Transform>();
            foreach (Transform child in childs)
            {
                if (child.name == "R_HandIndex1")
                {
                    HandPos = child.gameObject;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(GetComponent<Character>()._equipment)
        {
            //斧
            if(GetComponent<Character>()._equipment.GetComponent<Weapon>()._weapontype == Weapon_Type.Axe)
            {
                if (NowEquipment) Destroy(NowEquipment);
                var weapon = Instantiate(Resources.Load("NewWeapon/Axe/Axe_"), HandPos.transform) as GameObject;
                NowEquipment = weapon;
            }
            //ハンドガン
            else if (GetComponent<Character>()._equipment.GetComponent<Weapon>()._weapontype == Weapon_Type.Gun)
            {
                if (NowEquipment) Destroy(NowEquipment);
                var weapon = Instantiate(Resources.Load("NewWeapon/HundGun/HGun_"), HandPos.transform) as GameObject;
                NowEquipment = weapon;
            }
            //ライフル
            else if (GetComponent<Character>()._equipment.GetComponent<Weapon>()._weapontype == Weapon_Type.Rifle)
            {
                if (NowEquipment) Destroy(NowEquipment);
                var weapon = Instantiate(Resources.Load("NewWeapon/Rifle/Rifle_"), HandPos.transform) as GameObject;
                NowEquipment = weapon;
            }
            //ナイフ
            else if (GetComponent<Character>()._equipment.GetComponent<Weapon>()._weapontype == Weapon_Type.Knife)
            {
                if (NowEquipment) Destroy(NowEquipment);
                var weapon = Instantiate(Resources.Load("NewWeapon/Knife/Knife_"), HandPos.transform) as GameObject;
                NowEquipment = weapon;
            }
            //槍
            else if (GetComponent<Character>()._equipment.GetComponent<Weapon>()._weapontype == Weapon_Type.Spear)
            {
                if (NowEquipment) Destroy(NowEquipment);
                var weapon = Instantiate(Resources.Load("NewWeapon/Lance/Lance_"), HandPos.transform) as GameObject;
                NowEquipment = weapon;
            }
            //拳
            else if (GetComponent<Character>()._equipment.GetComponent<Weapon>()._weapontype == Weapon_Type.Fist)
            {
                if (NowEquipment) Destroy(NowEquipment);
                NowEquipment = null;
            }
            else//素手
            {
                if (NowEquipment) Destroy(NowEquipment);
                NowEquipment = null;
            }

        }
    }
    public void RunningStart()
    {
        _animator.CrossFade("Run", 0.0f);
    }

    public void RunnigEnd()
    {
        _animator.CrossFade("NoneDamy", 0.0f);
    }
}
