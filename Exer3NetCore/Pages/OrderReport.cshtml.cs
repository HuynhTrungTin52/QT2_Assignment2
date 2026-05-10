using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Exer3NetCore.Data;
using Exer3NetCore.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Exer3NetCore.Pages;

[Authorize]
public class OrderReportModel : PageModel
{
    private readonly ECommerceRepository _repository;

    public OrderReportModel(ECommerceRepository repository)
    {
        _repository = repository;
    }

    [BindProperty(SupportsGet = true)]
    public string OrderId { get; set; } = string.Empty;

    public DateTime OrderDate { get; private set; }
    public string AgentId { get; private set; } = string.Empty;
    public List<OrderDetail> Details { get; private set; } = new();
    public bool HasOrder => !string.IsNullOrWhiteSpace(OrderId) && Details.Count > 0;

    public async Task OnGetAsync()
    {
        await LoadOrderAsync();
    }

    public async Task<IActionResult> OnGetExportAsync(string orderId)
    {
        OrderId = orderId;
        await LoadOrderAsync();

        if (!HasOrder)
        {
            return Page();
        }

        var pdfBytes = GeneratePdf();
        return File(pdfBytes, "application/pdf", $"order-{OrderId}.pdf");
    }

    private async Task LoadOrderAsync()
    {
        if (string.IsNullOrWhiteSpace(OrderId))
        {
            return;
        }

        var (order, details) = await _repository.GetOrderAsync(OrderId);
        if (order is null)
        {
            return;
        }

        OrderDate = order.OrderDate;
        AgentId = order.AgentId;
        Details = details;
    }

    private byte[] GeneratePdf()
    {
        return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(30);
                    page.DefaultTextStyle(x => x.FontSize(12));

                    page.Header().Text($"Order {OrderId}").FontSize(20).SemiBold();

                    page.Content().Column(column =>
                    {
                        column.Spacing(10);
                        column.Item().Text($"Date: {OrderDate:yyyy-MM-dd}");
                        column.Item().Text($"Agent: {AgentId}");

                        column.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(3);
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                            });

                            table.Header(header =>
                            {
                                header.Cell().Element(CellStyle).Text("Item");
                                header.Cell().Element(CellStyle).AlignRight().Text("Qty");
                                header.Cell().Element(CellStyle).AlignRight().Text("Unit");
                                header.Cell().Element(CellStyle).AlignRight().Text("Total");
                            });

                            foreach (var detail in Details)
                            {
                                table.Cell().Element(CellStyle).Text(detail.ItemId);
                                table.Cell().Element(CellStyle).AlignRight().Text(detail.Quantity.ToString());
                                table.Cell().Element(CellStyle).AlignRight().Text(detail.UnitAmount.ToString("N0"));
                                table.Cell().Element(CellStyle).AlignRight().Text((detail.Quantity * detail.UnitAmount).ToString("N0"));
                            }
                        });
                    });
                });
            })
            .GeneratePdf();
    }

    private static IContainer CellStyle(IContainer container)
    {
        return container.BorderBottom(1).BorderColor(Colors.Grey.Lighten2).PaddingVertical(4);
    }
}
