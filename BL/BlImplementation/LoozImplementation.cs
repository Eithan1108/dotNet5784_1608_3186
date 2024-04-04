

namespace BlImplementation;
using BO;
using System.Data;



internal class LoozImplementation : BlApi.ILooz
{

    private DalApi.IDal _dal = Factory.Get;
    public DateTime? GetEndDate() => _dal.Looz.GetEndDate();

    public DateTime? GetStartDate() => _dal.Looz.GetStartDate();

    public DateTime? GetProjectDataScreen() => _dal.Looz.GetProjectDataScreen();


    public void SetEndDate(DateTime? endDate) => _dal.Looz.SetEndDate(endDate);

    public void SetStartDate(DateTime? startDate) => _dal.Looz.SetStartDate(startDate);

    public void SetProjectDataScreen(DateTime? projectDataScreen) => _dal.Looz.SetProjectDataScreen(projectDataScreen);
}

