using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loader : MonoBehaviour
{

    public GameObject gameManager;
     public GameObject soundManager;//GameManagerを格納

    public void Awake()
    {
        if(GameManager.instance == null)//GameManagerのinstanceをnullチェック
        {
            Instantiate(gameManager);//nullならGameManagerを生成する
        }
        if(SoundManager.instance == null)//GameManagerのinstanceをnullチェック
        {
            Instantiate(soundManager);//nullならGameManagerを生成する
        }

    }
}