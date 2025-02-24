using MVP.Views.Interface;
using UnityEngine;
using UnityEngine.UI;

namespace MVP.Views
{
    public class MergeUIView : MonoBehaviour, IMergeUIView
    {
        [field: SerializeField] public Button DoneButton { get; private set; }
        
    }
}
