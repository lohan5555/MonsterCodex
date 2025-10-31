using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class NPCSystem : MonoBehaviour
{
    private UIDocument uiDocument;

    [Tooltip("Nom exact dans UI Builder (champ 'Name')")]
    public string buttonName = "ButtonTalk";

    private VisualElement talkVE;
    private bool playerInRange = false;

    private void Start()
    {
        uiDocument = FindAnyObjectByType<UIDocument>();
        // trouve ou v�rifie l'UIDocument
        if (uiDocument == null)
        {
            var playerUI = FindAnyObjectByType<TouchscreenInput>();
            if (playerUI != null)
            {
                uiDocument = playerUI.GetComponent<UIDocument>();
            }
        }

        var root = uiDocument.rootVisualElement;
        if (root == null)
        {
            Debug.LogError("[NPCSystem] rootVisualElement null. UIDocument pas pr�t ?");
            return;
        }

        // cherche l'�l�ment par name (c'est un VisualElement dans ton cas)
        talkVE = root.Q<VisualElement>(buttonName);

        if (talkVE == null)
        {
            Debug.LogError($"[NPCSystem] '{buttonName}' introuvable dans le root. V�rifie le champ Name dans UI Builder et que tu as assign� le bon UIDocument.");
            // dump court pour aider au debug
            var all = root.Query<VisualElement>().ToList();
            Debug.Log($"[NPCSystem] total VisualElements dans root: {all.Count} (liste 0..10)");
            for (int i = 0; i < Mathf.Min(10, all.Count); i++)
                Debug.Log($"  VE[{i}]: type={all[i].GetType().Name}, name='{all[i].name}', classes='{string.Join(",", all[i].GetClasses())}'");
            return;
        }

        Debug.Log("[NPCSystem] Element trouv�: type=" + talkVE.GetType().Name + " name=" + talkVE.name + " classes=" + string.Join(",", talkVE.GetClasses()));

        // s'assurer que le picking mode permet le clic (sinon ClickEvent ne se d�clenchera pas)
        talkVE.pickingMode = PickingMode.Position;

        // cacher au d�part
        talkVE.style.display = DisplayStyle.None;

        // �coute le clic (m�me si c'est un VisualElement)
        talkVE.RegisterCallback<ClickEvent>(evt => OnTalkButtonPressed());

       
    }

    // si un parent a display None on le force visible (fallback)
    private void EnsureAncestorsVisible(VisualElement ve)
    {
        if (ve == null) return;
        VisualElement current = ve.parent;
        while (current != null)
        {
            // si un anc�tre est cach�, on le rend visible
            if (current.style.display == DisplayStyle.None)
            {
                current.style.display = DisplayStyle.Flex;
            }
            current = current.parent;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerInRange = true;
        Debug.Log("[NPCSystem] Player entre");

        if (talkVE != null)
        {
            // si un parent est cach� on le rend visible (sinon l'enfant restera invisible)
            EnsureAncestorsVisible(talkVE);

            talkVE.style.display = DisplayStyle.Flex;
            // en cas de styles USS agressifs on peut aussi forcer l'opacit� et la visibilit�
            talkVE.style.opacity = 1f;
            Debug.Log("[NPCSystem] OnTriggerEnter: talkVE.display = " + talkVE.style.display);
        }
        else
        {
            Debug.LogWarning("[NPCSystem] OnTriggerEnter : talkVE null");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerInRange = false;
        Debug.Log("[NPCSystem] Player sort");

        if (talkVE != null)
        {
            talkVE.style.display = DisplayStyle.None;
            Debug.Log("[NPCSystem] OnTriggerExit: talkVE.display = " + talkVE.style.display);
        }
        else
        {
            Debug.LogWarning("[NPCSystem] OnTriggerExit : talkVE null");
        }
    }

    private void OnTalkButtonPressed()
    {
        if (!playerInRange) return;

        Debug.Log("[NPCSystem] Dialogue started !");

        if (DialogueManager.Instance == null)
        {
            Debug.LogError("DialogueManager.Instance est null !");
            return;
        }

        // Charge le bon dialogue selon la sc�ne + inventaire
        DialogueManager.Instance.LoadDialogueForCurrentScene();

        // Puis lance le dialogue
        var npcLines = DialogueManager.Instance.dialogues.npc_1;
        DialogueUI.Instance.StartDialogue(npcLines);
    }


    public void OnTouch(InputAction.CallbackContext context)
    {
        if (context.performed)
            DialogueUI.Instance?.NextLine();
    }

}