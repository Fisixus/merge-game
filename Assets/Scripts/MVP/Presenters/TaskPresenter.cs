using System;
using System.Collections.Generic;
using System.Linq;
using Core.Factories.Interface;
using Core.GridEffects;
using Core.GridPawns;
using Core.Helpers;
using Core.Tasks;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using MVP.Models.Interface;
using MVP.Presenters.Handlers;
using UnityEngine;

namespace MVP.Presenters
{
    public class TaskPresenter
    {
        private const int MaxTaskCount = 2;
        private readonly IGridModel _gridModel;
        private readonly MergePresenter _mergePresenter;
        private readonly ITaskModel _taskModel;
        private readonly ITaskUIFactory _taskUIFactory;
        private readonly IApplianceFactory _applianceFactory;
        private readonly DisappearEffectHandler _disappearEffectHandler;
        private readonly GridPawnFactoryHandler _gridPawnFactoryHandler;

        private List<TaskUI> _activeTasks = new List<TaskUI>();
        private GridPawn[,] _grid;
        
        public TaskPresenter(IGridModel gridModel, DisappearEffectHandler disappearEffectHandler, GridPawnFactoryHandler gridPawnFactoryHandler, ITaskModel taskModel, 
            ITaskUIFactory taskUIFactory, IApplianceFactory applianceFactory)
        {
            _gridModel = gridModel;
            _disappearEffectHandler = disappearEffectHandler;
            _gridPawnFactoryHandler = gridPawnFactoryHandler;
            //_mergePresenter = mergePresenter;
            _taskModel = taskModel;
            _taskUIFactory = taskUIFactory;
            _applianceFactory = applianceFactory;
        }

        public async UniTask InitializeTasks()
        {
            _grid = _gridModel.Grid;
            for (int i = 0; i < MaxTaskCount; i++)
            {
                LoadNextTask().Forget();
            }

            UpdateTasks();
            
            await UniTask.Delay(TimeSpan.FromSeconds(0.1f), DelayType.DeltaTime);

        }

        public async UniTask CompleteTask(int taskID, List<Appliance> appliancesToDestroy)
        {
            var taskToComplete = _activeTasks.FirstOrDefault(task => task.TaskID == taskID);
            if (taskToComplete == null)
            {
                Debug.LogWarning("Completed task cannot find the active tasks!");
                return;
            }

            DestroyAppliances(appliancesToDestroy);
            UpdateTasks();
            await AnimateCompletedTask(taskToComplete);
            _activeTasks.Remove(taskToComplete);
            _taskUIFactory.DestroyObj(taskToComplete);
            _taskModel.CompleteTask(taskID);
            LoadNextTask().Forget();
        }

        private async UniTask AnimateCompletedTask(TaskUI completedTask, float animDuration = 0.3f)
        {
            var cg = completedTask.CanvasGroup;
            cg.interactable = false;
            cg.blocksRaycasts = false;
            DOTween.To(() => cg.alpha, x => cg.alpha = x, 0, animDuration);
            await UniTask.Delay(TimeSpan.FromSeconds(animDuration), DelayType.DeltaTime);

        }

        private async UniTask AnimateNewTask(TaskUI newTask, float animDuration = 0.3f)
        {
            var cg = newTask.CanvasGroup;
            cg.interactable = true;
            cg.blocksRaycasts = true;
            DOTween.To(() => cg.alpha, x => cg.alpha = x, 1, animDuration);
            await UniTask.Delay(TimeSpan.FromSeconds(animDuration), DelayType.DeltaTime);

        }
        
        private void HandleApplianceDestruction(GridPawn pawn)
        {
            _disappearEffectHandler.PlayDisappearEffect(pawn.transform.position, ColorType.Green).Forget();
            _gridPawnFactoryHandler.DestroyPawn(pawn);
            _gridModel.UpdateGridPawn(pawn, true);
            pawn.PawnEffect.SetFocus(false);
        }
        
        private void DestroyAppliances(List<Appliance> appliancesToDestroy)
        {
            foreach (var appliance in appliancesToDestroy)
            {
                HandleApplianceDestruction(appliance);
            }
        }

        private async UniTask LoadNextTask()
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
            newTaskUI.SubscribeDoneButton(this);
            _activeTasks.Add(newTaskUI);
            newTaskUI.CanvasGroup.alpha = 0f;
            await AnimateNewTask(newTaskUI);
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
