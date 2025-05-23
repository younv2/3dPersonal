/// <summary>
/// 캐릭터 매니저( 타 스크립트에서 플레이어 받아오는 용도)
/// </summary>
public class CharacterManager : MonoSingleton<CharacterManager>
{
    public Player player;

    protected override void Awake()
    {
        base.Awake();
        
    }
}
