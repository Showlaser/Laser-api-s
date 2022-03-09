using System.Net.WebSockets;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Vote_API.Logic;
using Vote_API.Models.FromFrontend;
using Vote_API.Models.Helper;

namespace Vote_API.Controllers
{
    [ApiController]
    public class WebsocketController : ControllerBase
    {
        private readonly WebsocketVoteEventSubscriber _websocketVoteEventSubscriber;

        public WebsocketController(WebsocketVoteEventSubscriber websocketVoteEventSubscriber)
        {
            _websocketVoteEventSubscriber = websocketVoteEventSubscriber;
        }

        [HttpGet("/ws")]
        public async Task Get()
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
                var buffer = new byte[1024 * 4];
                await webSocket.ReceiveAsync(
                    new ArraySegment<byte>(buffer), CancellationToken.None);

                string data = Encoding.UTF8.GetString(buffer);
                var info = JsonConvert.DeserializeObject<WebsocketInfo>(data);
                _websocketVoteEventSubscriber.Subscribe(info);

                await Echo(webSocket, Guid.NewGuid(), voteDataUuid);
            }
            else
            {
                HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            }
        }

        private async Task Echo(WebSocket webSocket, Guid webSocketUuidIdentifier, Guid voteDataUuid)
        {
            var buffer = new byte[1024 * 4];
            var receiveResult = await webSocket.ReceiveAsync(
                new ArraySegment<byte>(buffer), CancellationToken.None);

            while (!receiveResult.CloseStatus.HasValue)
            {
                byte[] bytes = Encoding.UTF8.GetBytes(webSocketUuidIdentifier.ToString());
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
        }
    }
}
