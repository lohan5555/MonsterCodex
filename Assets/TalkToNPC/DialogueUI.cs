using UnityEngine;
using UnityEngine.UIElements;

public class DialogueUI : MonoBehaviour
{
    public static DialogueUI Instance;

    public UIDocument uiDocument; // assigne ton UIDocument dans l'inspector

    private Label dialogueLabel;
    private DialogueLine[] currentDialogue;
    private int currentIndex;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        if (uiDocument != null)
        {
            var root = uiDocument.rootVisualElement;
            dialogueLabel = root.Q<Label>("DialogueLabel"); // Name du Label dans UI Builder
            if (dialogueLabel == null)
            {
                Debug.LogError("[DialogueUI] impossible de trouver 'DialogueLabel' dans l'UIDocument !");
            }
            else
            {
                Debug.Log("[DialogueUI] Label trouvé avec succès !");
            }
        }
        else
        {
            Debug.LogError("[DialogueUI] UIDocument non assigné !");
        }
    }

    public void StartDialogue(DialogueLine[] lines)
    {
        if (lines == null || lines.Length == 0)
        {
            Debug.LogWarning("[DialogueUI] Aucun dialogue à afficher !");
            return;
        }

        currentDialogue = lines;
        currentIndex = 0;
        Debug.Log($"[DialogueUI] Dialogue lancé avec {lines.Length} lignes");
        ShowCurrentLine();
    }

    private void ShowCurrentLine()
    {
        if (currentDialogue != null && currentIndex < currentDialogue.Length)
        {
            if (dialogueLabel != null)
            {
                dialogueLabel.text = currentDialogue[currentIndex].text;
                Debug.Log($"[DialogueUI] Ligne {currentDialogue[currentIndex].id} : {currentDialogue[currentIndex].text}");
            }
        }
        else
        {
            Debug.LogWarning("[DialogueUI] Pas de ligne à afficher ou index hors limite");
        }
    }

    public void NextLine()
    {
        if (currentDialogue == null)
        {
            Debug.Log("[DialogueUI] Pas de dialogue en cours");
            return;
        }

        currentIndex++;
        if (currentIndex < currentDialogue.Length)
        {
            ShowCurrentLine();
        }
        else
        {
            if (dialogueLabel != null)
                dialogueLabel.text = ""; // fin du dialogue
            Debug.Log("[DialogueUI] Dialogue terminé !");
            currentDialogue = null;
        }
    }
}
