namespace BuptAssistant.CampusNetwork
{
    public enum NetworkType
    {
        Wifi,
        Ethernet,
        Mobile,
        WiMax,
        Bluetooth,
        Vpn,
        Others
    }

    public class NetworkStatus
    {
        public string Name { get; set; }
        public NetworkType Type { get; set; }
    }
    public interface INetworkStatus
    {
        NetworkStatus GetNetworkStatus();
    }
}