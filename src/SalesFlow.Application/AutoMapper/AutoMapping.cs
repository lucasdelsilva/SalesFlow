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
        // CREATE: Mapeamento de criação
        CreateMap<RequestSaleCreateJson, Sale>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Number, opt => opt.Ignore())
            .ForMember(dest => dest.Date, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(dest => dest.TotalAmount, opt => opt.Ignore())
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));

        CreateMap<RequestSaleItemCreateJson, SaleItem>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.TotalPrice, opt =>
                opt.MapFrom(src => (src.Quantity * src.UnitPrice).RoundToTwo()));

        // UPDATE: Mapeamento de atualização da venda
        // UPDATE: Mapeamento de atualização da venda
        CreateMap<RequestSaleUpdateJson, Sale>()
            .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.CustomerName))
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Number, opt => opt.Ignore())
            .ForMember(dest => dest.Date, opt => opt.Ignore())
            .ForMember(dest => dest.TotalAmount, opt => opt.Ignore())
            .ForMember(dest => dest.Items, opt => opt.Ignore());

        // UPDATE: Mapeamento de atualização do item
        CreateMap<RequestSaleItemUpdateJson, SaleItem>()
            .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.ProductName))
            .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
            .ForMember(dest => dest.UnitPrice, opt => opt.MapFrom(src => src.UnitPrice))
            .ForMember(dest => dest.TotalPrice, opt =>
                opt.MapFrom(src => (src.Quantity * src.UnitPrice).RoundToTwo()));
    }

    private void EntityToResponse()
    {
        // GET ALL: Lista resumida de vendas
        CreateMap<List<Sale>, ResponseSalesJson>()
            .ForMember(dest => dest.Sales, opt => opt.MapFrom(src => src))
            .ForMember(dest => dest.Total, opt =>
                opt.MapFrom(src => src.Sum(x => x.TotalAmount).RoundToTwo()));

        CreateMap<Sale, ResponseSaleShortJson>()
            .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.CustomerName))
            .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date.ToString("dd/MM/yyyy")))
            .ForMember(dest => dest.Number, opt => opt.MapFrom(src => src.Number))
            .ForMember(dest => dest.Value, opt =>
                opt.MapFrom(src => Math.Round(src.TotalAmount, 2)));

        // GET BY ID: Detalhes completos da venda
        CreateMap<Sale, ResponseSaleJson>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.QuantityItems, opt => opt.MapFrom(src => src.Items.Count))
            .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date.ToString("dd/MM/yyyy")))
            .ForMember(dest => dest.TotalAmount, opt =>
                opt.MapFrom(src => Math.Round(src.TotalAmount, 2)));

        CreateMap<SaleItem, ResponseSaleItemJson>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.TotalPrice, opt =>
                opt.MapFrom(src => Math.Round(src.TotalPrice, 2)))
            .ForMember(dest => dest.UnitPrice, opt =>
                opt.MapFrom(src => Math.Round(src.UnitPrice, 2)));

        // POST/PUT: Resposta após criar/atualizar
        CreateMap<Sale, ResponseSaleCreateOrUpdateJson>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date.ToString("dd/MM/yyyy")))
            .ForMember(dest => dest.QuantityItems, opt => opt.MapFrom(src => src.Items.Count));

        CreateMap<SaleItem, SaleItemResponse>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));
    }
}