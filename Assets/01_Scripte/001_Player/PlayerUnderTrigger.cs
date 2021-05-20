﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 自機の足部分接触物判定処理
/// 更新日時:0406
/// </summary>
public class PlayerUnderTrigger : BaseUnderTrigger {
    protected PlayerAnimator _pAnimator;
    public bool IsJumpUp { get; set; }

    protected new void Start() {
        base.Start();
        _pAnimator = this.transform.parent.GetComponent<PlayerAnimator>();
    }//Start

    //条件文を見やすくするようにする returnを使う(1104)
    private void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.tag == "Stage") {
            IsUnderTrigger = true;
            _pAnimator.AniFall = false;
        }//if
        if(col.gameObject.tag == "Gimmick"&& 
            (ParentObj.transform.localEulerAngles.z == 0 && ParentObj.transform.localScale.y >0)){
            IsUnderTrigger = true;
        }
    }//OnTriggerEnter2D

    private void OnTriggerStay2D(Collider2D col) {
        if (col.gameObject.tag != "Stage"&&col.gameObject.tag != "PlatformEffector")
            return;
        IsGimmickJump = false;
    }//OnTriggerStay2D

    private void OnTriggerExit2D(Collider2D col) {
        if(col.gameObject.tag == "Spring") {
            IsRise = true;//バネジャンプ直後の通過床での不具合解消
            IsJumpUp = true;
        }//if
        if(col.gameObject.tag == "Stage"||col.gameObject.tag == "Gimmick") {
            IsRise = true;
            IsUnderTrigger = false;
        }//if
    }//OnTriggerExit2D

}//UnderTrigger
