using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;

namespace RepositoryLayer.Entity
{
    public class CollaboratorEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long CollaboratorId { get; set; }
        public string EmailId { get; set; }

        [ForeignKey("Notes")]
        public long NoteId { get; set; }
        [JsonIgnore]
        public virtual NotesEntity Notes { get; set; }

        [ForeignKey("User")]
        public long UserId { get; set; }
        [JsonIgnore]
        public virtual UserEntity User { get; set; }
    }
}
