using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Entity_ClassDefaultStatus : ScriptableObject
{	
	public List<Param> param = new List<Param> ();

	[System.SerializableAttribute]
	public class Param
	{
		
		public int id;
		public string name;
		public int hp;
		public int str;
		public int skl;
		public int spd;
		public int luk;
		public int def;
		public int cur;
		public int move;
		public int hp_r;
		public int str_r;
		public int skl_r;
		public int spd_r;
		public int luk_r;
		public int def_r;
		public int cur_r;
		public int move_r;
		public int hp_max;
		public int str_max;
		public int skl_max;
		public int spd_max;
		public int luk_max;
		public int def_max;
		public int cur_max;
		public int move_max;
	}
}