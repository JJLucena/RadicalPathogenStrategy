using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ---------------------------------------------------------------------------------
// SCRIPT PARA LA GESTI�N DE AUDIO (vinculado a un GameObject vac�o "AudioManager")
// ---------------------------------------------------------------------------------
public class AudioManager : MonoBehaviour
{

    private float sfxvolume = 1;

    // Instancia �nica del AudioManager (porque es una clase Singleton) STATIC
    public static AudioManager instance;

    // Se crean dos AudioSources diferentes para que se puedan
    // reproducir los efectos y la m�sica de fondo al mismo tiempo
    //public AudioSource sfxSource; // Componente AudioSource para efectos de sonido
    public AudioSource musicSource; // Componente AudioSource para la m�sica de fondo

    // En vez de usar un vector de AudioClips (que podr�a ser) vamos a utilizar un Diccionario
    // en el que cargaremos directamente los recursos desde la jerarqu�a del proyecto
    // Cada entrada del diccionario tiene una string como clave y un AudioClip como valor
    public Dictionary<string, AudioClip> sfxClips = new Dictionary<string, AudioClip>();
    public Dictionary<string, AudioClip> musicClips = new Dictionary<string, AudioClip>();

    // M�todo Awake que se llama al inicio antes de que se active el objeto. �til para inicializar
    // variables u objetos que ser�n llamados por otros scripts (game managers, clases singleton, etc).
    private void Awake()
    {

        // ----------------------------------------------------------------
        // AQU� ES DONDE SE DEFINE EL COMPORTAMIENTO DE LA CLASE SINGLETON
        // Garantizamos que solo exista una instancia del AudioManager
        // Si no hay instancias previas se asigna la actual
        // Si hay instancias se destruye la nueva
        if (instance == null) instance = this;
        else { Destroy(gameObject); return; }
        // ----------------------------------------------------------------

        // No destruimos el AudioManager aunque se cambie de escena
        DontDestroyOnLoad(gameObject);

        // Cargamos los AudioClips en los diccionarios
        LoadSFXClips();
        LoadMusicClips();

    }

    private void Start()
    {
        /*
        float savedVolume = PlayerPrefs.GetFloat("MusicVolume", 0.72f);
        AudioManager.instance.ChangeMusicVolume(savedVolume);
        float savedSFX = PlayerPrefs.GetFloat("SFXVolume", 0.72f);
        AudioManager.instance.ChangeSFXVolume(savedSFX);
        */
        PlayMusic("MainTheme");  // Reproduce la m�sica principal
    }

    // M�todo privado para cargar los efectos de sonido directamente desde las carpetas
    private void LoadSFXClips()
    {
        // Los recursos (ASSETS) que se cargan en TIEMPO DE EJECUCI�N DEBEN ESTAR DENTRO de una carpeta denominada /Assets/Resources/SFX
        sfxClips["ClickUI"] = Resources.Load<AudioClip>("SFX/clickui");
        sfxClips["Laser"] = Resources.Load<AudioClip>("SFX/laser");
        sfxClips["Pop"] = Resources.Load<AudioClip>("SFX/pop");
        sfxClips["Slash"] = Resources.Load<AudioClip>("SFX/slash");
    }

    // M�todo privado para cargar la m�sica de fondo directamente desde las carpetas
    private void LoadMusicClips()
    {
        // Los recursos (ASSETS) que se cargan en TIEMPO DE EJECUCI�N DEBEN ESTAR DENTRO de una carpeta denominada /Assets/Resources/Music
        musicClips["MainTheme"] = Resources.Load<AudioClip>("Music/Background");
    }

    // M�todo de la clase singleton para reproducir efectos de sonido
    public void PlaySFX(string clipName)
    {
        if (sfxClips.ContainsKey(clipName))
        {
            // Metodo que crea un un nuevo audioSource, reproduce el sonido, espera a que termine y lo borra
            PlaySoundAndDestroy(sfxClips, clipName, false);
        }
        else Debug.LogWarning("El AudioClip " + clipName + " no se encontr� en el diccionario de sfxClips.");
    }

    // M�todo de la clase singleton para reproducir m�sica de fondo
    public void PlayMusic(string clipName)
    {
        if (musicClips.ContainsKey(clipName))
        {
            musicSource.clip = musicClips[clipName];
            musicSource.Play();

            #region new
            //PlaySoundAndDestroy(musicClips, clipName, true);
            #endregion
        }
        else Debug.LogWarning("El AudioClip " + clipName + " no se encontr� en el diccionario de musicClips.");
    }

    #region Create new AudioSource, Play Sound, Wait and Destroy
    private void PlaySoundAndDestroy(Dictionary<string, AudioClip> dictionaryClips, string clipName, bool loop)
    {
        //Debug.Log(clipName + ", " + loop);
        // Crear un nuevo AudioSource
        AudioSource newAudioSource = gameObject.AddComponent<AudioSource>();

        // Asignar el clip al nuevo AudioSource
        newAudioSource.clip = dictionaryClips[clipName];
        // Asignar volumen de settings
        newAudioSource.volume = sfxvolume;
        // Reproducir el sonido
        newAudioSource.Play();

        if (loop)
        {
            newAudioSource.loop = true;
        }
        else
        {
            //Corrutina que espere el tiempo del clip y lo borre
            float timeSound = newAudioSource.clip.length;

            // Autodestruir el AudioSource despu�s de que termine de reproducir el sonido
            Destroy(newAudioSource, timeSound);
        }
    }
    #endregion

    public float GetSFXDuration(string clipName)
    {
        return sfxClips[clipName].length;
    }
    public void ChangeMusicVolume(float value)
    {
        musicSource.volume = value;
    }

    public void ChangeSFXVolume(float value)
    {
        sfxvolume = value;
    }
}