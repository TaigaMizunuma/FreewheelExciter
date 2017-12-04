using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Entity_CharaList : ScriptableObject
{	
	public List<Param> param = new List<Param> ();

	[System.SerializableAttribute]
	public class Param
	{
		
		public int id;
		public string name;
		public string job;
		public int lv;
		public int hp;
		public int sp;
		public int str;
		public int skl;
		public int spd;
		public int luk;
		public int def;
		public int cur;
		public int move;
		public string skill1;
		public string skill2;
		public string skill3;
		public string skill4;
	}
}