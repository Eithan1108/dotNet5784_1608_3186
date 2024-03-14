namespace BlApi
{
    public interface ILooz
    {
        DateTime? GetStartDate();
        DateTime? GetEndDate();
        DateTime? GetProjectDataScreen();

        void SetStartDate(DateTime? startDate);
        void SetEndDate(DateTime? endDate);
        void SetProjectDataScreen(DateTime? projectDataScreen);

    }
}