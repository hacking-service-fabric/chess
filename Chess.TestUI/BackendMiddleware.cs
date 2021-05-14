using Chess.Data.Common.Models.V1;
using Chess.Queue.Common;
using Chess.Queue.Common.Models;
using Microsoft.AspNetCore.Http;
using PhoneNumbers;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace Chess.TestUI
{
    public class BackendMiddleware: IMiddleware
    {
        private readonly ISmsQueueServiceAccessor _queueServiceAccessor;

        private readonly List<string> _conversation = new List<string>();
        private int _lastResponse = -1;

        public BackendMiddleware(ISmsQueueServiceAccessor queueServiceAccessor)
        {
            _queueServiceAccessor = queueServiceAccessor;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                context.Response.StatusCode = 200;
                switch (context.Request.Method)
                {
                    case "GET":
                        await GetAsync(context);
                        break;
                    case "POST":
                        await PostAsync(context);
                        break;
                    default:
                        await next.Invoke(context);
                        break;
                }
            }
            catch (Exception e)
            {
                context.Response.StatusCode = 500;
                await context.Response.WriteAsync(e.Message);
            }
        }

        private async Task GetAsync(HttpContext context)
        {
            if (_lastResponse == -1)
            {
                await context.Response.WriteAsync(JsonSerializer.Serialize(new[]
                {
                    new
                    {
                        text = "0",
                        content = "http://localhost:8788/chess-pieces.jpg"
                    }
                }));
                _lastResponse = 0;
            }
            else if (_conversation.Count > _lastResponse)
            {
                await context.Response.WriteAsync(JsonSerializer.Serialize(new[]
                {
                    new
                    {
                        text = _conversation.Count.ToString()
                    }
                }));
                _lastResponse = _conversation.Count;
            }
            else
            {
                await context.Response.WriteAsync("[]");
            }
        }

        private async Task PostAsync(HttpContext context)
        {
            try
            {
                var recipients = new[]
                {
                    new PhoneNumber(),
                    new PhoneNumber()
                };
                var payload = context.Request.Form["text"];

                await _queueServiceAccessor.GetInstance(recipients).Enqueue(new SmsModel
                {
                    Conversation = new ConversationDto
                    {
                        HostPhoneNumber = new PhoneNumber(),
                        PhoneNumbers = recipients
                    },
                    TextContent = payload
                });

                _conversation.Add(context.Request.Form["text"]);
            }
            catch (Exception e)
            {
                ServiceEventSource.Current.ServiceException(e.ToString());
            }
        }
    }
}