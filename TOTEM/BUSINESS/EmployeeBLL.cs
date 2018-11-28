using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL;
using Entities;

namespace BUSINESS
{
    /// <summary>
    /// Classe negócio do objeto Employee.
    /// Responsável por todas as ações necessárias 
    /// para manipulação de objetos do tipo employee 
    /// que a classe de visualização precisar.
    /// </summary>
    public class EmployeeBLL
    {
        private Employee employee;

        /// <summary>
        /// Construtor padrão.
        /// </summary>
        public EmployeeBLL()
        {
            this.employee = new Employee();
        }
        
        /// <summary>
        /// Método que retorna um objeto employee a partir
        /// do número SAP do integrante. 
        /// Utilizado pela MOD (Mão de Obra Direta)
        /// </summary>
        /// <param name="userLogin">Código SAP</param>
        /// <returns>Objeto employee</returns>
        public Employee GetEmployee(string userLogin)
        {
            EmployeeDAL employeeDal = new EmployeeDAL();
            this.employee = employeeDal.getEmployeeByLogin(userLogin);
            return this.employee;
        }

        /// <summary>
        /// Método que retorna um objeto employee a partir
        /// do login de rede do integrante. 
        /// Utilizado pela MOI (Mão de Obra Indireta)
        /// </summary>
        /// <param name="userLogin">Login de rede do domínio eepsa.com.br</param>
        /// <returns>Objeto employee</returns>
        public Employee GetEmployeeAD(string userLogin)
        {
            EmployeeDAL employeeDal = new EmployeeDAL();
            this.employee = employeeDal.getEmployeeByLoginAD(userLogin);
            return this.employee;
        }

    }
}
