using NerdStore.Catalogo.Domain.Events;
using NerdStore.Core.Bus;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NerdStore.Catalogo.Domain
{
    public class EstoqueService : IEstoqueService
    {
        private readonly IProdutoRepository _produtoRepository;
        private readonly IMediatorHandler _mediatorHandler;
        public EstoqueService(IProdutoRepository produtoRepository, IMediatorHandler mediatorHandler)
        {
            _produtoRepository = produtoRepository;
            _mediatorHandler = mediatorHandler;
        }
        public async Task<bool> DebitarEstoque(Guid produtoId, int quantidade)
        {
            var produto = await _produtoRepository.ObterPorId(produtoId);

            if (produto is null)
                return false;

            if (!produto.PossuiEstoque(quantidade))
                return false;

            produto.DebitarEstoque(quantidade);

            if (produto.QuantidadeEstoque < 10)
                await _mediatorHandler.PublicarEvento(new ProdutoAbaixoEstoqueEvent(produto.Id, produto.QuantidadeEstoque));

            _produtoRepository.Atualizar(produto);

            return await _produtoRepository.UnitOfWork.Commit();
        }

        public async Task<bool> ReporEstoque(Guid produtoId, int quantidade)
        {
            var produto = await _produtoRepository.ObterPorId(produtoId);

            if (produto is null)
                return false;

            produto.ReporEstoque(quantidade);

            return await _produtoRepository.UnitOfWork.Commit();
        }

        public void Dispose()
        {
            _produtoRepository.Dispose();
        }
    }
}