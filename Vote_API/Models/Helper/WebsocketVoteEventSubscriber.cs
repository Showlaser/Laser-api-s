using Mapster;
using Newtonsoft.Json;
using System.Net.WebSockets;
using System.Text;
using Vote_API.Models.Dto;
using Vote_API.Models.ToFrontend;

namespace Vote_API.Models.Helper
{
    public class WebsocketVoteEventSubscriber
    {
        private readonly List<WebsocketInfo> _websockets = new();

        public void Subscribe(WebsocketInfo websocketInfo)
        {
            _websockets.Add(websocketInfo);
        }

        public void UnSubscribe(WebsocketInfo websocketInfo)
        {
            _websockets.Remove(websocketInfo);
        }

        public async Task OnUpdate(VoteDataDto data)
        {
            List<WebsocketInfo> websocketsToInform = _websockets.FindAll(wsi =>
                wsi.Identifier?.VoteDataUuid == data.Uuid);

            VoteDataViewmodel voteDataViewmodel = data.Adapt<VoteDataViewmodel>();

            foreach (WebsocketInfo wsi in websocketsToInform)
            {
                string json = JsonConvert.SerializeObject(voteDataViewmodel);
                byte[] bytesToSend = Encoding.UTF8.GetBytes(json);

                await wsi.WebSocket.SendAsync(new ArraySegment<byte>(bytesToSend,
                    0, bytesToSend.Length), WebSocketMessageType.Text,
                    WebSocketMessageFlags.EndOfMessage, CancellationToken.None);
            }
        }

        public Task DeleteClosedWebsockets()
        {
            _websockets.RemoveAll(wsi => wsi.WebSocket.State is WebSocketState.Closed or WebSocketState.Aborted);
            return Task.CompletedTask;
        }
    }
}
