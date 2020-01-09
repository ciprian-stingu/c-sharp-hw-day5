namespace InterfaceSegregationIdentityAfter.Contracts
{
    public interface IManager
    {
        void ChangePassword(string oldPass, string newPass);
    }
}
