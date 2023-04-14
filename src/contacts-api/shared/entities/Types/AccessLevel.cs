namespace Milochau.Contacts.Shared.Entities.Types
{
    public enum AccessLevel
    {
        /// <summary>No access</summary>
        /// <remarks>Nothing can be done</remarks>
        None = 0,

        /// <summary>Read access</summary>
        /// <remarks>Can read data</remarks>
        Read = 1,

        /// <summary>Read & Write access</summary>
        /// <remarks>Can read and edit data</remarks>
        Change = 3,

        /// <summary>Read, Write & Manage access</summary>
        /// <remarks>Can read and edit data, manage access policies</remarks>
        Manage = 7,
    }
}
