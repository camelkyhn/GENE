using System.Threading.Tasks;
using Gene.Middleware.Bases;
using Gene.Middleware.System;

namespace Gene.Business.IServices
{
    public interface ICRUDService<in TKey, in TDto, in TFilter, TViewModel> where TDto : BaseDto<TKey> where TFilter : BaseFilter where TViewModel : BaseViewModel
    {
        Task<Result<TViewModel>> GetAsync(TKey id);
        Task<Result<TViewModel>> GetListAsync(TFilter filter);
        Task<Result<TViewModel>> CreateAsync(TDto dto);
        Task<Result<TViewModel>> UpdateAsync(TDto dto);
        Task<Result<TViewModel>> DeleteAsync(TKey id);
    }
}