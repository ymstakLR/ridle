﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObjects_Circle : MonoBehaviour {

    private Vector2 _centerPos;//中心のPosition //処理が長くなるので変数にした

    [SerializeField,Tooltip("半径の広さ")]
    private int decisionRadius = 1;
    [SerializeField,Tooltip("床の移動スピード")]
    private float decisionSpeed = 1;
    [SerializeField, Tooltip("配置するオブジェクト数とオブジェクト情報")]
    private List<GameObject> mMovingObjectList;
    [SerializeField,Tooltip("時計回り判定")]
    private bool isClockCycle;

    //オブジェクト数が3の場合 OneCircleTimeを1/3にしてそれぞれのオブジェクトに割り当てる
    private readonly float OneCircleTime = (float)6.25;//一周に使用する時間

    private float _circleCorrection;//配置するオブジェクトの補正位置 for文で回す

    private List<GameObject> cloneObjectList = new List<GameObject>();

    void Start() {
        _centerPos = transform.position;
        _circleCorrection = OneCircleTime / mMovingObjectList.Count;
        for (int i = 0; i < mMovingObjectList.Count; i++) {
            GameObject cloneObj = Instantiate(mMovingObjectList[i],this.transform);//生成
            cloneObj.transform.parent = this.gameObject.transform;//子要素
            cloneObjectList.Add(cloneObj);//リストに追加
        }//for
    }//Start

    private void FixedUpdate() {
        CircleMovingWork();
    }//FixedUpdate

    private void CircleMovingWork() {
        for(int i = 1; i <= mMovingObjectList.Count ; i++) {
            float cirleCorrection = _circleCorrection * i;
            float decisionPosX = decisionRadius * Mathf.Sin((Time.time + cirleCorrection) * decisionSpeed);
            float decisionPosY = decisionRadius * Mathf.Cos((Time.time + cirleCorrection) * decisionSpeed);

            if (isClockCycle) {//回転を逆にする
                decisionPosY = -decisionPosY;
            }//if
            cloneObjectList[i-1].transform.position = new Vector2(_centerPos.x - decisionPosX, _centerPos.y + decisionPosY);
        }//for
    }//CircleMovingWork

}//MovingObjects_Circle
