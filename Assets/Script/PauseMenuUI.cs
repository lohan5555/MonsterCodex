using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class PauseMenuUI : MonoBehaviour
{
    private VisualElement root;
    private VisualElement pauseMenu;
    private VisualElement pauseButton;
    private VisualElement grimoirePanel;
    private Button resumeButton;
    private Button grimoireButton;
    private Button quitterButton;
    private Button ButtonRetourGrimoire;

    private bool isPaused = false;
    void Start()
    {
        //on lie l'UI au gameManager
        GameManager.Instance.RegisterUI(gameObject);
    }
    void Awake()
    {
        var uiDocument = GetComponent<UIDocument>();
        root = uiDocument.rootVisualElement;

        pauseMenu = root.Q<VisualElement>("PauseMenu");
        pauseButton = root.Q<VisualElement>("ButtonPause");
        resumeButton = root.Q<Button>("ButtonResume");
        grimoireButton = root.Q<Button>("ButtonGrimoire");
        quitterButton = root.Q<Button>("ButtonQuitter");
        grimoirePanel = root.Q<VisualElement>("GrimoirePanel");
        ButtonRetourGrimoire = root.Q<Button>("ButtonRetourGrimoire");

        if (pauseButton != null)
            pauseButton.RegisterCallback<ClickEvent>(OnPauseClicked);

        if (resumeButton != null)
            resumeButton.clicked += OnResumeClicked;
        if (grimoireButton != null)
            grimoireButton.clicked += OnGrimoireClicked;
        if (quitterButton != null)
            quitterButton.clicked += OnQuitterClicked;
        if (ButtonRetourGrimoire != null)
            ButtonRetourGrimoire.clicked += OnButtonRetourGrimoireClicked;
    }

    private void OnPauseClicked(ClickEvent evt)
    {
        TogglePause();
    }

    private void OnResumeClicked()
    {
        TogglePause();
    }

    private void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            Time.timeScale = 0f;
            pauseMenu.style.display = DisplayStyle.Flex;
            pauseButton.style.display = DisplayStyle.None;
        }
        else
        {
            Time.timeScale = 1f;
            pauseMenu.style.display = DisplayStyle.None;
            pauseButton.style.display = DisplayStyle.Flex;
        }
    }

    public GameObject player;
    public GameObject gameManager; 
    private void OnQuitterClicked()
    {
        //on détruit les objets DontDestroyOnLoad pour recharger completement le jeu
        Destroy(player);
        Destroy(gameManager);

        //on remet le temps en marche avant de charger la scène suivante
        Time.timeScale = 1f;
        SceneManager.LoadScene("MenuPrincipal");
    }

    private void OnGrimoireClicked()
    {
        Debug.Log("affichage du grimoire");
        grimoirePanel.style.display = DisplayStyle.Flex;
    }

    private void OnButtonRetourGrimoireClicked()
    {
        Debug.Log("fermeture du grimoire");
        grimoirePanel.style.display = DisplayStyle.None;
    }
}
