
namespace BlImplementation;
using BlApi;
using DO;
using System.Xml.Linq;
using Dal;
using System.Security.Cryptography;
using System.Net.Mail;
using BO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Globalization;




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

    public IManager Manager =>  new ManagerImplementation(); // create new ManagerImplementation

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

    public void Reset(bool wothManager)
    {
        if(wothManager)
        {
            s_bl.ResetManager();
        }
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

    public void setProjectStartDate(DateTime date)
    { 
        s_bl.Looz.SetStartDate(date);
    }

    public void setProjectEndDate(DateTime date)
    {
        s_bl.Looz.SetEndDate(date);

    }

    public bool projectStarted()
    {
        return s_bl.Looz.GetStartDate() != null;
    }

    public void SetManagerEmail(String managerEmail)
    {
        s_bl.Manager.SetManagerEmail(managerEmail);
    }

    public void SetManagerPassWord(String managerPassword)
    {
        if (managerPassword == null)
            throw new BlBadPasswordException("Password is null");
        s_bl.Manager.SetManagerPassWord(managerPassword);
    }

    public bool ManagerExist()
    {
        return s_bl.Manager.GetManagerEmail() != null && s_bl.Manager.GetManagerPassWord() != null;
    }

    public void CreateManager(string email, string password)
    {
        if (!IsEmailValid(email)) // check if email is valid
            throw new BlBadEmailException("Email format is not valid");
        s_bl.Manager.SetManagerEmail(email);
        s_bl.Manager.SetManagerPassWord(password);
    }

    public bool ManagerLogIn(string password)
    {
        return s_bl.Manager.GetManagerPassWord() == password;
    }

    public void ResetManager()
    {
        s_bl.Manager.SetManagerEmail("");
        s_bl.Manager.SetManagerPassWord("");
    }

    private static bool IsEmailValid(string email)
    {
        try
        {
            MailAddress mail = new MailAddress(email);
            return true;
        }
        catch (FormatException)
        {
            return false;
        }
    } // check if email is valid in format xxxx@xxxx.xxxx


    public void ExportToPdf()
    {
        string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Project_Management.pdf");
        string projecManager = Manager.GetManagerEmail();
        DateTime? projectStartDate = s_bl.Looz.GetStartDate();
        if (projectStartDate == null)
            CreatePdfFile(filePath, "Project Managing", projecManager, DateTime.MinValue);
        else
            CreatePdfFile(filePath, "Project Managing", projecManager, projectStartDate.Value);
    }

    public static void CreatePdfFile(string filePath, string projectName, string projectManager, DateTime projectStartDate)
    {
        // Create a new document
        Document document = new Document(PageSize.LETTER, 54, 54, 72, 54); // Set page margins

        // Create a writer that listens to the document
        PdfWriter.GetInstance(document, new FileStream(filePath, FileMode.Create));

        // Open the document
        document.Open();

        // Set background color
        document.Add(new Chunk("\n")); // Add a new line


        // Add header section
        AddHeader(document, projectName, projectManager, projectStartDate);

        // Add tasks section
        AddTasksSection(document);

        AddEngineersSection(document);

        // Close the document
        document.Close();
    }

    private static void AddHeader(Document document, string projectName, string projectManager, DateTime projectStartDate)
    {
        // Create a table for the header
        PdfPTable headerTable = new PdfPTable(2);
        headerTable.WidthPercentage = 100;

        Font headingFont = FontFactory.GetFont(FontFactory.COURIER, 10, Font.NORMAL);
        Paragraph heading = new Paragraph(s_bl.Looz.GetProjectDataScreen().Value.ToString("dd/M/yyyy"), headingFont);
        heading.SpacingBefore = 0; 
        heading.SpacingAfter = 10;
        document.Add(heading);

        // Add project name cell
        Font titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 20);
        PdfPCell projectNameCell = new PdfPCell(new Phrase(projectName, titleFont));
        projectNameCell.PaddingBottom = 10;
        projectNameCell.Border = Rectangle.NO_BORDER;
        headerTable.AddCell(projectNameCell);

        // Add project details cell
        Font detailsFont = FontFactory.GetFont(FontFactory.HELVETICA, 12);
        PdfPCell detailsCell = new PdfPCell();
        detailsCell.HorizontalAlignment = Element.ALIGN_RIGHT;
        detailsCell.PaddingBottom = 10;
        detailsCell.Border = Rectangle.NO_BORDER;

        // Create a table for project details
        PdfPTable detailsTable = new PdfPTable(2);
        detailsTable.WidthPercentage = 100;

        detailsTable.AddCell(new PdfPCell(new Phrase("Project Manager:", detailsFont)));
        detailsTable.AddCell(new PdfPCell(new Phrase(projectManager, detailsFont)));

        detailsTable.AddCell(new PdfPCell(new Phrase("Start Date:", detailsFont)));
        if(projectStartDate != DateTime.MinValue)
            detailsTable.AddCell(new PdfPCell(new Phrase(projectStartDate.ToShortDateString(), detailsFont)));
        else
            detailsTable.AddCell(new PdfPCell(new Phrase("Not set", detailsFont)));

        detailsCell.AddElement(detailsTable);
        headerTable.AddCell(detailsCell);

        // Add the header table to the document
        document.Add(headerTable);
    }

    private static void AddTasksSection(Document document)
    {


        Font headingFont = FontFactory.GetFont(FontFactory.HELVETICA, 14, Font.BOLD);
        Font tableHeaderFont = FontFactory.GetFont(FontFactory.HELVETICA, 12, Font.BOLD);
        Font dataFont = FontFactory.GetFont(FontFactory.HELVETICA, 10);

        // Add a heading for the tasks
        Paragraph heading = new Paragraph("Tasks", headingFont);
        heading.SpacingBefore = 20;
        heading.SpacingAfter = 10;
        document.Add(heading);

        // Create a table with 6 columns
        PdfPTable table = new PdfPTable(6);
        table.SpacingBefore = 10;
        table.SpacingAfter = 10;
        table.WidthPercentage = 90;
        table.DefaultCell.PaddingBottom = 5;
        table.DefaultCell.PaddingTop = 5;

        // Set column widths based on content
        float[] columnWidths = new float[] { 5f, 35f, 15f, 15f, 15f, 10f };
        table.SetWidths(columnWidths);

        // Add table headers with background color
        table.AddCell(new PdfPCell(new Phrase("ID", tableHeaderFont)) { BackgroundColor = new BaseColor(230, 230, 230) });
        table.AddCell(new PdfPCell(new Phrase("Description", tableHeaderFont)) { BackgroundColor = new BaseColor(230, 230, 230) });
        table.AddCell(new PdfPCell(new Phrase("Scheduled Date", tableHeaderFont)) { BackgroundColor = new BaseColor(230, 230, 230) });
        table.AddCell(new PdfPCell(new Phrase("ForecastDate", tableHeaderFont)) { BackgroundColor = new BaseColor(230, 230, 230) });
        table.AddCell(new PdfPCell(new Phrase("Status", tableHeaderFont)) { BackgroundColor = new BaseColor(230, 230, 230) });
        table.AddCell(new PdfPCell(new Phrase("Dependent Tasks", tableHeaderFont)) { BackgroundColor = new BaseColor(230, 230, 230) });

        // Add tasks to the table
        IEnumerable<BO.Task> tasks = s_bl.Task.GetTasksList(null).OrderBy(task => task.Id);
        foreach (var task in tasks)
        {

            table.AddCell(new PdfPCell(new Phrase(task.Id.ToString(), dataFont)));
            table.AddCell(new PdfPCell(new Phrase(task.Description, dataFont)));
            if (task.ScheduledDate != DateTime.MinValue)
            {
                string ScheduledDateformatted = task.ScheduledDate.Value.ToString("dd/M/yyyy");
                string ForecastDateformatted = task.ForecastDate.Value.ToString("dd/M/yyyy");
                table.AddCell(new PdfPCell(new Phrase(ScheduledDateformatted, dataFont)));
                table.AddCell(new PdfPCell(new Phrase(ForecastDateformatted, dataFont)));
            }
            else
            {
                table.AddCell(new PdfPCell(new Phrase("Not set yet", dataFont)));
                table.AddCell(new PdfPCell(new Phrase("Not set yet", dataFont)));
            }
            table.AddCell(new PdfPCell(new Phrase(task.Status.ToString(), dataFont)));
            table.AddCell(new PdfPCell(new Phrase(string.Join(", ", task.Dependencies.Select(t => t.Id)), dataFont)));

        }

        // Add the table to the document
        document.Add(table);


        // Add a horizontal rule for separation
        document.Add(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.5f, 100, BaseColor.GRAY, Element.ALIGN_CENTER, 1)));

        // Add task status summary
        Paragraph summary1 = new Paragraph();
        summary1.Add(new Chunk("Task Status Summary:", tableHeaderFont));
        summary1.Add(Chunk.NEWLINE);

        // Add task status summary
        int doneTasksCount = tasks.Count(t => t.Status == BO.Status.Done);
        int inDangerTasksCount = tasks.Count(t => t.Status == BO.Status.InJeopardy);
        int onTrackTasksCount = tasks.Count(t => t.Status == BO.Status.OnTrack);
        int unsheduledTasksCount = tasks.Count(t => t.Status == BO.Status.Unsheduled);
        int sheduledTasksCount = tasks.Count(t => t.Status == BO.Status.Scheduled);


        Font summaryFont = FontFactory.GetFont(FontFactory.COURIER_BOLD, 10);
        Paragraph summary = new Paragraph();
        summary.Add(new Chunk($"{(doneTasksCount * 100.0f / tasks.Count()):0.0}% of your tasks are done.", summaryFont));
        summary.Add(Chunk.NEWLINE);
        summary.Add(new Chunk($"{(inDangerTasksCount * 100.0f / tasks.Count()):0.0}% of your tasks are in danger.", summaryFont));
        summary.Add(Chunk.NEWLINE);
        summary.Add(new Chunk($"{(onTrackTasksCount * 100.0f / tasks.Count()):0.0}% of your tasks are on track.", summaryFont));
        summary.Add(Chunk.NEWLINE);
        summary.Add(new Chunk($"{(unsheduledTasksCount * 100.0f / tasks.Count()):0.0}% of your tasks are unsheduled.", summaryFont));
        summary.Add(Chunk.NEWLINE);
        summary.Add(new Chunk($"{(sheduledTasksCount * 100.0f / tasks.Count()):0.0}% of your tasks are sheduled.", summaryFont));
        document.Add(summary);
    }

    private static void AddEngineersSection(Document document)
    {
        // Add a heading for the engineers
        Font headingFont = FontFactory.GetFont(FontFactory.HELVETICA, 14, Font.BOLD);
        Paragraph heading = new Paragraph("Engineers", headingFont);
        heading.SpacingBefore = 20;
        heading.SpacingAfter = 10;
        document.Add(heading);

        // Create a table with 6 columns
        PdfPTable table = new PdfPTable(6);
        table.SpacingBefore = 10;
        table.SpacingAfter = 10;
        table.WidthPercentage = 90;
        table.DefaultCell.PaddingBottom = 5;
        table.DefaultCell.PaddingTop = 5;

        // Set column widths based on content
        float[] columnWidths = new float[] { 12f, 17f, 22f, 14f, 8f, 10f };
        table.SetWidths(columnWidths);

        // Add table headers with background color
        Font tableHeaderFont = FontFactory.GetFont(FontFactory.HELVETICA, 12, Font.BOLD);
        table.AddCell(new PdfPCell(new Phrase("ID", tableHeaderFont)) { BackgroundColor = new BaseColor(230, 230, 230) });
        table.AddCell(new PdfPCell(new Phrase("Name", tableHeaderFont)) { BackgroundColor = new BaseColor(230, 230, 230) });
        table.AddCell(new PdfPCell(new Phrase("Email", tableHeaderFont)) { BackgroundColor = new BaseColor(230, 230, 230) });
        table.AddCell(new PdfPCell(new Phrase("Level", tableHeaderFont)) { BackgroundColor = new BaseColor(230, 230, 230) });
        table.AddCell(new PdfPCell(new Phrase("Working on task", tableHeaderFont)) { BackgroundColor = new BaseColor(230, 230, 230) });
        table.AddCell(new PdfPCell(new Phrase("Cost (per hour)", tableHeaderFont)) { BackgroundColor = new BaseColor(230, 230, 230) });

        // Add engineers to the table
        Font dataFont = FontFactory.GetFont(FontFactory.HELVETICA, 10);
        IEnumerable<BO.Engineer> engineersListForPDF = s_bl.Engineer.GetEngineersList(null).OrderBy(engineer => engineer.Name);
        foreach (var engineer in engineersListForPDF)
        {
            table.AddCell(new PdfPCell(new Phrase(engineer.Id.ToString(), dataFont)));
            table.AddCell(new PdfPCell(new Phrase(engineer.Name, dataFont)));
            table.AddCell(new PdfPCell(new Phrase(engineer.Email, dataFont)));
            table.AddCell(new PdfPCell(new Phrase(engineer.Level.ToString(), dataFont)));
            if (engineer.Task != null)
                table.AddCell(new PdfPCell(new Phrase(engineer.Task.Id.ToString(), dataFont)));
            else
                table.AddCell(new PdfPCell(new Phrase("-", dataFont)));
            table.AddCell(new PdfPCell(new Phrase($"{engineer.Cost:C}", dataFont)));
        }

        // Add the table to the document
        document.Add(table);

        document.Add(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.5f, 100, BaseColor.GRAY, Element.ALIGN_CENTER, 1)));
        Paragraph summary1 = new Paragraph();
        summary1.Add(new Chunk("Engineers Summary:", tableHeaderFont));
        summary1.Add(Chunk.NEWLINE);

        // Add engineer level summary
        Font summaryFont = FontFactory.GetFont(FontFactory.COURIER_BOLD, 10);
        Paragraph summary = new Paragraph();
        summary.SpacingBefore = 10;

        var engineersByLevel = engineersListForPDF.GroupBy(e => e.Level)
                                                   .OrderBy(g => g.Key)
                                                   .Select(g => new { Level = g.Key, Count = g.Count() });

        foreach (var levelGroup in engineersByLevel)
        {
            summary.Add(new Chunk($"{levelGroup.Count} engineer(s) are {levelGroup.Level}.", summaryFont));
            summary.Add(Chunk.NEWLINE);
        }

        document.Add(summary);
    }



}

