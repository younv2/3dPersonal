
public class GameManager : MonoSingleton<GameManager>
{
    void Start()
    {
        SoundManager.Instance.PlaySound(SoundType.BGM, "bgm", true);
    }

}
