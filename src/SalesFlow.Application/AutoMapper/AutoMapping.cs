using AutoMapper;
using SalesFlow.Application.AutoMapper.Helpers;
using SalesFlow.Communication.Request.Sales;
using SalesFlow.Communication.Response.Sales;
using SalesFlow.Domain.Entities;

namespace SalesFlow.Application.AutoMapper;
public class AutoMapping : Profile
{
    public AutoMapping()
    {
        RequestToEntity();
        EntityToResponse();
    }
    private void RequestToEntity()
    {
        // Mapeamento de DTO's para ENTITIES
        CreateMap<RequestSaleCreateOrUpdateJson, Sale>()
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));

        CreateMap<SaleItem, SaleItemResponse>()
            .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src => (src.Quantity * src.UnitPrice).RoundToTwo()));
    }

    private void EntityToResponse()
    {
        //Mapeamento de ENTITIES para DTO's
        CreateMap<Sale, ResponseSaleCreateJson>()
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date.ToString("dd/MM/yyyy")))
                .ForMember(dest => dest.QuantityItems, opt => opt.MapFrom(src => src.Items.Count()));

        CreateMap<SaleItem, SaleItemResponse>();

        CreateMap<Sale, ResponseSaleShortJson>()
            .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.CustomerName))
            .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date.ToString("dd/MM/yyyy")))
            .ForMember(dest => dest.Number, opt => opt.MapFrom(src => src.Number))
                .ForMember(dest => dest.Value, opt => opt.MapFrom(src => Math.Round(src.TotalAmount, 2)));

        CreateMap<List<Sale>, ResponseSalesJson>()
            .ForMember(dest => dest.Sales, opt => opt.MapFrom(src => src))
            .ForMember(dest => dest.Total, opt => opt.MapFrom(src => src.Sum(x => x.TotalAmount).RoundToTwo()));
    }

}

