using System;
using UnityEngine;
using Unity.Notifications.Android;
using UnityEngine.Android;

public class MobileNotificationManager : MonoBehaviour
{
    public static MobileNotificationManager Ins;
    
    private void Awake()
    {
        if (Ins == null)
        {
            Ins = this;
            DontDestroyOnLoad(gameObject);

            RequestPermissions();
            RegisterNotificationChannel();
        }
        else Destroy(gameObject);
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        AndroidNotificationCenter.CancelAllNotifications();
    }

    private void RequestPermissions()
    {
        if (!Permission.HasUserAuthorizedPermission("android.permission.POST_NOTIFICATIONS"))
        {
            Permission.RequestUserPermission("android.permission.POST_NOTIFICATIONS");
        }
    }

    private void RegisterNotificationChannel()
    {
        var channel = new AndroidNotificationChannel
        {
            Id = "default_channel",
            Name = "Idle Chest Maximum!",
            Importance = Importance.Default,
            Description = "Open the game now to claim your rewards!!!"
        };
        
        AndroidNotificationCenter.RegisterNotificationChannel(channel);
    }

    public void SendNotification(string title, string body, DateTime date)
    {
        var notification = new AndroidNotification();
        notification.Title = title;
        notification.Text = body;
        notification.FireTime = date;
        notification.SmallIcon = "icon_1";
        notification.LargeIcon = "icon_0";

        AndroidNotificationCenter.SendNotification(notification, "default_channel");
    }
}