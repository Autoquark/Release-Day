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
        private float _targetSize;
        bool _first = true;

        public float TravelSpeed = 0.2f;
        public float SizeSpeed = 0.2f;
        public float MinCameraSize = 5.0f;

        private void Start()
        {
            var camera = GetComponent<Camera>();
            SetBounds(camera);

            _targetSize = MinCameraSize;
        }

        private void SetBounds(Camera camera)
        {
            var bounds = FindObjectsOfType<CameraBoundBehaviour>();
            _minX = bounds.MinOrDefault(x => x.transform.position.x, -999) + camera.orthographicSize * camera.aspect;
            _maxX = bounds.MaxOrDefault(x => x.transform.position.x, 999) - camera.orthographicSize * camera.aspect;
            _minY = bounds.MinOrDefault(x => x.transform.position.y, -999) + camera.orthographicSize;
            _maxY = bounds.MaxOrDefault(x => x.transform.position.y, 999) - camera.orthographicSize;

            if (_minX > _maxX)
            {
                _minX = _maxX = (_minX + _maxX) / 2;
            }

            if (_minY > _maxY)
            {
                _minY = _maxY = (_minY + _maxY) / 2;
            }
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

            var include = FindObjectsOfType<CameraIncludeBehaviour>();

            var all_points = include.Select(x => x.transform).Concat(rev_players.Select(x => x.transform));

            Vector3 min = all_points.Aggregate(first_player.transform.position, (x, y) => Vector3.Min(x, y.position));
            Vector3 max = all_points.Aggregate(first_player.transform.position, (x, y) => Vector3.Max(x, y.position));

            float dx = (max.x - min.x) / camera.aspect;
            float dy = max.y - min.y;

            _targetSize = Mathf.Max(MinCameraSize, dx, dy);

            Vector3 keep_pos = camera.transform.position;

            foreach (var point in all_points)
            {
                AccommodatePlayer(point);
            }

            _targetPosition = camera.transform.position;

            camera.transform.position = keep_pos;

            if (_targetPosition != null) {
                Vector3 full_move = _targetPosition - camera.transform.position;
                float max_move = full_move.magnitude;

                if (rev_players.Count > 1)
                {
                    max_move = Mathf.Min(max_move, TravelSpeed * Time.deltaTime);
                }

                if (max_move > 0)
                {
                    camera.transform.position += full_move.normalized * max_move;
                }

                float full_size_change = _targetSize - camera.orthographicSize;

                if (full_size_change != 0.0f)
                {
                    float scale_speed = SizeSpeed * Time.deltaTime;
                    full_size_change = Mathf.Min(scale_speed, Mathf.Max(-scale_speed, full_size_change));
                    camera.orthographicSize += full_size_change;

                    SetBounds(camera);
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
