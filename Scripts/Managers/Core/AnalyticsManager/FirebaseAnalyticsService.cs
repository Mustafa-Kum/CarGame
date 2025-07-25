using System;
using Firebase.Analytics;
using Firebase.Crashlytics;
using UnityEngine;

namespace _Game.Scripts.General.AnalyticsManager
{
    public class FirebaseAnalyticsService : IAnalyticsService
    {
        public void LogEvent(string eventName, AnalyticData analyticData)
        {
            try
            {
                string ReplaceColonWithUnderscore(string input)
                {
                    return input.Replace(':', '_');
                }
                eventName = ReplaceColonWithUnderscore(eventName);
                {
                    if (analyticData == null)
                    {
                        FirebaseAnalytics.LogEvent(eventName);
                        return;
                    }
                    var firebaseParameters = new Parameter[analyticData.Parameters.Count];
                    var index = 0;
                    foreach (var param in analyticData.Parameters)
                    {
                        // Convert parameter value to string or handle different types as needed
                        Debug.Log($"FirebaseAnalyticsService: LogEvent: {eventName} - {param.Key} - {param.Value}");
                        firebaseParameters[index] = new Parameter(param.Key, param.Value.ToString());
                        index++;
                    }

                    FirebaseAnalytics.LogEvent(eventName, firebaseParameters);
                }
            }
            catch (Exception e)
            {
                Crashlytics.LogException(e);
                throw;
            }
        }
    }
}