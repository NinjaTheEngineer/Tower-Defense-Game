using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OptionsUI : MonoBehaviour
{
    [SerializeField] private SoundManager soundManager;
    [SerializeField] private MusicManager musicManager;

    private TextMeshProUGUI soundVolumeText;
    private TextMeshProUGUI musicVolumeText;

    private void Awake()
    {
        soundVolumeText = transform.Find("SoundVolumeText").GetComponent<TextMeshProUGUI>();
        musicVolumeText = transform.Find("MusicVolumeText").GetComponent<TextMeshProUGUI>();

        transform.Find("SoundIncreaseBtn").GetComponent<Button>().onClick.AddListener(() =>
        {
            soundManager.IncreaseVolume();
            UpdateSoundText();
        });


        transform.Find("SoundDecreaseBtn").GetComponent<Button>().onClick.AddListener(() =>
        {
            soundManager.DecreaseVolume();
            UpdateSoundText();
        });


        transform.Find("MusicIncreaseBtn").GetComponent<Button>().onClick.AddListener(() =>
        {
            musicManager.IncreaseVolume();
            UpdateMusicText();
        });


        transform.Find("MusicDecreaseBtn").GetComponent<Button>().onClick.AddListener(() =>
        {
            musicManager.DecreaseVolume();
            UpdateMusicText();
        });


        transform.Find("MainMenuBtn").GetComponent<Button>().onClick.AddListener(() =>
        {
            Time.timeScale = 1f;
            GameSceneManager.Load(GameSceneManager.Scene.MainMenuScene);
        });

        transform.Find("EdgeScrollingToggle").GetComponent<Toggle>().onValueChanged.AddListener((bool set) =>
        {
            CameraHandler.Instance.SetEdgeScrolling(set);
        });

    }

    private void Start()
    {
        ToggleVisibile();
        UpdateSoundText();
        UpdateMusicText();

        transform.Find("EdgeScrollingToggle").GetComponent<Toggle>().SetIsOnWithoutNotify(CameraHandler.Instance.GetEdgeScrolling());
    }

    private void UpdateSoundText()
    {
        soundVolumeText.SetText(Mathf.RoundToInt(soundManager.GetVolume() * 100).ToString());
    }
    private void UpdateMusicText()
    {
        musicVolumeText.SetText(Mathf.RoundToInt(musicManager.GetVolume() * 100).ToString());
    }

    public void ToggleVisibile()
    {
        gameObject.SetActive(!gameObject.activeSelf);

        if (gameObject.activeSelf)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }
}
