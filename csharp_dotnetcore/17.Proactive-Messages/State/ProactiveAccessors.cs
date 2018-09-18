﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Bot.Builder;

namespace Microsoft.BotBuilderSamples
{
    /// <summary>
    /// Defines <see cref="IStatePropertyAccessor{T}"/> for use with this bot.
    /// </summary>
    public class ProactiveAccessors
    {
        /// <summary>A unique ID to use for this property accessor.</summary>
        public const string JobLogDataName = "ProactiveBot.JobLogAccessor";

        public ProactiveAccessors(JobState jobState)
        {
            JobState = jobState;
        }

        /// <summary>Gets or sets the state property accessor for the job log.</summary>
        /// <value>"Running" jobs (represented by <see cref="JobLog.JobData"/>).</value>
        public IStatePropertyAccessor<JobLog> JobLogData { get; set; }

        /// <summary>
        /// Gets the JobState object.
        /// </summary>
        /// <value>
        /// User and Conversation independent state object.
        /// </value>
        public JobState JobState { get; }
    }
}
