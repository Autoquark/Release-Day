using Assets.Common;
using Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Assets.Extensions;

namespace Assets.Behaviours
{
    class CameraFollowBehaviour : MonoBehaviour
    {
        private const float _margin = 0.3f;

        private float _minX;
        private float _maxX;
        private float _minY;
        private float _maxY;
        private Vector3 _targetPosition;
        bool _first = true;

        public float TravelSpeed = 0.2f;

        private void Start()
        {
            var camera = GetComponent<Camera>();
            var bounds = FindObjectsOfType<CameraBoundBehaviour>();
            _minX = bounds.MinOrDefault(x => x.transform.position.x, -999) + camera.orthographicSize * camera.aspect;
            _maxX = bounds.MaxOrDefault(x => x.transform.position.x, 999) - camera.orthographicSize * camera.aspect;
            _minY = bounds.MinOrDefault(x => x.transform.position.y, -999) + camera.orthographicSize;
            _maxY = bounds.MaxOrDefault(x => x.transform.position.y, 999) - camera.orthographicSize;
        }

        private void Update()
        {
            PlayerControllerBehaviour first_player = this.FirstPlayer();
            if (_first && first_player != null)
            {
                _first = false;
                AccommodatePlayer(first_player.transform);
            }

            List<PlayerControllerBehaviour> rev_players = this.AllPlayers().ToList();
            rev_players.Reverse();

            var camera = GetComponent<Camera>();

            Vector3 keep_pos = camera.transform.position;

            foreach (var player in rev_players)
            {
                AccommodatePlayer(player.transform);
            }

            _targetPosition = camera.transform.position;

            camera.transform.position = keep_pos;

            if (_targetPosition != null) {
                Vector3 full_move = _targetPosition - camera.transform.position;
                float len = full_move.magnitude;
                float max_move = Mathf.Min(len, Time.deltaTime * TravelSpeed);
                Vector3 move = full_move.normalized * max_move;

                if (move != Vector3.zero)
                {
                    camera.transform.position += move;
                }
            }
        }

        private void AccommodatePlayer(Transform player)
        {
            var camera = GetComponent<Camera>();
            var deadzone = RectEx.FromCorners(camera.ViewportToWorldPoint(new Vector2(_margin, _margin)), camera.ViewportToWorldPoint(new Vector2(1 - _margin, 1 - _margin)));

            var playerPosition = player.position;
            if (playerPosition.x < deadzone.xMin)
            {
                camera.transform.position -= new Vector3(deadzone.xMin - playerPosition.x, 0);
            }
            else if (playerPosition.x > deadzone.xMax)
            {
                camera.transform.position += new Vector3(playerPosition.x - deadzone.xMax, 0);
            }

            if (playerPosition.y < deadzone.yMin)
            {
                camera.transform.position -= new Vector3(0, deadzone.yMin - playerPosition.y);
            }
            else if (playerPosition.y > deadzone.yMax)
            {
                camera.transform.position += new Vector3(0, playerPosition.y - deadzone.yMax);
            }

            camera.transform.position = new Vector3(Mathf.Clamp(camera.transform.position.x, _minX, _maxX), Mathf.Clamp(camera.transform.position.y, _minY, _maxY), transform.position.z);
        }
    }
}
