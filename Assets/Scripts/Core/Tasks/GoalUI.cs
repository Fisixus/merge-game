using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Tasks
{
    public class GoalUI : MonoBehaviour
    {
        [field: SerializeField] public Image GoalImage { get; private set; }
        
        [field: SerializeField] public Image CheckImage { get; private set; }
        public Goal Goal { get;  set; }
        
        
        private void OnEnable()
        {
            CheckImage.transform.localScale = Vector3.zero;
        }
        
        public void SetCheckImage(bool isChecked)
        {
            CheckImage.transform.DOKill();

            if (isChecked)
            {
                CheckImage.transform.DOScale(1f, 0.3f);
            }
            else
            {
                CheckImage.transform.localScale = Vector3.zero;
            }
        }
        
 
    }
}
