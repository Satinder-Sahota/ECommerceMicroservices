using CartService.Models;
using HotChocolate.Types;

namespace CartService.GraphQL
{
    public class CartType:ObjectType<CartItem>
    {
        protected override void Configure(IObjectTypeDescriptor<CartItem> descriptor)
        {
            descriptor.Field(c => c.Id).Type<NonNullType<IntType>>();
            descriptor.Field(c => c.ProductId).Type<NonNullType<IntType>>();
            descriptor.Field(c => c.ProductName).Type<NonNullType<StringType>>();
            descriptor.Field(c => c.Price).Type<NonNullType<DecimalType>>();
            descriptor.Field(c => c.Quantity).Type<NonNullType<IntType>>();
            descriptor.Field(c => c.Total).Type<NonNullType<DecimalType>>().Ignore();
        }
    }
}
