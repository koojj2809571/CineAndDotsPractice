using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace DOTweenTest
{
    public class HeartTween : MonoBehaviour
    {

        public Image heart;
        
        // Start is called before the first frame update
        void Start()
        {
            heart.DOFade(0, 1).SetLoops(-1, LoopType.Yoyo);
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
