﻿using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using System;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class CubeDataController : MonoBehaviour
{

    [SerializeField] MomentListContainer momentsList;

    [Space]
    //1st name
    [Space]
    [SerializeField] VideoPlayer momentVideoPlayer;
    [SerializeField] TextMeshPro playerName;
    //2nd score a,b name a,b, home ,away 
    [Space]
    [SerializeField] TextMeshPro team_a_name_text;
    [SerializeField] TextMeshPro team_a_scoreRun_text;
    [SerializeField] TextMeshPro team_a_scoreWic_text;
    [SerializeField] TextMeshPro team_a_score;
    [SerializeField] TextMeshPro overs_a_text;
    // [SerializeField] TextMeshPro team_a_homeAway_text;
    [Space]
    [SerializeField] TextMeshPro team_b_name_text;
    [SerializeField] TextMeshPro team_b_scoreRun_text;
    [SerializeField] TextMeshPro team_b_scoreWic_text;
    [SerializeField] TextMeshPro team_b_score;
    [SerializeField] TextMeshPro overs_b_text;
    // [SerializeField] TextMeshPro team_b_homeAway_text;
    //3rd player number
    [Space]
    [SerializeField] TextMeshPro playerNumber_text;
    [SerializeField] TextMeshPro playerNumber_reflection_text;
    [SerializeField] TextMeshPro momentLengthText;
    [SerializeField] TextMeshPro rarityText;
    [Space]
    [SerializeField] TextMeshPro momentDateText;
    [SerializeField] TextMeshPro actionTypeText;
    [SerializeField] TextMeshPro teamNameText;
    [Space]
    [SerializeField] GameObject momentCubeGO;
    [Space]
    [Space]
    [SerializeField] MeshRenderer packImage_renderer;
    [SerializeField] MeshRenderer video_SS_BG_renderer1;
    [SerializeField] MeshRenderer video_SS_BG_renderer2;
    [SerializeField] SpriteRenderer numberBG_renderer;
    [SerializeField] MeshRenderer ballBG_renderer;
    [SerializeField] MeshRenderer videoThumbnail;
    [SerializeField] SpriteRenderer teamLogoImage_renderer;
    [SerializeField] SpriteRenderer surfaceFour_renderer;
    [SerializeField] Image surfaceFour_Image;
    [SerializeField] SpriteRenderer surfaceFourShadow_renderer;
    [SerializeField] Image surfaceFour_renderer_Gradiant;
    [SerializeField] TextMeshPro hostName;
    [SerializeField] TextMeshPro hostNameShadow;
    [SerializeField] TextMeshPro tournamentStage;
    [SerializeField] TextMeshPro tournamentStageShadow;

    [SerializeField] SpriteRenderer assosiationLogoImage_renderer;
    [SerializeField] SpriteRenderer companyLogoImage_renderer;
    [SerializeField] TextMeshPro copyrightText;
    [SerializeField] bool interactable;
    [SerializeField] Text txtTesting;
    static CubeDataController instance;

 public   int alreadyDownloadedElementCount;

    public static Action onEverythingLoaded;
    public static int VisualisedIndex => instance.indexToBeVisualised;
    public static string FileName => instance.filename;

    public static Action audioCanBeEnabled;
    public static Action audioCanBeDisabled;
    public static Action setDefaultAnimationBeforeStart;


    //public static Action setDefaultAnimationBeforeStart;

    int indexToBeVisualised = 0;
    string filename;
    List<Surface> surfaces = new List<Surface>();

    bool alreadyPrepared = false;
    internal bool isSurfaceFourImageDownloaded;


    private void Start()
    {
        instance = this;

        //Application.targetFrameRate = 60;

        if (interactable)
        {
            SetMomentData(momentsList.moments[momentsList.currentMoment]);
        }
    }

#if UNITY_EDITOR
    private void OnEnable()
    {
        VideoRecorder.recordingStopped += OnRecordingStopped;
        IntroAnimController.videoPlay += OnAudioEnabled;
        VideoInfo.scaleDownVideo += OnAudioDisabled;
    }

    private void OnDisable()
    {
        VideoRecorder.recordingStopped -= OnRecordingStopped;
        IntroAnimController.videoPlay -= OnAudioEnabled;
        VideoInfo.scaleDownVideo -= OnAudioDisabled;
    }
#endif

    void OnAudioEnabled()
    {
        // momentVideoPlayer.SetDirectAudioMute(0, false);
        if (alreadyPrepared)
        {
            //Application.targetFrameRate = 145;
            Debug.Log("framerate setted " + indexToBeVisualised.ToString());
        }

    }

    void OnAudioDisabled()
    {
        // momentVideoPlayer.SetDirectAudioMute(0, true);
        //Application.targetFrameRate = 60;
        Debug.Log("framerate reseted " + indexToBeVisualised.ToString());
    }

    void OnRecordingStopped()
    {
        if (indexToBeVisualised + 1 < surfaces.Count)
        {
            indexToBeVisualised += 1;
            alreadyPrepared = false;
            VisualiseSurface(surfaces[indexToBeVisualised]);
        }
        else if (VideoRecorder.recordingState == RecordingState.VideoRecording)
        {
            EventManager.Dispatch(EventManager.EventType.RecordingStateChanged, null);

            indexToBeVisualised = 0;
            alreadyPrepared = false;
            VisualiseSurface(surfaces[indexToBeVisualised]);
        }
        setDefaultAnimationBeforeStart?.Invoke();
    }

    void ChangeMomentData(MomentDataContainer momentData)
    {
        momentVideoPlayer.clip = momentData.momentClip;
        playerName.text = momentData.playerName;
        team_a_name_text.text = momentData.team_a_name;
        team_a_scoreRun_text.text = momentData.team_a_scoreRun;
        team_a_scoreWic_text.text = momentData.team_a_scoreWic;
        // team_a_homeAway_text.text = momentData.team_a_homeAway;
        team_b_name_text.text = momentData.team_b_name;
        team_b_scoreRun_text.text = momentData.team_b_scoreRun;
        team_b_scoreWic_text.text = momentData.team_b_scoreWic;
        // team_b_homeAway_text.text = momentData.team_b_homeAway;
        playerNumber_reflection_text.text = playerNumber_text.text = momentData.playerNumber;


        if (momentData.packImage != null)
        {
            packImage_renderer.material.mainTexture = momentData.packImage.texture;
        }
        if (momentData.video_SS_BG != null)
        {
            video_SS_BG_renderer1.material.mainTexture = momentData.video_SS_BG.texture;
            video_SS_BG_renderer2.material.mainTexture = momentData.video_SS_BG.texture;
        }
        if (momentData.numberBG != null)
        {
            numberBG_renderer.sprite = momentData.numberBG;
        }
        if (momentData.teamLogoImage != null)
        {
            teamLogoImage_renderer.sprite = momentData.teamLogoImage;
        }
        if (momentData.assosiationLogoImage != null)
        {
            //assosiationLogoImage_renderer.sprite = momentData.assosiationLogoImage;
        }
        if (momentData.companyLogoImage != null)
        {
            companyLogoImage_renderer.sprite = momentData.companyLogoImage;
        }

        momentCubeGO.SetActive(true);
//#if UNITY_EDITOR
//        if (momentsList != null && !momentsList.interactable)
//        {
//            recorder.StartVideoRecording(momentData.name + "_" + momentData.team_a_name + "_" + momentData.team_b_name);
//        }
//#endif
    }

    public void SetMomentData(MomentDataContainer momentData)
    {
        ChangeMomentData(momentData);
    }

    public void ResetScene()
    {
//#if UNITY_EDITOR
//        if (!momentsList.interactable)
//        {
//            recorder.StopRecording();
//        }
//#endif
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


    void VisualiseSurface(Surface surface)
    {
        alreadyDownloadedElementCount = 0;
        isSurfaceFourImageDownloaded = false;

        Element1 element1 = surface.element1;
        Element2 element2 = surface.element2;
        Element3 element3 = surface.element3;
        Element4 element4 = surface.element4;
        Element5 element5 = surface.element5;

        playerName.text = element1.player_name;

        playerNumber_reflection_text.text = playerNumber_text.text = element3.player_jersey.ToString();

        string[] scoreInfos = element4.score.Split(' ');

        string nation1 = scoreInfos[0];
        string nation2 = scoreInfos[3];
        string[] scoreNumbers1 = scoreInfos[1].Split('/');
        string runScore1 = scoreNumbers1[0];
        string wicOverScore1 = scoreNumbers1[1];
        string[] scoreNumbers2 = scoreInfos[4].Split('/');
        string runScore2 = scoreNumbers2[0];
        string wicOverScore2 = scoreNumbers2[1];
        string[] wicOverScores1 = wicOverScore1.Split('(');
        string wic1 = wicOverScores1[0];
        string over1 = '(' + wicOverScores1[1];
        string[] wicOverScores2 = wicOverScore2.Split('(');
        string wic2 = wicOverScores2[0];
        string over2 = '(' + wicOverScores2[1];

        string teamScore1 = runScore1 + "/" + wic1;
        string teamScore2 = runScore2 + "/" + wic2;


        team_a_name_text.text = nation1.ToUpperInvariant();
        team_a_scoreRun_text.text = runScore1;
        team_a_scoreWic_text.text = wic1;
        team_a_score.text = teamScore1;
        overs_a_text.text = over1;

        team_b_name_text.text = nation2.ToUpperInvariant();
        team_b_scoreRun_text.text = runScore2;
        team_b_scoreWic_text.text = wic2;
        team_b_score.text = teamScore2;
        overs_b_text.text = over2;

        string[] tempDate = element2.moment_date.Split(' ');
        tempDate[0] = tempDate[0].ToUpper();
        string[] number = tempDate[1].Split(',');
        if (number.Length == 2)
        {
            int a = int.Parse(number[0]);
            if (a < 10)
                tempDate[1] = "0" + tempDate[1];
        }

        momentDateText.text = tempDate[0] + " " + tempDate[1] + " " + tempDate[2];
        momentLengthText.text = element1.moment_length;

        rarityText.text = element1.rarity;

        actionTypeText.text = element2.action_type;
        teamNameText.text = element2.team_name;

        copyrightText.text = element5.copyright_info;
        filename = element5.file_name;

        hostName.text = element3.host;
        hostNameShadow.text = element3.host;

        tournamentStage.text = element3.tournamentStage;
        tournamentStageShadow.text = element3.tournamentStage;

        if (momentVideoPlayer != null || momentVideoPlayer)
        {
            momentVideoPlayer.url = element1.video_url;

            momentVideoPlayer.frame = 0;
            momentVideoPlayer.Play();
            // if (element5.audio_enable == "FALSE")
            // {
            //     momentVideoPlayer.SetDirectAudioMute(0, true);
            // }
            // else
            // {
            //     momentVideoPlayer.SetDirectAudioMute(0, false);
            // }
            //if (!IntroAnimController.audioIsPlaying)
            //  momentVideoPlayer.SetDirectAudioMute(0, true);
            Debug.Log("play, 1");
            momentVideoPlayer.started += MomentVideoPlayer_prepareCompleted;
        }
        BallManager.SetBall(GetCubeRarityType(element1.rarity));

        ImageLoader.GetTexture2D(TextureArrived, element4.image_url);

        ImageLoader.GetTexture2D(VideoBackgroundImageArrived, element2.image_url);

        ImageLoader.GetTexture2D(surfaceFourImageArrived, element3.surfaceFour);
        StartCoroutine(ChangeSurfaceFourColor(element1.border_color, GetCubeRarityType(element1.rarity)));

        ImageLoader.GetTexture2D(ThumbnailImageArrived, element1.video_thumbnail);

        ImageLoader.GetTexture2D(MonoChromBackgroundArrived, element1.image_url);

        //ImageLoader.GetTexture2D(FederationImageArrived, element5.federation_image_url);

        //ImageLoader.GetTexture2D(FlagImageArrived, element5.flag_image_url);

        IntroAnimController.SetIntroAnimController(element1.rarity);


        BorderSetter.SetBorder(GetCubeRarityType(element1.rarity), element1.border_color);

        //MaterialColourHandler.SetColor(element1.border_color, GetCubeRarityType(element1.rarity));

        LightingController.SetLighting(GetCubeRarityType(element1.rarity));

        //AudioLoader.GetAudio(AudioRecieved, element5.audio_enable, element5.audio_url);

    }

    public void AudioRecieved()
    {
        //IncreaseElementCountAndCheckIfAllDownloaded();
    }

    public void convertJsonToData(string jsonString)
    {
#if UNITY_EDITOR
        if (VideoRecorder.IsRecording)
        {
            indexToBeVisualised = 0;

            ClassClass className = JsonConvert.DeserializeObject<ClassClass>(jsonString);

            string json = JsonConvert.SerializeObject(className);

            surfaces = className.className;

            VisualiseSurface(surfaces[indexToBeVisualised]);
        }
        else
        {
            Surface surface = JsonConvert.DeserializeObject<Surface>(jsonString);

            VisualiseSurface(surface);
        }
#else
            Surface surface = JsonConvert.DeserializeObject<Surface>(jsonString);

            VisualiseSurface(surface);
#endif

    }

    private void MomentVideoPlayer_prepareCompleted(VideoPlayer source)
    {
        if (StateController.GetState()!=State.Interaction)
        {
            if (!alreadyPrepared)
            {
                momentVideoPlayer.frame = 0;
                momentVideoPlayer.Stop();
                Debug.Log("stop, 2");
                momentVideoPlayer.frame = 0;
                alreadyPrepared = true;
            }
            else
            {
                momentVideoPlayer.frame = 0;
                //momentVideoPlayer.Play();
                //if (!IntroAnimController.audioIsPlaying)
                //  momentVideoPlayer.SetDirectAudioMute(0, true);
                //Debug.Log("play, 2");
            }
        }
        else
        {
            ////momentVideoPlayer.frame = 0;
            //momentVideoPlayer.Stop();
        }
      
    }

    void TextureArrived(Texture2D texture2D)
    {
        packImage_renderer.material.mainTexture = texture2D;
        IncreaseElementCountAndCheckIfAllDownloaded();
    }

    void VideoBackgroundImageArrived(Texture2D texture2D)
    {
        ballBG_renderer.material.mainTexture = texture2D;
        IncreaseElementCountAndCheckIfAllDownloaded();
    }

     void surfaceFourImageArrived(Texture2D texture2D)
    {
        // surfaceFour_renderer.sprite = texture2D;
        surfaceFour_Image.sprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), new Vector2(0.5f, 0.5f));
        surfaceFourShadow_renderer.sprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), new Vector2(0.5f, 0.5f));
        isSurfaceFourImageDownloaded = true;
        //IncreaseElementCountAndCheckIfAllDownloaded();
    }

    IEnumerator ChangeSurfaceFourColor(string borderColor, CubeRarityType rarity)
    {
        yield return new WaitUntil(() => isSurfaceFourImageDownloaded);
        surfaceFour_renderer_Gradiant.gameObject.SetActive(false);
        MaterialColourHandler.SetColor(borderColor, rarity, surfaceFour_Image, surfaceFourShadow_renderer,
         surfaceFour_renderer_Gradiant.gameObject, hostName, hostNameShadow, tournamentStage, tournamentStageShadow);
        IncreaseElementCountAndCheckIfAllDownloaded();
    }
    void ThumbnailImageArrived(Texture2D texture2D)
    {
        videoThumbnail.material.mainTexture = texture2D;
        //video monochrome
        IncreaseElementCountAndCheckIfAllDownloaded();
    }

    void MonoChromBackgroundArrived(Texture2D texture2D)
    {
        //ballBG_renderer.material.mainTexture = texture2D;
        video_SS_BG_renderer1.material.mainTexture = texture2D;
        video_SS_BG_renderer2.material.mainTexture = texture2D;

        IncreaseElementCountAndCheckIfAllDownloaded();
    }

    void FederationImageArrived(Texture2D texture2D)
    {
        //if (texture2D!=null)
        //{
        //  assosiationLogoImage_renderer.sprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), new Vector2(0.5f, 0.5f));
        //}

        //IncreaseElementCountAndCheckIfAllDownloaded();
    }

    void FlagImageArrived(Texture2D texture2D)
    {
        //teamLogoImage_renderer.sprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), new Vector2(0.5f, 0.5f));

        //IncreaseElementCountAndCheckIfAllDownloaded();
    }

    void IncreaseElementCountAndCheckIfAllDownloaded()
    {
        alreadyDownloadedElementCount += 1;
        Debug.LogError("______ Download counter ______" + alreadyDownloadedElementCount);
        if (alreadyDownloadedElementCount == 5)
        //if (alreadyDownloadedElementCount == 3)
        {
            //setDefaultAnimationBeforeStart?.Invoke();
            onEverythingLoaded?.Invoke();
            txtTesting.text = "all loaded";
            alreadyDownloadedElementCount = 0;
        }
        else
        {
            txtTesting.text = alreadyDownloadedElementCount.ToString();
        }
    }

    CubeRarityType GetCubeRarityType(string rarity)
    {
        rarity = rarity.ToUpperInvariant();

        switch (rarity)
        {
            case "EPIC":
                return CubeRarityType.epic;
            case "GENESIS":
                return CubeRarityType.genesis;
            case "LEGENDARY":
                return CubeRarityType.legendary;
            case "PLATINUM":
                return CubeRarityType.platinum;
            case "RARE":
                return CubeRarityType.rare;
            case "COMMON":
                return CubeRarityType.common;

        }

        return CubeRarityType.notdefined;
    }
}

