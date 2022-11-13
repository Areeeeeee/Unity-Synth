using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscillator : MonoBehaviour
{
    public float _volume = .1f;


    public double _frequency; 
    private double _increment; //how much the x axis moves per audio sample
    private double _phase;
    private double _audioSampleRate;
    private float _gain;

    public float _octaveMultiplier = 1; //on awake octave set to middle C

    private float[] _frequencies; //array of all available tones

    private int _currentWave; //sine = 0 tri = 1 square = 2 saw = 3

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

            if (Input.GetKeyUp("a") || Input.GetKeyUp("w") || Input.GetKeyUp("s") || Input.GetKeyUp("e") || Input.GetKeyUp("d") || Input.GetKeyUp("f") || Input.GetKeyUp("t") || Input.GetKeyUp("g") || Input.GetKeyUp("y") || Input.GetKeyUp("h") || Input.GetKeyUp("u") || Input.GetKeyUp("j"))
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

    void OnAudioFilterRead(float[] data, int channels) //is called when an audiosource tries to play sound, takes an array of data, which = the raw PCM data of the audio source's current output
    {
        _increment = _frequency * 2.0 * Mathf.PI / _audioSampleRate; // movement along the x axis of the wave's function

        for (int i = 0; i < data.Length; i += channels) //i = output y value along the wave's function
        {
            _phase += _increment; //finds what x value the wave's function is at
            if (_currentWave == 0) //is sine wave currently selected?
            {
                data[i] = (float)(_gain * Mathf.Sin((float)_phase)); //y value = Sine wave function * gain
            }

            if (_currentWave == 1) //is triangle wave currently selected?
            {
                data[i] = (float)(_gain * (double)Mathf.PingPong((float)_phase, 1.0f)); //y value = PingPong aka Triangle wave function * gain
            }

            if (_currentWave == 2) //is square wave currently selected?
            {
                if (_gain * Mathf.Sin((float)_phase) >= 0 * _gain) //if y value is greater than 0 along the Sine wave function...
                {
                    data[i] = (float)_gain * 0.5f; //y value = max amplitude
                }
                else
                {
                    data[i] = (-(float)_gain) * 0.5f; //otherwise, y value = min amplitude
                }
            }

            if (_currentWave == 3) //is saw wave currently selected?
            {
                if (_gain * Mathf.Sin((float)_phase) >= 0 * _gain) //if y value is greater than 0 along the Sine wave function...
                {
                    data[i] = (float)(_gain * (double)Mathf.PingPong((float)_phase, 1.0f)); //y value= Triangle wave function
                }
                else
                {
                    data[i] = (-(float)_gain) * 0.5f; //if not, y value = min amplitude
                }
            }

            if (channels == 2) //sends data to both speakers if audio is set to stereo
            {
                data[i + 1] = data[i];
            }

            if(_phase > (Mathf.PI * 2)) // if phase > 2PI, phase is reset to 0
            {
                _phase = 0.0;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _audioSampleRate = AudioSettings.outputSampleRate; //initializes sample rate (set from Unity audio settings)
        _frequencies = new float[12]; //creates array of frequencies 

    }

    // Update is called once per frame
    void Update()
    {
        _gain = _volume;

        _frequencies[0] = 261.63f * _octaveMultiplier; //C4
        _frequencies[1] = 277.18f * _octaveMultiplier; //C#4
        _frequencies[2] = 293.66f * _octaveMultiplier; //D4
        _frequencies[3] = 311.13f * _octaveMultiplier; //D#4
        _frequencies[4] = 329.63f * _octaveMultiplier; //E4
        _frequencies[5] = 349.23f * _octaveMultiplier; //F4
        _frequencies[6] = 369.99f * _octaveMultiplier; //F#4
        _frequencies[7] = 392.00f * _octaveMultiplier; //G4
        _frequencies[8] = 415.30f * _octaveMultiplier; //G#4
        _frequencies[9] = 440f * _octaveMultiplier; //A4
        _frequencies[10] = 466.16f * _octaveMultiplier; //A#4
        _frequencies[11] = 493.88f * _octaveMultiplier; //B4

        if (Input.GetKeyDown("1")) //sine wave selector
        {
            _currentWave = 0;
        }

        if (Input.GetKeyDown("2")) //triangle wave selector
        {
            _currentWave = 1;
        }

        if (Input.GetKeyDown("3")) //square wave selector
        {
            _currentWave = 2;
        }

        if (Input.GetKeyDown("4")) //saw wave selector
        {
            _currentWave = 3;
        }

        if (Input.GetKeyDown("z")) //octave down
        {
            _octaveMultiplier = _octaveMultiplier / 2;
        }

        if (Input.GetKeyDown("x")) //octave up
        {
            _octaveMultiplier = _octaveMultiplier * 2;
        }

        //envelope
        if (Input.GetKey("a") || Input.GetKey("w") || Input.GetKey("s") || Input.GetKey("e") || Input.GetKey("d") || Input.GetKey("f") || Input.GetKey("t") || Input.GetKey("g") || Input.GetKey("y") || Input.GetKey("h") || Input.GetKey("u") || Input.GetKey("j"))
        {
            StartCoroutine(Attack(_audioSource, _attack, _maxVolume, _minVolume));
        }

        if (Input.GetKeyUp("a") || Input.GetKeyUp("w") || Input.GetKeyUp("s") || Input.GetKeyUp("e") || Input.GetKeyUp("d") || Input.GetKeyUp("f") || Input.GetKeyUp("t") || Input.GetKeyUp("g") || Input.GetKeyUp("y") || Input.GetKeyUp("h") || Input.GetKeyUp("u") || Input.GetKeyUp("j"))
        {
            StartCoroutine(Release(_audioSource, _release, _maxVolume, _minVolume));
        }

        //attack length up
        if (Input.GetKeyDown("p"))
        {
            _attack = _attack + 0.1f;
        }
        //attack length down
        if (Input.GetKeyDown("o"))
        {
            _attack = _attack - 0.1f;
        }
        //release length up
        if (Input.GetKeyDown(";"))
        {
            _release = _release + 0.1f;
        }
        //release length down
        if (Input.GetKeyDown("l"))
        {
            _release = _release - 0.1f;
        }

        //keyboard keys
        if (Input.GetKeyDown("a")) //C
        {
            _frequency = _frequencies[0];
        }

        if (Input.GetKeyDown("w")) //C#
        {
            _frequency = _frequencies[1];
        }

        if (Input.GetKeyDown("s")) //D
        {
            _frequency = _frequencies[2];
        }

        if (Input.GetKeyDown("e")) //D#
        {
            _frequency = _frequencies[3];
        }

        if (Input.GetKeyDown("d")) //E
        {
            _frequency = _frequencies[4];
        }

        if (Input.GetKeyDown("f")) //F
        {
            _frequency = _frequencies[5];
        }

        if (Input.GetKeyDown("t")) //F#
        {
            _frequency = _frequencies[6];
        }

        if (Input.GetKeyDown("g")) //G
        {
            _frequency = _frequencies[7];
        }

        if (Input.GetKeyDown("y")) //G#
        {
            _frequency = _frequencies[8];
        }

        if (Input.GetKeyDown("h")) //A
        {
            _frequency = _frequencies[9];
        }

        if (Input.GetKeyDown("u")) //A#
        {
            _frequency = _frequencies[10];
        }

        if (Input.GetKeyDown("j")) //B
        {
            _frequency = _frequencies[11];
        }
    }
}
