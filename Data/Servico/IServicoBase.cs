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
        Task<bool> Inserir(T objeto);

        Task<bool> Excluir(T objeto);

        Task<bool> Alterar(T objeto);

        Task<ICollection<T>> ListarTodos();

        Task<T> BuscarPorId(K id);
    }
}
