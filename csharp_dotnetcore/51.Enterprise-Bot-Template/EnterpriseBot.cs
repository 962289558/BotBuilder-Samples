﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Extensions.Options;

namespace EnterpriseBot
{
    /// <summary>
    /// Main entry point and orchestration for bot.
    /// </summary>
    public class EnterpriseBot : IBot
    {
        private readonly BotServices _services;
        private readonly SemaphoreSlim _semaphore;
        private readonly EnterpriseBotAccessors _accessors;

        /// <summary>
        /// Initializes a new instance of the <see cref="EnterpriseBot"/> class.
        /// </summary>
        /// <param name="botServices">Bot services.</param>
        /// <param name="accessors">Bot State Accessors.</param>
        public EnterpriseBot(BotServices botServices, EnterpriseBotAccessors accessors)
        {
            _accessors = accessors;
            _services = botServices;

            // a semaphore to serialize access to the bot state
            _semaphore = accessors.SemaphoreSlim;

            Dialogs = new DialogSet(accessors.ConversationDialogState);
            Dialogs.Add(new MainDialog(_services));
        }

        private DialogSet Dialogs { get; set; }

        /// <summary>
        /// Run every turn of the conversation. Handles orchestration of messages.
        /// </summary>
        /// <param name="turnContext">Bot Turn Context.</param>
        /// <param name="cancellationToken">Task CancellationToken.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task OnTurnAsync(ITurnContext turnContext, CancellationToken cancellationToken)
        {
            try
            {
                await _semaphore.WaitAsync();

                var dc = await Dialogs.CreateContextAsync(turnContext);
                var result = await dc.ContinueAsync();

                if (result.Status == DialogTurnStatus.Empty)
                {
                    await dc.BeginAsync(MainDialog.Name);
                }
            }
            finally
            {
                _semaphore.Release();
            }
        }
    }
}