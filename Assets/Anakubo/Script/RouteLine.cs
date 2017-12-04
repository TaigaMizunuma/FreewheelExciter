using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RouteLine : MonoBehaviour {
    LineRenderer lRend;

	// Use this for initialization
	void Start () {
        lRend = GetComponent<LineRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void LineRend(List<GameObject> square)
    {
        lRend.SetVertexCount(square.Count);
        lRend.SetWidth(0.2f, 0.2f);
        List<Vector3> pos = new List<Vector3>();
        for(int i = 0; i < square.Count; i++)
        {
            pos.Add(square[i].transform.position+new Vector3(0,1.0f,0));
        }
        for(int i = 0; i < pos.Count; i++)
        {
            lRend.SetPosition(i, pos[i]);
        }
    }

    public void LineDelete()
    {
        lRend.SetVertexCount(0);
    }
}
