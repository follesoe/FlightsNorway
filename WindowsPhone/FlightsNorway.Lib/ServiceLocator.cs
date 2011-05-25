using TinyMessenger;

namespace FlightsNorway.Lib
{
    public static class ServiceLocator
    {
        public static ITinyMessengerHub Messenger { get; private set; }

        static ServiceLocator()
        {
            Messenger = new TinyMessengerHub();
        }
    }
}
