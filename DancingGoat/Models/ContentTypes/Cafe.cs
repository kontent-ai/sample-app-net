using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace DancingGoat.Models
{
    [ModelMetadataType(typeof(CafeMetadata))]
    public partial class Cafe
    {
    }

    public class CafeMetadata
    {
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [EmailAddress]
        public string Email { get; set; }
    }
}