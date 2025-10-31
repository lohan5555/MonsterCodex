using UnityEngine;
using UnityEngine.SceneManagement;

public class Niveau : MonoBehaviour
{
    public string SceneDestination;

    private void OnTriggerEnter(Collider other)
    {
        PlayerInventory playerInv = FindAnyObjectByType<PlayerInventory>();

        if (playerInv != null)
        {
            if (SceneDestination == "level_Two" && playerInv.items.Contains(0))
            {
                SceneManager.LoadScene(SceneDestination);
            }
            if (SceneDestination == "level_Three" && playerInv.items.Contains(1))
            {
                SceneManager.LoadScene(SceneDestination);
            }
        }
    }
}
