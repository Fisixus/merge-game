using System.Collections.Generic;
using Core.GridPawns.Enum;
using UnityEngine;

namespace Core.Tasks
{
    [CreateAssetMenu(fileName = "New Task", menuName = "Task System/New Task")]
    public class TaskSO : ScriptableObject
    {
        [field: SerializeField] public int TaskID { get; private set; }
        [field: SerializeField] public Texture CharTexture { get; private set; }
        [field: SerializeField] public List<Goal> Goals { get; private set; }
        //TODO:Reward for later steps
    }

    [System.Serializable]
    public class Goal
    {
        [field: SerializeField] public ApplianceType ApplianceType { get; private set; }
        [field: SerializeField] public int Level { get; private set; }
    }
}