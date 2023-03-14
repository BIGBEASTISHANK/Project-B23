using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class MainMenus : MonoBehaviour
{
    // Variables
    /////////////
    private const string MixerMaster = "Master";
    private const string MixerMenuSFX = "MenuSFX";
    private const string MixerMenuMusic = "MenuMusic";
    private const string MixerInGameSFX = "InGameSFX";
    private const string MixerInGameMusic = "InGameMusic";

    private const string SoundMaster = "Sound_Master";
    private const string SoundMenuSFX = "Sound_MenuSFX";
    private const string SoundMenuMusic = "Sound_MenuMusic";
    private const string SoundInGameSFX = "Sound_InGameSFX";
    private const string SoundInGameMusic = "Sound_InGameMusic";

    [Header("Audios")]
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private AudioSource clickSound;

    [Header("Sliders")]
    [SerializeField] private Slider master;
    [SerializeField] private Slider menuSfx;
    [SerializeField] private Slider inGameSfx;
    [SerializeField] private Slider menuMusic;
    [SerializeField] private Slider inGameMusic;

    [Header("Menus")]
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject donateMenu;
    [SerializeField] private GameObject soundsMenu;
    [SerializeField] private GameObject controllsMenu;
    [SerializeField] private GameObject settingsMenu;

    // References
    //////////////
    private void FixedUpdate()
    {
        // Setting values
        master.value = PlayerPrefs.GetFloat(SoundMaster);
        menuSfx.value = PlayerPrefs.GetFloat(SoundMenuSFX);
        inGameSfx.value = PlayerPrefs.GetFloat(SoundInGameSFX);
        menuMusic.value = PlayerPrefs.GetFloat(SoundMenuMusic);
        inGameMusic.value = PlayerPrefs.GetFloat(SoundInGameMusic);
    }

    // Start Game
    public void StartGame()
    {
        // Play Sound
        clickSound.Play();
        // Loading next scene
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
    }

    // Donates
    public void Donates()
    {
        // Turning off other menus
        mainMenu.SetActive(false);
        controllsMenu.SetActive(false);
        soundsMenu.SetActive(false);
        settingsMenu.SetActive(false);
        // Enabling menu
        donateMenu.SetActive(true);
        // Play Sound
        clickSound.Play();
    }

    // Sounds
    public void Sounds()
    {
        // Turning off other menus
        mainMenu.SetActive(false);
        controllsMenu.SetActive(false);
        donateMenu.SetActive(false);
        settingsMenu.SetActive(false);
        // Enabling menu
        soundsMenu.SetActive(true);
        // Play Sound
        clickSound.Play();
    }

    // Controlls
    public void Controlls()
    {
        // Turning off other menus
        soundsMenu.SetActive(false);
        donateMenu.SetActive(false);
        settingsMenu.SetActive(false);
        mainMenu.SetActive(false);
        // Enabling menu
        controllsMenu.SetActive(true);
        // Play Sound
        clickSound.Play();
    }

    // Settings
    public void Settings()
    {
        // Turning off other menus
        mainMenu.SetActive(false);
        controllsMenu.SetActive(false);
        soundsMenu.SetActive(false);
        donateMenu.SetActive(false);
        // Enabling menu
        settingsMenu.SetActive(true);
        // Play Sound
        clickSound.Play();
    }

    // Master Sound
    public void Sound_Master(float value)
    {
        // Changing Volume
        mixer.SetFloat(MixerMaster, value);
        PlayerPrefs.SetFloat(SoundMaster, value); // Saving it locally
    }

    // InGameMusic Sound
    public void Sound_InGameMusic(float value)
    {
        // Changing Volume
        mixer.SetFloat(MixerMaster, value);
        PlayerPrefs.SetFloat(SoundInGameMusic, value); // Saving it locally
    }

    // InGameSFX Sound
    public void Sound_InGameSFX(float value)
    {
        // Changing Volume
        mixer.SetFloat(MixerInGameSFX, value);
        PlayerPrefs.SetFloat(SoundInGameSFX, value); // Saving it locally
    }

    // MenuMusic Sound
    public void Sound_MenuMusic(float value)
    {
        // Changing Volume
        mixer.SetFloat(MixerMenuMusic, value);
        PlayerPrefs.SetFloat(SoundMenuMusic, value); // Saving it locally
    }

    // MenuSFX Sound
    public void Sound_MenuSFX(float value)
    {
        // Changing Volume
        mixer.SetFloat(MixerMenuSFX, value);
        PlayerPrefs.SetFloat(SoundMenuSFX, value); // Saving it locally
    }

    // Sounds Clear Save
    public void SoundReset()
    {
        // Setting volume to 0
        mixer.SetFloat(MixerMaster, 0);
        mixer.SetFloat(MixerMaster, 0);
        mixer.SetFloat(MixerInGameSFX, 0);
        mixer.SetFloat(MixerMenuMusic, 0);
        mixer.SetFloat(MixerMenuSFX, 0);

        // Removing PlayerPrefs
        PlayerPrefs.DeleteKey(SoundMaster);
        PlayerPrefs.DeleteKey(SoundInGameMusic);
        PlayerPrefs.DeleteKey(SoundInGameSFX);
        PlayerPrefs.DeleteKey(SoundMenuMusic);
        PlayerPrefs.DeleteKey(SoundMenuSFX);
        // Play Sound
        clickSound.Play();
    }

    // Back to Settings
    public void BackToSettings()
    {
        // Turning off other menus
        mainMenu.SetActive(false);
        soundsMenu.SetActive(false);
        donateMenu.SetActive(false);
        controllsMenu.SetActive(false);
        // Enabling menu
        settingsMenu.SetActive(true);
        // Play Sound
        clickSound.Play();
    }

    // Back to main
    public void BackToMain()
    {
        // Turning off other menus
        soundsMenu.SetActive(false);
        donateMenu.SetActive(false);
        settingsMenu.SetActive(false);
        controllsMenu.SetActive(false);
        // Enabling menu
        mainMenu.SetActive(true);
        // Play Sound
        clickSound.Play();
    }

    // Quit
    public void QuitGame()
    {
        // Play Sound
        clickSound.Play();
        // Quiting game
        Application.Quit();
    }
}
