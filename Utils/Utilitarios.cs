using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FornecedoresEmpresa.Utils
{
    public class Utilitarios
    {
        public static int CalculaIdade(DateTime dataNascimento, DateTime dataFim)
        {
            int idade = dataFim.Year - dataNascimento.Year;

            if (dataFim.Month < dataNascimento.Month || (dataFim.Month == dataNascimento.Month && dataFim.Day < dataNascimento.Day))
                idade--;

            return idade;
        } 
    }
}
