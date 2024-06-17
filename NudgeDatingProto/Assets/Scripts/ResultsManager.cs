using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;

public class ResultsManager : MonoBehaviour
{
    public static ResultsManager instance;
    
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI UITitleTextObject;
    [SerializeField] private TextMeshProUGUI UISubTitleTextObject;
    [SerializeField] private TextMeshProUGUI UIInfoTextObject;
    [SerializeField][TextArea(0, 40)] private string UITitleText;
    [SerializeField][TextArea(0, 40)] private string UISubTitleText;
    [SerializeField][TextArea(0, 40)] private string UIInfoText;
    [SerializeField] private TextMeshProUGUI UIContactTagTextObject;
    [SerializeField] private TextMeshProUGUI UIDatingPoolTagTextObject;

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
                <title>clickedy.click @CPDP.ai</title>
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

    [Header("Text Placeholders")]
    [SerializeField] private string userIdPlaceholder;
    [SerializeField] private string userEmailPlaceholderUser;
    [SerializeField] private string userEmailPlaceholderAction;
    [SerializeField] private string matchedIdPlaceholder;
    [SerializeField] private string matchedEmailPlaceholder;
    [SerializeField] private string actionNamePlaceholder;

    [Header("User Mail")]
    private string userToAdress;
    private string userID;
    private MailAddress clickedyMailAdressDating;
    [SerializeField] private string clickedyAddressDating = "your_email_address";
    [SerializeField] private string clickedyDisplayNameDating = "your_display_name";
    [SerializeField] private string userMailSubject = "Email subject";
    [SerializeField][TextArea(0, 40)] private string userMailOpeningParagraph;
    [SerializeField][TextArea(0, 40)] private string userMailConnectOptionParagraph;
    [SerializeField][TextArea(0, 40)] private string userMailDatingPoolOptionParagraph;
    [SerializeField][TextArea(0, 40)] private string userMailClosingParagraph;
    private string emailBodyUser;

    [Header("Matched Mail")]
    private string matchedToAdress;
    private string matchedID;
    [SerializeField] private string matchedMailSubject = "Email subject";
    [SerializeField][TextArea(0, 40)] private string matchedMailAllParagraph;
    private string emailBodyMatched;

    [Header("Action Mail")]
    private string actionToAdress;
    private string actionName;
    private MailAddress clickedyMailAdressAction;
    [SerializeField] private string clickedyAddressAction = "your_email_address";
    [SerializeField] private string clickedyDisplayNameAction = "your_display_name";
    [SerializeField] private string actionMailSubject = "Email subject";
    [SerializeField][TextArea(0, 40)] private string actionMailAllParagraph;
    private string emailBodyAction;


    private RouterManager routerManager;
    private NewDataManager dataManager;

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

    private void Start()
    {
        //TestMailOnStart();
    }

    private void TestMailOnStart()
    {
        //Test email
        routerManager = RouterManager.instance;
        dataManager = NewDataManager.instance;

        matchedID = "123456";
        userID = "987654";
        matchedToAdress = "hey@leonvanoldenborgh.nl";

        clickedyMailAdressDating = new MailAddress(clickedyAddressDating, clickedyDisplayNameDating);
        clickedyMailAdressAction = new MailAddress(clickedyAddressAction, clickedyDisplayNameAction);

        emailBodyUser = "";
        emailBodyMatched = "";
        emailBodyAction = "";

        UITitleTextObject.text = ReplacePlaceholders(UITitleText);
        UISubTitleTextObject.text = ReplacePlaceholders(UISubTitleText);
        UIInfoTextObject.text = ReplacePlaceholders(UIInfoText);

        UIContactTagTextObject.text = $"I want to get into contact with user #{matchedID}";
        UIDatingPoolTagTextObject.text = $"I want to get added to the clickedy.click dating pool as user #{userID}";
    }

