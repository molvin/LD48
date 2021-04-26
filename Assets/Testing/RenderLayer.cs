using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Testing
{
    public class RenderLayer : MonoBehaviour
    {
        public float Height;
        private void LateUpdate()
        {
            transform.position = new Vector3(transform.position.x, -transform.position.z + (10f * Height), transform.position.z);
        }
    }
}
