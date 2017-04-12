using System;
using BachMaiCR.DBMapping.Models;
using BachMaiCR.DataAccess;
using BachMaiCR.Utilities;
using BachMaiCR.Web.Common;
using BachMaiCR.Web.Serivces;

namespace BachMaiCR.Web.App_Start
{
    public class BachMaiCRConfig
    {
        public bool Initialization()
        {
            IUnitOfWork unitOfWork = new UnitOfWork();
            ITransaction transaction = null;
            try
            {
                transaction = unitOfWork.BeginTransaction();
                var admin = unitOfWork.Users.GetByUserName(WebConst.DefaultAdminAccount);
                if (admin == null)
                {
                    string salt = Encrypt.RandomString(3);
                    admin = new ADMIN_USER()
                        {
                            USERNAME = WebConst.DefaultAdminAccount,
                            USERCODE = "ROOTADMIN",
                            SALT = salt,
                            PASSWORD = Encrypt.Sha1HashWithHex(@"1233456!@#" + salt),
                            ISACTIVED = true,
                            ISDELETE = false,
                            CREATE_DATE = DateTime.Now,
                            UPDATE_DATE = DateTime.Now
                        };
                    unitOfWork.Users.Add(admin);
                }

                var service = new WebpageActionService(unitOfWork);
                service.RefreshAllWebpageAction();
                service.AddOrUpdateWebpageActionToRootAdmin(admin);

                transaction.Commit();
                return true;
            }
            catch 
            {
                if (transaction != null)
                {
                    transaction.Rollback();
                }
                return false;
            }
            finally
            {
                if (transaction != null)
                {
                    transaction.Dispose();
                }
            }
        }
    }
}