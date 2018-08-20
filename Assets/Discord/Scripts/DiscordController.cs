using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[System.Serializable]
public class DiscordJoinEvent : UnityEngine.Events.UnityEvent<string> { }

[System.Serializable]
public class DiscordSpectateEvent : UnityEngine.Events.UnityEvent<string> { }

[System.Serializable]
public class DiscordJoinRequestEvent : UnityEngine.Events.UnityEvent<DiscordRpc.JoinRequest> { }


public class DiscordController : MonoBehaviour
{
    public DiscordRpc.RichPresence presence = new DiscordRpc.RichPresence();
    public string applicationId;
    public string optionalSteamId;
    public int callbackCalls;
    public int clickCounter;
    public DiscordRpc.JoinRequest joinRequest;
    public UnityEngine.Events.UnityEvent onConnect;
    public UnityEngine.Events.UnityEvent onDisconnect;
    public UnityEngine.Events.UnityEvent hasResponded;
    public DiscordJoinEvent onJoin;
    public DiscordJoinEvent onSpectate;
    public DiscordJoinRequestEvent onJoinRequest;
    
    DiscordRpc.EventHandlers handlers;

  
    public InputField iField;
    public void PleaseWork()
    {
        presence.state = iField.text;
    }







    //Emotions
    public void EmotionBored()
    {
        presence.smallImageKey = "bored";
        presence.smallImageText = string.Format("Bored AF");
        presence.state = string.Format("Living in boredom");

        DiscordRpc.UpdatePresence(presence);
    }

    public void EmotionThinking()
    {
        presence.smallImageKey = "thinking";
        presence.smallImageText = string.Format("Hmm");
        presence.state = string.Format("Contemplating");

        DiscordRpc.UpdatePresence(presence);
    }

    public void EmotionHeartEyes()
    {
        presence.smallImageKey = "heart_eyes";
        presence.smallImageText = string.Format("<3");
        presence.state = string.Format("In love");

        DiscordRpc.UpdatePresence(presence);
    }

    public void EmotionTriumph()
    {
        presence.smallImageKey = "triumph";
        presence.smallImageText = string.Format(">:(");
        presence.state = string.Format("Transcended to an angry bull");

        DiscordRpc.UpdatePresence(presence);
    }

    public void EmotionNoMouth()
    {
        presence.smallImageKey = "no_mouth";
        presence.smallImageText = string.Format("o o");
        presence.state = string.Format(". . .");

        DiscordRpc.UpdatePresence(presence);
    }

    //Activities
    public void ActTV()
    {
        presence.largeImageKey = "watching_tv";
        presence.largeImageText = string.Format("Watching TV");
        presence.details = string.Format("Consuming Entertainment");
        DiscordRpc.UpdatePresence(presence);
    }

    public void ActHeadphones()
    {
        presence.largeImageKey = "headphones";
        presence.largeImageText = string.Format("Listening");
        presence.details = string.Format("Listening to music with my ears");
        DiscordRpc.UpdatePresence(presence);
    }

    public void ActRabbits()
    {
        presence.largeImageKey = "rabbits";
        presence.details = string.Format("Watching my sister's stupid rabbits");
        DiscordRpc.UpdatePresence(presence);
    }

    public void ActCoding()
    {
        presence.largeImageKey = "coding";
        presence.largeImageText = string.Format("Coding");
        presence.details = string.Format("Developing cool apps");
        DiscordRpc.UpdatePresence(presence);
    }
    






     //Click Counter
    public void OnClick()
    {
        Debug.Log("You clicked the button idiot");
        clickCounter++;

        presence.details = string.Format("Button clicked {0} times", clickCounter);

        DiscordRpc.UpdatePresence(presence);
    }

    //Other Stuff Which Doesn't Do Anything
    public void RequestRespondYes()
    {
        Debug.Log("Discord: responding yes to Ask to Join request");
        DiscordRpc.Respond(joinRequest.userId, DiscordRpc.Reply.Yes);
        hasResponded.Invoke();
    }

    public void RequestRespondNo()
    {
        Debug.Log("Discord: responding no to Ask to Join request");
        DiscordRpc.Respond(joinRequest.userId, DiscordRpc.Reply.No);
        hasResponded.Invoke();
    }

    public void ReadyCallback()
    {
        ++callbackCalls;
        Debug.Log("Discord: ready");
        onConnect.Invoke();
    }

    public void DisconnectedCallback(int errorCode, string message)
    {
        ++callbackCalls;
        Debug.Log(string.Format("Discord: disconnect {0}: {1}", errorCode, message));
        onDisconnect.Invoke();
    }

    public void ErrorCallback(int errorCode, string message)
    {
        ++callbackCalls;
        Debug.Log(string.Format("Discord: error {0}: {1}", errorCode, message));
    }

    public void JoinCallback(string secret)
    {
        ++callbackCalls;
        Debug.Log(string.Format("Discord: join ({0})", secret));
        onJoin.Invoke(secret);
    }

    public void SpectateCallback(string secret)
    {
        ++callbackCalls;
        Debug.Log(string.Format("Discord: spectate ({0})", secret));
        onSpectate.Invoke(secret);
    }

    public void RequestCallback(ref DiscordRpc.JoinRequest request)
    {
        ++callbackCalls;
        Debug.Log(string.Format("Discord: join request {0}#{1}: {2}", request.username, request.discriminator, request.userId));
        joinRequest = request;
        onJoinRequest.Invoke(request);
    }

    void Start()
    {
    }

    void Update()
    {
        DiscordRpc.RunCallbacks();
    }

    void OnEnable()
    {
        Debug.Log("Discord: init");
        callbackCalls = 0;

        handlers = new DiscordRpc.EventHandlers();
        handlers.readyCallback = ReadyCallback;
        handlers.disconnectedCallback += DisconnectedCallback;
        handlers.errorCallback += ErrorCallback;
        handlers.joinCallback += JoinCallback;
        handlers.spectateCallback += SpectateCallback;
        handlers.requestCallback += RequestCallback;
        DiscordRpc.Initialize(applicationId, ref handlers, true, optionalSteamId);
    }

    void OnDisable()
    {
        Debug.Log("Discord: shutdown");
        DiscordRpc.Shutdown();
    }

    void OnDestroy()
    {

    }
}