using System;
using System.Collections.Generic;
using System.Net;
using HollowKnight.Factories;
using HollowKnight.Shared;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace HollowKnight.Audio
{
    // TODO: Implement for the audio sprint.
    // Central sound system: load, play, stop, volume control.
    // Should be initialized in Game1.LoadContent() and updated in Game1.Update().
    // Use GameState to switch background music.
    // Use KnightState / enemy state changes to trigger SFX.

    
    
    //TODO create dispose method and impliment IDispossable
    public class AudioManager : IDisposable
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

    public AudioManager()
        {
            _activateSoundEffectInstances = new List<SoundEffectInstance>();
        }

    private static AudioManager instance = new AudioManager();
    public static AudioManager Instance
        {
            get { return instance; }
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
            return PlaySoundEffect(soundEffect, GameConstants.MaxVolume, GameConstants.Pitch, GameConstants.Pan, false);
        }

        /// <summary>
        /// Plays the sound effect.
        /// </summary>
        /// <param name="soundEffect">The sound to be played</param>
        /// <param name="volume">The volume</param>
        /// <param name="pitch">Pitch ranging from -1.0 to 1.0</param>
        /// <param name="pan">Pan ranging from -1.0 (left speaker) to 1.0 (right speaker)</param>
        /// <param name="isLooped">Is the sound playing on repeat or just once</param>
        /// <returns></returns>
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

        /// <summary>
        /// Plays the gvien song
        /// </summary>
        /// <param name="song">Song to be played</param>
        /// <param name="isrepeating">Set to true by default</param>
        public void PlaySong(Song song, bool isRepeating = true)
        {
            if (MediaPlayer.State == MediaState.Playing)
            {
                MediaPlayer.Stop();
            }

            MediaPlayer.Volume = GameConstants.SongVolume;
            MediaPlayer.Play(song);
            MediaPlayer.IsRepeating = isRepeating;

        }

        /// <summary>
        /// Pauses all audio
        /// </summary>
        public void PauseAudio()
        {
            MediaPlayer.Pause();

            foreach(SoundEffectInstance soundEffectInstance in _activateSoundEffectInstances)
            {
                soundEffectInstance.Pause();
            }
        }


        public void ResumeAudio()
        {
            MediaPlayer.Resume();

            foreach(SoundEffectInstance soundEffectInstance in _activateSoundEffectInstances)
            {
                soundEffectInstance.Resume();
            }
        }


        /// <summary>
        /// Mutes all audio
        /// </summary>
        public void MuteAudio()
        {
            _previousSongVolume = MediaPlayer.Volume;
            _previousSoundEffectVolume = SoundEffect.MasterVolume;

            MediaPlayer.Volume = GameConstants.NoVolume;
            SoundEffect.MasterVolume = GameConstants.NoVolume;

            IsMuted = true;
        }

        public void UnmuteAudio()
        {
            MediaPlayer.Volume = _previousSongVolume;
            SoundEffect.MasterVolume = _previousSoundEffectVolume;

            IsMuted = false;
        }

        /// <summary>
        /// Called when pressing a key to mute/unmute all audio
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


        
    }
}
