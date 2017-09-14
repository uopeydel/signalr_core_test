using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.SignalR.Client;
using System.Threading;
using Microsoft.AspNetCore.Cors;

namespace signalr01.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("AllowAll")]
    public class TestHub2Controller : Controller
    {
        //localhost:58141/api/TestHub/action
        [HttpGet("action")]
        public async Task<IActionResult> ClientHub()
        {
            await OpenHubNow();

            return Ok();
        }

        //private static void AppendData(string value)
        //{
        //    //Use 'value' to do anything.
        //}

        //private static void AppendError(string value)
        //{
        //    //Use 'value' to do anything.
        //}

        private static async Task OpenHubNow() {
            var baseUrl = "http://localhost:58141/chat";
            baseUrl = "http://localhost:54021/NotificationHub";
            var connection = new HubConnectionBuilder()
                .WithUrl(baseUrl)
                .WithConsoleLogger()
                .Build();

            try
            {
                await connection.StartAsync();
                
                ////var cts = new CancellationTokenSource();
                ////Console.CancelKeyPress += (sender, a) =>
                ////{
                ////    a.Cancel = true;
                ////    //Console.WriteLine("Stopping loops...");
                ////    cts.Cancel();
                ////};

                //////Set up handler
                ////connection.On<string>("Send", AppendData);

                //connection.Closed += e =>
                //{
                //    AppendError("Connection closed." + e.Message);
                //    cts.Cancel();
                //    return Task.CompletedTask;
                //};

                var Text = "Test Text For Send xxxqe";
                await connection.InvokeAsync<object>("Send", Text);

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
