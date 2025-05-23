using System;
using System.Collections;
using UnityEngine;
/// <summary>
/// 플레이어 스탯 클래스
/// </summary>
public class PlayerStat
{
    public Action<float> OnHpChanged;
    public Action<float> OnStaminaChanged;
    private float maxHp;
    private float curHp;
    private float maxStamina;
    private float curStamina;
    private float moveSpeed;
    private float runSpeed;

    private float jumpPower;
    private int jumpCount;
    private int buffedJumpCount;

    private float equippedMoveSpeed;
    private float buffedMoveSpeed;

    public float MaxHp { get { return maxHp; } }
    public float MaxStamina { get { return maxStamina; } }
    public float CurHp 
    { 
        get 
        { 
            return curHp; 
        } 
        private set
        {
            curHp = value;
            OnHpChanged?.Invoke(curHp/maxHp);
        }
    }

    public float CurStamina 
    { 
        get 
        { 
            return curStamina; 
        } 
        private set 
        {
            curStamina = value;
            OnStaminaChanged?.Invoke(curStamina / maxStamina);
        } 
    }
    public float MoveSpeed { get { return moveSpeed + equippedMoveSpeed + buffedMoveSpeed; } }
    public float RunSpeed { get { return runSpeed; } }
    public float JumpPower { get { return jumpPower; } }
    public float JumpCount {  get { return jumpCount + buffedJumpCount; } }
    /// <summary>
    /// PlayerStat초기 값 설정
    /// </summary>
    public PlayerStat()
    {
        maxHp = 100;
        curHp = 100;
        maxStamina = 100;
        curStamina = 100;
        moveSpeed = 3;
        runSpeed = 5;
        jumpPower = 5;
        jumpCount = 1;
    }
    /// <summary>
    /// 스태미나 소모
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public bool SpendStamina(float value)
    {
        if (curStamina < value)
            return false;
        curStamina -= value;
        CurStamina = Mathf.Clamp(curStamina, 0, maxStamina);
        return true;
    }
    /// <summary>
    /// HP 회복
    /// </summary>
    /// <param name="value"></param>
    public void RecoveryHp(float value)
    {
        curHp += value;
        CurHp = Mathf.Clamp(curHp, 0, maxHp);
    }
    /// <summary>
    /// Stamina회복
    /// </summary>
    /// <param name="value"></param>
    public void RecoveryStamina(float value)
    {
        curStamina += value;
        CurStamina = Mathf.Clamp(curStamina, 0, maxStamina);
    }
    /// <summary>
    /// HP 소모
    /// </summary>
    /// <param name="value"></param>
    public void SpendHp(float value)
    {
        curHp -= value;
        CurHp = Mathf.Clamp(curHp, 0, maxHp);
    }
    /// <summary>
    /// 이속 버프 추가
    /// </summary>
    /// <param name="value"></param>
    public void AddMoveBuff(float value)
    {
        buffedMoveSpeed += value;
    }
    /// <summary>
    /// 이속 버프 삭제
    /// </summary>
    /// <param name="value"></param>
    public void RemoveMoveBuff(float value)
    {
        buffedMoveSpeed -= value;
    }
    /// <summary>
    /// 점프 증가 버프 추가
    /// </summary>
    /// <param name="value"></param>
    public void AddJumpCountBuff(int value)
    {
        buffedJumpCount += value;
    }
    /// <summary>
    /// 점프증가 버프 삭제
    /// </summary>
    /// <param name="value"></param>
    public void RemoveJumpCountBuff(int value)
    {
        buffedJumpCount -= value;
    }
    /// <summary>
    /// 이동 속도 올려주는 장비 장착
    /// </summary>
    /// <param name="value"></param>
    public void SetMoveEquip(float value)
    {
        equippedMoveSpeed = value;
    }
    /// <summary>
    /// 소모품 이벤트 처리
    /// </summary>
    /// <param name="effect"></param>
    public void ApplyConsumableEffect(ItemDataConsumable effect)
    {
        if (effect.isImmediately || effect.time <= 0)
        {
            ApplyImmediateEffect(effect);
        }
        else
        {
            CoroutineRunner.Instance.RunCoroutine(ApplyTimedEffect(effect));
        }
    }
    /// <summary>
    /// 즉시 적용되는 소모품 처리
    /// </summary>
    /// <param name="effect"></param>
    private void ApplyImmediateEffect(ItemDataConsumable effect)
    {
        switch (effect.type)
        {
            case ConsumableType.Health:
                RecoveryHp(effect.value);
                break;
            case ConsumableType.Stamina:
                RecoveryStamina(effect.value);
                break;
            case ConsumableType.MoveSpeed:
                SetMoveEquip(effect.value);
                break;
        }
    }
    /// <summary>
    /// 버프 관련 코루틴
    /// </summary>
    /// <param name="effect"></param>
    /// <returns></returns>
    private IEnumerator ApplyTimedEffect(ItemDataConsumable effect)
    {
        switch (effect.type)
        {
            case ConsumableType.MoveSpeed:
                AddMoveBuff(effect.value);
                yield return new WaitForSeconds(effect.time);
                RemoveMoveBuff(effect.value);
                break;
            case ConsumableType.JumpCount:
                AddJumpCountBuff((int)effect.value);
                yield return new WaitForSeconds(effect.time);
                RemoveJumpCountBuff((int)effect.value);
                break;
        }
    }
}
