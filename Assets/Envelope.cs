using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Envelope : MonoBehaviour
{
    public AudioSource _audioSource;
    public float _attack = .5f;
    public float _release = .5f;
    public float _maxVolume = 1f;
    public float _minVolume = 0f;

    public static IEnumerator Attack(AudioSource _audioSource, float _attack, float _maxVolume, float _minVolume)
    {
        float _currentTime = 0f;
        float _currentVolume = _audioSource.volume;

        while (Input.GetKey("a") || Input.GetKey("w") || Input.GetKey("s") || Input.GetKey("e") || Input.GetKey("d") || Input.GetKey("f") || Input.GetKey("t") || Input.GetKey("g") || Input.GetKey("y") || Input.GetKey("h") || Input.GetKey("u") || Input.GetKey("j"))
        {
            _currentTime += Time.deltaTime;
            _audioSource.volume = Mathf.Lerp(_currentVolume, _maxVolume, _currentTime / _attack);

            if(Input.GetKeyUp("a") || Input.GetKeyUp("w") || Input.GetKeyUp("s") || Input.GetKeyUp("e") || Input.GetKeyUp("d") || Input.GetKeyUp("f") || Input.GetKeyUp("t") || Input.GetKeyUp("g") || Input.GetKeyUp("y") || Input.GetKeyUp("h") || Input.GetKeyUp("u") || Input.GetKeyUp("j"))
            {
                yield break;
            }

            yield return null;
        }

        yield break;
    }

    public static IEnumerator Release(AudioSource _audioSource, float _release, float _maxVolume, float _minVolume)
    {
        float _currentTime = 0f;
        float _currentVolume = _audioSource.volume;

        while (_currentTime < _release)
        {
            _currentTime += Time.deltaTime;
            _audioSource.volume = Mathf.Lerp(_currentVolume, _minVolume, _currentTime / _release);

            if (Input.GetKeyDown("a") || Input.GetKeyDown("w") || Input.GetKeyDown("s") || Input.GetKeyDown("e") || Input.GetKeyDown("d") || Input.GetKeyDown("f") || Input.GetKeyDown("t") || Input.GetKeyDown("g") || Input.GetKeyDown("y") || Input.GetKeyDown("h") || Input.GetKeyDown("u") || Input.GetKeyDown("j"))
            {
                yield break;
            }

            yield return null;
        }

        yield break;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    void Awake()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("a") || Input.GetKey("w") || Input.GetKey("s") || Input.GetKey("e") || Input.GetKey("d") || Input.GetKey("f") || Input.GetKey("t") || Input.GetKey("g") || Input.GetKey("y") || Input.GetKey("h") || Input.GetKey("u") || Input.GetKey("j"))
        {
            StartCoroutine(Attack(_audioSource, _attack, _maxVolume, _minVolume));
        }

        if (Input.GetKeyUp("a") || Input.GetKeyUp("w") || Input.GetKeyUp("s") || Input.GetKeyUp("e") || Input.GetKeyUp("d") || Input.GetKeyUp("f") || Input.GetKeyUp("t") || Input.GetKeyUp("g") || Input.GetKeyUp("y") || Input.GetKeyUp("h") || Input.GetKeyUp("u") || Input.GetKeyUp("j"))
        {
            StartCoroutine(Release(_audioSource, _release, _maxVolume, _minVolume));
        }
    }
}
