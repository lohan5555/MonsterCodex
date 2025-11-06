using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance;

    void Start()
    {
        //on lie le Player au gameManager
        GameManager.Instance.RegisterPlayer(gameObject);
    }

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
        }
    }
}
