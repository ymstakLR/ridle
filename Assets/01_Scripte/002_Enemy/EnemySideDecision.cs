﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 敵の横から触れた場合の判定処理
/// 更新日時:0408
/// </summary>
public class EnemySideDecision : MonoBehaviour {
    public bool SideDecisionCol { get; set; }

    private void OnCollisionEnter2D(Collision2D col) {
        if (col.gameObject.tag == "Stage" || col.gameObject.tag == "Enemy" ||
            col.gameObject.tag == "StageEdge" || col.gameObject.tag == "Spring") {//ステージタグに触れた場合
            SideDecisionCol = true;
            Debug.Log("Side_COl_"+col.gameObject.name);
        }//if
    }//OnCollisionStay2D

    private void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.tag == "Stage" || col.gameObject.tag == "Enemy" ||
            col.gameObject.tag == "StageEdge" || col.gameObject.tag == "Spring") {//ステージタグに触れた場合
            SideDecisionCol = true;
        }//if
    }//OnTriggerEnter2D

    private void OnTriggerExit2D(Collider2D col) {
        if (col.gameObject.tag == "Stage" || col.gameObject.tag == "Enemy" ||
            col.gameObject.tag == "StageEdge" || col.gameObject.tag == "Spring") {//ステージタグに触れた場合
            SideDecisionCol = false;
        }//if
    }//OnTriggerEnter2D

}//EnemySideDecisionCOl
