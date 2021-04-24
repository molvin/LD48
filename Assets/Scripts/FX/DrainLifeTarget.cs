using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class DrainLifeTarget : MonoBehaviour
{
    public Transform tempshit;
    public Transform target;
    public GameObject parent;
    public Transform parentTransform;
    public float speed;

    private ParticleSystem system;

    private static ParticleSystem.Particle[] particles;

    // Start is called before the first frame update
    void Start()
    {
        //parentTransform = this.GetComponentInParent<Transform>().transform;
    }

    // Update is called once per frame
    void Update()
    {

        if (system == null) system = GetComponent<ParticleSystem>();


        if (Input.GetKeyDown(KeyCode.U)) {
            target.position = tempshit.transform.position;
            Debug.Log("sick");
            this.transform.position = target.position;
        }
        
        if(target == null) return;
        
        particles = new ParticleSystem.Particle[system.particleCount];
        var count = system.GetParticles(particles);
        for (int i = 0; i<count; i++) {
            var particle = particles[i];

            float distance = Vector3.Distance(target.position, parent.transform.position);
            Debug.Log(" Parent e : " + parent.transform.position);
            if (distance > 0.1f) {
                //particle.position = Vector3.Lerp(particle.position, parentTransform.position, Time.deltaTime * speed);
                particle.position = Vector3.MoveTowards(particle.position, parent.transform.position, Time.deltaTime * speed);
                Vector3 temp = parent.transform.position - particle.position;
                particles[i] = particle;
            }
        }
        system.SetParticles(particles, count);
    }
}
