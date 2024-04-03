namespace BlImplementation;
using BO;
using System.Data;
using System.Net;
using System.Net.Mail;


internal class ManagerImplementation : BlApi.IManager
{
    private DalApi.IDal _dal = Factory.Get;

    public String GetManagerEmail() => _dal.manager.GetManagerEmail();

    public String GetManagerPassWord() => _dal.manager.GetManagerPassWord();

    public void SetManagerEmail(String managerEmail) => _dal.manager.SetManagerEmail(managerEmail);

    public void SetManagerPassWord(String managerPassword) => _dal.manager.SetManagerPassWord(managerPassword);

    public void SendEmail(int randomNumber)
    {
        MailAddress to = new MailAddress(GetManagerEmail());

        MailAddress from = new MailAddress("kayamutmaoz@gmail.com");

        MailMessage email = new MailMessage(from, to);
        email.Subject = "Verification code to reset password - Project management ";
        email.Body = "Your verification code is: " + randomNumber; // the message that will be sent to the user

        SmtpClient smtp = new SmtpClient();
        smtp.Host = "smtp.gmail.com"; // mail server
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
        return GetManagerEmail() != null && GetManagerPassWord() != null;
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

}


