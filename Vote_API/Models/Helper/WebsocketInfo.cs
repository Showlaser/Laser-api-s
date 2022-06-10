using System.Net.WebSockets;
using Vote_API.Models.FromFrontend;

namespace Vote_API.Models.Helper
{
    public class WebsocketInfo
    {
        public WebSocket WebSocket { get; set; }
        public WebsocketIdentifier? Identifier { get; set; }
    }
}
