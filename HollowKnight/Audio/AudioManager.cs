using System;
using System.Collections.Generic;
using HollowKnight.Shared;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace HollowKnight.Audio
{
    // TODO: Implement for the audio sprint.
    // Central sound system: load, play, stop, volume control.
    // Should be initialized in Game1.LoadContent() and updated in Game1.Update().
    // Use GameState to switch background music.
    // Use KnightState / enemy state changes to trigger SFX.

    
    
    //TODO create dispose method and impliment IDispossable
    public class AudioManager
    {

    //Sound effect instances created so they can be pasued, unpaused, and/or disposed        
    private readonly List<SoundEffectInstance> _activateSoundEffectInstances; 
    //Tracks volume for song playback when muting and unmuting
    private float _previousSongVolume;

    //Tracks the volume for sound effect playback when mutting and unmutting
    private float _previousSoundEffectVolume;

    /// <summary>
    /// gets a value that indicates if audio is muted
    /// </summary>
    public bool IsMuted {get; private set;}

    /// <summary>
    /// Gets or Sets global volume of songs
    /// </summary>
    public float SongVolume
        {
            get
            {
                if (IsMuted)
                {
                    return GameConstants.NoVolume;
                }
                return MediaPlayer.Volume;
            }
            set
            {
                if (IsMuted)
                {
                    return;
                }

                MediaPlayer.Volume = Math.Clamp(value, GameConstants.NoVolume, GameConstants.MaxVolume);
            }
        }

        /// <summary>
        /// Gets or Sets the global volume of sound effects.
        /// </summary>
        public float SoundEffectVolume
        {
            get
            {
                if (IsMuted)
                {
                    return GameConstants.NoVolume;
                }

                return SoundEffect.MasterVolume;
            }
            set
            {
                if (IsMuted)
                {
                    return;
                }

                SoundEffect.MasterVolume = Math.Clamp(value, GameConstants.NoVolume, GameConstants.MaxVolume);

            }
        }
        

    public bool IsDisposed {get; private set;}


    //Constructor
    public AudioManager()
        {
            _activateSoundEffectInstances = new List<SoundEffectInstance>();

        }

        //Finalizer -> called when object is collected by garbage collector
        ~AudioManager() => Dispose(false);

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
                foreach (SoundEffectInstance soundEffectINstance in _activateSoundEffectInstances)
                {
                    soundEffectINstance.Dispose();
                }
                _activateSoundEffectInstances.Clear();
            }
            IsDisposed = true;
        }

        public void Update()
        {
            for (int i = _activateSoundEffectInstances.Count - 1; i >= 0; i--)
            {
                SoundEffectInstance instance = _activateSoundEffectInstances[i];

                if (instance.State == SoundState.Stopped)
                {
                    if (!instance.IsDisposed)
                    {
                        instance.Dispose();
                    }
                    _activateSoundEffectInstances.RemoveAt(i);
                }
            }
        }

        /// <summary>
        /// Plays given sound effect.
        /// </summary>
        /// <param name="soundEffect"> Sound effect to be played</param>
        /// <returns></returns>
        public SoundEffectInstance PlaySoundEffect(SoundEffect soundEffect)
        {
            //TODO replace magic numbers => GameConstants
            return PlaySoundEffect(soundEffect, GameConstants.MaxVolume, 0.0f, 0.0f, false);
        }


        public SoundEffectInstance PlaySoundEffect(SoundEffect soundEffect, float volume, float pitch, float pan, bool isLooped)
        {
            SoundEffectInstance soundEffectInstance = soundEffect.CreateInstance();

            soundEffectInstance.Volume = volume;
            soundEffectInstance.Pitch = pitch;
            soundEffectInstance.Pan = pan;
            soundEffectInstance.IsLooped = isLooped;

            soundEffectInstance.Play();

            _activateSoundEffectInstances.Add(soundEffectInstance);

            return soundEffectInstance;
        }



        
    }
}
