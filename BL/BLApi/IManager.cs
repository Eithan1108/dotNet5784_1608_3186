namespace BlApi
{
    public interface IManager
    {
        String GetManagerEmail();
        String GetManagerPassWord();

        void SetManagerEmail(String managerEmail);
        void SetManagerPassWord(String managerPassword);

        void SendEmail(int randomNumber);

    }
}
