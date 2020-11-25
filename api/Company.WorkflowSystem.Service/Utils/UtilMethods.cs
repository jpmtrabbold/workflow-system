using System;
using System.Collections.Generic;
using System.Text;

namespace Company.WorkflowSystem.Service.Utils
{
    public static class UtilMethods
    {
        public static string GenerateHtmlLinkButtons(List<(string urlSuffix, string buttonName)> links, string baseUrl)
        {
            var container = @"
                <table width='100%' border='0' cellspacing='0' cellpadding='0'>
                  <tr>
                    <td>
                      <table border='0' cellspacing='0' cellpadding='0'>
                        <tr>
                          {links}
                        </tr>
                      </table>
                    </td>
                  </tr>
                </table>
            ";

            var linksText = "";
            foreach (var link in links)
            {
                var url = CombineUrl(baseUrl, link.urlSuffix);

                var linkText = @"
                    <td align='center' style='border-radius: 3px;' bgcolor='#2a7ddd'>
                        <a href='{url}' target='_blank' style='font-size: 16px; font-family: Helvetica, Arial, sans-serif; color: #ffffff; text-decoration: none; text-decoration: none;border-radius: 3px; padding: 12px 18px; border: 1px solid #2a7ddd; display: inline-block;'>
                            {buttonName}
                        </a>
                    </td>
                ";

                linkText = linkText.Replace("{url}", url);
                linkText = linkText.Replace("{buttonName}", link.buttonName);

                if (!string.IsNullOrWhiteSpace(linksText))
                    linksText += "<td style='width: 16px'></td>";

                linksText += linkText;
            }
            container = container.Replace("{links}", linksText);

            return container;
        }

        public static string CombineUrl(string uri1, string uri2)
        {
            uri1 = uri1.TrimEnd('/');
            uri2 = uri2.TrimStart('/');
            return string.Format("{0}/{1}", uri1, uri2);
        }
    }
}
