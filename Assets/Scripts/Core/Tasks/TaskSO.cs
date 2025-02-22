using UnityEngine;

namespace Core.Tasks
{
    [CreateAssetMenu(fileName = "New Task", menuName = "Task System/New Task")]
    public class TaskSO : ScriptableObject
    {
        [field: SerializeField] public int TaskID { get; private set; }
        [field: SerializeField] public Texture CharTexture { get; private set; }
        //TODO:Goals
        //TODO:Reward for later steps
    }
}