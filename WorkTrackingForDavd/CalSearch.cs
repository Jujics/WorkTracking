﻿using System.IO;
using Ical.Net.CalendarComponents;
using Calendar = Ical.Net.Calendar;

namespace WorkTrackingForDavd;

public class CalSearch
{
    public static readonly List<int> EventListYear = new List<int>();

    public static void StoreNumberEvent()
    {
        
        EventListYear.Clear();
        
        for(DateTime date = DateTime.Today - TimeSpan.FromDays(370); date <= DateTime.Today; date = date.AddDays(1))
        {
            EventListYear.Add(CalendarEvents("TasksCalendar.ics", date));
        }
    }
    
    public static int CalendarEvents(string icalFilePath, DateTime targetDate)
    {
        int howManyEventInDay = 0;

        if (!File.Exists(icalFilePath))
        {
            throw new FileNotFoundException("Could not find ical file", icalFilePath);
        }
        
        var calendar = Calendar.Load(File.ReadAllText(icalFilePath));
        var calendarEvents = calendar.Events;
        foreach (var calendarEvent in calendarEvents)
        {
            if (IsEventOnDate(calendarEvent, targetDate))
            {
                howManyEventInDay++;
            }
        }
        return howManyEventInDay;
    }
    
    private static bool IsEventOnDate(CalendarEvent calendarEvent, DateTime targetDate)
    {
        var targetStart = targetDate.Date;
        var targetEnd = targetDate.Date.AddDays(1);
    
        
        var eventStart = calendarEvent.Start.Value;
        var eventEnd = calendarEvent.End.Value;
        
        if (calendarEvent.IsAllDay)
        {
            return eventStart.Date <= targetDate.Date && eventEnd.Date > targetDate.Date;
        }
    
        return (eventStart >= targetStart && eventStart < targetEnd) || 
               (eventEnd > targetStart && eventEnd <= targetEnd) || 
               (eventStart <= targetStart && eventEnd >= targetEnd);
    }
    
}