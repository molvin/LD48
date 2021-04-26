using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Testing
{
    public class RenderLayer : MonoBehaviour
    {
        public int layer = 0;
        private void LateUpdate()
        {
            transform.position = new Vector3(transform.position.x, -transform.position.z + 10 * layer, transform.position.z);
        }
    }
}
