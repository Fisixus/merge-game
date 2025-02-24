using System.Collections.Generic;
using System.Linq;
using Core.Factories.Interface;
using Core.GridPawns;
using Core.Helpers;
using Core.Tasks;
using MVP.Models.Interface;
using UnityEngine;

namespace MVP.Presenters.Handlers
{
    public class TaskHandler
    {
        private const int MaxTaskCount = 2;
        private readonly ITaskModel _taskModel;
        private readonly ITaskUIFactory _taskUIFactory;
        private readonly IApplianceFactory _applianceFactory;

        private List<TaskUI> _activeTasks = new List<TaskUI>();
        private GridPawn[,] _grid;
        public TaskHandler(ITaskModel taskModel, ITaskUIFactory taskUIFactory, IApplianceFactory applianceFactory)
        {
            _taskModel = taskModel;
            _taskUIFactory = taskUIFactory;
            _applianceFactory = applianceFactory;
        }

        public void InitializeTasks(GridPawn[,] grid)
        {
            _grid = grid;

            for (int i = 0; i < MaxTaskCount; i++)
            {
                LoadNextTask();
            }

            UpdateTasks();
        }

        public void CompleteTask(int taskID)
        {
            var taskToComplete = _activeTasks.FirstOrDefault(task => task.TaskID == taskID);
            if (taskToComplete != null)
            {
                _activeTasks.Remove(taskToComplete); // Remove from active tasks
                _taskUIFactory.DestroyObj(taskToComplete);
                _taskModel.CompleteTask(taskID);
                LoadNextTask();
            }
            else
            {
                Debug.LogWarning("Completed task could not find the active tasks!");
            }
            
        }

        private void LoadNextTask()
        {
            var taskInfo = _taskModel.GetNextTask();
            if (taskInfo == null) return;

            var newTaskUI = _taskUIFactory.CreateObj();
            
            newTaskUI.TaskID = taskInfo.TaskID;
            newTaskUI.CharImage.texture = taskInfo.CharTexture;

            foreach (var goal in taskInfo.Goals)
            {
                if (_applianceFactory.ApplianceDataDict.TryGetValue(goal.ApplianceType, out var applianceData) &&
                    applianceData.ApplianceLevelDataDict.TryGetValue(goal.Level, out var levelData))
                {
                    newTaskUI.SetGoalUI(goal, levelData.ApplianceSprite);
                }
                else
                {
                    Debug.LogWarning($"Missing appliance data for {goal.ApplianceType} at level {goal.Level}");
                }
            }
            
            _activeTasks.Add(newTaskUI);
        }

        public void UpdateTasks()
        {
            foreach (var taskUI in _activeTasks)
            {
                foreach (var goalUI in taskUI.ActiveGoals)
                {
                    var goalPawn = GridPawnFinderHelper.FindGridPawn(_grid, goalUI.Goal.ApplianceType, goalUI.Goal.Level);
                    taskUI.MatchGoal(goalUI, goalPawn);
                }
            }
        }
    }
}
