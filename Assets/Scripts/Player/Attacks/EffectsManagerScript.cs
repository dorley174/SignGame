using UnityEngine;

public class EffectsManager : MonoBehaviour
{
    public static EffectsManager Instance;
    public AttackEffect effect;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        effect = GetComponent<AttackEffect>();
    }   
}