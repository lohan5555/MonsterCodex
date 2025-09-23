using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;

public class NPCSystem : MonoBehaviour
{
    [Tooltip("UIDocument du prefab UI_TouchScreenInput (drag & drop ici). Laisser vide pour auto-find.")]
    public UIDocument uiDocument;

    [Tooltip("Nom exact dans UI Builder (champ 'Name')")]
    public string buttonName = "ButtonTalk";

    private VisualElement talkVE;
    private bool playerInRange = false;

    private void Start()
    {
        // trouve ou vérifie l'UIDocument
        if (uiDocument == null)
        {
            if (uiDocument == null)
            {
                Debug.LogError("[NPCSystem] Aucun UIDocument trouvé dans la scène. Assigne UI_TouchScreenInput dans l'inspector !");
                return;
            }
            Debug.Log("[NPCSystem] UIDocument trouvé automatiquement : " + uiDocument.gameObject.name);
        }

        var root = uiDocument.rootVisualElement;
        if (root == null)
        {
            Debug.LogError("[NPCSystem] rootVisualElement null. UIDocument pas prêt ?");
            return;
        }

        // cherche l'élément par name (c'est un VisualElement dans ton cas)
        talkVE = root.Q<VisualElement>(buttonName);

        if (talkVE == null)
        {
            Debug.LogError($"[NPCSystem] '{buttonName}' introuvable dans le root. Vérifie le champ Name dans UI Builder et que tu as assigné le bon UIDocument.");
            // dump court pour aider au debug
            var all = root.Query<VisualElement>().ToList();
            Debug.Log($"[NPCSystem] total VisualElements dans root: {all.Count} (liste 0..10)");
            for (int i = 0; i < Mathf.Min(10, all.Count); i++)
                Debug.Log($"  VE[{i}]: type={all[i].GetType().Name}, name='{all[i].name}', classes='{string.Join(",", all[i].GetClasses())}'");
            return;
        }

        Debug.Log("[NPCSystem] Element trouvé: type=" + talkVE.GetType().Name + " name=" + talkVE.name + " classes=" + string.Join(",", talkVE.GetClasses()));

        // s'assurer que le picking mode permet le clic (sinon ClickEvent ne se déclenchera pas)
        talkVE.pickingMode = PickingMode.Position;

        // cacher au départ
        talkVE.style.display = DisplayStyle.None;

        // écoute le clic (même si c'est un VisualElement)
        talkVE.RegisterCallback<ClickEvent>(evt => OnTalkButtonPressed());

       
    }

    // si un parent a display None on le force visible (fallback)
    private void EnsureAncestorsVisible(VisualElement ve)
    {
        if (ve == null) return;
        VisualElement current = ve.parent;
        while (current != null)
        {
            // si un ancêtre est caché, on le rend visible
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
            // si un parent est caché on le rend visible (sinon l'enfant restera invisible)
            EnsureAncestorsVisible(talkVE);

            talkVE.style.display = DisplayStyle.Flex;
            // en cas de styles USS agressifs on peut aussi forcer l'opacité et la visibilité
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

        var npcLines = DialogueManager.Instance.dialogues.npc_1;
        DialogueUI.Instance.StartDialogue(npcLines);
    }

    public void OnTouch(InputAction.CallbackContext context)
    {
        if (context.performed)
            DialogueUI.Instance?.NextLine();
    }

}