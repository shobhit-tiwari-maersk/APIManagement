namespace APIManagement
{
    public interface IStorage
    {
        Client AddClient(Client client);
        Client GetClientById(string id);
        QuotaCounter GetCounterByClientId(string key);
        void RemoveCounter(string key);

        bool ValidateSecret(string secret);
        void SetCounter(string clientId, QuotaCounter counter);
    }
}
