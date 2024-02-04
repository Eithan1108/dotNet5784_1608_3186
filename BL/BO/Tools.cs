namespace BO
{
    internal static class Tools
    {
        public static DO.Engineer BoDoAdapter(this BO.Engineer engineer) // adapter from BO.Engineer to DO.Engineer
        {
            if (engineer.Id is null)

                throw new BadIdException("id must be positive");

            DO.Engineer doEngineer = new DO.Engineer((int)engineer.Id!, engineer.Name, engineer.Email, (DO.EngineerExperience)engineer.Level, engineer.Cost); // create new DO.Engineer
            return doEngineer;
        }

        public static BO.Engineer DoBoAdapter(this DO.Engineer engineer, BO.TaskInEngineer taskInEngineer) // adapter from DO.Engineer to BO.Engineer
        {
            BO.Engineer boEngineer = new BO.Engineer { Id = engineer.Id, Name = engineer.Name, Email = engineer.Email, Level = (BO.EngineerExperience)engineer.Level, Cost = engineer.Cost, Task = taskInEngineer }; // create new BO.Engineer
            return boEngineer;
        }

    }
}