    public void Initialise()
    {
        routerManager = RouterManager.instance;
        dataManager = NewDataManager.instance;

        matchedID = dataManager.matchedProfileData.id.Substring(dataManager.matchedProfileData.id.Length - 6);
        userID = dataManager.ownProfileData.id.Substring(dataManager.matchedProfileData.id.Length - 6);
        matchedToAdress = dataManager.matchedProfileData.email;

        clickedyMailAdressDating = new MailAddress(clickedyAddressDating, clickedyDisplayNameDating);
        clickedyMailAdressAction = new MailAddress(clickedyAddressAction, clickedyDisplayNameAction);

        emailBodyUser = "";
        emailBodyMatched = "";
        emailBodyAction = "";

        UITitleTextObject.text = ReplacePlaceholders(UITitleText);
        UISubTitleTextObject.text = ReplacePlaceholders(UISubTitleText);
        UIInfoTextObject.text = ReplacePlaceholders(UIInfoText);

        UIContactTagTextObject.text = $"I want to get into contact with user #{matchedID}";
        UIDatingPoolTagTextObject.text = $"I want to get added to the clickedy.click dating pool as user #{userID}";
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
        userToAdress = emailFieldMain.text;
        if (!IsValidEmail(userToAdress)) { return; }

        if(!contactToggle.isOn && !datingPoolToggle.isOn) { return; }

        emailBodyUser += htmlStart;

        emailBodyUser += userMailOpeningParagraph;

        if (contactToggle.isOn)
        {
            emailBodyUser += userMailConnectOptionParagraph; 
        }

        if (datingPoolToggle.isOn)
        {
            emailBodyUser += userMailDatingPoolOptionParagraph;

            dataManager.ownProfileData.email = userToAdress;

            //dataManager.SaveUserDataLocal(dataManager.ownProfileData);

            SendMatchedMail();
        }

        emailBodyUser += userMailClosingParagraph;

        emailBodyUser += htmlEnd;

        string finalUserMailText = ReplacePlaceholders(emailBodyUser);

        MailAddress toMailAdress = new MailAddress(userToAdress);

        MailMessage message = new MailMessage(clickedyMailAdressDating, toMailAdress);
        message.IsBodyHtml = true;
        message.Subject = userMailSubject;
        message.Body = finalUserMailText;
        Debug.Log(finalUserMailText);
        SmtpClient smtpClient = new SmtpClient("mail.antagonist.nl", 587);
        smtpClient.EnableSsl = true;
        smtpClient.Credentials = new NetworkCredential("clickedy.click@leonvanoldenborgh.nl", "wildclicker");
        smtpClient.Send(message);

        //Reset input field and toggles
        contactToggle.isOn = false;
        datingPoolToggle.isOn = false;
        emailFieldMain.text = "Email sent succesfully!";
        emailBodyUser = "";
    }

    public void SendMatchedMail()
    {
        if (!IsValidEmail(matchedToAdress)) { return; }

        emailBodyMatched += htmlStart;

        emailBodyMatched += matchedMailAllParagraph;

        emailBodyMatched += htmlEnd;

        string finalMatchedMailText = ReplacePlaceholders(emailBodyMatched);

        MailAddress toMailAdress = new MailAddress(matchedToAdress);

        MailMessage message = new MailMessage(clickedyMailAdressDating, toMailAdress);
        message.IsBodyHtml = true;
        message.Subject = matchedMailSubject;
        message.Body = finalMatchedMailText;
        Debug.Log(finalMatchedMailText);
        SmtpClient smtpClient = new SmtpClient("mail.antagonist.nl", 587);
        smtpClient.EnableSsl = true;
        smtpClient.Credentials = new NetworkCredential("clickedy.click@leonvanoldenborgh.nl", "wildclicker");
        smtpClient.Send(message);
        emailBodyMatched = "";
    }

    public void OnConfirmAction()
    {
        actionToAdress = emailFieldAction.text;
        actionName = nameFieldAction.text;
        if (!IsValidEmail(actionToAdress)) { return; }

        emailBodyAction += htmlStart;

        emailBodyAction += actionMailAllParagraph;

        emailBodyAction += htmlEnd;

        string finalActionMailText = ReplacePlaceholders(emailBodyAction);

        MailAddress toMailAdress = new MailAddress(actionToAdress);

        MailMessage message = new MailMessage(clickedyMailAdressAction, toMailAdress);
        message.IsBodyHtml = true;
        message.Subject = actionMailSubject;
        message.Body = finalActionMailText;
        Debug.Log(finalActionMailText);
        SmtpClient smtpClient = new SmtpClient("mail.antagonist.nl", 587);
        smtpClient.EnableSsl = true;
        smtpClient.Credentials = new NetworkCredential("clickedy.action@leonvanoldenborgh.nl", "wildclicker");
        smtpClient.Send(message);

        emailFieldAction.text = "Email sent succesfully!";
        nameFieldAction.text = "...";
        emailBodyAction = "";
    }

    private bool IsValidEmail(string email)
    {
        string pattern = @"^\s*[a-zA-Z0-9._%+-]+\s*@\s*[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}\s*$";
        return Regex.IsMatch(email, pattern);
    }

    private string ReplacePlaceholders(string inputText)
    {
        string outputText;
        outputText = inputText.Replace(userIdPlaceholder, userID);
        outputText = outputText.Replace(userEmailPlaceholderUser, userToAdress);
        outputText = outputText.Replace(userEmailPlaceholderAction, actionToAdress);
        outputText = outputText.Replace(matchedIdPlaceholder, matchedID);
        outputText = outputText.Replace(matchedEmailPlaceholder, matchedToAdress);
        outputText = outputText.Replace(actionNamePlaceholder, actionName);
        return outputText;
    }

}
