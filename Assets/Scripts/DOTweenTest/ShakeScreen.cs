using DG.Tweening;
using UnityEngine;

namespace DOTweenTest
{
    public class ShakeScreen : MonoBehaviour
    {

        public float time;
        public float force;
        public int number;
        public float ran;
        public bool snap = false;
        public bool fakeIn = true;
        
        // Start is called before the first frame update
        void Start()
        {
            transform.DOShakePosition(time, force, number, ran, snap, fakeIn);
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
