using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Testing
{
    public class RenderLayer : MonoBehaviour
    {
        public Transform Player;
        public float Speed;

        private void Update()
        {
            Vector3 input = Vector3.forward * Input.GetAxisRaw("Vertical") + Vector3.right * Input.GetAxisRaw("Horizontal");
            input.Normalize();

            Vector3 Position = Player.transform.position + input * Speed * Time.deltaTime;
            Position.y = -Position.z;
            Player.transform.position = Position;
        }
    }
}
