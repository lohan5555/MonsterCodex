using UnityEngine;
using UnityEngine.UIElements;

public class factLoader : MonoBehaviour
{
    [SerializeField] private UIDocument uiDocument;
    private ListView listView;

    void Start()
    {
        Debug.Log("[monsterData] start monsterData");
        var root = uiDocument.rootVisualElement;
        listView = root.Q<ListView>("monsterList");
        var monsterData = MonsterDataLoader.LoadData("monsterData");
        if (listView == null)
        {
            Debug.LogError("[ListViewTest] ❌ Impossible de trouver le ListView 'monsterList'");
            return;
        }

        // Crée la vue d’un élément (le visuel d’une ligne)
        listView.makeItem = () =>
        {
            var label = new Label();
            label.AddToClassList("monster-item");
            return label;
        };
        Debug.Log($"[monsterData] {monsterData.data.Length} lignes à afficher dans la liste");

        // Remplit chaque élément avec la donnée correspondante
        listView.bindItem = (element, i) =>
        {
            (element as Label).text = monsterData.data[i].text;
            Debug.Log(monsterData.data[i].text);
        };

        listView.itemsSource = monsterData.data;
        listView.fixedItemHeight = 40; // à ajuster selon ton style
        listView.selectionType = SelectionType.None; // si tu veux éviter la sélection

        listView.Rebuild();

    }
}
