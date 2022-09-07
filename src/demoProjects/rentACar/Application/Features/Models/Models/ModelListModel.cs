using Application.Features.Models.Dto;
using Core.Persistence.Paging;

namespace Application.Features.Models.Models;

public class ModelListModel : BasePageableModel
{
    public IList<ModelListDto> Items { get; set; }
}
