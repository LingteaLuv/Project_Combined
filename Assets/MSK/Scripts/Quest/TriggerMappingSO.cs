using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Quest/TriggerMappingTable")]
public class TriggerMappingSO : ScriptableObject
{
    public List<TriggerQuestPair> pairs;

    [System.Serializable]
    public class TriggerQuestPair
    {
        public string TriggerName;
        public string QuestID;
    }
}
