using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSpawnManager : MonoBehaviour
{
    //public static string nextSpawnID = "default";

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SpawnPoint spawn = FindAnyObjectByType<SpawnPoint>();

        if (spawn != null)
        {
            var player = FindAnyObjectByType<PlayerInventory>()?.gameObject;

            if (player != null)
            {
                Debug.Log("Joueur placer");
                player.transform.position = spawn.transform.position;
            }
            else
            {
                Debug.LogWarning($"Aucun joueur trouvé après le chargement de la scène {scene.name} !");
            }
        }
    }
}
