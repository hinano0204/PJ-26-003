using UnityEngine;
using Unity.Netcode;

// Mob (MovingObject)の状態管理スクリプト
// 抽象クラス
public abstract class MobStatus : MonoBehaviour
{
    // enum説明用
    //const int NORMAL = 0;
    //const int ATTACK = 1;
    //const int DIE = 2;

    protected enum StateEnum
    {
        Normal, // 通常
        Attack, // 攻撃中
        Die,    // 死亡
    }

    // 移動可能かどうか
    public bool IsMovable => StateEnum.Normal == _state;

    // 攻撃可能かどうか
    public bool IsAttackable => StateEnum.Normal == _state;

    // ライフの最大値を返す
    public float LifeMax => lifeMax;

    // ライフの値を返す
    public float Life => _life;


    // ライフ最大値
    [SerializeField] private float lifeMax = 10f;

    protected Animator _animator;
    protected StateEnum _state = StateEnum.Normal;  // 動くオブジェクトの状態
    private float _life;    // 現在のライフ値


    // 派生クラス(EnemyStatusなど)から書き換えられるメソッド
    protected virtual void Start()
    {
        // 初期ライフは満タン(最大値)に設定
        _life = lifeMax;
        // アニメーターの取得
        //_animator = GetComponentInChildren<Animator>();

        LIfeGaugeContener.Instance.Add(this);
    }

    // 派生クラス(EnemyStatusなど)から書き換えられるメソッド
    // キャラクターが倒れた時の処理
    protected virtual void OnDie()
    {
        LIfeGaugeContener.Instance.Remove(this);
    }

    // ダメージを受ける
    public void Damage(int damage)
    {
        if (_state == StateEnum.Die) return;

        // ライフから受けたダメージを引く
        _life -= damage;
        // もしダメージを受けても0よりライフがあるか
        if (_life > 0) return;

        // キャラクターの状態を死亡時にする
        _state = StateEnum.Die;
        // Dieのアニメーションを再生する
        //_animator.SetTrigger("Die");

        // キャラクターが倒れた時の処理を実行する
        OnDie();
    }

    // 可能であれば攻撃中の状態に遷移する
    public void GoToAttackStateIfPossible()
    {
        // 攻撃可能でない場合メソッドから抜ける
        if (!IsAttackable) return;

        // 状態を攻撃に変更する
        _state = StateEnum.Attack;
        // 攻撃のアニメーションを再生する
       // _animator.SetTrigger("Attack");

    }

    // 可能であればNormalの状態に遷移する
    public void GoToNormalStateIfPossible()
    {
        // 現在の状態が死亡してたらメソッドを抜ける
        if (_state == StateEnum.Die) return;

        // 状態をNormalの状態にする
        _state = StateEnum.Normal;
    }
}
