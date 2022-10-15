using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    [SerializeField] private List<Particle> particleDirectory;

    
    // Start is called before the first frame update
    public void PlayParticle(string name, Vector3 position)
    {
        foreach (Particle particle in particleDirectory)
        {
            if (particle.Name == name)
                particle.PlayParticle(position);
            
        }
        
    }

}

[System.Serializable]
struct Particle
{
    public string Name;
    public ParticleSystem ParticleSystem;
    
    public GameObject GameObject;
    
    public void PlayParticle(Vector3 position)
    {
        GameObject.transform.position = position;
        ParticleSystem.Play();
    }
}

