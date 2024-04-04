namespace DalApi
{
    public interface IManager
    {
        String GetManagerPassWord();
        String GetManagerEmail();


        void SetManagerPassWord(String managerPassWord);
        void SetManagerEmail(String managerEmail);

        void Reset(bool wothManager);
    }
}
