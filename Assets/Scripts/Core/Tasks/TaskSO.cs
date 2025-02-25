using System.Collections.Generic;
using UnityEngine;

namespace Core.Tasks
{
    [CreateAssetMenu(fileName = "New Task", menuName = "Task System/New Task")]
    public class TaskSO : ScriptableObject
    {
        [field: SerializeField] public int TaskID { get; private set; }

        [field: SerializeField] public Texture CharTexture { get; private set; }

        //Max 3 per task recommended
        [field: SerializeField] public List<Goal> Goals { get; private set; }
        //TODO:Reward for later steps
    }
}