[Serializable]
public class ClassClass
{
    public List<Surface> className = new List<Surface>();
}

[Serializable]
public class Surface
{
    public Element1 element1;
    public Element2 element2;
    public Element3 element3;
    public Element4 element4;
    public Element5 element5;

    public Surface(Element1 element1, Element2 element2, Element3 element3, Element4 element4, Element5 element5)
    {
        this.element1 = element1;
        this.element2 = element2;
        this.element3 = element3;
        this.element4 = element4;
        this.element5 = element5;
    }
}

[Serializable]
public class Element1
{
    //video side
    public string player_name;
    public string moment_length;
    public string border_color;
    public string rarity;
    public string image_url;
    public string video_thumbnail;
    public string video_url;

    public Element1(string player_name, string moment_length, string rarity, string image_url, string thumbnail_url)
    {
        this.player_name = player_name;
        this.moment_length = moment_length;
        this.rarity = rarity;
        this.image_url = image_url;
        this.video_thumbnail = thumbnail_url;

    }
}

[Serializable]
public class Element2
{
    //invisible side
    public string action_type;
    public string image_url;
    public string moment_date;
    public string team_name;

    public Element2(string action_type, string image_url, string moment_date, string team_name)
    {
        this.action_type = action_type;
        this.image_url = image_url;
        this.moment_date = moment_date;
        this.team_name = team_name;
    }
}

