using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DATA
{
    /// <summary>
    /// Classe repsonsável por retornar uma string de conexão com a base do humanresources
    /// </summary>
    public class ConnectionString
    {
        /// <summary>
        /// Construtor padrão
        /// </summary>
        public ConnectionString()
        {
        }

        /// <summary>
        /// Método estático que retorna uma conexão com a base oracle. 
        /// Aqui deve ser configurada a string de conexão da aplicação e o usuário de acesso.
        /// </summary>
        public static string getConnection
        {
            get
            {
                return "Provider=OraOLEDB.Oracle;Data Source=(DESCRIPTION=(CID=GTU_APP)(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=LDCDB01)(PORT= 1521)))(CONNECT_DATA=(SID=CRP)(SERVER=DEDICATED)));User Id=F_GL_TOTEM_DM;Password=fgltotemdmENSora2014";
            }
        }
    }
}
