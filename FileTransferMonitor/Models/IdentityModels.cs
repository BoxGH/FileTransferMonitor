using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;

namespace FileTransferMonitor.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            FilesDownloadNotes = new HashSet<FilesDownloadNote>();
        }
        ICollection<FilesDownloadNote> FilesDownloadNotes { get; set; }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }
        public DbSet<FilesDownloadNote> FilesDownloadNotes { get; set; }
        public DbSet<FilesUploadNote> FilesUploadNotes { get; set; }
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }

    public class FilesDownloadNote
    {
        [Key]
        public long Id { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime? DateTimeDownload { get; set; }
        public string FileOperations { get; set; }
        public string ApplicationUserId { get; set; }
        public long FilesUploadNoteId { get; set; }      
        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual FilesUploadNote FilesUploadNote { get; set; }
    }
     
    public class FilesUploadNote
    {
        public FilesUploadNote()
        {
            FilesDownloadNotes = new HashSet<FilesDownloadNote>();
        }
        [Key]
        public long Id { get; set; }
        [Required]
        [StringLength(256)]
        public string Name { get; set; }
        [Required]
        public decimal Size { get; set; }
        [Required]
        [StringLength(128)]
        public string Type { get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime DataTimeUpload { get; set; }
        [Required]
        [StringLength(256)]
        public string UserName { get; set; }
        [StringLength(256)]
        public string Path { get; set; }
        public virtual ICollection<FilesDownloadNote> FilesDownloadNotes { get; set; }
    }
}