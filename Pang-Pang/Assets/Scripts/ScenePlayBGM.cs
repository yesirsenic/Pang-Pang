using UnityEngine;

public class ScenePlayBGM : MonoBehaviour
{
    public AudioClip clip;

    private void Start()
    {
        AudioManager.Instance.PlayBGM(clip, true);
    }

}
