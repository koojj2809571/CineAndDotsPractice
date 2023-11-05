using DG.Tweening;
using UnityEngine;

namespace DOTweenTest
{
    public class DoTweenTest : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            var tweener =  transform.DOMove(new Vector3(7, 2, 3), 3).From();
            // var tweener = transform.DOLocalRotate(new Vector3(-180, 0, 0), 4);
            tweener.SetAutoKill(false);
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                transform.DOPlayForward();
            }
            
            if (Input.GetMouseButtonDown(1))
            {
                transform.DOPlayBackwards();
            }
        }
    }
}
