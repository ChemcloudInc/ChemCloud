using ChemCloud.IServices.QueryModel;
using ChemCloud.Model;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace ChemCloud.IServices
{
    public interface IAttentionService :  IService, IDisposable
    {
        bool AddAttention(long UserId, long ShopId, long ProductId);
        bool DeleteAttention(long Id);
        PageModel<Attention> GetAttentions(AttentionQuery AttentionQueryModel);
        PageModel<Attention> SupplierGetAttentions(AttentionQuery AttentionQueryModel);
        Attention GetAttention(long Id);
    }
}
