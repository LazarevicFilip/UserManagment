namespace UserManagment.Controllers
{

    
        public record GroupResult(int Id, string Name);

        public record UserResult(int Id, string NameIdentifier, string Email, string? FirstName, string? LastName);
    
}
