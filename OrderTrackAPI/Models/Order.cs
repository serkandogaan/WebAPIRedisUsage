using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace OrderTrackAPI.Models;

public partial class Order
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("CustomerID")]
    public int CustomerId { get; set; }

    public DateTime OrderDate { get; set; }

    [StringLength(300)]
    public string ShipAdress { get; set; } = null!;

    [InverseProperty("Order")]
    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}
