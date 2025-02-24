using System;
using System.Collections.Generic;
using Core.GridPawns;
using Core.GridPawns.Enum;
using Core.Helpers;
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
        [field: SerializeField] public List<GoalUI> AllGoalUIs { get; private set; }
        
        private int _goalUIIndex;
        private Dictionary<GoalUI, Appliance> _matchedAppliances; // ✅ Track matched pawns
        
        public List<GoalUI> ActiveGoals { get; private set; }
        private void OnEnable()
        {
            _goalUIIndex = 0;
            DoneButton.transform.localScale = Vector3.zero;
            
            _matchedAppliances = new Dictionary<GoalUI, Appliance>();
            ActiveGoals = new List<GoalUI>();
            
            foreach (var goalUI in AllGoalUIs)
            {
                goalUI.gameObject.SetActive(false);
            }
            
            DoneButton.onClick.AddListener(OnCompleteTask);
            
            //TODO: Listen Merge has happened(CheckGoals)
        }


        private void OnDisable()
        {
            DoneButton.onClick.RemoveListener(OnCompleteTask);
            foreach (var goalUI in AllGoalUIs)
            {
                goalUI.gameObject.SetActive(false);
            }

            //TODO: Stop listening Merge has happened(CheckGoals)
        }

        public void SetGoalUI(Goal goal, Sprite sprite)
        {
            AllGoalUIs[_goalUIIndex].gameObject.SetActive(true);
            var newGoal = new Goal
            {
                ApplianceType = goal.ApplianceType,
                Level = goal.Level
            };
            AllGoalUIs[_goalUIIndex].Goal = newGoal;
            AllGoalUIs[_goalUIIndex].GoalImage.sprite = sprite;
            
            ActiveGoals.Add(AllGoalUIs[_goalUIIndex++]);
        }

        public void MatchGoal(GoalUI goalUI, GridPawn pawn)
        {
            if (pawn is Appliance appliance)
            {
                _matchedAppliances[goalUI] = appliance; // Store valid match
                goalUI.SetCompletion(true); // Mark goal as completed
                appliance.PawnEffect.SetGlowing(true);
            }
            else
            {
                _matchedAppliances.Remove(goalUI); // Remove invalid match
                goalUI.SetCompletion(false); // Reset completion if no match is found
            }

            CheckAllGoalsCompleted(); // Recalculate if "Done" button should be visible
        }


        private void CheckAllGoalsCompleted()
        {
            if (_matchedAppliances.Count == ActiveGoals.Count)
            {
                SetDoneButton(true);
            }
            else
            {
                SetDoneButton(false);
            }
        }

        private void OnCompleteTask()
        {
            //TODO: Destroy mergedAppliances only goals though
            //
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
                DoneButton.transform.DOScale(0f, 0.3f);
            }
        }
        
    }
}
