using DalApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal
{
    internal class LoozImplementation : ILooz
    {
        public DateTime? GetEndDate()
        {
            return DataSource.Config.ProjectEndDate;
        }

        public DateTime? GetStartDate()
        {
            return DataSource.Config.ProjectStartDate;
        }

        public DateTime? GetProjectDataScreen()
        {
            return DataSource.Config.ProjectDataScreen;
        }

        public void SetEndDate(DateTime? endDate)
        {
            DataSource.Config.ProjectEndDate = endDate;
        }

        public void SetStartDate(DateTime? startDate)
        {
            DataSource.Config.ProjectStartDate = startDate;
        }

        public void SetProjectDataScreen(DateTime? projectDataScreen)
        {
            DataSource.Config.ProjectDataScreen = projectDataScreen;
        }
        
    }
}
