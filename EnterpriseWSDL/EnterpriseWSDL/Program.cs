using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using EnterpriseWSDL.com.salesforce.enterprise;
namespace EnterpriseWSDL
{
    class Program
    {
        static void Main(string[] args)
        {
            SforceService SfdcBinding = null;
            LoginResult CurrentLoginResult = null;
            SfdcBinding = new SforceService();
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                CurrentLoginResult = SfdcBinding.login(Constants.USERNAME, Constants.PASSWORD + Constants.TOKEN);
                SfdcBinding.Url = CurrentLoginResult.serverUrl;
                SfdcBinding.SessionHeaderValue = new SessionHeader();
                SfdcBinding.SessionHeaderValue.sessionId = CurrentLoginResult.sessionId;
                QueryResult queryResult = null;
                String SOQL = "";
                SOQL = "select FirstName, LastName, Phone from Lead LIMIT 10";
                queryResult = SfdcBinding.query(SOQL);
                if (queryResult.size > 0)
                {
                    for (int i = 0; i < queryResult.records.Length; i++)
                    {
                        Lead lead = (Lead)queryResult.records[i];
                        string firstName = lead.FirstName;
                        string lastName = lead.LastName;
                        string businessPhone = lead.Phone;
                        Console.WriteLine("First Name " + firstName + ", Last Name " + lastName + ", Phone " + businessPhone);
                    }
                }
                else
                {
                    Console.WriteLine("No records returned.");
                }
            }
            catch (System.Web.Services.Protocols.SoapException e)
            {
                // This is likley to be caused by bad username or password
                SfdcBinding = null;
                Console.WriteLine("SOAP Exception occured " + e.Message.ToString());
            }
            catch (Exception e)
            {
                // This is something else, probably comminication
                SfdcBinding = null;
                Console.WriteLine("Exception occured " + e.Message.ToString());
            }
        }
    }
}