[Serializable]
public class Element3
{
    //numbered side
    //numbered side
    public int player_jersey;
    public string surfaceFour;
    public string host;
    public string tournamentStage;


    public Element3(int player_jersey, string surfaceFour, string host, string tournamentStage)
    {
        this.player_jersey = player_jersey;
        this.surfaceFour = surfaceFour;
        this.host = host;
        this.tournamentStage = tournamentStage;

    }
}

[Serializable]
public class Element4
{
    //score side
    public string score;
    public string image_url;

    public Element4(string score, string image_url)
    {
        this.score = score;
        this.image_url = image_url;
    }
}

[Serializable]
public class Element5
{
    //logo side
    public string federation_image_url;
    public string flag_image_url;
    public string copyright_info;

    public string audio_enable;
    public string audio_url;
    public string file_name;

    public Element5(string federation_image_url, string flag_image_url, string copyright_info, string audio_enable, string audio_url, string file_name)
    {
        this.federation_image_url = federation_image_url;
        this.flag_image_url = flag_image_url;
        this.copyright_info = copyright_info;
        this.audio_enable = audio_enable;
        this.audio_url = audio_url;
        this.file_name = file_name;
    }
}

public enum CubeRarityType
{
    epic,
    genesis,
    legendary,
    platinum,
    rare,
    common,
    notdefined,
}