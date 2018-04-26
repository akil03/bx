using System;
using System.Collections.Generic;
using System.Linq;
using EasyMobile;
using UnityEngine;
public class NotificationManager : MonoBehaviour
{
    [Header("One-Time Local Notification")]
    public string title = "Demo Notification";
    public string subtitle = "Demo Notification Subtitle";
    public string message = "Demo notification message";
    public string categoryId;
    public bool fakeNewUpdate = true;
    public int delayHours = 0;
    public int delayMinutes = 0;
    public int delaySeconds = 15;

    [Header("Repeat Local Notification")]
    public string repeatTitle = "Demo Repeat Notification";
    public string repeatSubtitle = "Demo Repeat Notification Subtitle";
    public string repeatMessage = "Demo repeat notification message";
    public string repeatCategoryId;
    public int repeatDelayHours = 0;
    public int repeatDelayMinutes = 0;
    public int repeatDelaySeconds = 25;
    public NotificationRepeat repeatType = NotificationRepeat.EveryMinute;


    public static NotificationManager instance;
    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        bool autoInit = EM_Settings.Notifications.IsAutoInit;
    }

    public void Init()
    {
        if (Notifications.IsInitialized())
        {
            NativeUI.Alert("Already Initialized", "Notification module is already initalized.");
            return;
        }

        Notifications.Init();
    }

    public void ScheduleLocalNotification()
    {
        if (!InitCheck())
            return;

        var notif = new NotificationContent();
        notif.title = title;
        notif.subtitle = subtitle;
        notif.body = message;

        notif.userInfo = new Dictionary<string, object>();
        notif.userInfo.Add("string", "OK");
        notif.userInfo.Add("number", 3);

        if (fakeNewUpdate)
            notif.userInfo.Add("newUpdate", true);

        notif.categoryId = categoryId;

        DateTime triggerDate = DateTime.Now + new TimeSpan(delayHours, delayMinutes, delaySeconds);
        Notifications.ScheduleLocalNotification(triggerDate, notif);
    }

    public void Chestunlocked(string no, string type, int seconds)
    {
        if (!InitCheck())
            return;

        var notif = new NotificationContent();
        notif.title = "Chest opened!";
        notif.subtitle = no;
        notif.body = type + " chest has been opened!";
        CancelNotification(notif, seconds);

    }

    public void LifeFull(int seconds)
    {
        if (!InitCheck())
            return;

        var notif = new NotificationContent();
        notif.title = "Life is full!";
        notif.body = " Come battle!";
        CancelNotification(notif, seconds);
    }

    public void ScheduleRepeatLocalNotification()
    {
        if (!InitCheck())
            return;

        var notif = new NotificationContent();
        notif.title = repeatTitle;
        notif.body = repeatMessage;
        notif.categoryId = repeatCategoryId;

        Notifications.ScheduleLocalNotification(new TimeSpan(repeatDelayHours, repeatDelayMinutes, repeatDelaySeconds), notif, repeatType);
    }

    public void CancelPendingLocalNotification()
    {
        if (!InitCheck())
            return;
    }

    public void CancelAllPendingLocalNotifications()
    {
        if (!InitCheck())
            return;

        Notifications.CancelAllPendingLocalNotifications();
        NativeUI.Alert("Alert", "Canceled all pending local notifications of this app.");
    }

    public void RemoveAllDeliveredNotifications()
    {
        Notifications.ClearAllDeliveredNotifications();
        NativeUI.Alert("Alert", "Cleared all shown notifications of this app.");
    }

    bool InitCheck()
    {
        bool isInit = Notifications.IsInitialized();

        if (!isInit)
        {
            NativeUI.Alert("Alert", "Please initialize first.");
        }

        return isInit;
    }

    void CancelNotification(NotificationContent notif, int seconds)
    {
        Notifications.GetPendingLocalNotifications(pendingNotifs =>
        {
            try
            {
                var result = pendingNotifs.First(a => a.content.title == notif.title && a.content.body == notif.body && a.content.subtitle == notif.subtitle);
                if (result != null)
                {
                    Notifications.CancelPendingLocalNotification(result.id);
                }
            }
            catch { }
            DateTime triggerDate = DateTime.Now + new TimeSpan(delayHours, delayMinutes, seconds);
            Notifications.ScheduleLocalNotification(triggerDate, notif);
        });
    }
}