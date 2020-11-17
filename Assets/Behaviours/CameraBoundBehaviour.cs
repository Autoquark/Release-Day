using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Behaviours
{
    class CameraBoundBehaviour : MonoBehaviour
    {
        private const float gizmoSize = 0.5f;

        private void OnDrawGizmos()
        {
            Gizmos.DrawLine(transform.position + Vector3.left * gizmoSize, transform.position + Vector3.right * gizmoSize);
            Gizmos.DrawLine(transform.position + Vector3.up * gizmoSize, transform.position + Vector3.down * gizmoSize);
        }
    }
}
