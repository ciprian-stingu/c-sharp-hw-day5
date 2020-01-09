namespace InterfaceSegregationIdentityAfter
{
    using InterfaceSegregationIdentityAfter.Contracts;

    public class AccountController
    {
        private readonly IManager manager;

        public AccountController(IManager manager)
        {
            this.manager = manager;
        }

        public void ChangePassword(string oldPass, string newPass)
        {
            this.manager.ChangePassword(oldPass, newPass);
        }
    }
}
