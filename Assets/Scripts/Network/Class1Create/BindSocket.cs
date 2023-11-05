using System;
using System.Collections;
using System.Net.Sockets;
using System.Text;
using System.Timers;
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace Network
{
    public class BindSocket : MonoBehaviour
    {

        public float speed = 1;
        public GameObject target;
        
        private Socket _socket;
        private Vector3 _lastPos;

        private Vector3 CurPos => transform.position;
        private Vector3 CurPosXZ => new Vector2(CurPos.x, CurPos.z);
        private Vector3 TargetPos => target.transform.position;
        private Vector3 TargetPosXZ => new Vector2(TargetPos.x, TargetPos.z);
        private float MoveDist => Vector3.Distance(CurPos, _lastPos);
        private float TargetDist => Vector2.Distance(CurPosXZ, TargetPosXZ);

        private void Start()
        {
            _socket = ConUtil.CreateSocket();
            SyncUtil.ConSync(ref _socket, "127.0.0.1", 9999);
            _lastPos = CurPos;
        }

        private void Update()
        {
            MoveByKey();
            CheckTarget();
            UploadCurTime();
        }

        private void MoveByKey()
        {
            var hMove = Input.GetAxis("Horizontal");
            var vMove = Input.GetAxis("Vertical");
            transform.position += new Vector3(hMove, 0, vMove) * Time.deltaTime * speed;
        }

        private void CheckTarget()
        {
            if (TargetDist > 1) return;
            UploadPosition();
        }

        private void UploadCurTime()
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                SyncUtil.SendText($"游戏已进行：{Time.timeAsDouble}s", _socket);
            }
        }

        private void UploadPosition()
        {
            if (_socket == null) return;
            SyncUtil.SendText($"当前位置：{CurPos.ToString()}", _socket);
            _lastPos = CurPos;
            StartCoroutine(nameof(WaitServer));
        }

        private IEnumerator WaitServer()
        {
            yield return SyncUtil.WaiServer(_socket);
        }
    }
}