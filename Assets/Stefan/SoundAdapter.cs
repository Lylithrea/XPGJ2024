using UnityEngine;

public class SoundAdapter : MonoBehaviour
{
    [SerializeField] SoundName _soundName;
    [SerializeField] string _name;
    [SerializeField] float _volume;
    [SerializeField] bool _loop;

    public void PlaySound()
    {
        SoundManager.Instance.PlaySound(_soundName,_volume,_name, _loop);
    }

    public void StopSound()
    {
        if(string.IsNullOrEmpty(_name))
        {
            Debug.LogError($"Name of {_soundName} sound to stop is not specified");
            return;
        }
        SoundManager.Instance.StopSound(_name);
    }
}
