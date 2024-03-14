using DalApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal
{
    internal class ManagerImplementation : IManager
    {
        public string GetManagerEmail()
        {
            return DataSource.Config.ManagerEmail;
        }

        public string GetManagerPassWord()
        {
            return DataSource.Config.ManagerPassWord;
        }

        public void SetManagerEmail(string managerEmail)
        {
            DataSource.Config.ManagerEmail = managerEmail;
        }

        public void SetManagerPassWord(string managerPassWord)
        {
            DataSource.Config.ManagerPassWord = managerPassWord;
        }
    }
}
