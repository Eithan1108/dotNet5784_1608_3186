

namespace BlImplementation;
using BO;
using System.Data;



internal class LoozImplementation : BlApi.ILooz
{
    private DalApi.IDal _dal = Factory.Get;
    public DateTime? GetEndDate() => _dal.looz.GetEndDate();

    public DateTime? GetStartDate() => _dal.looz.GetStartDate();

    public void SetEndDate(DateTime? endDate) => _dal.looz.SetEndDate(endDate);

    public void SetStartDate(DateTime? startDate) => _dal.looz.SetStartDate(startDate);
}

