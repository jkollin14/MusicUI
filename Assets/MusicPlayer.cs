using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class MusicPlayer : MonoBehaviour
{
    public enum SeekDirection { Forward, Backward }

    public AudioSource source;
    public List<AudioClip> clips = new List<AudioClip>();

    [SerializeField]
    [HideInInspector]
    private int currentIndex = 0;

    private FileInfo[] soundFiles;
    private List<string> validExtensions = new List<string> { ".ogg", ".wav", ".mp3" };
    private string absoluteAssetsPath = "./";
    // private string userMusicPath = "./";

    private void Start()
    {
        //being able to test in unity
        if (Application.isEditor)
        {
            Debug.Log("Test");
            absoluteAssetsPath = "Assets\\music";
            // userMusicPath = System.Environment.GetEnvironmentVariable("USERPROFILE").ToString() + "\\Music";
            // Debug.Log(userMusicPath);
        }
        if (source == null) source = gameObject.AddComponent<AudioSource>();
        ReloadSounds();
    }

    public void PlayAmbient()
    {
        for (var i = 0; i < clips.Count; i++)
        {
            if (clips[i].name == "ambient.wav")
            {
                source.clip = clips[i];
                source.Play();
            }
        }
    }

    public void PlayOffice()
    {
        for (var i = 0; i < clips.Count; i++)
        {
            if (clips[i].name == "office.wav")
            {
                source.clip = clips[i];
                source.Play();
            }
        }
    }

    public void PlayRainforrest()
    {
        for (var i = 0; i < clips.Count; i++)
        {
            if (clips[i].name == "rainforrest.wav")
            {
                source.clip = clips[i];
                source.Play();
            }
        }
    }

    public void Stop()
    {
        source.Stop();
    }

    private void ReloadSounds()
    {
        clips.Clear();
        // get all valid files from  absolute file path
        Debug.Log(absoluteAssetsPath);
        var absoluteInfo = new DirectoryInfo(absoluteAssetsPath);
        soundFiles = absoluteInfo.GetFiles()
            .Where(f => IsValidFileType(f.Name))
            .ToArray();

        // and load them
        foreach (var s in soundFiles)
        {
            Debug.Log(s);
            StartCoroutine(LoadFile(s.FullName));
        }

        /*
        // get all valid files from  the user's music path
        var info = new DirectoryInfo(userMusicPath);
        soundFiles = info.GetFiles()
            .Where(f => IsValidFileType(f.Name))
            .ToArray();

        // and load them
        foreach (var t in soundFiles)
            // Debug.Log(s);
            StartCoroutine(LoadFile(t.FullName));
            */
    }


    private void Seek(SeekDirection d)
    {
        if (d == SeekDirection.Forward)
            currentIndex = (currentIndex + 1) % clips.Count;
        else
        {
            currentIndex--;
            if (currentIndex < 0) currentIndex = clips.Count - 1;
        }
    }

    private bool IsValidFileType(string fileName)
    {
        return validExtensions.Contains(Path.GetExtension(fileName));
        // Alternatively, you could go fileName.SubString(fileName.LastIndexOf('.') + 1); that way you don't need the '.' when you add your extensions
    }

    IEnumerator LoadFile(string path)
    {
        WWW www = new WWW("file://" + path);
        print("loading " + path);

        AudioClip clip = www.GetAudioClip(false);
        while (!clip.isReadyToPlay)
            yield return www;

        print("done loading");
        clip.name = Path.GetFileName(path);
        Debug.Log(clip);
        clips.Add(clip);
    }
}