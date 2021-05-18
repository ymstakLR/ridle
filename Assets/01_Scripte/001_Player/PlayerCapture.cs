﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///自機が捕獲されたときにPlayerCrtlの代わりに使用する処理
///処理
///プレイヤーの押したボタン数を記憶
///ボタン数が一定になったら
///自機を子要素から外す
///このスクリプトをOFFにしてPlayerCrtlをONにする
///更新日時:0401
/// </summary>
public class PlayerCapture : MonoBehaviour {
    private EnemyArm _enemyArm;
    private PlayerAnimator _pAnimator;
    private PlayerJump _pJump;

    private readonly int NUNBER_CANCELLATIONS = 10;//捕獲から解放されるまでのカウント数

    private int _uncaptureCount = 0;//捕獲解除さするまでのカウント数

    private bool _isFarstProcess;

    private void Start() {
        _pAnimator = GetComponent<PlayerAnimator>();
        _pJump = this.GetComponent<PlayerJump>();
        _isFarstProcess = true;
    }//Start

    void Update() {
        if (_isFarstProcess) {
            this.transform.Find("Attack").GetComponent<PlayerAttack>().enabled = false;
            this.GetComponent<PlayerJump>().JumpTypeFlag = EnumJumpTypeFlag.normal;
            _isFarstProcess = false;
        }//isFarstProcess

        if (Input.anyKeyDown) {
            _uncaptureCount += 1;
        }//if
        if(_uncaptureCount > NUNBER_CANCELLATIONS ||
            _pAnimator.AniDamage) {//アームから解放される処理
            _enemyArm = this.transform.parent.GetComponent<EnemyArm>();//解放時に一度だけ呼ばれるのでUpdateに記述した(0929)
            _enemyArm.UncatchTimer = 0;//親子関係解除前に行う(0929)
            this.transform.parent.gameObject.GetComponent<CapsuleCollider2D>().enabled = false;//捕獲しているアームの当たり判定非表示
            this.transform.parent = null;//アームの子オブジェクトから親オブジェクトに変更する
            this.GetComponent<Animator>().SetBool("AniCapture", false);//ここからしか使用しないのでPlayerAnimationに含めずに処理する(7/15)
            this.GetComponent<PlayerManager>().enabled = true;
            this.transform.Find("Attack").GetComponent<PlayerAttack>().enabled = true;
            _pJump.IsJump = true;
            _uncaptureCount = 0;
            _isFarstProcess = true;
            this.enabled = false;
            //この下に処理を記述しても反映されない
        }//if
        
    }//Update
}//PlayerCapture
