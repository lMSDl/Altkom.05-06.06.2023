namespace Models
{
    [Flags]
    public enum Roles
    {
        Delete = 1 << 0,
        Read = 1 << 1,
        UserAdmin = 1 << 2,
        Create = 1 << 3
    }
}