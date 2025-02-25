using Core.Tasks;

namespace MVP.Models.Interface
{
    public interface ITaskModel
    {
        void CompleteTask(int taskID);
        public TaskSO GetNextTask();
    }
}