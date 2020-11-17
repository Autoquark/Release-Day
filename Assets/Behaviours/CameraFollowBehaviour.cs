using Assets.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Behaviours
{
    class CameraFollowBehaviour : MonoBehaviour
    {
        private const float _margin = 0.3f;

        private Lazy<Transform> _player;
        private float _minX;
        private float _maxX;
        private float _minY;
        private float _maxY;

        public CameraFollowBehaviour()
        {
            _player = new Lazy<Transform>(() => FindObjectOfType<PlayerControllerBehaviour>().transform);
        }

        private void Start()
        {
            var camera = GetComponent<Camera>();
            var bounds = FindObjectsOfType<CameraBoundBehaviour>();
            _minX = bounds.Min(x => x.transform.position.x) + camera.orthographicSize * camera.aspect;
            _maxX = bounds.Max(x => x.transform.position.x) - camera.orthographicSize * camera.aspect;
            _minY = bounds.Min(x => x.transform.position.y) + camera.orthographicSize;
            _maxY = bounds.Max(x => x.transform.position.y) - camera.orthographicSize;
        }

        private void Update()
        {
            var camera = GetComponent<Camera>();
            var deadzone = RectEx.FromCorners(camera.ViewportToWorldPoint(new Vector2(_margin, _margin)), camera.ViewportToWorldPoint(new Vector2(1 - _margin, 1 - _margin)));

            var playerPosition = _player.Value.position;
            if (playerPosition.x < deadzone.xMin)
            {
                transform.position -= new Vector3(deadzone.xMin - playerPosition.x, 0);
            }
            else if (playerPosition.x > deadzone.xMax)
            {
                transform.position += new Vector3(playerPosition.x - deadzone.xMax, 0);
            }

            if (playerPosition.y < deadzone.yMin)
            {
                transform.position -= new Vector3(0, deadzone.yMin - playerPosition.y);
            }
            else if (playerPosition.y > deadzone.yMax)
            {
                transform.position += new Vector3(0, playerPosition.y - deadzone.yMax);
            }

            transform.position = new Vector3(Mathf.Clamp(transform.position.x, _minX, _maxX), Mathf.Clamp(transform.position.y, _minY, _maxY), transform.position.z);
        }
    }
}
