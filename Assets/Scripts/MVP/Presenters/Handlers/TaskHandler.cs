using System.Collections.Generic;
using Core.Factories.Interface;
using Core.GridPawns;
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

        public TaskHandler(ITaskModel taskModel, ITaskUIFactory taskUIFactory, IApplianceFactory applianceFactory)
        {
            _taskModel = taskModel;
            _taskUIFactory = taskUIFactory;
            _applianceFactory = applianceFactory;
        }

        public void InitializeTasks()
        {
            for (int i = 0; i < MaxTaskCount; i++)
            {
                LoadNextTask();
            }
        }

        public void CompleteTask(int taskID)
        {
            _taskModel.CompleteTask(taskID);
            LoadNextTask();
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
        }


        public void MarkMatchingPawnsWIthGoals(GridPawn[,] grid)
        {
            
        }
        
        
    }
}
