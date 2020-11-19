using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Assets.Data;
using UnityEngine.Tilemaps;

namespace Assets.Behaviours
{

    class CorpseController : MonoBehaviour
    {
        private readonly Lazy<Tilemap> _tileMap;

        CorpseController()
        {
            _tileMap = new Lazy<Tilemap>(FindObjectOfType<Tilemap>);
        }

        private void Update()
        {
            if (_tileMap.Value.localBounds.SqrDistance(transform.position) > 400)
            {
                GameObject.Destroy(gameObject);
            }
        }
    }
}
