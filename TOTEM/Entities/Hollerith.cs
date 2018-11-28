using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entities
{

    /// <summary>
    /// Classe responsável pelo modelo da entidade hollerith (demonstrativo).   
    /// Modelo baseado na tabela DM_FILES SCHEMA HUMANRESOURCES.
    /// </summary>
    public class Hollerith
    {
        public Hollerith() { }

        private int id;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        private string _cod_sap;

        public string Cod_sap
        {
            get { return _cod_sap; }
            set { _cod_sap = value; }
        }

        private string _month;

        public string Month
        {
            get { return _month; }
            set { _month = value; }
        }

        private string _year;

        public string Year
        {
            get { return _year; }
            set { _year = value; }
        }

        private string _type;

        public string Type
        {
            get { return _type; }
            set { _type = value; }
        }

        private string _path;

        public string Path
        {
            get { return _path; }
            set { _path = value; }
        }

        /// <summary>
        /// Construtor padrão para criação de um objeto hollerith (demonstrativo).
        /// Modelo baseado na tabela DM_FILES SCHEMA HUMANRESOURCES.
        /// </summary>
        /// <param name="id">campo do tipo number na base oracle</param>
        /// <param name="cod_sap">campo do tipo VARCHAR2 na base oracle</param>
        /// <param name="month">campo do tipo VARCHAR2 na base oracle</param>
        /// <param name="year">campo do tipo VARCHAR2 na base oracle</param>
        /// <param name="type">campo do tipo VARCHAR2 na base oracle</param>
        /// <param name="path">campo do tipo VARCHAR2 na base oracle</param>
        public Hollerith(int id, string cod_sap, string month, string year, string type, string path)
        {
            this.Id = id;
            this.Cod_sap = cod_sap;
            this.Month = month;
            this.Year = year;
            this.Type = type;
            this.Path = path;
        }
    }
}
