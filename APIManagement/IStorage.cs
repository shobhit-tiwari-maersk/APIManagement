namespace APIManagement
{
    public interface IStorage
    {
        Client AddClient(Client client);
        bool ValidateSecret(string secret);
    }
}
