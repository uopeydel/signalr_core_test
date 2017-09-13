using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.SignalR.Client;
using System.Threading;

namespace signalr01.Controllers
{
    [Route("api/[controller]")]
    public class TestHubController : Controller
    {
        //localhost:58141/api/TestHub/action
        [HttpGet("action")]
        public IActionResult ClientHub()
        {
            OpenHubNow();

            return Ok();
        }

        private static async Task OpenHubNow() {
            var baseUrl = "http://localhost:58141/chat";
            var connection = new HubConnectionBuilder()
                .WithUrl(baseUrl)
                .WithConsoleLogger()
                .Build();

            try
            {
                await connection.StartAsync();
                Console.WriteLine("Connected to {0}", baseUrl);

                var cts = new CancellationTokenSource();
                Console.CancelKeyPress += (sender, a) =>
                {
                    a.Cancel = true;
                    Console.WriteLine("Stopping loops...");
                    cts.Cancel();
                };

                // Set up handler
                connection.On<string>("Send", Console.WriteLine);

                connection.Closed += e =>
                {
                    Console.WriteLine("Connection closed.");
                    cts.Cancel();
                    return Task.CompletedTask;
                };

                var ctsTask = Task.Delay(-1, cts.Token);

                while (!cts.Token.IsCancellationRequested)
                {
                    var completedTask = await Task.WhenAny(Task.Run(() => Console.ReadLine(), cts.Token), ctsTask);
                    if (completedTask == ctsTask)
                    {
                        break;
                    }

                    var line = await (Task<string>)completedTask;

                    if (line == null)
                    {
                        break;
                    }

                    await connection.InvokeAsync<object>("Send", cts.Token, line);
                }
            }
            catch (AggregateException aex) when (aex.InnerExceptions.All(e => e is OperationCanceledException))
            {
            }
            catch (OperationCanceledException)
            {
            }
            finally
            {
                await connection.DisposeAsync();
            }
        }
    }
         
    
}
