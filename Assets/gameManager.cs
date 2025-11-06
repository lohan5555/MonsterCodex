using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject player;
    public GameObject mobileUI;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.LoadScene("level_One");
    }

    public void RegisterPlayer(GameObject p)
    {
        //on lie le Player au gameManager
        player = p;
    }

    public void RegisterUI(GameObject ui)
    {
        //on lie l'UI au gameManager
        mobileUI = ui;
    }
}
