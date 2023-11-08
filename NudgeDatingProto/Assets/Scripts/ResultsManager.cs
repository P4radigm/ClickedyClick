using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using UnityEngine.Networking;
using UnityEngine.Windows;
using Unity.VisualScripting;

public class ResultsManager : MonoBehaviour
{
    public static ResultsManager instance;

    [SerializeField] private TextMeshProUGUI titleTextObject;
    [SerializeField] private TextMeshProUGUI thanksTextObjectOne;
    [SerializeField] private TextMeshProUGUI thanksTextObjectTwo;
    [SerializeField][TextArea(0, 40)] private string titleText;
    [SerializeField][TextArea(0, 40)] private string thanksTextOne;
    [SerializeField][TextArea(0, 40)] private string thanksTextTwo;
    [SerializeField] private TextMeshProUGUI contactText;
    [SerializeField] private TextMeshProUGUI datingPoolText;

    [SerializeField] private TMP_InputField emailFieldMain;
    [SerializeField] private Toggle contactToggle;
    [SerializeField] private GameObject contactCheckedGraphic;
    [SerializeField] private GameObject contactUncheckedGraphic;
    [SerializeField] private Toggle datingPoolToggle;
    [SerializeField] private GameObject datingPoolCheckedGraphic;
    [SerializeField] private GameObject datingPoolUncheckedGraphic;

    [SerializeField] private TMP_InputField emailFieldAction;
    [SerializeField] private TMP_InputField nameFieldAction;

    private string htmlStart = @"
        <html>
            <head>
                <title>clickedy.click @ IMPAKT</title>
                  <style>
                    body {
                        font-family: Arial, Helvetica, sans-serif;
                         }
                  </style>
            </head>
            <body>
            <pre>";
    private string htmlEnd = @"</pre>
            </body>
        </html>
    ";

    [SerializeField] private string ownIdPlaceholder;
    [SerializeField] private string ownEmailPlaceholderMain;
    [SerializeField] private string ownEmailPlaceholderAction;
    [SerializeField] private string otherIdPlaceholder;
    [SerializeField] private string otherEmailPlaceholder;
    [SerializeField] private string namePlaceholder;

    [SerializeField] private string fromAddressMain = "your_email_address";
    [SerializeField] private string fromDisplayName = "your_display_name";
    private MailAddress fromMailAdressMain;
    private MailAddress fromMailAdressAction;
    private string toAddressMain;
    private string toAddressOther;
    private string toAddressAction;
    private string nameAction;

    [SerializeField] private string subjectMain = "Email subject";
    [SerializeField][TextArea(0, 40)] private string thanksParagraph;
    [SerializeField][TextArea(0, 40)] private string connectParagraph;
    [SerializeField][TextArea(0, 40)] private string addedParagraph;
    [SerializeField][TextArea(0, 40)] private string endingParagraph;
    [SerializeField] private string subjectOther = "Email subject";
    [SerializeField][TextArea(0, 40)] private string connectedEmail;


    [SerializeField] private string fromAddressAction = "your_email_address";
    [SerializeField] private string subjectAction = "Email subject";
    [SerializeField][TextArea(0, 40)] private string actionParagraph;

    private string matchedUserID;


    private RouterManager routerManager;
    private NewDataManager dataManager;

    private string emailBodyMain;
    private string emailBodyOther;
    private string emailBodyAction;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Initialise()
    {
        routerManager = RouterManager.instance;
        dataManager = NewDataManager.instance;

        matchedUserID = dataManager.matchedProfileData.id.Substring(dataManager.matchedProfileData.id.Length - 6);

        fromMailAdressMain = new MailAddress(fromAddressMain, fromDisplayName);
        fromMailAdressAction = new MailAddress(fromAddressAction, fromDisplayName);

        emailBodyMain = "";
        emailBodyOther = "";
        emailBodyAction = "";

        string modifiedTitleText = titleText.Replace(ownIdPlaceholder, dataManager.ownProfileData.id.Substring(dataManager.ownProfileData.id.Length - 6));
        titleTextObject.text = modifiedTitleText.Replace(otherIdPlaceholder, matchedUserID);

        string modifiedThanksTextOne = thanksTextOne.Replace(ownIdPlaceholder, dataManager.ownProfileData.id.Substring(dataManager.ownProfileData.id.Length - 6));
        thanksTextObjectOne.text = modifiedThanksTextOne.Replace(otherIdPlaceholder, matchedUserID);
        
        string modifiedThanksTextTwo = thanksTextTwo.Replace(ownIdPlaceholder, dataManager.ownProfileData.id.Substring(dataManager.ownProfileData.id.Length - 6));
        thanksTextObjectTwo.text = modifiedThanksTextTwo.Replace(otherIdPlaceholder, matchedUserID);

        contactText.text = $"I want to get into contact with user #{matchedUserID}";
        datingPoolText.text = $"I want to get added to the clickedy.click dating pool as user #{dataManager.ownProfileData.id.Substring(dataManager.ownProfileData.id.Length - 6)}";
    }

