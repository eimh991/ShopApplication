namespace Shop.DTO
{
    public class ChangeUserRoleDTO
    {
        public int UserId { get; set; }
        public string NewRole { get; set; } = string.Empty;
    }
}
