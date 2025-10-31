using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory: MonoBehaviour
{
    public List<int> items = new List<int>();

    public void AddItem(int itemId)
    {
        items.Add(itemId);
        Debug.Log("Objet " + itemId + " ajouter à l'inventaire :" + string.Join(", ", items));
    }

    public void clear()
    {
        items.Clear();
    }
}
