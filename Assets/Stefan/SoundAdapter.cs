using UnityEngine;

public class SoundAdapter : MonoBehaviour
{
    [SerializeField] SoundName _soundName;
    [SerializeField] bool _useData;
    [SerializeField] SoundData _soundData;
    public void PlaySound()
    {
        if (_useData)
            SoundManager.Instance.PlaySound(_soundName,_soundData);
        else
            SoundManager.Instance.PlaySound(_soundName);
    }
}
