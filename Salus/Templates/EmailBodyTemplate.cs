namespace Salus.Templates
{
    public class EmailBodyTemplate
    {
        public string EmailBody(string imgUrl, string username, string url)
        {
            return
                    $"<div style=\"background-color:#01a36269;\r\n" +
                    $"            height: 100%;\r\n" +
                    $"            scroll-behavior: smooth;\">\r\n" +
                    $"    <div style=\"margin-left: 20%;\r\n" +
                    $"                background-color: rgb(255, 255, 255);\r\n" +
                    $"                width: 60%;\r\n" +
                    $"                padding-bottom: 2vw;\">\r\n" +
                    $"        <img style=\"width: 50%;\r\n" +
                    $"                    margin-left: 25%;\"\r\n" +
                    $"                    src='{imgUrl}' alt=\"Logo\">\r\n" +
                    $"        <h1 style=\"text-align: center;\r\n" +
                    $"                    font-size: 4vw;\r\n" +
                    $"                    color:#01a362;\r\n" +
                    $"                    font-family: Cambria, Cochin, Georgia, Times, 'Times New Roman', serif;\">\r\n" +
                    $"                    Click the button, </br>\r\n" +
                    $"                    to verify your account!\r\n" +
                    $"        </h1>\r\n" +
                    $"        <div style=\"margin: 5vw;\">\r\n" +
                    $"            <h2 style=\"font-size: 3.5vw;\r\n" +
                    $"                        text-align: center;\">\r\n" +
                    $"                        Hi {username}!\r\n" +
                    $"            </h2>\r\n" +
                    $"            <p style=\"font-size: 2.5vw;\">\r\n" +
                    $"                Thanks for using Salus - Healthy lifestyle!\r\n" +
                    $"                Please click on the button below to confirm your email address.\r\n" +
                    $"            </p>\r\n" +
                    $"            <strong style=\"font-size: 2.5vw;\">After the verification process is complete:</strong>\r\n" +
                    $"            <ul>\r\n" +
                    $"                <li style=\"font-size: 2.5vw;\">\r\n" +
                    $"                    Please set your personal information!\r\n" +
                    $"                </li>\r\n" +
                    $"                <li style=\"font-size: 2.5vw;\">\r\n" +
                    $"                    Enjoy our app!\r\n" +
                    $"                </li>\r\n" +
                    $"            </ul>\r\n" +
                    $"            <form name=\"myForm\" target=\"_blank\" action='{url}' method=\"post\">\r\n" +
                    $"                <button style=\"font-family: Georgia, 'Times New Roman', Times, serif;\r\n" +
                    $"                            align-items: center;\r\n" +
                    $"                            background-image: linear-gradient(144deg,#01a362, #00cf7c 50%,#01a362);\r\n" +
                    $"                            border: 0;\r\n" +
                    $"                            border-radius: 20px;\r\n" +
                    $"                            color: #ffffff;\r\n" +
                    $"                            display: flex;\r\n" +
                    $"                            font-size: 2.5vw;\r\n" +
                    $"                            justify-content: center;\r\n" +
                    $"                            padding: 3px;\r\n" +
                    $"                            cursor: pointer;\r\n" +
                    $"                            margin-top: 5vw;\r\n" +
                    $"                            width: 80%;\r\n" +
                    $"                            margin-left: 10%;\" type=\"submit\" name=\"submit_param\" value=\"submit_value\">\r\n" +
                    $"                            <span class=\"text\"\r\n" +
                    $"                                  style=\"background-color: #005734;\r\n" +
                    $"                                  padding: 16px 24px;\r\n" +
                    $"                                  border-radius: 20px;\r\n " +
                    $"                                  width: 100%;\r\n" +
                    $"                                  height: 3vw;\">\r\n" +
                    $"                                  VERIFY\r\n" +
                    $"                            </span>\r\n" +
                    $"                </button>\r\n" +
                    $"            </form>\r\n" +
                    $"        </div>\r\n" +
                    $"    </div>\r\n" +
                    $"</div>";
        }
    }
}
