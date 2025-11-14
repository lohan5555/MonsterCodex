using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;
using System.Linq;

public class factLoader : MonoBehaviour
{
    [SerializeField] private UIDocument uiDocument;
    private ListView listView;
    public PlayerInventory playerInventory;

    void Start()
    {
        var listFacts = playerInventory.getFacts();
        var root = uiDocument.rootVisualElement;
        listView = root.Q<ListView>("monsterList");
        var monsterData = MonsterDataLoader.LoadData("monsterData");

        //on filtre pour n'afficher que ce que le joueur peut voir
        List<MonsterEntry> visibleFacts = new List<MonsterEntry>();
        foreach (int factId in listFacts)
        {
            if (factId >= 0 && factId < monsterData.data.Length)
                visibleFacts.Add(monsterData.data[factId]);
        }

        // Crée la vue d’un élément (le visuel d’une ligne)
        listView.makeItem = () =>
        {
            var label = new Label();
            label.AddToClassList("monster-item");
            return label;
        };

        // Remplit chaque élément avec la donnée correspondante
        listView.bindItem = (element, i) =>
        {
            (element as Label).text = visibleFacts[i].text;
            Debug.Log(visibleFacts[i].text);
        };

        listView.itemsSource = visibleFacts;
        listView.fixedItemHeight = 40;
        listView.selectionType = SelectionType.None;

        listView.Rebuild();

    }
}
