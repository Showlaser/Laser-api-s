using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.WebSockets;
using System.Text;
using Vote_API.Logic;
using Vote_API.Models.FromFrontend;
using Vote_API.Models.Helper;

namespace Vote_API.Controllers
{
    [ApiController]
    public class WebsocketController : ControllerBase
    {
        private readonly WebsocketVoteEventSubscriber _websocketVoteEventSubscriber;
        private readonly VoteLogic _voteLogic;

        public WebsocketController(WebsocketVoteEventSubscriber websocketVoteEventSubscriber, VoteLogic voteLogic)
        {
            _websocketVoteEventSubscriber = websocketVoteEventSubscriber;
            _voteLogic = voteLogic;
        }

        [HttpGet("/ws")]
        public async Task Get()
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                using WebSocket webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
                var buffer = new byte[1024 * 4];
                await webSocket.ReceiveAsync(
                    new ArraySegment<byte>(buffer), CancellationToken.None);

                string data = Encoding.UTF8.GetString(buffer);
                WebsocketIdentifier? identifier = JsonConvert.DeserializeObject<WebsocketIdentifier>(data);

                await _voteLogic.Find(new VoteJoinData
                {
                    JoinCode = identifier.JoinCode,
                    AccessCode = identifier.AccessCode
                }); // check if the access code is valid by throwing exception

                identifier.WebsocketUuid = Guid.NewGuid();
                WebsocketInfo websocketInfo = new()
                {
                    Identifier = identifier,
                    WebSocket = webSocket
                };

                _websocketVoteEventSubscriber.Subscribe(websocketInfo);
                await Echo(webSocket, websocketInfo);
            }
            else
            {
                HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            }
        }

        private async Task Echo(WebSocket webSocket, WebsocketInfo info)
        {
            byte[]? buffer = new byte[1024 * 4];
            WebSocketReceiveResult? receiveResult = await webSocket.ReceiveAsync(
                new ArraySegment<byte>(buffer), CancellationToken.None);

            while (!receiveResult.CloseStatus.HasValue)
            {
                byte[] bytes = Encoding.UTF8.GetBytes(info.Identifier.WebsocketUuid.ToString());
                await webSocket.SendAsync(
                    new ArraySegment<byte>(bytes, 0, bytes.Length),
                    receiveResult.MessageType,
                    receiveResult.EndOfMessage,
                    CancellationToken.None);

                receiveResult = await webSocket.ReceiveAsync(
                    new ArraySegment<byte>(buffer), CancellationToken.None);
            }

            await webSocket.CloseAsync(
                receiveResult.CloseStatus.Value,
                receiveResult.CloseStatusDescription,
                CancellationToken.None);
            _websocketVoteEventSubscriber.UnSubscribe(info);
        }
    }
}
