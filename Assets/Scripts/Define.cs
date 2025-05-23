/// <summary>
/// Enum 및 상수 필드 관리
/// </summary>
public enum SoundType { Master, BGM, SFX }
public enum ItemType
{
    Equipable, Consumable, Resource
}
public enum ConsumableType
{
    Health,
    Stamina,
    MoveSpeed,
    JumpCount
}

public static class LayerString
{
    public const string Wall = "Wall";
    public const string Ground = "Ground";
}
public static class TagString
{
    public const string Player = "Player";
}