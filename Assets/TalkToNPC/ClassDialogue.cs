using UnityEngine;

[System.Serializable]
public class DialogueChoice
{
    public string text;
    public int next;
}

[System.Serializable]
public class DialogueLine
{
    public int id;
    public string text;
    public DialogueChoice[] choices;
}

[System.Serializable]
public class DialogueData
{
    public DialogueLine[] npc_1;  // nom du NPC
}

