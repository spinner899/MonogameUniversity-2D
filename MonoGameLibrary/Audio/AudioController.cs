using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace MonoGameLibrary.Audio;

public class AudioController : IDisposable
{
    // Tracks sound effect instances creatred so they can be paused, unpaused, and/or disposed.
    private readonly List<SoundEffectInstance> _activeSoundEffectInstances;

    // Tracks volume for song playback when muting and unmuting
    private float _previousSongVolume;

    // Tracks volume for sound effect playback when muting and unmuting.
    private float _previousSoundEffectVolume;

    /// <summary>
    /// Gets a value that indicates if audio is muted
    /// </summary>
    public bool IsMuted { get; private set; }

    /// <summary>
    /// Gets or Sets global volume of songs
    /// </summary>
    /// <remarks>
    /// If IsMuted is true, getter will always return 0.0f and setter will ignore setting the volume.
    /// </remarks>
    public float SongVolume
    {
        get
        {
            if(IsMuted)
            {
                return 0.0f;
            }

            return MediaPlayer.Volume;
        }
        set
        {
            if(IsMuted)
            {
                return;
            }

            MediaPlayer.Volume = Math.Clamp(value, 0.0f, 1.0f);
        }
    }

    /// <summary>
    /// Gets or sets global volume of sound effects
    /// </summary>
    /// <remarks>
    /// Also has code to account for IsMuted case
    /// </remarks>
    public float SoundEffectVolume
    {
        get
        {
            if(IsMuted)
            {
                return 0.0f;
            }

            return SoundEffect.MasterVolume;
        }
        set
        {
            if(IsMuted)
            {
                return;
            }

            SoundEffect.MasterVolume = Math.Clamp(value, 0.0f, 1.0f);
        }
    }

    /// <summary>
    /// Gets a value that indicates if this audio controller has been disposed.
    /// </summary>
    public bool IsDisposed { get; private set; }

    /// <summary>
    /// Creates new audio controller instance
    /// </summary>
    public AudioController()
    {
        _activeSoundEffectInstances = new List<SoundEffectInstance>();
    }

    // Finalizer called when object is collected by garbage collector
    ~AudioController() => Dispose(false);

    /// <summary>
    /// Updates this audio controller
    /// </summary>
    public void Update()
    {
        for (int i = _activeSoundEffectInstances.Count - 1; i >= 0; i--)
        {
            SoundEffectInstance instance = _activeSoundEffectInstances[i];

            if (instance.State == SoundState.Stopped)
            {
                if (!instance.IsDisposed)
                {
                    instance.Dispose();
                }
                _activeSoundEffectInstances.RemoveAt(i);
            }
        }
    }

    /// <summary>
    /// Plays given sound effect.
    /// </summary>
    /// <param name="soundEffect">The sound effect to play.</param>
    /// <returns>The sound effect instance created by this method.</returns>
    public SoundEffectInstance PlaySoundEffect(SoundEffect soundEffect)
    {
        return PlaySoundEffect(soundEffect, 1.0f, 0.0f, 0.0f, false);
    }

    /// <summary>
    /// Plays give sound effect with specified properties.
    /// </summary>
    /// <param name="soundEffect">The sound effect to play.</param>
    /// <param name="volume">The volume, rangning from 0.0 (silent) to 1.0 (full volume).</param>
    /// <param name="pitch">The pitch adjustment, ranging from -1.0 (down an octave) to 1.0 (up an octave). 0.0 is no change.</param>
    /// <param name="pan">The panning, ranging from -1.0 (left speaker) to 1.0 (right speaker). 0.0 is centered speakers.</param>
    /// <param name="isLooped">Whether the sound effect should be looped after playback.</param>
    /// <returns></returns>
    public SoundEffectInstance PlaySoundEffect(SoundEffect soundEffect, float volume, float pitch, float pan, bool isLooped)
    {
        // Create an instance from sound effect given.
        SoundEffectInstance soundEffectInstance = soundEffect.CreateInstance();

        // Apply volume, pitch, pan, and loop values specified.
        soundEffectInstance.Volume = volume;
        soundEffectInstance.Pitch = pitch;
        soundEffectInstance.Pan = pan;
        soundEffectInstance.IsLooped = isLooped;

        // Tell the instance to play
        soundEffectInstance.Play();

        // Add it to active instances for tracking
        _activeSoundEffectInstances.Add(soundEffectInstance);

        return soundEffectInstance;
    }

    /// <summary>
    /// Plays the given song.
    /// </summary>
    /// <param name="song">The song to play.</param>
    /// <param name="isRepeating">Specify if the song should be repeated. Default is true.</param>
    public void PlaySong(Song song, bool isRepeating = true)
    {
        // Check if media player is already playing, if so, stop it.
        // If we don't stop it, this could cause issues.
        if (MediaPlayer.State == MediaState.Playing)
        {
            MediaPlayer.Stop();
        }

        MediaPlayer.Play(song);
        MediaPlayer.IsRepeating = isRepeating;
    }

    /// <summary>
    /// Pauses all audio
    /// </summary>
    public void PauseAudio()
    {
        // Pause any active songs.
        MediaPlayer.Pause();

        // Pause any active sound effects.
        foreach (SoundEffectInstance soundEffectInstance in _activeSoundEffectInstances)
        {
            soundEffectInstance.Pause();
        }
    }

    /// <summary>
    /// Resumes plays of all previous paused audio.
    /// </summary>
    public void ResumeAudio()
    {
        // Resume paused music
        MediaPlayer.Resume();

        // Resume any active sound effects.
        foreach(SoundEffectInstance soundEffectInstance in _activeSoundEffectInstances)
        {
            soundEffectInstance.Resume();
        }
    }

    /// <summary>
    /// Mutes all audio.
    /// </summary>
    public void MuteAudio()
    {
        // Store volume to be restored during UnmuteAudio
        _previousSongVolume = MediaPlayer.Volume;
        _previousSoundEffectVolume = SoundEffect.MasterVolume;

        // Set all volumes to 0
        MediaPlayer.Volume = 0.0f;
        SoundEffect.MasterVolume = 0.0f;

        IsMuted = true;
    }

    /// <summary>
    /// Unmates all audio to volume prior to muting.
    /// </summary>
    public void UnmuteAudio()
    {
        // Restore previous volume values.
        MediaPlayer.Volume = _previousSongVolume;
        SoundEffect.MasterVolume = _previousSoundEffectVolume;

        IsMuted = false;
    }

    /// <summary>
    /// Toggles current audio mute state.
    /// </summary>
    public void ToggleMute()
    {
        if (IsMuted)
        {
            UnmuteAudio();
        }
        else
        {
            MuteAudio();
        }
    }

    /// <summary>
    /// Disposes of this audio controller and cleans up resources.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }


    protected void Dispose(bool disposing)
    {
        if (IsDisposed)
        {
            return;
        }

        if (disposing)
        {
            foreach (SoundEffectInstance soundEffectInstance in _activeSoundEffectInstances)
            {
                soundEffectInstance.Dispose();
            }
            _activeSoundEffectInstances.Clear();
        }

        IsDisposed = true;
    }
}