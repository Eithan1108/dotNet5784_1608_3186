namespace DalApi
{
    public interface ILooz
    {
        DateTime? GetStartDate();
        DateTime? GetEndDate();

        void SetStartDate(DateTime? startDate);
        void SetEndDate(DateTime? endDate);
    }
}