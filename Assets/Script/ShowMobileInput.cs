using UnityEngine;
using UnityEngine.UIElements;

public class ShowMobileInput : MonoBehaviour
{
    void Start()
    {
        // Cherche le premier UIDocument actif dans la scène
        UIDocument doc = Object.FindFirstObjectByType<UIDocument>();
        if (doc == null)
        {
            Debug.LogError("[ShowMobileInput] Aucun UIDocument trouvé dans la scène !");
            return;
        }

        var root = doc.rootVisualElement;
        var mobileInput = root.Q<VisualElement>("MobileInput");

        if (mobileInput != null)
        {
            mobileInput.style.display = DisplayStyle.Flex;
            Debug.Log("[ShowMobileInput] #MobileInput affiché !");
        }
        else
        {
            Debug.LogWarning("[ShowMobileInput] Élément #MobileInput introuvable !");
        }
    }
}
