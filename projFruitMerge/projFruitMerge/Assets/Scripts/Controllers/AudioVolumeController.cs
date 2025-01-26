using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class AudioVolumeController : MonoBehaviour
{
    [SerializeField] List<AudioSource> Sounds;

    public void SetVolume(float volume)
    {
        foreach(AudioSource source in Sounds)
        {
            source.volume = volume;
        }
    }
}
