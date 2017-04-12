using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;



namespace BachMaiCR.Web
{
    interface IAppUser : IPrincipal
    {
        int UserId { get; set; }
        string UserName { get; set; }
        string UserLogin { get; set; }
    }



    public class AppUser : IAppUser
    {
        public IIdentity Identity { get; private set; }
        public bool IsInRole(string role) { return false; }

        public AppUser(string name)
        {
            this.Identity = new GenericIdentity(name);
        }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserLogin { get; set; }
    }

    public class AppUserPrincipalSerializeModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserLogin { get; set; }
    }
}
