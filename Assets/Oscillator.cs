using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Oscillator : MonoBehaviour
{
    public AudioMixerSnapshot _noteOn;
    public AudioMixerSnapshot _noteOff;
    public float _attack;
    public float _release;

    public double _frequency; //frequency of the wave
    private double _increment; //how much the phase of the wave progresses on x axis per audio sample
    private double _phase; //phase or position along the wave form
    private double _audioSampleRate; //audio samples per second
    private float _gain;

    public float _octaveMultiplier = 1;

    private float _volume = 0.1f;

    private float[] _frequencies; //array of all available tones

    private int _currentWave; //sine = 0 tri = 1 square = 2

    void OnAudioFilterRead(float[] data, int channels) //is called when an audiosource tries to play sound, takes an array of data, which = the raw PCM data of the audio source's current output
    {
        _increment = _frequency * 2.0 * Mathf.PI / _audioSampleRate; // increment per sample = 2PI*Hz/samples per sec

        for (int i = 0; i < data.Length; i += channels) //data[i] = current wave value by sample
        {
            _phase += _increment;
            if (_currentWave == 0) //is sine wave currently selected?
            {
                data[i] = (float)(_gain * Mathf.Sin((float)_phase)); //wave value = Sine wave function * gain
            }

            if (_currentWave == 1) //is triangle wave currently selected?
            {
                data[i] = (float)(_gain * (double)Mathf.PingPong((float)_phase, 1.0f)); //wave value = PingPong(triangle) wave function * gain
            }

            if (_currentWave == 2) //is square wave currently selected?
            {
                if (_gain * Mathf.Sin((float)_phase) >= 0 * _gain) //if wave value is greater than 0 along the Sine wave function...
                {
                    data[i] = (float)_gain * 0.5f; //wave value = max amplitude
                }
                else
                {
                    data[i] = (-(float)_gain) * 0.5f; //otherwise, wave value = min amplitude
                }
            }

            if (_currentWave == 3) //is saw wave currently selected?
            {
                if (_gain * Mathf.Sin((float)_phase) >= 0 * _gain) //if wave amplitude is greater than 0 along the Sine wave function...
                {
                    data[i] = (float)(_gain * (double)Mathf.PingPong((float)_phase, 1.0f)); //wave amplitude = Triangle wave function
                }
                else
                {
                    data[i] = (-(float)_gain) * 0.5f; //if not, wave amplitude = min amplitude
                }
            }

            if (channels == 2)
            {
                data[i + 1] = data[i];
            }

            if(_phase > (Mathf.PI * 2))
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

        //simple ADSR, only Attack & Release using unity mixer snapshots
        if (Input.GetKey("a") || Input.GetKey("w") || Input.GetKey("s") || Input.GetKey("e") || Input.GetKey("d") || Input.GetKey("f") || Input.GetKey("t") || Input.GetKey("g") || Input.GetKey("y") || Input.GetKey("h") || Input.GetKey("u") || Input.GetKey("j"))
        {
            _noteOn.TransitionTo(_attack);
        }

        else
        {
            _noteOff.TransitionTo(_release);
        }

        _gain = _volume;

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
