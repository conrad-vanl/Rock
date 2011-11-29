//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the T4\Model.tt template.
//
//     Changes to this file will be lost when the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
//
// THIS WORK IS LICENSED UNDER A CREATIVE COMMONS ATTRIBUTION-NONCOMMERCIAL-
// SHAREALIKE 3.0 UNPORTED LICENSE:
// http://creativecommons.org/licenses/by-nc-sa/3.0/
//
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

using Rock.Models;

namespace Rock.Models.Core
{
    /// <summary>
    /// Defined Type POCO Entity.
    /// </summary>
    [Table( "coreDefinedType" )]
    public partial class DefinedType : ModelWithAttributes<DefinedType>, IAuditable, IOrdered
    {
		/// <summary>
		/// Gets or sets the System.
		/// </summary>
		/// <value>
		/// System.
		/// </value>
		[DataMember]
		public bool System { get; set; }
		
		/// <summary>
		/// Gets or sets the Field Type Id.
		/// </summary>
		/// <value>
		/// Field Type Id.
		/// </value>
		[DataMember]
		public int? FieldTypeId { get; set; }
		
		/// <summary>
		/// Gets or sets the Order.
		/// </summary>
		/// <value>
		/// Order.
		/// </value>
		[DataMember]
		public int Order { get; set; }
		
		/// <summary>
		/// Gets or sets the Name.
		/// </summary>
		/// <value>
		/// Name.
		/// </value>
		[MaxLength( 100 )]
		[DataMember]
		public string Name { get; set; }
		
		/// <summary>
		/// Gets or sets the Description.
		/// </summary>
		/// <value>
		/// Description.
		/// </value>
		[DataMember]
		public string Description { get; set; }
		
		/// <summary>
		/// Gets or sets the Created Date Time.
		/// </summary>
		/// <value>
		/// Created Date Time.
		/// </value>
		[DataMember]
		public DateTime? CreatedDateTime { get; set; }
		
		/// <summary>
		/// Gets or sets the Modified Date Time.
		/// </summary>
		/// <value>
		/// Modified Date Time.
		/// </value>
		[DataMember]
		public DateTime? ModifiedDateTime { get; set; }
		
		/// <summary>
		/// Gets or sets the Created By Person Id.
		/// </summary>
		/// <value>
		/// Created By Person Id.
		/// </value>
		[DataMember]
		public int? CreatedByPersonId { get; set; }
		
		/// <summary>
		/// Gets or sets the Modified By Person Id.
		/// </summary>
		/// <value>
		/// Modified By Person Id.
		/// </value>
		[DataMember]
		public int? ModifiedByPersonId { get; set; }
		
		/// <summary>
        /// Gets a Data Transfer Object (lightweight) version of this object.
        /// </summary>
        /// <value>
        /// A <see cref="DefinedTypeDTO"/> object.
        /// </value>
		public virtual DefinedTypeDTO DataTransferObject
		{
			get { return new DefinedTypeDTO( this ); }
		}

        /// <summary>
        /// Gets the auth entity.
        /// </summary>
		[NotMapped]
		public override string AuthEntity { get { return "Core.DefinedType"; } }
        
		/// <summary>
        /// Gets or sets the Defined Values.
        /// </summary>
        /// <value>
        /// Collection of Defined Values.
        /// </value>
		public virtual ICollection<DefinedValue> DefinedValues { get; set; }
        
		/// <summary>
        /// Gets or sets the Field Type.
        /// </summary>
        /// <value>
        /// A <see cref="FieldType"/> object.
        /// </value>
		public virtual FieldType FieldType { get; set; }
        
		/// <summary>
        /// Gets or sets the Created By Person.
        /// </summary>
        /// <value>
        /// A <see cref="Crm.Person"/> object.
        /// </value>
		public virtual Crm.Person CreatedByPerson { get; set; }
        
		/// <summary>
        /// Gets or sets the Modified By Person.
        /// </summary>
        /// <value>
        /// A <see cref="Crm.Person"/> object.
        /// </value>
		public virtual Crm.Person ModifiedByPerson { get; set; }

    }

    /// <summary>
    /// Defined Type Data Transfer Object.
    /// </summary>
	/// <remarks>
	/// Data Transfer Objects are a lightweight version of the Entity object that are used
	/// in situations like serializing the object in the REST api
	/// </remarks>
    public partial class DefinedTypeDTO
    {
        /// <summary>
        /// The Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the GUID.
        /// </summary>
        /// <value>
        /// The GUID.
        /// </value>
        public Guid Guid { get; set; }

		/// <summary>
		/// Gets or sets the System.
		/// </summary>
		/// <value>
		/// System.
		/// </value>
		public bool System { get; set; }

		/// <summary>
		/// Gets or sets the Field Type Id.
		/// </summary>
		/// <value>
		/// Field Type Id.
		/// </value>
		public int? FieldTypeId { get; set; }

		/// <summary>
		/// Gets or sets the Order.
		/// </summary>
		/// <value>
		/// Order.
		/// </value>
		public int Order { get; set; }

		/// <summary>
		/// Gets or sets the Name.
		/// </summary>
		/// <value>
		/// Name.
		/// </value>
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the Description.
		/// </summary>
		/// <value>
		/// Description.
		/// </value>
		public string Description { get; set; }

		/// <summary>
		/// Gets or sets the Created Date Time.
		/// </summary>
		/// <value>
		/// Created Date Time.
		/// </value>
		public DateTime? CreatedDateTime { get; set; }

		/// <summary>
		/// Gets or sets the Modified Date Time.
		/// </summary>
		/// <value>
		/// Modified Date Time.
		/// </value>
		public DateTime? ModifiedDateTime { get; set; }

		/// <summary>
		/// Gets or sets the Created By Person Id.
		/// </summary>
		/// <value>
		/// Created By Person Id.
		/// </value>
		public int? CreatedByPersonId { get; set; }

		/// <summary>
		/// Gets or sets the Modified By Person Id.
		/// </summary>
		/// <value>
		/// Modified By Person Id.
		/// </value>
		public int? ModifiedByPersonId { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DefinedTypeDTO"/> class.
        /// </summary>
		public DefinedTypeDTO()
		{
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="DefinedTypeDTO"/> class.
        /// </summary>
        /// <param name="definedType">The Defined Type.</param>
		public DefinedTypeDTO( DefinedType definedType )
		{
			Id = definedType.Id;
			Guid = definedType.Guid;
			System = definedType.System;
			FieldTypeId = definedType.FieldTypeId;
			Order = definedType.Order;
			Name = definedType.Name;
			Description = definedType.Description;
			CreatedDateTime = definedType.CreatedDateTime;
			ModifiedDateTime = definedType.ModifiedDateTime;
			CreatedByPersonId = definedType.CreatedByPersonId;
			ModifiedByPersonId = definedType.ModifiedByPersonId;
		}
	}

    /// <summary>
    /// Defined Type Configuration class.
    /// </summary>
    public partial class DefinedTypeConfiguration : EntityTypeConfiguration<DefinedType>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DefinedTypeConfiguration"/> class.
        /// </summary>
        public DefinedTypeConfiguration()
        {
			this.HasOptional( p => p.FieldType ).WithMany( p => p.DefinedTypes ).HasForeignKey( p => p.FieldTypeId );
			this.HasOptional( p => p.CreatedByPerson ).WithMany().HasForeignKey( p => p.CreatedByPersonId );
			this.HasOptional( p => p.ModifiedByPerson ).WithMany().HasForeignKey( p => p.ModifiedByPersonId );
		}
    }
}
