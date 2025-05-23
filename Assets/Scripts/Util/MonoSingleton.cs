using UnityEngine;
/// <summary>
/// ΩÃ±€≈Ê ≈¨∑°Ω∫
/// </summary>
/// <typeparam name="T"></typeparam>
public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
    private static T _instance;

    private static readonly object Lock = new object();
    private static bool _applicationIsQuitting;

    protected virtual bool ShouldRename => false;

    public static T Instance
    {
        get
        {
            if (_applicationIsQuitting)
            {
                return _instance;
            }

            lock (Lock)
            {
                if (_instance)
                    return _instance;

                _instance = (T)FindAnyObjectByType(typeof(T));

                if (!_instance)
                {
                    _instance = new GameObject(typeof(T).ToString(), typeof(T)).GetComponent<T>();
                }

                return _instance;
            }
        }
    }
    protected virtual void Awake()
    {
        if (_instance &&
            _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        if (!_instance)
        {
            _instance = (T)this;
        }

        if (ShouldRename)
        {
            name = typeof(T).ToString();
        }
    }

    protected virtual void OnDestroy()
    {
        _instance = null;
    }

    protected virtual void OnApplicationQuit()
    {
        _applicationIsQuitting = true;
    }

    public void EmptyMethod()
    {
    }
}