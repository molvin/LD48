using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Testing
{
    public class RenderLayer : MonoBehaviour
    {
        public int layer = 0;
        private void Update()
        {
            transform.position = new Vector3(transform.position.x, transform.position.z * layer, transform.position.z);
        }
    }
}
