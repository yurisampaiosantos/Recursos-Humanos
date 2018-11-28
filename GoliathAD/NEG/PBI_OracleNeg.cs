using System;
using DAL;
using System.Collections.Generic;

namespace Negocio
{
    public class PBI_OracleNeg
    {
       public void AtualizarPBI()
        {
            PBI_OracleDAL obj = new PBI_OracleDAL();
            obj.Inserir();
        }
    }
}
