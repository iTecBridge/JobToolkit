﻿using AnyCache.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace JobToolkit.Core.Repositories.Cache
{
    public class CacheJobRepository : IJobRepository, IEnumerable
    {
        private readonly IFormatter Formatter;
        private readonly IAnyCache Cache;

        public CacheJobRepository(IAnyCache cache)
        {
            this.Formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            this.Cache = cache;
        }

        public CacheJobRepository(IFormatter formatter, IAnyCache cache)
        {
            this.Formatter = formatter;
            this.Cache = cache;
        }


        public string Add(Job job)
        {
            string key = Guid.NewGuid().ToString().Replace("-", string.Empty);
            Cache.Add(key, job, null);
            return key;

            //MemoryStream stream = new MemoryStream();
            //Formatter.Serialize(stream, job);
            //string data = Encoding.UTF8.GetString(stream.ToArray());
        }

        public Job Get(string jobId)
        {
            return (Job)Cache.Get(jobId);
            //MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(jobId));

            //return (Job)Formatter.Deserialize(stream);
        }
               
        public void Update(Job job)
        {
            Cache.Set(job.Id, job, null);
        }
        public void UpdatExecStatus(Job job)
        {
            Update(job);
            
            //// History
            //if (job.LastExecutanInfo != null)
            //{
            //    if (job.LastExecutanInfo.Status == JobExecutanStatus.Running) // new execInfo
            //        ;
            //}
        }
        public Job Remove(string jobId)
        {
            return (Job)Cache.Remove(jobId);
        }

        public void RemoveAll()
        {
            Cache.ClearCache();
        }

        public IEnumerator<Job> GetEnumerator()
        {
            foreach (var item in Cache)
                yield return (Job)item.Value;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            // call the generic version
            return this.GetEnumerator();
        }

        public List<Job> GetAll()
        {
            List<Job> jobs = new List<Job>();
            foreach (var item in Cache)
                jobs.Add((Job)item.Value);
            return jobs;
        }

        public List<Job> GetAll(JobDataQueryCriteria criteria)
        {
            List<Job> jobs = new List<Job>();
            foreach (var item in Cache)
            {
                if(criteria.IsMatch((Job)item.Value))
                    jobs.Add((Job)item.Value);
            }
            return jobs;
        }
    }
}