    public void CheckToggleGraphics()
    {
        contactCheckedGraphic.SetActive(contactToggle.isOn);
        contactUncheckedGraphic.SetActive(!contactToggle.isOn);
        datingPoolCheckedGraphic.SetActive(datingPoolToggle.isOn);
        datingPoolUncheckedGraphic.SetActive(!datingPoolToggle.isOn);
    }

    public void OnConfirmMainLocal()
    {
        toAddressMain = emailFieldMain.text;
        if (!IsValidEmail(toAddressMain)) { return; }

        if(!contactToggle && !datingPoolText) { return; }

        emailBodyMain += htmlStart;

        emailBodyMain += thanksParagraph;

        if (contactToggle.isOn)
        {
            emailBodyMain += connectParagraph; 
        }

        if (datingPoolToggle.isOn)
        {
            emailBodyMain += addedParagraph;

            dataManager.ownProfileData.email = toAddressMain;

            dataManager.SaveUserDataLocal(dataManager.ownProfileData);

            SendOtherMail();
        }

        emailBodyMain += htmlEnd;

        emailBodyMain = ReplacePlaceholders(emailBodyMain);

        MailAddress toMailAdress = new MailAddress(toAddressMain);

        MailMessage message = new MailMessage(fromMailAdressMain, toMailAdress);
        message.IsBodyHtml = true;
        message.Subject = subjectMain;
        message.Body = emailBodyMain;
        SmtpClient smtpClient = new SmtpClient("mail.antagonist.nl", 587);
        smtpClient.EnableSsl = true;
        smtpClient.Credentials = new NetworkCredential("clickedy.click@leonvanoldenborgh.nl", "WildClicker");
        smtpClient.Send(message);

        //Reset input field and toggles
        contactToggle.isOn = false;
        datingPoolToggle.isOn = false;
        emailFieldMain.text = "Email sent succesfully!";
    }

    public void SendOtherMail()
    {
        toAddressOther = dataManager.matchedProfileData.email;
        if (!IsValidEmail(toAddressOther)) { return; }

        emailBodyOther += htmlStart;

        emailBodyOther += addedParagraph;

        emailBodyOther += htmlEnd;

        emailBodyOther = ReplacePlaceholders(emailBodyOther);

        MailAddress toMailAdress = new MailAddress(toAddressOther);

        MailMessage message = new MailMessage(fromMailAdressMain, toMailAdress);
        message.IsBodyHtml = true;
        message.Subject = subjectOther;
        message.Body = emailBodyOther;
        SmtpClient smtpClient = new SmtpClient("mail.antagonist.nl", 587);
        smtpClient.EnableSsl = true;
        smtpClient.Credentials = new NetworkCredential("clickedy.click@leonvanoldenborgh.nl", "wildclicker");
        smtpClient.Send(message);
    }

    public void OnConfirmAction()
    {
        toAddressAction = emailFieldAction.text;
        nameAction = nameFieldAction.text;
        if (!IsValidEmail(toAddressAction)) { return; }

        if (!contactToggle && !datingPoolText) { return; }

        emailBodyAction += htmlStart;

        emailBodyAction += actionParagraph;

        emailBodyAction += htmlEnd;

        emailBodyAction = ReplacePlaceholders(emailBodyAction);

        MailAddress toMailAdress = new MailAddress(toAddressAction);

        MailMessage message = new MailMessage(fromMailAdressAction, toMailAdress);
        message.IsBodyHtml = true;
        message.Subject = subjectAction;
        message.Body = emailBodyAction;
        SmtpClient smtpClient = new SmtpClient("mail.antagonist.nl", 587);
        smtpClient.EnableSsl = true;
        smtpClient.Credentials = new NetworkCredential("clickedy.action@leonvanoldenborgh.nl", "wildclicker");
        smtpClient.Send(message);

        emailFieldAction.text = "Email sent succesfully!";
        nameFieldAction.text = "...";
    }

