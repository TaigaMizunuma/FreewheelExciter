using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// カーソルの下に何があるか判断してUIに渡してる
/// </summary>
public class TargetLock : MonoBehaviour
{
    Image img;

    void Start()
    {
    }

    void Update()
    {
        Ray ray = new Ray(transform.position, -transform.up);
        RaycastHit hit = new RaycastHit();
        //カーソルの場所になにがあるか
        if (Physics.Raycast(ray, out hit, 1000.0f))
        {
            //床
            if (hit.transform.tag == "Floor")
            {
                var cost = hit.transform.GetComponent<Square_Info>().GetCost();
                if(cost >= 999)
                {
                    //FindObjectOfType<StatusUI>().setMapStatus("移動不可マップ");
                }
                else
                {
                    //FindObjectOfType<StatusUI>().setMapStatus("移動可能マップ");
                }
            }
            //エネミ-
            if (hit.collider.tag == "Enemy")
            {
                FindObjectOfType<StatusUI>().setUnitStatus(
                    hit.collider.GetComponent<Character>()._name,
                    hit.collider.GetComponent<Character>()._totalhp,
                    hit.collider.GetComponent<Character>()._totalMaxhp);
            }

            //味方
            if(hit.collider.tag == "Player")
            {
                FindObjectOfType<StatusUI>().setUnitStatus(
                    hit.collider.GetComponent<Character>()._name,
                    hit.collider.GetComponent<Character>()._totalhp,
                    hit.collider.GetComponent<Character>()._totalMaxhp);
            }
        }
    }
}
