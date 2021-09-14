using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIManagement
{
    public class PersistenceService : IStorage
    {
        private List<Client> _clients = new List<Client>();
        public Client AddClient(Client client)
        {
            client.Secret = Guid.NewGuid();
            _clients.Add(client);
            return client;
        }
    }
}
