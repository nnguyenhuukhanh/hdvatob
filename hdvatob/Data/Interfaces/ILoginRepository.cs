using hdvatob.ViewModel;


namespace hdvatob.Data.Interfaces
{
    public interface ILoginRepository:IRepository<LoginModel>
    {
        LoginModel login(string username, string password, string mact);
        int changePass(string username, string newPass);
    }
}
