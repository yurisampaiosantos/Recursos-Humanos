using DAO.benefit;
using DAO.Odeprev;
using DAO.Taxes;
using DTO.Benefit;
using DTO.Odeprev;
using DTO.Taxes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;



namespace WebServiceInternal
{
    /// <summary>
    /// Summary description for WebServiceInternal
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class WebServiceInternal : System.Web.Services.WebService
    {


        #region Autentication User

        [WebMethod]
        
        public bool AutenticationUser(string name, string pass)
        {
            bool result = false;

            UserId userid = new UserId();

            userid.Name = name;
            userid.Password = pass;

            Autentication autentication = new Autentication();

            if (autentication.userIsValid(userid))
            {
                result = true;
            }

            return result;
        }

        #endregion

        #region List Benefit



        [WebMethod]
        public List<AccidentInsurance> listAccidentInsurance()
        {
            AccidentInsuranceDAO getAccident = new AccidentInsuranceDAO();

            return getAccident.ListAccidentInsurance();

        }


        [WebMethod]
        public List<DentalInsurance> listDentalInsurance()
        {
            DentalInsuranceDAO getDental = new DentalInsuranceDAO();

            return getDental.ListDentalInsurance();

        }


        [WebMethod]
        public List<HealthInsurance> listHealtlInsurance()
        {
            HealthInsuranceDAO getHealth = new HealthInsuranceDAO();
            
            return getHealth.ListHealthInsurance();

        }

        [WebMethod]
        public List<MealVoucher> listMealVoucher()
        {
            MealVoucherDAO getMeal = new MealVoucherDAO();

            return getMeal.ListMealVoucher();

        }

        [WebMethod]
        public List<SupplementalInsurance> listSupplementalInsurance()
        {
            SupplementalInsuranceDAO getSupplementalInsurance = new SupplementalInsuranceDAO();

            return getSupplementalInsurance.ListSupplementalInsurance();

        }

        #endregion


        #region List Odeprev

        [WebMethod]
        public List<PrivatePensionAge> listPrivatePensionAge()
        {
            PrivatePensionAgeDAO getPrivatePensionAge = new PrivatePensionAgeDAO();

            return getPrivatePensionAge.ListPrivatePensionAge();

        }


        [WebMethod]
        public List<PrivatePensionSalary> listPrivatePensionSalary()
        {
            PrivatePensionSalaryDAO getPrivatePensionSalary = new PrivatePensionSalaryDAO();

            return getPrivatePensionSalary.ListPrivatePensionSalary();

        }


        [WebMethod]
        public List<PrivatePensionYrsSrv> listPrivatePensionYrsSrv()
        {
            PrivatePensionYrsSrvDAO getPrivatePensionYrsSrv = new PrivatePensionYrsSrvDAO();

            return getPrivatePensionYrsSrv.ListPrivatePensionYrsSrv();

        }
        

        #endregion


        #region List Taxes

        [WebMethod]
        public List<Inss> listInss()
        {
            InssDAO getInss = new InssDAO();

            return getInss.ListInss();

        }

        [WebMethod]
        public List<Ir> listIr()
        {
            IrDAO getIr = new IrDAO();

            return getIr.ListIr();

        }

        [WebMethod]
        public List<Salary> listSalary()
        {
            SalaryDAO getSalary = new SalaryDAO();

            return getSalary.ListSalary();

        }
        

        #endregion


    }




}
