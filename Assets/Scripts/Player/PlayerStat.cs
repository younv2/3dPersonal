public class PlayerStat
{
    private float maxHp;
    private float curHp;
    private float maxStamina;
    private float curStamina;
    private float moveSpeed;
    private float runSpeed;

    public float MaxHp {  get { return maxHp; } }
    public float MaxStamina { get {  return maxStamina; } }
    public float CurHp { get { return curHp; } }
    public float CurStamina { get { return curStamina; } }
    public float MoveSpeed { get { return moveSpeed; } }
    public float RunSpeed { get { return runSpeed; } }

    public float HpRatio => curHp / maxHp;
    public float StaminaRatio => curStamina / maxStamina;
    

    public PlayerStat()
    {
        maxHp = 100;
        curHp = 100;
        maxStamina = 100;
        curStamina = 100;
        moveSpeed = 3;
        runSpeed = 5;
    }
}
