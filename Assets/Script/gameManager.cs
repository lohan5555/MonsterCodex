using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject player;
    public GameObject mobileUI;

    // Cache pour stocker les dialogues
    public Dictionary<string, DialogueData> dialoguesCache = new Dictionary<string, DialogueData>();

    [SerializeField] private string baseUrl = "https://billyboy16.github.io/unity-dialogues/";

    // Bouton à afficher si pas de connexion / échec JSON
    [SerializeField] private GameObject mainMenuButton;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        StartCoroutine(LoadAllDialogues());
    }

    IEnumerator LoadAllDialogues()
    {
        Debug.Log("[GameManager] Téléchargement des dialogues depuis GitHub Pages...");

        string[] dialogueFiles = {
            "dialogue_1/dialogues_1.json",
            "dialogue_1/dialogues_2.json",
            "dialogue_2/dialogues_1.json",
            "dialogue_2/dialogues_2.json",
            "dialogue_3/dialogues_1.json"
        };

        bool success = true;

        foreach (string file in dialogueFiles)
        {
            string url = baseUrl + file;
            Debug.Log($"[GameManager] Téléchargement : {url}");

            using (UnityWebRequest request = UnityWebRequest.Get(url))
            {
                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.Success)
                {
                    try
                    {
                        DialogueData data = JsonUtility.FromJson<DialogueData>(request.downloadHandler.text);
                        dialoguesCache[file] = data;
                        Debug.Log($"[GameManager] Dialogue chargé : {file}");
                    }
                    catch (System.Exception e)
                    {
                        Debug.LogError($"[GameManager] Erreur parsing JSON ({file}) : {e.Message}");
                        success = false;
                        break; // stop si erreur
                    }
                }
                else
                {
                    Debug.LogError($"[GameManager] Erreur de téléchargement : {file} ({request.error})");
                    success = false;
                    break; // stop si échec téléchargement
                }
            }
        }

        if (success)
        {
            Debug.Log("[GameManager] Tous les dialogues ont été chargés !");
            SceneManager.LoadScene("level_One");
        }
        else
        {
            Debug.LogWarning("[GameManager] Impossible de charger les dialogues. Activation du bouton B_MAINMENU.");
            if (mainMenuButton != null)
                mainMenuButton.gameObject.SetActive(true);
        }
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
