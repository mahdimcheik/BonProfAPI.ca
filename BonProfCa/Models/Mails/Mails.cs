namespace BonProfCa.Models;

public class MailApp
{
    public string? MailTo { get; set; }
    public string? MailSubject { get; set; }
    public string? MailBody { get; set; }
    public string? MailFrom { get; set; }
    public bool SendCopy { get; set; } = false;
    public UserApp? Sender { get; set; }
    public UserApp? Receiver { get; set; }

}

public class ResetPasswordModel
{
    public UserApp Receiver { get; set; }
    public string ValidationLink { get; set; }
    public string WebsiteLink { get; set; }

    public ResetPasswordModel(UserApp receiver, string resetLink, string websiteLink)
    {
        Receiver = receiver;
        ValidationLink = resetLink;
        WebsiteLink = websiteLink;
    }
}

public class ConfirmMailModel
{
    public UserApp Receiver { get; set; }
    public string ConfirmLink { get; set; }
    public string WebsiteLink { get; set; }

    public ConfirmMailModel(UserApp receiver, string confirmLink, string websiteLink)
    {
        Receiver = receiver;
        ConfirmLink = confirmLink;
        WebsiteLink = websiteLink;
    }
}

public class ReminderModel
{
    public UserApp Teacher { get; set; }
    public UserApp Student { get; set; }
    public string WebsiteLink { get; set; }
    public bool forTeacher { get; set; } = true;

    public ReminderModel(UserApp teacher, UserApp student,  string websiteLink)
    {
        Teacher = teacher;
        Student = student;
        WebsiteLink = websiteLink;
    }
}