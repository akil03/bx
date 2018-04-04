﻿using System.Collections.Generic;
using GameSparks.Api.Requests;
using UnityEngine;

public class GameSparkRequests
{
    public void Request(string eventKey, string attributeKey, string attributeValue, StringDelegate strDelegate = null)
    {
        new LogEventRequest()
            .SetEventKey(eventKey)
            .SetEventAttribute(attributeKey, attributeValue)
        .Send((response) =>
        {
            if (response.HasErrors)
            {
                Debug.Log(eventKey + " request failed!");
            }
            else
            {

            }
            if (strDelegate != null)
                strDelegate(response.JSONString);
        });
    }

    public void Request(string eventKey, Dictionary<string, object> dictionary, StringDelegate SuccessCallback, StringDelegate FailedCallback = null)
    {
        var req = new LogEventRequest();
        req.SetEventKey(eventKey);
        foreach (var item in dictionary)
        {
            req.SetEventAttribute(item.Key, item.Value.ToString());
        }
        req.Send((response) =>
        {
            if (response.HasErrors)
            {
                if (FailedCallback != null)
                {
                    FailedCallback(response.JSONString);
                }
            }
            else
            {
                SuccessCallback(response.JSONString);
            }
        });
    }

    public void Request(string eventKey, StringDelegate strDelegate = null)
    {
        new LogEventRequest()
            .SetEventKey(eventKey)
            .Send((response) =>
            {
                if (response.HasErrors)
                {
                    Debug.Log("Request failed!");
                }
                else
                {
                    Debug.Log("Request success!");
                }
                if (strDelegate != null)
                    strDelegate(response.JSONString);
            });
    }
}
