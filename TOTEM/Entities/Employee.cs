using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entities
{
    /// <summary>
    /// Classe responsável pelo modelo da entidade Employee (Integrante).
    /// Modelo baseado em dados da view master do HR SCHEMA SAP_HR - VW_DOMAIN_SAP_HR.
    /// </summary>

    public class Employee
    {
        private string _login;

        public string Login
        {
            get { return _login; }
            set { _login = value; }
        }
        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private string _sap_number;

        public string Sap_number
        {
            get { return _sap_number; }
            set { _sap_number = value; }
        }

        private string _cpf;

        public string Cpf
        {
            get { return _cpf; }
            set { _cpf = value; }
        }

        private string _email;

        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }

        public Employee() { }

        /// <summary>
        /// Construtor padrão para criação de um objeto do tipo employee (integrante).
        /// Modelo baseado em dados da view master do HR do SCHEMA SAP_HR - VW_DOMAIN_SAP_HR.
        /// </summary>
        /// <param name="login">Campo do tipo VARCHAR2 na base oracle</param>
        /// <param name="name">Campo do tipo VARCHAR2 na base oracle</param>
        /// <param name="sap_number">Campo do tipo VARCHAR2 na base oracle</param>
        /// <param name="email">Campo do tipo VARCHAR2 na base oracle</param>
        public Employee(string login, string name, string sap_number, string email)
        {
            this.Login = login;
            this.Name = name;
            this.Sap_number = sap_number;
            this.Email = email;
        }

    }
}
