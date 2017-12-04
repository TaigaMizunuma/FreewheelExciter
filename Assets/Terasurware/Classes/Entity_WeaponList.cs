using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Entity_WeaponList : ScriptableObject
{	
	public List<Param> param = new List<Param> ();

	[System.SerializableAttribute]
	public class Param
	{
		
		public int id;
		public string name;
		public int atk;
		public int hit;
		public int critical;
		public int weight;
		public int range_min;
		public int range_max;
		public string effect;
		public string type;
		public int atk_count;
		public string message;
		public int ex_hp;
		public int ex_sp;
		public int ex_str;
		public int ex_skl;
		public int ex_spd;
		public int ex_luk;
		public int ex_def;
		public int ex_cur;
		public int ex_move;
	}
}