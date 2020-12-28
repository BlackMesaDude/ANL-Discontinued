namespace ANL.Server.Networking.Entity.Generics
{
    /// <summary>
    /// Generic entity that defines a simple user
    /// </summary>
    public class User : Interfaces.IEntity 
    {
        /// <summary>
        /// Id of this user
        /// </summary>
        public int id;

        /// <summary>
        /// Username of this user
        /// </summary>
        public string username; 

        /// <summary>
        /// Creates a new user based on id and username
        /// </summary>
        /// <param name="id">Id of the new user</param>
        /// <param name="username">Display name of the new user</param>
        public User(int id, string username)
        {
            this.id = id;
            this.username = username;
        }

        /// <summary>
        /// Updates the user
        /// </summary>
        public void Update() 
        {
            // todo: needs to be implemented
        }
    }
}