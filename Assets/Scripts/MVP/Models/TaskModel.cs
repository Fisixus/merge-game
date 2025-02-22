using System.Collections.Generic;
using System.Linq;
using Core.Tasks;
using MVP.Models.Interface;
using UnityEngine;

namespace MVP.Models
{
    public class TaskModel : ITaskModel
    {
        private List<TaskSO> _allTasks;  // All available tasks
        private List<int> _completedTaskIDs; // Only store IDs of completed tasks

        public TaskModel()
        {
            LoadTasks();
        }
        
        private void LoadTasks()
        {
            // Load all available tasks from Resources
            _allTasks = Resources.LoadAll<TaskSO>("Tasks").ToList();

            // Load completed task IDs from PlayerPrefs
            _completedTaskIDs = LoadCompletedTasks();

            // Remove completed tasks from allTasks
            _allTasks = _allTasks.Where(task => !_completedTaskIDs.Contains(task.TaskID)).ToList();

            Debug.Log($"Loaded Tasks: {_allTasks.Count}, Completed Tasks: {_completedTaskIDs.Count}");
        }
        

        public void CompleteTask(int taskID)
        {
            if (!_completedTaskIDs.Contains(taskID))
            {
                _completedTaskIDs.Add(taskID);
                SaveCompletedTasks();

                // Remove completed task from `allTasks`
                _allTasks = _allTasks.Where(task => !_completedTaskIDs.Contains(task.TaskID)).ToList();

                Debug.Log($"Task {taskID} completed! Remaining Tasks: {_allTasks.Count}");
            }
        }

        private void SaveCompletedTasks()
        {
            string json = string.Join(",", _completedTaskIDs.Select(id => id.ToString()).ToArray());
            PlayerPrefs.SetString("CompletedTasks", json);
            PlayerPrefs.Save();
        }

        private List<int> LoadCompletedTasks()
        {
            if (!PlayerPrefs.HasKey("CompletedTasks")) return new List<int>();

            string json = PlayerPrefs.GetString("CompletedTasks");
            return json.Split(',').Select(int.Parse).ToList();
        }

        // public void ResetTasks()
        // {
        //     _completedTaskIDs.Clear();
        //     PlayerPrefs.DeleteKey("CompletedTasks");
        //     LoadTasks();
        // }
    }
}
