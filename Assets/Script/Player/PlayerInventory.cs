using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory: MonoBehaviour
{
    public List<int> items = new List<int>();
    public List<int> facts = new List<int>();

    public void AddItem(int itemId)
    {
        items.Add(itemId);
        Debug.Log("Objet " + itemId + " ajouter à l'inventaire :" + string.Join(", ", items));
    }

    public void AddFacts(int factId)
    {
        facts.Add(factId);
        Debug.Log("Objet " + factId + " ajouter à l'inventaire :" + string.Join(", ", facts));
    }

    public void clear()
    {
        items.Clear();
        facts.Clear();
    }
}
