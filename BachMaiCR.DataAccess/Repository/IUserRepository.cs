using BachMaiCR.DBMapping.Models;
using BachMaiCR.Utilities;
using System.Collections.Generic;

namespace BachMaiCR.DataAccess.Repository
{
    public interface IUserRepository : IRepository<ADMIN_USER>
    {
        IEnumerable<string> GetActionCodesByUserId(int userId);

        IEnumerable<string> GetActionCodesByUserName(string userName);

        PagedList<ADMIN_USER> GetAll(int page, int size, string sort, string sortDir, bool isdelete);

        PagedList<ADMIN_USER> GetAll(int page, int size, string sort, string sortDir, bool isdelete, List<int> departments);

        List<ADMIN_USER> GetAll(List<int> departments, string key, int currentUserId, int createdUserId);

        ADMIN_USER GetByUserName(string userName);

        ADMIN_USER GetByUserCode(string userCode);

        PagedList<ADMIN_USER> SearchAllByUserName(string key, int page, int size, string sort, string sortDir);

        PagedList<ADMIN_USER> SearchAllByUserName(string key, int page, int size, string sort, string sortDir, List<int> departments);

        List<ADMIN_USER> GetAll(string sort, string sortDir);

        bool CheckUserExit(string userName);

        int Count();

        string GetUrlActive(string UserName);

        bool ExistReferenceUser(int usrID);

        bool ExistReferenceDepartment(int deprtID);

        void UpdateUserByDoctorId(int doctorId, bool active);
    }
}