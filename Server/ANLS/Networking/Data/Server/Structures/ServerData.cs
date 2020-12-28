namespace ANL.Server.Networking.Data.Server.Structures 
{
    /// <summary>
    /// Defines the main server data that should be contained
    /// </summary>
    public class ServerData 
    {
        private string _serverName = "Default Server";
        private int _aimingPort = 8888;

        private int _maximumUsers = 30;

        /// <summary>
        /// Gets the server name
        /// </summary>
        /// <value></value>
        public string ServerName { get => _serverName; }

        /// <summary>
        /// Gets the aiming port where the server is running
        /// </summary>
        /// <value></value>
        public int AimingPort { get => _aimingPort; }

        /// <summary>
        /// Gets the maximum allowed users in this server
        /// </summary>
        /// <value></value>
        public int MaximumUsers { get => _maximumUsers; }

        /// <summary>
        /// Creates a new data object based on the name, port and users
        /// </summary>
        /// <param name="name">Name of the server</param>
        /// <param name="port">Free port where the server should run</param>
        /// <param name="maxUsers">Maximum users allowed in this server</param>
        public ServerData(string name, int port, int maxUsers)
        {
            this._serverName = name;
            this._aimingPort = port;
            
            this._maximumUsers = maxUsers;
        }
    }
}