
namespace BlImplementation;
using BlApi;
using DO;
using System.Xml.Linq;
using Dal;



/// <summary>
/// // This class is the main class of the BL layer
/// </summary>
internal class Bl : IBl
{

    static readonly string s_tasks_xml = "tasks";
    static readonly string s_engineers_xml = "engineers";
    static readonly string s_dependence_xml = "dependences";
    static readonly string s_config_xml = "data-config";
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get;

    public ITask Task =>  new TaskImplementation(this); // create new TaskImplementation
    public IEngineer Engineer =>  new EngineerImplementation(); // create new EngineerImplementation

    public ILooz Looz =>  new LoozImplementation(); // create new LoozImplementation

    private static DateTime s_Clock = (DateTime)s_bl.Looz.GetProjectDataScreen();
    public DateTime Clock { get { return s_Clock; } private set { s_Clock = value; } }

    public void AddHourInPl(int hour)
    {
        s_Clock = s_Clock.AddHours(hour);
        s_bl.Looz.SetProjectDataScreen(s_Clock);

    }

    public void AddDaysInPl(int days)
    {
        s_Clock = s_Clock.AddDays(days);
        s_bl.Looz.SetProjectDataScreen(s_Clock);
    }

    public void InitialClock()
    {
        s_Clock = DateTime.Now;
        s_bl.Looz.SetProjectDataScreen(s_Clock);
    }

    public void Reset()
    {
        s_bl.InitialClock();
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

