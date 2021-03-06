﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enemy全体で使用する処理　このスクリプトを継承してEnemy処理を作成していく
/// 更新日時:0602
/// </summary>
public class EnemyParent : MonoBehaviour {
    public AudioManager AudioManager { get; set; }
    public EnemyBodyTrigger EnemyBodyTrigger { get; set; }
    public Animator EnemyAnimator { get; set; }//Enemyのアニメーション取得変数
    public Rigidbody2D RB2D { get; set; }
    public Transform EnemyTransform { get; set; }

    private EnemyAppearanceManager _appearanceManager;
    protected StageStatusManagement _stageClearManagement;

    private EnemyCount _enemyCount;
    protected GameObject _playerObject;
    protected Score _uiScore;

    public int EnemyMissFoll { get; set; }//敵がミス状態になったときに落下する速度

    public float EnemySpeed { get; set; }//敵のスピード補間変数

    public bool AniMiss { get; set; }//ミスアニメーションの判定変数

    protected int _enemyScore;//敵を倒した時に得られるスコア(個別に設定する)

    public void Start() {//初めに行う処理
        AudioManager = GameObject.Find("GameManager").GetComponent<AudioManager>();
        EnemyAnimator = this.GetComponent<Animator>();
        RB2D = this.GetComponent<Rigidbody2D>();
        EnemyTransform = this.GetComponent<Transform>();
        EnemyBodyTrigger = this.transform.Find("BodyTrigger").GetComponent<EnemyBodyTrigger>();

        _playerObject = GameObject.Find("Ridle"); 
        _enemyCount = GameObject.Find("UI").transform.Find("UIText").transform.Find("EnemyCount").GetComponent<EnemyCount>();
        _uiScore = GameObject.Find("UI").transform.Find("UIText").transform.Find("ScoreNumText").GetComponent<Score>();

        _appearanceManager = this.transform.parent.GetComponent<EnemyAppearanceManager>();
        _stageClearManagement = GameObject.Find("Stage").GetComponent<StageStatusManagement>();
        if (_playerObject.transform.position.x < this.transform.position.x) {
            this.transform.localScale = new Vector2(-this.transform.localScale.x, this.transform.localScale.y);
        }//if
    }//Start

    public void ParentUpdate() {
        EnemyMiss();
        EnemyErasure();
    }//ParentUpdate 

    /// <summary>
    /// ミスしたときの処理
    /// </summary>
    public void EnemyMiss() {
        if (!EnemyBodyTrigger.IsEnemyDamage)
            return;
        AudioManager.PlaySE("NomalEnemyDamage");
        AniMiss = true;
        EnemyAnimator.SetBool("AniMiss", AniMiss);//ミスアニメーションを行う
        RB2D.velocity = new Vector2(0, EnemyMissFoll);
        SetLayerRecursively();
        _enemyCount.EnemyCountDecrease();
        _uiScore.AddScore(_enemyScore);
        EnemyBodyTrigger.IsEnemyDamage = false;
        MissColliderChange();
    }//EnemyMiss

    /// <summary>
    /// 全てのレイヤーを変更する
    /// </summary>
    public void SetLayerRecursively() {
        this.gameObject.layer = LayerMask.NameToLayer("MissEnemy");
        foreach (Transform child in transform) {
            child.gameObject.layer = LayerMask.NameToLayer("MissEnemy");
        }//foreach
    }//SetLayerRecursively
    
    /// <summary>
    /// ミスしたときの当たり判定変更処理
    /// </summary>
    private void MissColliderChange() {
        CapsuleCollider2D col = this.gameObject.GetComponent<CapsuleCollider2D>();
        col.size = new Vector2(0.05f, col.size.y);
        col.direction = CapsuleDirection2D.Vertical;
        
    }//MissColliderChange

    /// <summary>
    /// 時期と一定距離離れたら消去する処理(ミスしていないとき)
    /// </summary>
    public void EnemyErasure() {
        if (60 < Mathf.Abs(_playerObject.transform.position.x - this.transform.position.x) ||
            40 < Mathf.Abs(_playerObject.transform.position.y - this.transform.position.y)||
            (_stageClearManagement.StageStatus != EnumStageStatus.Normal && this.tag == "Enemy")) {
            _appearanceManager.enabled = true;
            Destroy(this.gameObject);
        }//if
    }//EnemyErasure

    /// <summary>
    /// 当たり判定を変更する処理
    /// </summary>
    protected void ColliderChange(Vector2 offset,Vector2 size,CapsuleDirection2D direction) {
        CapsuleCollider2D col = this.GetComponent<CapsuleCollider2D>();
        col.offset = offset;
        col.size = size;
        col.direction = direction;
        CapsuleCollider2D tri = this.transform.Find("BodyTrigger").GetComponent<CapsuleCollider2D>();
        tri.offset = offset;
        tri.size = size;
        tri.direction = direction;
    }//ColliderChange

}//EnemyParent
