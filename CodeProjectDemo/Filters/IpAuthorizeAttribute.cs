using CodeProjectDemo.Models.Exceptions;
using CodeProjectDemo.Models.IP;
using System;
using System.IO;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace CodeProjectDemo.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class IpAuthorizeAttribute : AuthorizationFilterAttribute
    {
        private string[] AllowedSingleIPs { get; set; }

        private readonly IPList _allowedIPListToCheck = new IPList();
        private readonly string _filePath;

        public IpAuthorizeAttribute(string filePath)
        {
            _filePath = filePath;
        }
        public override void OnAuthorization(HttpActionContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }

            if (File.Exists(_filePath))
            {
                AllowedSingleIPs = File.ReadAllLines(_filePath);
            }
            else
            {
                return;
            }

            string userIpAddress = string.Empty;

            if (filterContext.Request.Properties.ContainsKey("MS_HttpContext"))
            {
                userIpAddress = ((HttpContextWrapper)filterContext.Request
                    .Properties["MS_HttpContext"])
                    .Request
                    .UserHostAddress;
            }
            else if (HttpContext.Current != null)
            {
                userIpAddress = HttpContext.Current.Request.UserHostAddress;

                //userIpAddress = HttpContext.Current.Request.UserHostAddress;
                //userIpAddress = HttpContext.Current.Request.UserAgent;
                //userIpAddress = HttpContext.Current.Request.Url.OriginalString;
            }            

            bool ipAllowed = false;

            try
            {
                ipAllowed = CheckAllowedIPs(userIpAddress);
            }
            catch (Exception)
            {
                // to catch
            }
            finally
            {
                if (!ipAllowed)
                    throw new ForbiddenException("Origin not allowed");
            }
        }

        private bool CheckAllowedIPs(string userIpAddress)
        {
            if (AllowedSingleIPs.Length > 0 || AllowedSingleIPs != null)
            {
                AddSingleIPs(AllowedSingleIPs, _allowedIPListToCheck);
            }
            return _allowedIPListToCheck.CheckNumber(userIpAddress);
        }

        private void AddSingleIPs(string[] ips, IPList list)
        {
            list.Clear();
            foreach (string ip in ips)
                list.Add(ip);
        }
    }
}
