using UnityEngine;
using UnityEngine.UIElements;

public class BossUI : MonoBehaviour
{
    public static BossUI Instance;

    private VisualElement bossBar;
    private VisualElement bossFill;
    private Label bossName;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        var root = GetComponent<UIDocument>().rootVisualElement;

        bossBar = root.Q<VisualElement>("BossHealthBarContainer");
        bossFill = root.Q<VisualElement>("BossHealthFill");
        bossName = root.Q<Label>("BossNameLabel");

        bossBar.style.display = DisplayStyle.None;
    }

    public void ShowBossBar(string name, int maxHealth)
    {
        bossBar.style.display = DisplayStyle.Flex;
        bossName.text = name;
        bossFill.style.width = new Length(100, LengthUnit.Percent);
    }

    public void UpdateBossHealth(float current, float max)
    {
        float pct = Mathf.Clamp01(current / max) * 100f;
        bossFill.style.width = new Length(pct, LengthUnit.Percent);
    }

    public void HideBossBar()
    {
        bossBar.style.display = DisplayStyle.None;
    }
}
