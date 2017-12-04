using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Entity_StoryTestTwo : ScriptableObject
{	
	public List<Param> param = new List<Param> ();

	[System.SerializableAttribute]
	public class Param
	{
		
		public int StoryNumber;
		public int ID;
		public string Name;
		public string Story;
		public int LeftOneImage;
		public int RightOneImage;
		public int LeftTwoImage;
		public int RightTwoImage;
		public string LeftOne;
		public string RightOne;
		public string LeftTwo;
		public string RightTwo;
	}
}