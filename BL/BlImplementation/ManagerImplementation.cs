namespace BlImplementation;
using BO;
using System.Data;


internal class ManagerImplementation : BlApi.IManager
{
    private DalApi.IDal _dal = Factory.Get;

    public String GetManagerEmail() => _dal.manager.GetManagerEmail();

    public String GetManagerPassWord() => _dal.manager.GetManagerPassWord();

    public void SetManagerEmail(String managerEmail) => _dal.manager.SetManagerEmail(managerEmail);

    public void SetManagerPassWord(String managerPassword) => _dal.manager.SetManagerPassWord(managerPassword);

}