    //public void OnConfirmNoDatabase()
    //{
    //    toAddress = emailField.text;
    //    if (!IsValidEmail(toAddress)) { return; }

    //    if (!contactToggle && !datingPoolText) { return; }

    //    emailBody += htmlStart;

    //    emailBody += emailTextStart;
    //    emailBody += dataManager.currentUserData.id.Substring(dataManager.currentUserData.id.Length - 6);

    //    emailBody += emailTextAfterID;

    //    if (contactToggle.isOn) { emailBody += emailTextContact; }

    //    if (datingPoolToggle.isOn)
    //    {
    //        emailBody += emailTextDatingPool;

    //        dataManager.currentUserData.email = toAddress;
    //    }

    //    emailBody += emailTextEnd;
    //    emailBody += htmlEnd;

    //    MailAddress toMailAdress = new MailAddress(toAddress);

    //    MailMessage message = new MailMessage(fromMailAdress, toMailAdress);
    //    message.IsBodyHtml = true;
    //    message.Subject = subject;
    //    message.Body = emailBody;
    //    SmtpClient smtpClient = new SmtpClient("mail.antagonist.nl", 587);
    //    smtpClient.EnableSsl = true;
    //    smtpClient.Credentials = new NetworkCredential("nudge.dating@leonvanoldenborgh.nl", "WildClicker");
    //    smtpClient.Send(message);

    //    //Reset input field and toggles
    //    contactToggle.isOn = false;
    //    datingPoolToggle.isOn = false;
    //    emailField.text = "Email sent succesfully!";
    //}

    private bool IsValidEmail(string email)
    {
        string pattern = @"^\s*[a-zA-Z0-9._%+-]+\s*@\s*[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}\s*$";
        return Regex.IsMatch(email, pattern);
    }

    private string ReplacePlaceholders(string inputText)
    {
        string outputText;
        outputText = inputText.Replace(ownIdPlaceholder, dataManager.ownProfileData.id.Substring(dataManager.ownProfileData.id.Length - 6));
        outputText = outputText.Replace(ownEmailPlaceholderMain, toAddressMain);
        outputText = outputText.Replace(ownEmailPlaceholderAction, toAddressAction);
        outputText = outputText.Replace(otherIdPlaceholder, dataManager.matchedProfileData.id.Substring(dataManager.ownProfileData.id.Length - 6));
        outputText = outputText.Replace(otherEmailPlaceholder, dataManager.matchedProfileData.email);
        outputText = outputText.Replace(namePlaceholder, nameAction);
        return outputText;
    }

    //public void HandleAfterUpload(UnityWebRequest.Result result)
    //{
    //    if (result != UnityWebRequest.Result.Success && result != UnityWebRequest.Result.ProtocolError)
    //    {
    //        emailBody += emailTextEnd;
    //        emailBody += htmlEnd;

    //        MailAddress toMailAdress = new MailAddress(toAddress);

    //        MailMessage message = new MailMessage(fromMailAdress, toMailAdress);
    //        message.IsBodyHtml = true;
    //        message.Subject = subject;
    //        message.Body = emailBody;
    //        SmtpClient smtpClient = new SmtpClient("mail.antagonist.nl", 587);
    //        smtpClient.EnableSsl = true;
    //        smtpClient.Credentials = new NetworkCredential("nudge.dating@leonvanoldenborgh.nl", "WildClicker");
    //        smtpClient.Send(message);

    //        //Reset input field and toggles
    //        contactToggle.isOn = false;
    //        datingPoolToggle.isOn = false;
    //        emailField.text = "Email sent succesfully!";
    //    }
    //    else
    //    {
    //        //Sucks, enable retry uploading button?
    //        //Maybe handle dirty data and wrong email exceptions seperately
    //    }
    //}
}
