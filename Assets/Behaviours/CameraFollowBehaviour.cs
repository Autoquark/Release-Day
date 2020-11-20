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
        private const float _margin = 0.4f;
        private const float _multi_margin = 0.2f;

        private float _minX;
        private float _maxX;
        private float _minY;
        private float _maxY;
        private Vector3 _targetPosition;
        private float _targetSize;
        private bool _first = true;
        private float _shrinkStart = 0;

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

        bool InBounds(Transform trn)
        {
            var bounds = FindObjectsOfType<CameraBoundBehaviour>();
            var minX = bounds.MinOrDefault(x => x.transform.position.x, -999);
            var maxX = bounds.MaxOrDefault(x => x.transform.position.x, 999);
            var minY = bounds.MinOrDefault(x => x.transform.position.y, -999);
            var maxY = bounds.MaxOrDefault(x => x.transform.position.y, 999);

            return trn.position.x > minX
                && trn.position.x < maxX
                && trn.position.y > minY
                && trn.position.y < maxY;
        }

        private void Update()
        {
            PlayerControllerBehaviour first_player = this.FirstPlayer();

            if (first_player == null)
                return;

            if (_first)
            {
                _first = false;
                AccommodatePlayer(first_player.transform, _margin);
            }

            List<PlayerControllerBehaviour> rev_players = this.AllPlayers().ToList();
            rev_players.Reverse();

            var camera = GetComponent<Camera>();

            var include = FindObjectsOfType<CameraIncludeBehaviour>();

            var all_points = include.Select(x => x.transform).Concat(rev_players.Select(x => x.transform)).Where(x => InBounds(x)).ToList();

            float margin = all_points.Count > 1 ? _multi_margin : _margin;

            Vector3 min = all_points.Aggregate(first_player.transform.position, (x, y) => Vector3.Min(x, y.position));
            Vector3 max = all_points.Aggregate(first_player.transform.position, (x, y) => Vector3.Max(x, y.position));

            float dx = (max.x - min.x) / camera.aspect;
            float dy = (max.y - min.y);

            _targetSize = Mathf.Max(MinCameraSize, dx / (2 - margin * 4), dy / (2 - margin * 4));

            Vector3 keep_pos = camera.transform.position;

            foreach (var point in all_points)
            {
                AccommodatePlayer(point, margin);
            }

            _targetPosition = camera.transform.position;

            camera.transform.position = keep_pos;

            if (_targetPosition != null) {
                Vector3 full_move = _targetPosition - camera.transform.position;
                float max_move = full_move.magnitude;

                if (all_points.Count() > 1)
                {
                    max_move = Mathf.Min(max_move, TravelSpeed * Time.deltaTime);
                }

                if (max_move > 0)
                {
                    camera.transform.position += full_move.normalized * max_move;
                }

                float full_size_change = _targetSize - camera.orthographicSize;

                if (full_size_change > 0.0f)
                {
                    float scale_speed = SizeSpeed * Time.deltaTime;
                    full_size_change = Mathf.Min(scale_speed, Mathf.Max(-scale_speed, full_size_change));
                    camera.orthographicSize += full_size_change;

                    SetBounds(camera);
                }
                else if (full_size_change < -0.001f)
                {
                    if (_shrinkStart != 0 && _shrinkStart < Time.time)
                    {
                        float scale_speed = SizeSpeed * Time.deltaTime;
                        full_size_change = Mathf.Min(scale_speed, Mathf.Max(-scale_speed, full_size_change));

                        if (full_size_change < -0.001f)
                        {
                            camera.orthographicSize += full_size_change;
                            SetBounds(camera);
                        }
                    }
                    else if (_shrinkStart == 0)
                    {
                        _shrinkStart = Time.time + 2;
                    }
                }
                else
                {
                    _shrinkStart = 0;
                }
            }
        }

        private void AccommodatePlayer(Transform player, float margin)
        {
            var camera = GetComponent<Camera>();

            var deadzone = RectEx.FromCorners(camera.ViewportToWorldPoint(new Vector2(margin, margin)), camera.ViewportToWorldPoint(new Vector2(1 - margin, 1 - margin)));

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
