using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIManagement
{
    public class PersistenceService : IStorage
    {
        private List<Client> _clients = new List<Client>();
        private Dictionary<Guid, QuotaCounter> quotas = new Dictionary<Guid, QuotaCounter>();
        public Client AddClient(Client client)
        {
            client.Secret = Guid.NewGuid();
            _clients.Add(client);
            return client;
        }

        public Client GetClientById(string id)
        {
            var secret = new Guid(id);
            return _clients.Where(a => a.Secret == secret).First();
        }

        public QuotaCounter GetCounterByClientId(string key)
        {
            var guidKey = new Guid(key);
            QuotaCounter value; 
            quotas.TryGetValue(guidKey, out value);
            return value;
        }

        public void RemoveCounter(string key)
        {
            var guidKey = new Guid(key);
            quotas.Remove(guidKey);
        }
        public bool ValidateSecret(string secret)
        {
            return _clients.Any(x => x.Secret.ToString() == secret);
        }

        public void SetCounter(string clientId,QuotaCounter counter)
        {
            var key = new Guid(clientId);
            quotas[key] = counter;
        }
    }
}
