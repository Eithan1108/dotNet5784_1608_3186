namespace BlApi
{
    public interface IManager
    {
        String GetManagerEmail();
        String GetManagerPassWord();

        void SetManagerEmail(String managerEmail);
        void SetManagerPassWord(String managerPassword);
        void SendEmail(int randomNumber);

        public bool ManagerExist();

        public void CreateManager(string email, string password);

        public bool ManagerLogIn(string password);

        public void ResetManager();

        public void Reset(bool wothManager);

    }
}
