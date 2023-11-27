using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.OrderAggregation;

namespace Talabat.Repository.Data.Configrations
{
    public class OrderConfig : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.Property(o => o.Status).HasConversion(ostatus => ostatus.ToString(), ostatus => (OrderStatus)Enum.Parse(typeof(OrderStatus), ostatus));
            builder.Property(o => o.SubTotal).HasColumnType("decimal(18,2)");
            builder.OwnsOne(o => o.ShippingAddress, sa => sa.WithOwner());
            builder.HasOne(O => O.DeliveryMethod).WithMany().OnDelete(DeleteBehavior.NoAction);

        }
    }
}
