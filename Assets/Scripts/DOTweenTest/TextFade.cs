using DG.Tweening;
using TMPro;
using UnityEngine;

namespace DOTweenTest
{
    public class TextFade : MonoBehaviour
    {

        public TMP_Text text;
        
        // Start is called before the first frame update
        void Start()
        {
            text.alpha = 0;
            DOTween.Sequence().Append(text.DOFade(1f, 2)).OnComplete(Callback);
        }

        private void Callback()
        {
            text.text = "DOTween";
        }
    }
}
