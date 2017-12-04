using UnityEngine;
using UnityEngine.UI;

public class DamegeUI : MonoBehaviour
{
    Text _damegeTxt;
    public float _offset;//文字が動く範囲
    Vector3 _endPos;

    // Use this for initialization
    void Start()
    {
        _damegeTxt = GetComponent<Text>();
        _endPos = transform.localPosition;
        _endPos.y += _offset;
        //Debug.Log(_endPos);
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.localPosition.y < _endPos.y)
        {
            var pos = transform.localPosition;
            pos.y += 15.0f;//スピード
            transform.localPosition = pos;
            //Debug.Log(transform.localPosition);
        }
        else
        {
            Destroy(gameObject, 0.5f);
        }
    }

    public void setDamegeTxt(float value)
    {
        GetComponent<Text>().text = value.ToString();
    }

    public void setDamegeTxt(int value)
    {
        GetComponent<Text>().text = value.ToString();
    }
}
