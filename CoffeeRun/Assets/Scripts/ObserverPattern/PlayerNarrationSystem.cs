using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNarrationSystem : MonoBehaviour, IObserver
{
    [SerializeField] Subject _playerSubject;
    [SerializeField] int _sprintCount = 0;
    int _sprintAudioThreshold = 2;
    Coroutine _currentSprintResetRoutine = null;

    AudioSource _audioPlayer; 
    public AudioClip _sprintingAudioClip;
    public AudioClip _collectAudioClip;

    void Awake()
    {
        _audioPlayer = GetComponent<AudioSource>(); 
    }

    public void OnNotify(PlayerActions action)
    {
        switch (action)
        {
            case (PlayerActions.Sprint):      
                if (_currentSprintResetRoutine != null)
                {
                    StopCoroutine(_currentSprintResetRoutine);
                }

                //increment jump count
                _sprintCount += 1;

                if (_sprintCount > 3)
                {
                    _sprintCount = 0;
                }

                if (_sprintCount == _sprintAudioThreshold)
                {
                    _audioPlayer.clip = _sprintingAudioClip;
                    _audioPlayer.Play();
                }

                 _currentSprintResetRoutine = StartCoroutine(IJumpResetRoutine());
                return;

            case (PlayerActions.Collect):
                _audioPlayer.clip = _collectAudioClip;
                _audioPlayer.Play();
                return;

            default:
                return;         
        }
    }

    private void OnEnable()
    {
        //add itself to the subject's list of observers
        _playerSubject.AddObserver(this); 
    }

    private void OnDisable()
    {
        //remove itself to the subject's list of observers
        _playerSubject.RemoveObserver(this); 
    }

    IEnumerator IJumpResetRoutine()
    {
        yield return new WaitForSeconds(5f);
        _sprintCount = 0; 
    }
}
