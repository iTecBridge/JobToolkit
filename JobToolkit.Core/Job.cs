﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobToolkit.Core
{
    [Serializable]
    public class Job
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public JobTask Task { get; set; }
        public int Priority { get; set; }
        public bool Enabled { get; set; }
        public JobStatus Status { get; set; }
        public CronExpression Cron { get; set; }
        public DateTimeOffset ScheduleTime { get; set; }
        public DateTimeOffset? NextScheduleTime { get; set; }
        public DateTimeOffset? ScheduleEndTime { get; set; }
        public JobExecutanInfo LastExecutanInfo { get; set; }
        public AutomaticRetryPolicy AutomaticRetry { get; set; }
        public string Owner { get; set; }
        public string Description { get; set; }
        public DateTimeOffset CreateTime { get; set; }
        public DateTimeOffset? UpdateTime { get; set; }

        public Job(JobTask task, AutomaticRetryPolicy automaticRetry = null)
            : this(task, DateTimeOffset.Now, null, null, automaticRetry)
        {
        }

        public Job(JobTask task, DateTimeOffset scheduleTime, AutomaticRetryPolicy automaticRetry = null)
            : this(task, scheduleTime, null, null, automaticRetry)
        {
        }

        public Job(JobTask task, TimeSpan offset, AutomaticRetryPolicy automaticRetry = null)
            : this(task, DateTimeOffset.Now.Add(offset), null, null, automaticRetry)
        {
        }

        public Job(JobTask task, DateTimeOffset scheduleTime, CronExpression cron, DateTimeOffset? scheduleEndTime, AutomaticRetryPolicy automaticRetry)
        {
            if (task == null) throw new ArgumentNullException(nameof(task));

            this.Id = Guid.NewGuid().ToString().Replace("-", string.Empty);
            this.Title = task.Title;
            this.Task = task;
            this.Priority = 5;
            this.Enabled = true;
            this.Status = JobStatus.Scheduled;
            this.Cron = cron;            
            this.ScheduleTime = scheduleTime;
            this.NextScheduleTime = scheduleTime;
            this.ScheduleEndTime = scheduleEndTime;
            this.AutomaticRetry = automaticRetry;
            this.Description = task.Description;
            this.CreateTime = DateTimeOffset.Now;
        }

        public DateTimeOffset? GetNextScheduleTime(DateTimeOffset after)
        {
            DateTimeOffset? nextTime = null;
            if (this.Cron != null)
            {
                nextTime = this.Cron.GetNextTime(after);
                if (this.ScheduleEndTime.HasValue && nextTime > this.ScheduleEndTime)
                    nextTime = null;
            }
            return nextTime;
        }
    }
}
