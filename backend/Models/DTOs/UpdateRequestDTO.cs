namespace backend.Models.DTOs
{
    public record UpdateRequestDTO(int Id, string Name, string Description, double Price, int Quantity);
    
}
