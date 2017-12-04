using System.Collections;
using UnityEngine;
using System;

/// <summary>
/// 遅延実行関数用クラス
/// </summary>
public static class DelayMethod
{
      //使い方　ラムダ使えば引数も渡せる
      //StartCoroutine(DelayMethod.DelayMethodCall(3.5f, () =>
      //{
      //    Debug.Log("call");
      //    Method(引数);
      //}));
    /// <summary>
    /// 遅延して関数実行させるやつ
    /// </summary>
    /// <param name="waitTime">遅延時間</param>
    /// <param name="action">関数</param>
    /// <returns></returns>
    public static IEnumerator DelayMethodCall(float waitTime, Action action)
    {
        yield return new WaitForSeconds(waitTime);
        action();
    }
}
