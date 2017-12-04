using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Entity_Skill_EffectList : ScriptableObject
{	
	public List<Param> param = new List<Param> ();

	[System.SerializableAttribute]
	public class Param
	{
		
		public int id;
		public string name;
		public string target;
		public int range_min;
		public int range_max;
		public int cost;
		public string message;
		public string effect1;
		public string effect2;
		public string effect3;
		public string effect4;
	}
}