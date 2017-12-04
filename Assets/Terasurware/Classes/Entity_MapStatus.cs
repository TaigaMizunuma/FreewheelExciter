using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Entity_MapStatus : ScriptableObject
{	
	public List<Param> param = new List<Param> ();

	[System.SerializableAttribute]
	public class Param
	{
		
		public int id;
		public string name;
		public int cost;
		public int invasion;
		public int evasionRate;
		public int height;
	}
}