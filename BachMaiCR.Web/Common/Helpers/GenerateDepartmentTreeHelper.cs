using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI.WebControls;
using BachMaiCR.DBMapping.Models;
using BachMaiCR.Web.Models;
using BachMaiCR.DataAccess;
using BachMaiCR.DBMapping.ModelsExt;
namespace BachMaiCR.Web.Common.Helpers
{
    public static class GenerateDepartmentTreeHelper
    {
        public static List<LM_DEPARTMENT> GetRootDepartment( int parrentId)
        {
            UnitOfWork unit = new UnitOfWork();
            List<LM_DEPARTMENT> listDepartment = unit.Departments.GetChildDepartment(parrentId);
            unit.Dispose();
            return listDepartment;
        }

        public static bool Check(int id)
        {
            UnitOfWork unit = new UnitOfWork();
            return unit.Departments.ExistChild(id);
        }
        /// <summary>
        /// PhuongLt15:12/05/2014 => Danh sách cây phòng ban theo cây cha
        /// </summary>
        /// <param name="parrentId"></param>
        /// <returns></returns>
        public static string GetStringDepartment(int parrentId)
        {
            var vR = GenerateNoteAll(parrentId);
            return vR;
        }

        public static string GenerateNoteAll(int parrentId)
        {
            UnitOfWork unit = new UnitOfWork();
            LM_DEPARTMENT rootDepartment = unit.Departments.GetById(parrentId);
            string vR = "[{";
            vR += "id:" + rootDepartment.LM_DEPARTMENT_ID + ",text:'" + HttpUtility.HtmlAttributeEncode(rootDepartment.DEPARTMENT_NAME) + "'";
            vR += GenerateNote(rootDepartment.LM_DEPARTMENT_ID, unit);
            vR += "}]";
            unit.Dispose();
            return vR;
        }
        public static string GenerateNote(int departmentId, UnitOfWork unit)
        {
            List<LM_DEPARTMENT> listDepartment = unit.Departments.GetChildDepartment(departmentId);
            string vR = "";
            if (listDepartment.Count > 0)
            {
                vR += ",children: [";
                for (int i = 0; i < listDepartment.Count; i++)
                {

                    if (listDepartment[i].DEPARTMENT_PARENT_ID == departmentId)
                    {
                        if (i == 0)
                        {
                            vR += "{id:" + listDepartment[i].LM_DEPARTMENT_ID + ",text:'" + HttpUtility.HtmlAttributeEncode(listDepartment[i].DEPARTMENT_NAME) + "'";
                        }
                        else
                        {
                            vR += ",{id:" + listDepartment[i].LM_DEPARTMENT_ID + ",text:'" + HttpUtility.HtmlAttributeEncode(listDepartment[i].DEPARTMENT_NAME) + "'";
                        }
                        vR += GenerateNote(listDepartment[i].LM_DEPARTMENT_ID, unit);
                        vR += "}";
                    }
                    if (i == listDepartment.Count - 1)
                    {
                        vR += "]";
                    }
                }
                listDepartment = null;
            }
            return vR;
        }
        /// <summary>
        /// Lấy danh sách các đơn vị con bởi đơn vị cha
        /// </summary>
        /// <param name="parrentId"></param>
        /// <returns></returns>
        public static string GenerateDeparmentTree(int parrentId)
        {
            UnitOfWork unit = new UnitOfWork();
            LM_DEPARTMENT rootDepartment = unit.Departments.GetById(parrentId);
            string vR = ",";
            vR += rootDepartment.LM_DEPARTMENT_ID;
            GenerateDeparmentChild(rootDepartment.LM_DEPARTMENT_ID, unit, ref vR);           
            unit.Dispose();
            return vR;
        }
        public static void GenerateDeparmentChild(int departmentId, UnitOfWork unit, ref string vR)
        {
            List<LM_DEPARTMENT> listDepartment = unit.Departments.GetChildDepartment(departmentId);
            if (listDepartment.Count > 0)
            {

                for (int i = 0; i < listDepartment.Count; i++)
                {
                    if (listDepartment[i].DEPARTMENT_PARENT_ID == departmentId)
                    {
                        int deparmentParentId = listDepartment[i].LM_DEPARTMENT_ID;
                        vR += "," + deparmentParentId;
                        GenerateDeparmentChild(deparmentParentId, unit, ref vR);
                    }

                }
                listDepartment = null;
            }
        }
        ///phuong
        public static List<DEPARTMENTLIST> GenerateDepartmentAll(int parrentId)
        {
            UnitOfWork unit = new UnitOfWork();
            LM_DEPARTMENT rootDepartment = unit.Departments.GetById(parrentId);
            List<DEPARTMENTLIST> listItem = new List<DEPARTMENTLIST>();
            DEPARTMENTLIST item = new DEPARTMENTLIST();
            item.LM_DEPARTMENT_ID = rootDepartment.LM_DEPARTMENT_ID;
            item.DEPARTMENT_NAME = HttpUtility.HtmlAttributeEncode(rootDepartment.DEPARTMENT_NAME);
            listItem.Add(item);
            string vR = "";
            GenerateDepartment(rootDepartment.LM_DEPARTMENT_ID, unit, ref listItem, ref vR);
            item = null;
            return listItem;           
        }
        public static void GenerateDepartment(int departmentId, UnitOfWork unit, ref List<DEPARTMENTLIST> listItem, ref string vR)
        {
            List<LM_DEPARTMENT> listDepartment = unit.Departments.GetChildDepartment(departmentId);          
            if (listDepartment.Count > 0)          
            {              
                for (int i = 0; i < listDepartment.Count; i++)
                {

                    if (listDepartment[i].DEPARTMENT_PARENT_ID == departmentId)
                    {
                        vR += "    ";
                        DEPARTMENTLIST item = new DEPARTMENTLIST();
                        item.LM_DEPARTMENT_ID = listDepartment[i].LM_DEPARTMENT_ID;
                        item.DEPARTMENT_NAME = vR + "" + HttpUtility.HtmlAttributeEncode(listDepartment[i].DEPARTMENT_NAME);
                        listItem.Add(item);
                        item = null;
                    }                   
                }
                listDepartment = null;
            }           
        }
    }
}