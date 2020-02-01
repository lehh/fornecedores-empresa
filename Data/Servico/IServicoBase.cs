using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FornecedoresEmpresa.Data.Servico
{
    interface IServicoBase<T, K>
        where T : class
        where K : struct
    {
        void Inserir(T objeto);

        void Excluir(T objeto);

        void Alterar(T objeto);

        Task<ICollection<T>> ListarTodos();

        Task<T> BuscarPorId(K id);
    }
}
