//Grid線引くためのスクリプト
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGridScript : MonoBehaviour {

    private LineRenderer b_lineRenderer;

    private Vector3 blockScale;
    private Vector3 blockPos;

	void Start () {
        b_lineRenderer = this.GetComponent<LineRenderer>();
        MakeGrid();
    }

    void MakeGrid()
    {
        blockScale = this.gameObject.transform.localScale;
        blockPos = this.gameObject.transform.position;
        b_lineRenderer.startWidth = 0.1f;
        b_lineRenderer.endWidth = 0.1f;

        Vector3[] points = new Vector3[5];

        points[0] = new Vector3(blockPos.x + blockScale.x / 2,
                                blockPos.y + blockScale.y - 0.3f,
                                blockPos.z + blockScale.z / 2);

        points[1] = new Vector3(blockPos.x + blockScale.x / 2,
                                blockPos.y + blockScale.y - 0.3f,
                                blockPos.z - blockScale.z / 2);

        points[2] = new Vector3(blockPos.x - blockScale.x / 2,
                                blockPos.y + blockScale.y - 0.3f,
                                blockPos.z - blockScale.z / 2);

        points[3] = new Vector3(blockPos.x - blockScale.x / 2,
                                blockPos.y + blockScale.y - 0.3f,
                                blockPos.z + blockScale.z / 2);

        points[4] = new Vector3(blockPos.x + blockScale.x / 2,
                                blockPos.y + blockScale.y - 0.3f,
                                blockPos.z + blockScale.z / 2);

        b_lineRenderer.positionCount = points.Length;
        b_lineRenderer.SetPositions(points);
    }
}
