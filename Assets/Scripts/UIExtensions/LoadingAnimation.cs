using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;

namespace UIExtensions
{
    public class LoadingAnimation : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _loadingText;
        [SerializeField] private float _animationSpeed = 0.5f;

        private StringBuilder _textBuilder = new StringBuilder("Loading");
        private int _dotCount = 0;
        private const int _maxDots = 3;
        private Coroutine _animationCoroutine;

        private void OnEnable()
        {
            // Start the animation when the object becomes active
            _animationCoroutine = StartCoroutine(AnimateLoadingText());
        }

        private void OnDisable()
        {
            // Stop the animation when the object becomes inactive
            if (_animationCoroutine != null)
            {
                StopCoroutine(_animationCoroutine);
            }
        }

        private IEnumerator AnimateLoadingText()
        {
            while (true)
            {
                _textBuilder.Clear();
                _textBuilder.Append("Loading");

                for (int i = 0; i < _dotCount; i++)
                {
                    _textBuilder.Append(".");
                }

                _loadingText.text = _textBuilder.ToString();
                _dotCount = (_dotCount + 1) % (_maxDots + 1);
                yield return new WaitForSeconds(_animationSpeed);
            }
        }
    }
}