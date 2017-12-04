using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolEnemy : MonoBehaviour {
    // 巡回するマスを全て登録しておく
    public GameObject[] patrol_points;
    // 向かう場所の番号
    private int next_num = 0;
    // EnemyMoveBaseを登録
    private EnemyBase move_base;

	// Use this for initialization
	void Start () {
        move_base = gameObject.GetComponent<EnemyBase>();
        move_base.SetNextGoal(patrol_points[next_num]);
	}
	
	// Update is called once per frame
	void Update () {
        if (move_base.IsRemainder())
        {
            next_num++;
            if (next_num == patrol_points.Length) next_num = 0;
            move_base.SetNextGoal(patrol_points[next_num]);
        }
	}
}
