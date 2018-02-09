using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharaAnimManager : MonoBehaviour
{
    Animator _animator;
    GameObject NowEquipment;
    public GameObject HandPos;//武器を出現させる手の場所

    Weapon_Type _WeaponType;
   
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
        if(GetComponent<Character>()._equipment &&  _WeaponType != GetComponent<Character>()._equipment.GetComponent<Weapon>()._weapontype)
        {
            //斧
            if(GetComponent<Character>()._equipment.GetComponent<Weapon>()._weapontype == Weapon_Type.Axe)
            {
                if (NowEquipment) Destroy(NowEquipment);
                var weapon = Instantiate(Resources.Load("NewWeapon/Axe/NewAxe_"), HandPos.transform) as GameObject;
                NowEquipment = weapon;
                _WeaponType = Weapon_Type.Axe;
                _animator.CrossFade("NoneDamy", 0.0f);
            }
            //ハンドガン
            else if (GetComponent<Character>()._equipment.GetComponent<Weapon>()._weapontype == Weapon_Type.Gun)
            {
                if (NowEquipment) Destroy(NowEquipment);
                var weapon = Instantiate(Resources.Load("NewWeapon/HundGun/HGun_"), HandPos.transform) as GameObject;
                NowEquipment = weapon;
                _WeaponType = Weapon_Type.Gun;
                _animator.CrossFade("NoneDamy", 0.0f);
            }
            //ライフル
            else if (GetComponent<Character>()._equipment.GetComponent<Weapon>()._weapontype == Weapon_Type.Rifle)
            {
                if (NowEquipment) Destroy(NowEquipment);
                var weapon = Instantiate(Resources.Load("NewWeapon/Rifle/Rifle_"), HandPos.transform) as GameObject;
                NowEquipment = weapon;
                _WeaponType = Weapon_Type.Rifle;
                _animator.CrossFade("NoneDamy", 0.0f);
            }
            //ナイフ
            else if (GetComponent<Character>()._equipment.GetComponent<Weapon>()._weapontype == Weapon_Type.Knife)
            {
                if (NowEquipment) Destroy(NowEquipment);
                var weapon = Instantiate(Resources.Load("NewWeapon/Knife/Knife_"), HandPos.transform) as GameObject;
                NowEquipment = weapon;
                _WeaponType = Weapon_Type.Knife;
                _animator.CrossFade("NoneDamy", 0.0f);
            }
            //槍
            else if (GetComponent<Character>()._equipment.GetComponent<Weapon>()._weapontype == Weapon_Type.Spear)
            {
                if (NowEquipment) Destroy(NowEquipment);
                var weapon = Instantiate(Resources.Load("NewWeapon/Lance/Lance_"), HandPos.transform) as GameObject;
                NowEquipment = weapon;
                _WeaponType = Weapon_Type.Spear;
                _animator.CrossFade("NoneDamy", 0.0f);
            }
            //拳
            else if (GetComponent<Character>()._equipment.GetComponent<Weapon>()._weapontype == Weapon_Type.Fist)
            {
                if (NowEquipment) Destroy(NowEquipment);
                NowEquipment = null;
                _WeaponType = Weapon_Type.Fist;
                _animator.CrossFade("NoneDamy", 0.0f);
            }
            else//素手
            {
                if (NowEquipment) Destroy(NowEquipment);
                NowEquipment = null;
                _WeaponType = Weapon_Type.none;
                _animator.CrossFade("NoneDamy", 0.0f);
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
