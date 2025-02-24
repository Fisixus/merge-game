using System;
using System.Collections.Generic;
using Core.GridPawns;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Tasks
{
    public class TaskUI : MonoBehaviour
    {
        public int TaskID { get; set; }
        [field: SerializeField] public RawImage CharImage { get; set; }
        [field: SerializeField] public Button DoneButton { get; private set; }
        [field: SerializeField] public List<GoalUI> GoalUIs { get; private set; }
        

        private List<Appliance> _mergedAppliances = new List<Appliance>(4);
        private int _goalUIIndex;

        private void OnEnable()
        {
            _goalUIIndex = 0;
            DoneButton.transform.localScale = Vector3.zero;
            DoneButton.onClick.AddListener(OnCompleteTask);
            foreach (var goalUI in GoalUIs)
            {
                goalUI.gameObject.SetActive(false);
            }
            
            //TODO: Listen Merge has happened(CheckGoals)
        }


        private void OnDisable()
        {
            DoneButton.onClick.RemoveListener(OnCompleteTask);
            foreach (var goalUI in GoalUIs)
            {
                goalUI.gameObject.SetActive(false);
            }

            //TODO: Stop listening Merge has happened(CheckGoals)
        }

        public void CheckGoals(Appliance mergedAppliance)
        {
            foreach (var goalUI in GoalUIs)
            {
                var type = goalUI.Goal.ApplianceType;
                var level = goalUI.Goal.Level;

                if (mergedAppliance.ApplianceType == type && mergedAppliance.Level == level)
                {
                    goalUI.SetCheckImage(true);
                    //TODO:mergedAppliance.ApplianceEffect.SetGlowing(true);
                    _mergedAppliances.Add(mergedAppliance);
                }
                
            }
        }

        private void OnCompleteTask()
        {
            //TODO: Destroy mergedAppliances only goals though
            //
        }
        

        public void SetGoalUI(Goal goal, Sprite sprite)
        {
            GoalUIs[_goalUIIndex].gameObject.SetActive(true);
            var newGoal = new Goal();
            newGoal.ApplianceType = goal.ApplianceType;
            newGoal.Level = goal.Level;
            GoalUIs[_goalUIIndex].Goal = newGoal;
            GoalUIs[_goalUIIndex].GoalImage.sprite = sprite;
            _goalUIIndex++;
        }


        public void SetDoneButton(bool isDone)
        {
            DoneButton.transform.DOKill();

            if (isDone)
            {
                DoneButton.transform.DOScale(1f, 0.3f);
            }
            else
            {
                DoneButton.transform.localScale = Vector3.zero;
            }
        }
        
    }
}
