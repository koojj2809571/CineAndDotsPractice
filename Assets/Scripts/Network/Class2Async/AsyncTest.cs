using System.Net.Sockets;
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace Network
{
    public class AsyncTest: MonoBehaviour
    {

        public float speed = 1;
        public GameObject target;
        
        private Socket _socket;
        private Vector3 _lastPos;
        private bool _canUploadPosition = true;

        private Vector3 CurPos => transform.position;
        private Vector3 CurPosXZ => new Vector2(CurPos.x, CurPos.z);
        private Vector3 TargetPos => target.transform.position;
        private Vector3 TargetPosXZ => new Vector2(TargetPos.x, TargetPos.z);
        private float MoveDist => Vector3.Distance(CurPos, _lastPos);
        private float TargetDist => Vector2.Distance(CurPosXZ, TargetPosXZ);

        private void Start()
        {
            _socket = ConUtil.CreateSocket();
            AsyncUtil.ConAsync(_socket, "127.0.0.1", 9999);
            _lastPos = CurPos;
        }

        private void Update()
        {
            MoveByKey();
            UploadCurTime();
            CheckTarget();
            _lastPos = CurPos;
        }

        private void MoveByKey()
        {
            var hMove = Input.GetAxis("Horizontal");
            var vMove = Input.GetAxis("Vertical");
            transform.position += new Vector3(hMove, 0, vMove) * Time.deltaTime * speed;
        }

        private void CheckTarget()
        {
            switch (TargetDist)
            {
                case > 2:
                    _canUploadPosition = true;
                    return;
                case > 1:
                    return;
                default:
                    UploadPosition();
                    break;
            }
        }

        private void UploadCurTime()
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                AsyncUtil.Send(_socket, $"游戏已进行：{Time.timeAsDouble}s");
            }
        }

        private void UploadPosition()
        {
            if (!_canUploadPosition) return;
            if (_socket == null) return;
            _canUploadPosition = false;
            AsyncUtil.Send(_socket,$"当前位置：{CurPos.ToString()}");
        }
    }
}