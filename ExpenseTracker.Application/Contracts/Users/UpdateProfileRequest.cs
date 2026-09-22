namespace ExpenseTracker.Application.Contracts.Users;

public record UpdateProfileRequest(
    string FirstName,
    string LastName
);
