﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobToolkit.Core
{
    [Serializable]
    public class CronExpression
    {
        public string Expression { get; }

        public CronExpression(string exp)
        {
            Expression = exp;
        }

        public DateTimeOffset GetNextTime()
        {
            return GetNextTime(DateTimeOffset.Now);
        }

        public DateTimeOffset GetNextTime(DateTimeOffset after)
        {
            var tokens = Expression?.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (tokens.Length == 5)
                return NCrontab.Advanced.CrontabSchedule.Parse(Expression).GetNextOccurrence(after.DateTime);
            else if (tokens.Length == 6)
                return NCrontab.Advanced.CrontabSchedule.Parse(Expression, NCrontab.Advanced.Enumerations.CronStringFormat.WithSeconds).GetNextOccurrence(after.DateTime);
            else if (tokens.Length == 7)
                return NCrontab.Advanced.CrontabSchedule.Parse(Expression, NCrontab.Advanced.Enumerations.CronStringFormat.WithSecondsAndYears).GetNextOccurrence(after.DateTime);
            else
                throw new ApplicationException("invalid Crontab expression.");

            //NCrontab.CrontabSchedule.Parse("").GetNextOccurrence()
        }

        public DateTimeOffset GetNextTime(DateTimeOffset after, DateTimeOffset before)
        {
            var tokens = Expression?.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (tokens.Length == 5)
                return NCrontab.Advanced.CrontabSchedule.Parse(Expression).GetNextOccurrence(after.DateTime, before.DateTime);
            else if (tokens.Length == 6)
                return NCrontab.Advanced.CrontabSchedule.Parse(Expression, NCrontab.Advanced.Enumerations.CronStringFormat.WithSeconds).GetNextOccurrence(after.DateTime, before.DateTime);
            else if (tokens.Length == 7)
                return NCrontab.Advanced.CrontabSchedule.Parse(Expression, NCrontab.Advanced.Enumerations.CronStringFormat.WithSecondsAndYears).GetNextOccurrence(after.DateTime, before.DateTime);
            else
                throw new ApplicationException("invalid Crontab expression.");
        }

        public static CronExpression Parse(string exp)
        {
            CronExpression cron = new CronExpression(exp);
            return cron;
        }

        public static implicit operator string(CronExpression cron)
        {
            return cron.Expression;
        }

        public static implicit operator CronExpression(string exp)
        {
            return new CronExpression(exp);
        }

        public override string ToString()
        {
            return Expression;
        }

        /// <summary>
        /// Returns cron expression that fires every Second.
        /// </summary>
        public static string Secondly()
        {
            return "* * * * * *";
        }

        /// <summary>
        /// Returns cron expression that fires every minute at the specified second.
        /// </summary>
        /// <param name="second">The second in which the schedule will be activated (0-59).</param>
        public static string Minutely(int second)
        {
            return String.Format("{0} * * * * *", second);
        }

        /// <summary>
        /// Returns cron expression that fires every minute.
        /// </summary>
        public static string Minutely()
        {
            return "* * * * *";
        }

        /// <summary>
        /// Returns cron expression that fires every hour at the first minute.
        /// </summary>
        public static string Hourly()
        {
            return Hourly(minute: 0);
        }

        /// <summary>
        /// Returns cron expression that fires every hour at the specified minute.
        /// </summary>
        /// <param name="minute">The minute in which the schedule will be activated (0-59).</param>
        public static string Hourly(int minute)
        {
            return String.Format("{0} * * * *", minute);
        }

        /// <summary>
        /// Returns cron expression that fires every day at 00:00 UTC.
        /// </summary>
        public static string Daily()
        {
            return Daily(hour: 0);
        }

        /// <summary>
        /// Returns cron expression that fires every day at the first minute of
        /// the specified hour in UTC.
        /// </summary>
        /// <param name="hour">The hour in which the schedule will be activated (0-23).</param>
        public static string Daily(int hour)
        {
            return Daily(hour, minute: 0);
        }

        /// <summary>
        /// Returns cron expression that fires every day at the specified hour and minute
        /// in UTC.
        /// </summary>
        /// <param name="hour">The hour in which the schedule will be activated (0-23).</param>
        /// <param name="minute">The minute in which the schedule will be activated (0-59).</param>
        public static string Daily(int hour, int minute)
        {
            return String.Format("{0} {1} * * *", minute, hour);
        }

        /// <summary>
        /// Returns cron expression that fires every week at Monday, 00:00 UTC.
        /// </summary>
        public static string Weekly()
        {
            return Weekly(DayOfWeek.Monday);
        }

        /// <summary>
        /// Returns cron expression that fires every week at 00:00 UTC of the specified
        /// day of the week.
        /// </summary>
        /// <param name="dayOfWeek">The day of week in which the schedule will be activated.</param>
        public static string Weekly(DayOfWeek dayOfWeek)
        {
            return Weekly(dayOfWeek, hour: 0);
        }

        /// <summary>
        /// Returns cron expression that fires every week at the first minute
        /// of the specified day of week and hour in UTC.
        /// </summary>
        /// <param name="dayOfWeek">The day of week in which the schedule will be activated.</param>
        /// <param name="hour">The hour in which the schedule will be activated (0-23).</param>
        public static string Weekly(DayOfWeek dayOfWeek, int hour)
        {
            return Weekly(dayOfWeek, hour, minute: 0);
        }

        /// <summary>
        /// Returns cron expression that fires every week at the specified day
        /// of week, hour and minute in UTC.
        /// </summary>
        /// <param name="dayOfWeek">The day of week in which the schedule will be activated.</param>
        /// <param name="hour">The hour in which the schedule will be activated (0-23).</param>
        /// <param name="minute">The minute in which the schedule will be activated (0-59).</param>
        public static string Weekly(DayOfWeek dayOfWeek, int hour, int minute)
        {
            return String.Format("{0} {1} * * {2}", minute, hour, (int)dayOfWeek);
        }

        /// <summary>
        /// Returns cron expression that fires every month at 00:00 UTC of the first
        /// day of month.
        /// </summary>
        public static string Monthly()
        {
            return Monthly(day: 1);
        }

        /// <summary>
        /// Returns cron expression that fires every month at 00:00 UTC of the specified
        /// day of month.
        /// </summary>
        /// <param name="day">The day of month in which the schedule will be activated (1-31).</param>
        public static string Monthly(int day)
        {
            return Monthly(day, hour: 0);
        }

        /// <summary>
        /// Returns cron expression that fires every month at the first minute of the
        /// specified day of month and hour in UTC.
        /// </summary>
        /// <param name="day">The day of month in which the schedule will be activated (1-31).</param>
        /// <param name="hour">The hour in which the schedule will be activated (0-23).</param>
        public static string Monthly(int day, int hour)
        {
            return Monthly(day, hour, minute: 0);
        }

        /// <summary>
        /// Returns cron expression that fires every month at the specified day of month,
        /// hour and minute in UTC.
        /// </summary>
        /// <param name="day">The day of month in which the schedule will be activated (1-31).</param>
        /// <param name="hour">The hour in which the schedule will be activated (0-23).</param>
        /// <param name="minute">The minute in which the schedule will be activated (0-59).</param>
        public static string Monthly(int day, int hour, int minute)
        {
            return String.Format("{0} {1} {2} * *", minute, hour, day);
        }

        /// <summary>
        /// Returns cron expression that fires every year on Jan, 1st at 00:00 UTC.
        /// </summary>
        public static string Yearly()
        {
            return Yearly(month: 1);
        }

        /// <summary>
        /// Returns cron expression that fires every year in the first day at 00:00 UTC
        /// of the specified month.
        /// </summary>
        /// <param name="month">The month in which the schedule will be activated (1-12).</param>
        public static string Yearly(int month)
        {
            return Yearly(month, day: 1);
        }

        /// <summary>
        /// Returns cron expression that fires every year at 00:00 UTC of the specified
        /// month and day of month.
        /// </summary>
        /// <param name="month">The month in which the schedule will be activated (1-12).</param>
        /// <param name="day">The day of month in which the schedule will be activated (1-31).</param>
        public static string Yearly(int month, int day)
        {
            return Yearly(month, day, hour: 0);
        }

        /// <summary>
        /// Returns cron expression that fires every year at the first minute of the
        /// specified month, day and hour in UTC.
        /// </summary>
        /// <param name="month">The month in which the schedule will be activated (1-12).</param>
        /// <param name="day">The day of month in which the schedule will be activated (1-31).</param>
        /// <param name="hour">The hour in which the schedule will be activated (0-23).</param>
        public static string Yearly(int month, int day, int hour)
        {
            return Yearly(month, day, hour, minute: 0);
        }

        /// <summary>
        /// Returns cron expression that fires every year at the specified month, day,
        /// hour and minute in UTC.
        /// </summary>
        /// <param name="month">The month in which the schedule will be activated (1-12).</param>
        /// <param name="day">The day of month in which the schedule will be activated (1-31).</param>
        /// <param name="hour">The hour in which the schedule will be activated (0-23).</param>
        /// <param name="minute">The minute in which the schedule will be activated (0-59).</param>
        public static string Yearly(int month, int day, int hour, int minute)
        {
            return String.Format("{0} {1} {2} {3} *", minute, hour, day, month);
        }


    }
}
