using DG.Tweening;
using UnityEngine;

namespace DOTweenTest
{
    public class ColorTween : MonoBehaviour
    {


        private Material _col;
        // Start is called before the first frame update
        void Start()
        {
            _col = GetComponent<MeshRenderer>().material;
            _col.DOColor(Color.red, 2);
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
