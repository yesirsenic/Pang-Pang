using UnityEngine;

public class DeathManager : MonoBehaviour
{
    public static DeathManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public int DeathCount = 0;
}
