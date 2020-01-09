namespace InterfaceSegregationIdentityAfter
{
    using System.Collections.Generic;

    using InterfaceSegregationIdentityAfter.Contracts;

    public class AccountManager : IManager
    {
        public void ChangePassword(string oldPass, string newPass)
        {
            // change password
        }
    }
}
