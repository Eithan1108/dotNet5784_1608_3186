

namespace BlImplementation;
using BO;
using System.Data;



internal class LoozImplementation : BlApi.ILooz
{

    private DalApi.IDal _dal = Factory.Get;
    public DateTime? GetEndDate() => _dal.looz.GetEndDate();

    public DateTime? GetStartDate() => _dal.looz.GetStartDate();

    public DateTime? GetProjectDataScreen() => _dal.looz.GetProjectDataScreen();


    public void SetEndDate(DateTime? endDate) => _dal.looz.SetEndDate(endDate);

    public void SetStartDate(DateTime? startDate) => _dal.looz.SetStartDate(startDate);

    public void SetProjectDataScreen(DateTime? projectDataScreen) => _dal.looz.SetProjectDataScreen(projectDataScreen);
}

