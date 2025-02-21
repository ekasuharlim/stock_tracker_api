namespace StockTrackingApi.Controllers;

using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using StockTrackingApi.Data;
using StockTrackingApi.Services;
using System.Threading.Tasks;
using StockTrackingApi.Models;

[ApiController]
[Route("api/[controller]")]
public class TransactionsController : ControllerBase
{
    private readonly TransactionService _transactionService;
    private readonly StockTrackingDbContext _context;

    public TransactionsController(TransactionService transactionService, StockTrackingDbContext context)
    {
        _transactionService = transactionService;
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetTransactions()
    {
        var transactions = await _context.Transactions.AsNoTracking().ToListAsync();
        return Ok(transactions);
    }

    [HttpPost("buy")]
    public async Task<IActionResult> CreateTransaction([FromBody] Transaction transaction)
    {
        if (transaction == null || !ValidateTransaction(transaction, "Buy"))
            return BadRequest("Invalid buy transaction data.");

        try
        {
            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetTransactions), new { id = transaction.TransactionID }, transaction);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = "Transaction creation failed.", details = ex.Message });
        }
    }

    [HttpPost("sell")]
    public async Task<IActionResult> ProcessSale([FromBody] Transaction saleTransaction)
    {
        if (saleTransaction == null) return BadRequest("Invalid sale transaction data.");
        if (saleTransaction.TransactionType != "Sell") return BadRequest("Transaction must be of type 'Sell'.");

        if (saleTransaction == null || !ValidateTransaction(saleTransaction, "Sell"))
            return BadRequest("Invalid sale transaction data.");

        try
        {
            await _transactionService.ProcessSaleAsync(saleTransaction);
            return Ok(new { message = "Sale processed successfully." });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = "Sale processing failed.", details = ex.Message });
        }
    }

    private static bool ValidateTransaction(Transaction transaction, string requiredType)
    {
        return transaction.TransactionType == requiredType &&
               transaction.Quantity > 0 &&
               transaction.PricePerShare > 0;
    }

}