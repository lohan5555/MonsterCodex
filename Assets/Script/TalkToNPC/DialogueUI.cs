using UnityEngine;
using UnityEngine.UIElements;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public class DialogueUI : MonoBehaviour
{
    public static DialogueUI Instance;
    private UIDocument uiDocument;
    private VisualElement dialogueBox; // Background
    private Label dialogueLabel;       // Texte
    private DialogueLine[] currentDialogue;
    private int currentIndex;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }

        uiDocument = FindAnyObjectByType<UIDocument>();
        if (uiDocument == null)
        {
            var playerUI = FindAnyObjectByType<TouchscreenInput>();
            if (playerUI != null)
            {
                uiDocument = playerUI.GetComponent<UIDocument>();
            }
        }

        var root = uiDocument.rootVisualElement;

        // R�cup�re le background
        dialogueBox = root.Q<VisualElement>("DialogueBox");
        if (dialogueBox == null)
        {
            Debug.LogError("[DialogueUI] DialogueBox introuvable !");
            return;
        }

        // R�cup�re le label
        dialogueLabel = root.Q<Label>("DialogueLabel");
        if (dialogueLabel == null)
        {
            Debug.LogError("[DialogueUI] DialogueLabel introuvable !");
            return;
        }

        // Cache au d�part
        dialogueBox.style.display = DisplayStyle.None;
        dialogueLabel.style.display = DisplayStyle.None;
    }

    void Update()
    {
        if (currentDialogue == null) return;

        bool tapped = false;

#if ENABLE_INPUT_SYSTEM
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
            tapped = true;
        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
            tapped = true;
#else
        if (Input.GetMouseButtonDown(0) ||
            (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
            tapped = true;
#endif

        if (tapped)
        {
            NextLine();
        }
    }

    public void StartDialogue(DialogueLine[] lines)
    {
        if (lines == null || lines.Length == 0) return;

        currentDialogue = lines;
        currentIndex = 0;

        // Affiche background + label
        dialogueBox.style.display = DisplayStyle.Flex;
        dialogueLabel.style.display = DisplayStyle.Flex;

        ShowCurrentLine();
    }

    private void ShowCurrentLine()
    {
        if (currentDialogue != null && currentIndex < currentDialogue.Length)
        {
            dialogueLabel.text = currentDialogue[currentIndex].text;
            Debug.Log($"[DialogueUI] Ligne {currentIndex} : '{currentDialogue[currentIndex].text}'");
        }
        else
        {
            EndDialogue();
        }
    }

    public void NextLine()
    {
        if (currentDialogue == null) return;

        currentIndex++;

        if (currentIndex < currentDialogue.Length)
            ShowCurrentLine();
        else
            EndDialogue();
    }

    private void EndDialogue()
    {
        currentDialogue = null;

        // Cache tout
        dialogueBox.style.display = DisplayStyle.None;
        dialogueLabel.style.display = DisplayStyle.None;

        Debug.Log("[DialogueUI] Dialogue termin� et background + label cach�s");
    }
}
