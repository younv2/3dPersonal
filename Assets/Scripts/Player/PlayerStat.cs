using System;
using System.Collections;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

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
    public bool SpendStamina(float value)
    {
        if (curStamina < value)
            return false;
        curStamina -= value;
        CurStamina = Mathf.Clamp(curStamina, 0, maxStamina);
        return true;
    }
    public void RecoveryHp(float value)
    {
        curHp += value;
        CurHp = Mathf.Clamp(curHp, 0, maxHp);
    }
    public void RecoveryStamina(float value)
    {
        curStamina += value;
        CurStamina = Mathf.Clamp(curStamina, 0, maxStamina);
    }
    public void SpendHp(float value)
    {
        curHp -= value;
        CurHp = Mathf.Clamp(curHp, 0, maxHp);
    }
    public void AddMoveBuff(float value)
    {
        buffedMoveSpeed += value;
    }
    public void RemoveMoveBuff(float value)
    {
        buffedMoveSpeed -= value;
    }
    public void AddJumpCountBuff(int value)
    {
        buffedJumpCount += value;
    }
    public void RemoveJumpCountBuff(int value)
    {
        buffedJumpCount -= value;
    }
    public void SetMoveEquip(float value)
    {
        equippedMoveSpeed = value;
    }
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
