using System;
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
    public float MoveSpeed { get { return moveSpeed; } }
    public float RunSpeed { get { return runSpeed; } }

    public PlayerStat()
    {
        maxHp = 100;
        curHp = 100;
        maxStamina = 100;
        curStamina = 100;
        moveSpeed = 3;
        runSpeed = 5;
    }
    public bool SpendStamina(float value)
    {
        if (curStamina < value)
            return false;
        curStamina -= value;
        CurStamina = Mathf.Clamp(curStamina, 0, maxStamina);
        return true;
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
}
