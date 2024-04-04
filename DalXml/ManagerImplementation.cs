namespace Dal;
using DalApi;
using DO;
using System;
using System.Xml.Linq;


internal class ManagerImplementation : IManager
{

    static readonly string s_tasks_xml = "tasks";
    static readonly string s_engineers_xml = "engineers";
    static readonly string s_dependence_xml = "dependences";
    static readonly string s_config_xml = "data-config";
    static readonly DalApi.IDal _dal = Factory.Get;

    public string GetManagerEmail()
    {
        XElement root = XMLTools.LoadListFromXMLElement("data-config");
        XElement loozElement = root.Element("ManagerEmail")!;
        if (loozElement.Value == "")
            return null;
        return loozElement.Value;
    }

    public string GetManagerPassWord()
    {
        XElement root = XMLTools.LoadListFromXMLElement("data-config");
        XElement loozElement = root.Element("ManagerPassWord")!;
        if (loozElement.Value == "")
            return null;
        return loozElement.Value;
    }

    public void SetManagerEmail(string managerEmail)
    {
        XElement root = XMLTools.LoadListFromXMLElement("data-config");
        root.Element("ManagerEmail")!.Value = managerEmail;
        XMLTools.SaveListToXMLElement(root, "data-config");
    }

    public void SetManagerPassWord(string managerPassWord)
    {
        XElement root = XMLTools.LoadListFromXMLElement("data-config");
        root.Element("ManagerPassWord")!.Value = managerPassWord;
        XMLTools.SaveListToXMLElement(root, "data-config");
    }

    public void Reset(bool wothManager)
    {
        if (wothManager)
        {
            XElement root = XMLTools.LoadListFromXMLElement("data-config");
            root.Element("ManagerEmail")!.Value = "";
            root.Element("ManagerPassWord")!.Value = "";
            XMLTools.SaveListToXMLElement(root, "data-config");
        }
        _dal.Looz.SetProjectDataScreen(DateTime.Now);
        List<DO.Engineer> engineersClear = new List<DO.Engineer>();
        List<Dependence> dependenceClear = new List<Dependence>();
        List<DO.Task> tasksClear = new List<DO.Task>();
        XMLTools.SaveListToXMLSerializer<DO.Engineer>(engineersClear, s_engineers_xml);
        XMLTools.SaveListToXMLSerializer<DO.Task>(tasksClear, s_tasks_xml);
        XMLTools.SaveListToXMLSerializer<Dependence>(dependenceClear, s_dependence_xml);
        XElement configRestart = XMLTools.LoadListFromXMLElement(s_config_xml);
        configRestart.Element("NextTaskId")!.Value = "1";
        configRestart.Element("NextDependenceId")!.Value = "1";
        configRestart.Element("ProjectStartDate")!.Value = "";
        XMLTools.SaveListToXMLElement(configRestart, s_config_xml);
    }
}

