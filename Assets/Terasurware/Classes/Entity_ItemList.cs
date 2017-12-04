using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Entity_ItemList : ScriptableObject
{	
	public List<Param> param = new List<Param> ();

	[System.SerializableAttribute]
	public class Param
	{
		
		public int id;
		public string name;
		public int recovery;
		public string category;
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
		public string good_effect;
		public string bad_effect;
	}
}