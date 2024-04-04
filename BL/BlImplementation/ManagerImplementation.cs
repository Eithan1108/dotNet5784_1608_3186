namespace BlImplementation;
using BO;
using System.Data;
using System.Net;
using System.Net.Mail;


internal class ManagerImplementation : BlApi.IManager
{
    private DalApi.IDal _dal = Factory.Get;

    public String GetManagerEmail() => _dal.Manager.GetManagerEmail();

    public String GetManagerPassWord() => _dal.Manager.GetManagerPassWord();

    public void SetManagerEmail(String managerEmail) => _dal.Manager.SetManagerEmail(managerEmail);

    public void SetManagerPassWord(String managerPassword) => _dal.Manager.SetManagerPassWord(managerPassword);

    public void SendEmail(int randomNumber)
    {
        MailAddress to = new MailAddress(GetManagerEmail()); // the email address that the message will be sent to

        MailAddress from = new MailAddress("kayamutmaoz@gmail.com"); // the email address that the message will be sent from

        MailMessage email = new MailMessage(from, to); // create the email message
        email.Subject = "Verification code to reset password - Project management ";
        email.Body = "Your verification code is: " + randomNumber; // the message that will be sent to the user

        SmtpClient smtp = new SmtpClient();  // protocol for sending email
        smtp.Host = "smtp.gmail.com"; // mail server format
        smtp.Port = 587; // port number
        smtp.Credentials = new NetworkCredential("kayamutmaoz@gmail.com", "bglevhlsbuywqrlt"); // email and password
        smtp.DeliveryMethod = SmtpDeliveryMethod.Network; 
        smtp.EnableSsl = true;

        try
        {
            /* Send method called below is what will send off our email 
             * unless an exception is thrown.
             */
            smtp.Send(email);
        }
        catch (SmtpException ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }

    public bool ManagerExist()
    {
        return GetManagerEmail() != null && GetManagerPassWord() != null && GetManagerPassWord() != "" && GetManagerEmail() != "";
    }

    public void CreateManager(string email, string password)
    {
        if (!IsEmailValid(email)) // check if email is valid
            throw new BlBadEmailException("Email format is not valid");
        SetManagerEmail(email);
        SetManagerPassWord(password);
    }

    public bool ManagerLogIn(string password)
    {
        return GetManagerPassWord() == password;
    }

    public void ResetManager()
    {
        SetManagerEmail("");
        SetManagerPassWord("");
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


    public void Reset(bool wothManager)
    {
        _dal.Manager.Reset(wothManager);
    }
}


