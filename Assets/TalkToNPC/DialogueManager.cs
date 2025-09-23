using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;  // Singleton

    public DialogueData dialogues;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        TextAsset jsonText = Resources.Load<TextAsset>("dialogues"); // fichier dans Assets/Resources/dialogues.json
        if (jsonText != null)
        {
            dialogues = JsonUtility.FromJson<DialogueData>(jsonText.text);
            Debug.Log("[DialogueManager] JSON chargé avec succès !");
        }
        else
        {
            Debug.LogError("[DialogueManager] Impossible de charger dialogues.json depuis Resources !");
        }
    }
}
