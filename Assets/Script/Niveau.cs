using UnityEngine;
using UnityEngine.SceneManagement;

public class Niveau : MonoBehaviour
{
    public string NomDeScene;

    private void OnTriggerEnter(Collider other)
    {
        PlayerInventory playerInv = FindAnyObjectByType<PlayerInventory>();

        if (playerInv != null)
        {
             Debug.Log($"scène: {NomDeScene}");
            switch (NomDeScene)
            {
                case "level_Two":
                    if (playerInv.items.Contains(0))
                    {
                        PlayerSpawnManager.nextSpawnID = "default";
                        SceneManager.LoadScene(NomDeScene);
                    }
                    break;
                case "level_Three":
                    if (playerInv.items.Contains(1))
                    {
                        PlayerSpawnManager.nextSpawnID = "default";
                        SceneManager.LoadScene(NomDeScene);
                    }
                    break;
                default:
                    Debug.Log($"scene inconnu: {NomDeScene}");
                    break;
            }
        }
    }
}
