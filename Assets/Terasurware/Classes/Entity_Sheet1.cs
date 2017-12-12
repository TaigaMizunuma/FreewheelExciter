using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Entity_Sheet1 : ScriptableObject
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
	}
}