//キャラクターのイメージクラス
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterImageManager : MonoBehaviour {

    //メインキャラクター(仲間になるキャラ)の顔グラ
    [Tooltip("要素10まではメインキャラで固定にします")]
    public Sprite[] characterImage;

    //会話中の背景
    public Sprite[] backGround;
}
