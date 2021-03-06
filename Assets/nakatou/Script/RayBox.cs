﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayBox : MonoBehaviour
{
    public bool move_ = true;

    public Sprite normal;
    public Sprite target_lock;

    private GameObject selectSquare;

    AudioManager am;

    private int p_num = 0;

    private GameObject move_player;

    void Start()
    {
        am = FindObjectOfType<AudioManager>().GetComponent<AudioManager>();
        SetSelectSquare();
    }

    // Update is called once per frame
    void Update()
    {
        if(move_)
        {
            if((Input.GetKeyDown(KeyCode.RightArrow)||Input.GetAxis("AxisX")== 1 ) && selectSquare.GetComponent<Square_Info>().ExistNextSquare(1))
            {
                transform.Translate(1, 0, 0);
                SetSelectSquare();
                am.PlaySe("cursor");
            }
            else if ((Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetAxis("AxisX") == -1) && selectSquare.GetComponent<Square_Info>().ExistNextSquare(3))
            {
                transform.Translate(-1, 0, 0);
                SetSelectSquare();
                am.PlaySe("cursor");
            }
            else if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetAxis("AxisY") == 1) && selectSquare.GetComponent<Square_Info>().ExistNextSquare(0))
            {
                transform.Translate(0, 0, 1);
                SetSelectSquare();
                am.PlaySe("cursor");
            }
            else if ((Input.GetKeyDown(KeyCode.DownArrow) || Input.GetAxis("AxisY") ==-1) && selectSquare.GetComponent<Square_Info>().ExistNextSquare(2))
            {
                transform.Translate(0, 0, -1);
                SetSelectSquare();
                am.PlaySe("cursor");
            }

            if (Input.GetKeyDown(KeyCode.R)||Input.GetKeyDown(KeyCode.L))
            {
                GameObject[] players_ = GameObject.FindGameObjectsWithTag("Player");
                if (Input.GetKeyDown(KeyCode.R))
                {
                    p_num++;
                    if (players_.Length <= p_num) p_num = 0;
                }
                else
                {
                    p_num--;
                    if (p_num < 0) p_num = players_.Length - 1;
                }
                Transform p_pos = players_[p_num].GetComponent<Move_System>().GetNowPos().transform;
                transform.position = new Vector3(p_pos.position.x, transform.position.y, p_pos.position.z);
                SetSelectSquare();
                am.PlaySe("cursor");
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                am.PlaySe("cursor");
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                am.PlaySe("cursor");
            }
        }
    }

    /// <summary>
    /// カメラを行動するキャラの場所に移動
    /// </summary>
    /// <param name="enemy"></param>
    public void SetCameraPosition(GameObject obj)
    {
        transform.position = new Vector3(obj.transform.position.x, transform.position.y, obj.transform.position.z);
        SetSelectSquare();
    }

    void SetSelectSquare()
    {
        Ray ray = new Ray(transform.position, -transform.up);
        RaycastHit hit = new RaycastHit();
        //カーソルの場所になにがあるか
        if (Physics.Raycast(ray, out hit, 1000.0f))
        {
            //床
            if (hit.transform.tag == "Floor")
            {
                selectSquare = hit.transform.gameObject;
                var cost = hit.transform.GetComponent<Square_Info>().GetCost();
                if (cost >= 999)
                {
                    FindObjectOfType<StatusUI>().setMapStatus("移動不可マップ");
                }
                else
                {
                    FindObjectOfType<StatusUI>().setMapStatus("移動可能マップ");
                }
                if (move_player != null) move_player.GetComponent<Move_System>().LineRend(selectSquare);
            }
            //エネミ-
            else if (hit.collider.tag == "Enemy")
            {
                selectSquare = hit.transform.GetComponent<EnemyBase>().GetNowPos();
                FindObjectOfType<StatusUI>().setUnitStatus(
                    hit.collider.GetComponent<Character>()._name,
                    hit.collider.GetComponent<Character>()._totalhp,
                    hit.collider.GetComponent<Character>()._totalMaxhp);
            }

            else if (hit.collider.tag == "Player")
            {
                selectSquare = hit.transform.GetComponent<Move_System>().GetNowPos();
                FindObjectOfType<StatusUI>().setUnitStatus(
                   hit.collider.GetComponent<Character>()._name,
                   hit.collider.GetComponent<Character>()._totalhp,
                   hit.collider.GetComponent<Character>()._totalMaxhp);
            }
        }
    }

    public void SetMovePlayer(GameObject p)
    {
        move_player = p;
        if(move_player==null) GameObject.FindGameObjectWithTag("lRend").GetComponent<RouteLine>().LineDelete();
    }
}
