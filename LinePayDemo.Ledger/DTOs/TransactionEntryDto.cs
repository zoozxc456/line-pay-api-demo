namespace LinePayDemo.Ledger.DTOs;

public record TransactionEntryDto(Guid AccountId, decimal Credit, decimal Debit);