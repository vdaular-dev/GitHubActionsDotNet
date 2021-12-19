﻿using GitHubActionsDotNet.Models;
using System.Collections.Generic;

namespace GitHubActionsDotNet.Helpers
{
    public static class JobHelper
    {
        public static Job AddJob(
            string displayName = null,
            string runs_on = null,
            Step[] steps = null,
            Dictionary<string, string> env = null,
            string[] needs = null,
            int timeout_minutes = 0)
        {
            Job job = new Job
            {
                name = displayName,
                runs_on = runs_on,
                needs = needs,
                env = env,
                timeout_minutes = timeout_minutes,
                steps = steps
            };
            return job;
        }
    }
}
