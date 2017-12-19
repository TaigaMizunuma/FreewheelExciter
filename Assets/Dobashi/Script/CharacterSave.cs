using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSave : MonoBehaviour {

    public int _id;
    public int _job;
    public string _name;
    public Vector3 _pos;
    public bool _hero;
    public int _lv;
    public int _totallv;
    public int _hp;
    public int _totalhp;
    public int _str;
    public int _skl;
    public int _luk;
    public int _cur;
    public int _move;
    public int[] _addstatus;
    public int[] _addonestatus;
    public int[] _addbuffstatus;
    public List<Buff> _bufflist;
    public bool _stability;
    public int _state;
}
